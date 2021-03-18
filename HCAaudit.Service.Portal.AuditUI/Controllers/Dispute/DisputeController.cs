using HCAaudit.Service.Portal.AuditUI.Models;
using HCAaudit.Service.Portal.AuditUI.Services;
using HCAaudit.Service.Portal.AuditUI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    [Authorize]
    public class DisputeController : Controller
    {
        private readonly ILogger<DisputeController> _logger;
        private readonly IConfiguration config;
        private readonly IAuthService _authService;
        private readonly AuditToolContext _auditToolContext;
        private readonly bool isAuditor;
        private readonly IErrorLog _log;

        public DisputeController(ILogger<DisputeController> logger, IErrorLog log, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
            _authService = authService;
            isAuditor = _authService.CheckAuditorUserGroup().Result;
            _log = log;
        }

        [HttpGet]
        public IActionResult Index(int AuditMainId)
        {
            try
            {
                if (isAuditor)
                {
                    var disp = _auditToolContext.AuditMain.Where(y => y.Id == AuditMainId &&
                            (y.IsDisputed == true || y.IsEscalated == true)).FirstOrDefault();

                    if (disp != null)
                        return RedirectToAction("Details", "Search");

                    var model = new DisputeModel
                    {
                        //need check for isDisputed
                        AuditMain = _auditToolContext.AuditMain.FirstOrDefault(x => x.Id == AuditMainId)
                    };
                    
                    var auditResponses = _auditToolContext.AuditMainResponse.Where(x => x.AuditMainId == AuditMainId && x.IsNonCompliant == true).ToList();

                    var subcategory = _auditToolContext.SubCategory.Where(x => x.SubCatgId == model.AuditMain.SubcategoryId).FirstOrDefault();
                    var category = _auditToolContext.Category.Where(x => x.CatgId == model.AuditMain.ServiceGroupId).FirstOrDefault();

                    model.ServiceDeliveryGroupName = category == null ? string.Empty : category.CatgDescription;
                    model.SubCategoryName = subcategory == null ? string.Empty : subcategory.SubCatgDescription;

                    var listOfValues = _auditToolContext.ListOfValue.Where(x => x.IsActive == true).ToList();

                    var gracePeriod = new List<SelectListItem>
                    {
                        new SelectListItem() { Text = "--Select--", Value = "0", Selected = true }
                    };
                    foreach (var item in listOfValues.Where(x => x.CodeType.Trim() == "Grace Period"))
                    {
                        gracePeriod.Add(new SelectListItem() { Text = item.Code, Value = item.Id.ToString() });
                    }

                    var overturn = new List<SelectListItem>
                    {
                        new SelectListItem() { Text = "--Select--", Value = "0", Selected = true }
                    };
                    foreach (var item in listOfValues.Where(x => x.CodeType.Trim() == "Over Turn"))
                    {
                        overturn.Add(new SelectListItem() { Text = item.Code, Value = item.Id.ToString() });
                    }


                    var auditNonCompList = new List<AuditNonComplianceModel>();
                    foreach (var auditRes in auditResponses)
                    {
                        var questionText = _auditToolContext.QuestionBank.Where(x => x.QuestionId == auditRes.QuestionId).FirstOrDefault();

                        if (questionText != null)
                        {
                            auditNonCompList.Add(new AuditNonComplianceModel()
                            {
                                QuestionId = auditRes.QuestionId,
                                Question = questionText.QuestionDescription, //need to call service to get the question
                                IsCompliant = (bool)auditRes.IsCompliant,
                                IsNonCompliant = (bool)auditRes.IsNonCompliant,
                                IsHighNonCompliant  = (bool)auditRes.IsHighNonComplianceImpact,
                                DowngradeRequired = false,
                                IsCorrectionRequired = (bool)auditRes.IsCorrectionRequired,
                                NonComplianceComments = auditRes.NonComplianceComments,
                                TicketId = auditRes.TicketId,
                                QuestionRank = (int)auditRes.QuestionRank
                            });
                        }
                    }
                    model.GracePeriod = gracePeriod;
                    model.Overturn = overturn;
                    model.AuditNonComplianceModel = auditNonCompList;

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Index method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "DisputeController_Index", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SaveDispute([FromBody] List<AuditNonComplianceModel> model)
        {
            try
            {
                if (isAuditor)
                {
                    //Not handled error and logs
                    var ticketId = model.First().TicketId;
                    var auditMain = _auditToolContext.AuditMain.FirstOrDefault(x => x.TicketId == ticketId);
                    auditMain.IsDisputed = true;
                    auditMain.DisputeDate = DateTime.Now;
                    auditMain.ModifiedDate = DateTime.Now;
                    auditMain.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                    auditMain.DisputeAuditor34Id = _authService.LoggedInUserInfo().Result.HcaId;

                    var auditRes = _auditToolContext.AuditMainResponse.Where(x => x.TicketId == ticketId);
                    var dispute = new List<AuditDispute>();
                    foreach (var ques in model)
                    {
                        if (Convert.ToInt32(ques.GracePeriodId) > 0 || Convert.ToInt32(ques.OverturnId) > 0)
                        {
                            auditRes.First(x => x.QuestionId == ques.QuestionId).IsNonCompliant = false;
                            dispute.Add(new AuditDispute()
                            {
                                TicketId = ques.TicketId,
                                AuditMainId = auditMain.Id,
                                GracePeriodId = Convert.ToInt32(ques.GracePeriodId),
                                OverTurnId = Convert.ToInt32(ques.OverturnId),
                                QuestionId = Convert.ToInt32(ques.QuestionId),
                                QuestionRank = Convert.ToInt32(ques.QuestionRank),
                                IsDowngraded = ques.DowngradeRequired,
                                Comments = ques.Comment,
                                CreatedDate = DateTime.Now,
                                CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName,
                                ModifiedDate = DateTime.Now,
                                ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName
                            });
                        }
                    }
                    _auditToolContext.AuditMain.Update(auditMain);
                    _auditToolContext.AuditMainResponse.UpdateRange(auditRes);
                    _auditToolContext.AuditDispute.AddRange(dispute);
                    var result = _auditToolContext.SaveChanges();

                    SessionHelper.SetObjectAsJson(HttpContext.Session, Common.CaseIDSessionKeyName, ticketId);

                    FormatAndSendEmail(auditMain.Id);
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Index method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "DisputeController_Index", ErrorDiscription = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        private void FormatAndSendEmail(int mainID)
        {
            var rowStartTag = "<tr>";
            var rowEndTag = "</tr>";
            var cellStartTag = "<td>";
            var cellEndTag = "</td>";

            var auditMain = _auditToolContext.AuditMain.Where(x => x.Id == mainID).FirstOrDefault();
            if (auditMain != null)
            {
                var auditDispute = _auditToolContext.AuditDispute.Where(X => X.AuditMainId == mainID).ToList();
                var subCategory = _auditToolContext.SubCategory.Where(x => x.SubCatgId == auditMain.SubcategoryId).FirstOrDefault();
                var environment = auditMain.AuditType.Equals("Production") ? string.Empty : "[Training] ";
                var subject = environment + "Case Management Audit Dispute for Ticket #" + auditMain.TicketId;

                //var sendTo = sentoEmail; // + "@hca.corpad.net"; // To be removed while going into production.
               // var sendTo = _authService.GetEmailFrom34ID(_authService.LoggedInUserInfo().Result.HcaId).Result.ToString();

                //Required for PROD **********
                //var sendTo = _authService.GetEmailFrom34ID(sentoEmail).Result.ToString();

                var sendFrom = _authService.LoggedInUserInfo().Result.EmailAddress;
                var sendTo = sendFrom;
                var replyTo = _authService.LoggedInUserInfo().Result.EmailAddress;

                var body = "<b>Hi,</b><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This is the Case Management Audit Dispute and Resolution for Ticket #<b>" + auditMain.TicketId + "</b><br>";

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<html><Body>");
                stringBuilder.Append(body);
                stringBuilder.Append("<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Ticket Sub Category: " + (subCategory != null ? "<b>" + subCategory.SubCatgDescription + "</b>" : String.Empty));
                stringBuilder.Append("<br><br><table border='1' cellpadding='8'><tr bgcolor='#98C2DB'><th>Question Sequence</th><th>Question Description</th>");
                stringBuilder.Append("<th>Correction Required</th><th>Audit Comments</th><th>Grace Period</th><th>Over Turn</th><th>Downgrade</th><th>Dispute Comments</th></tr>");
                foreach (var item in auditDispute)
                {
                    var questionDesc = _auditToolContext.QuestionBank.Where(x => x.QuestionId == item.QuestionId).FirstOrDefault();
                    var auditResponseObject = _auditToolContext.AuditMainResponse.Where(
                        x => x.QuestionId == item.QuestionId &&
                        x.AuditMainId == item.AuditMainId &&
                        x.IsNonCompliant == true).FirstOrDefault();

                    if (questionDesc != null)
                    {
                        var gracePeriod = _auditToolContext.ListOfValue.Where(x => x.Id == (int)item.GracePeriodId).FirstOrDefault();
                        var overTurn = _auditToolContext.ListOfValue.Where(x => x.Id == (int)item.OverTurnId).FirstOrDefault();
                        var quesSeq = item.QuestionRank;
                        var quesDescription = questionDesc.QuestionDescription;
                        var correctionRequired = auditResponseObject != null && auditResponseObject.IsCorrectionRequired == true ? "Yes" : "No";
                        var responseComments = auditResponseObject != null ? auditResponseObject.NonComplianceComments : String.Empty;
                        var gracePeriodDesc = gracePeriod != null ? gracePeriod.Code : String.Empty;
                        var overTurnDesc = overTurn != null ? overTurn.Code : String.Empty;
                        var downgradeRequired =  item.IsDowngraded == true ? "Yes" : "No";
                        var comments = item.Comments ?? string.Empty;

                        stringBuilder.Append(rowStartTag + cellStartTag + quesSeq + cellEndTag + cellStartTag + quesDescription);
                        stringBuilder.Append(cellEndTag + cellStartTag + correctionRequired + cellEndTag + cellStartTag + responseComments);
                        stringBuilder.Append(cellEndTag + cellStartTag + gracePeriodDesc + cellEndTag + cellStartTag + overTurnDesc);
                        stringBuilder.Append(cellEndTag + cellStartTag + downgradeRequired );
                        stringBuilder.Append(cellEndTag + cellStartTag + comments + cellEndTag + rowEndTag);
                    }
                }

                stringBuilder.Append("</table>");
                stringBuilder.Append("</Body></html>");

                var emailObject = new EmailTemplate
                {
                    SendFrom = sendFrom,
                    SendTo = sendTo,
                    ReplyTo = replyTo,
                    Subject = subject,
                    SendFromName = _authService.LoggedInUserInfo().Result.LoggedInFullName,
                    EmailBody = stringBuilder.ToString()
                };
                EmailHelper emailHelper = new EmailHelper(config, _log);
                emailHelper.SendEmailNotification(emailObject);
            }
        }
    }
}