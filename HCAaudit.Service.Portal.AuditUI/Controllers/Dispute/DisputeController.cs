using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using HCAaudit.Service.Portal.AuditUI.Services;
using Microsoft.AspNetCore.Http;
using HCAaudit.Service.Portal.AuditUI.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Collections;
using HCAaudit.Service.Portal.AuditUI.ViewModel;

namespace HCAaudit.Service.Portal.AuditUI.Controllers
{
    [Authorize]
    public class DisputeController : Controller
    {
        private readonly ILogger<DisputeController> _logger;
        private readonly IConfiguration config;
        private readonly IAuthService _authService;
        private AuditToolContext _auditToolContext;
        private bool isAuditor;
        private IErrorLog _log;

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
                    var disp = _auditToolContext.AuditMain.Where(y => y.ID == (int)AuditMainId &&
                            y.isDisputed == true).FirstOrDefault();

                    if (disp != null)
                        return RedirectToAction("Index", "Search");

                    var model = new DisputeModel();
                    //need check for isDisputed
                    model.AuditMain = _auditToolContext.AuditMain.FirstOrDefault(x => x.ID == AuditMainId);
                    var auditResponses = _auditToolContext.AuditMainResponse.Where(x => x.AuditMainID == AuditMainId &&
                        x.isNonCompliant == true).ToList();
                    var subcategory = _auditToolContext.SubCategories.Where(x => x.SubCatgID == model.AuditMain.SubcategoryID).FirstOrDefault();
                    var category = _auditToolContext.Categories.Where(x => x.CatgID == model.AuditMain.ServiceGroupID).FirstOrDefault();

                    model.ServiceDeliveryGroupName = category == null ? string.Empty : category.CatgDescription;
                    model.SubCategoryName = subcategory == null ? string.Empty : subcategory.SubCatgDescription;

                    var listOfValues = _auditToolContext.ListOfValues.Where(x => x.IsActive).ToList();

                    var gracePeriod = new List<SelectListItem>();
                    gracePeriod.Add(new SelectListItem() { Text = "--Select--", Value = "0", Selected = true });
                    foreach (var item in listOfValues.Where(x => x.CodeType.Trim() == "Grace Period"))
                    {
                        gracePeriod.Add(new SelectListItem() { Text = item.Code, Value = item.ID.ToString() });
                    }

                    var overturn = new List<SelectListItem>();
                    overturn.Add(new SelectListItem() { Text = "--Select--", Value = "0", Selected = true });
                    foreach (var item in listOfValues.Where(x => x.CodeType.Trim() == "Over Turn"))
                    {
                        overturn.Add(new SelectListItem() { Text = item.Code, Value = item.ID.ToString() });
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
                                IsCompliant = auditRes.isCompliant,
                                IsNonCompliant = auditRes.isNonCompliant,
                                IsCorrectionRequired = auditRes.isCorrectionRequired,
                                NonComplianceComments = auditRes.NonComplianceComments,
                                TicketId = auditRes.TicketID,
                                QuestionRank = auditRes.QuestionRank

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
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "DisputeController_Index", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult GetData([FromBody] List<AuditNonComplianceModel> model)
        {

            try
            {
                if (isAuditor)
                {
                    //Not handled error and logs
                    var ticketId = model.First().TicketId;
                    var auditMain = _auditToolContext.AuditMain.FirstOrDefault(x => x.TicketID == ticketId);
                    auditMain.isDisputed = true;
                    auditMain.DisputeDate = DateTime.Now;
                    auditMain.ModifiedDate = DateTime.Now;
                    auditMain.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                    auditMain.DisputeAuditor34ID = _authService.LoggedInUserInfo().Result.HcaId;

                    var auditRes = _auditToolContext.AuditMainResponse.Where(x => x.TicketID == ticketId);
                    var dispute = new List<AuditDispute>();
                    foreach (var ques in model)
                    {
                        if (Convert.ToInt32(ques.GracePeriodId) > 0 || Convert.ToInt32(ques.OverturnId) > 0)
                        {
                            auditRes.First(x => x.QuestionId == ques.QuestionId).isNonCompliant = false;
                            dispute.Add(new AuditDispute()
                            {
                                TicketID = ques.TicketId,
                                AuditMainID = auditMain.ID,
                                GracePeriodId = Convert.ToInt32(ques.GracePeriodId),
                                OverTurnId = Convert.ToInt32(ques.OverturnId),
                                QuestionId = Convert.ToInt32(ques.QuestionId),
                                QuestionRank = Convert.ToInt32(ques.QuestionRank),
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

                    FormatAndSendEmail(auditMain.ID);

                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Index method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "DisputeController_Index", ErrorDiscription = ex.Message });
            }
            
            return RedirectToAction("Index", "Home");
        }


        private void FormatAndSendEmail(int mainID)
        {
            var rowStartTag = "<tr>";
            var rowEndTag = "</tr>";
            var cellStartTag = "<td>";
            var cellEndTag = "</td>";

            var auditMain = _auditToolContext.AuditMain.Where(x => x.ID == mainID).FirstOrDefault();
            if (auditMain != null)
            {
                var auditMainResponse = _auditToolContext.AuditDispute.Where(X => X.AuditMainID == mainID).ToList();
                //var category = _auditToolContext.Categories.Where(x => x.CatgID == auditMain.ServiceGroupID).FirstOrDefault();
                //var subCategory = _auditToolContext.SubCategories.Where(x => x.SubCatgID == auditMain.SubcategoryID).FirstOrDefault();

                var environment = auditMain.AuditType.Equals("Production") ? string.Empty : "[Training] ";
                var subject = environment + "Case Management Audit Dispute for Ticket #" + auditMain.TicketID;
                //var sendTo = auditMain.Agent34ID + "@hca.corpad.net";
                var sendTo = _authService.LoggedInUserInfo().Result.HcaId + "@hca.corpad.net"; // To be removed while going into production.
                var sendFrom = _authService.LoggedInUserInfo().Result.HcaId + "@hca.corpad.net";
                var replyTo = _authService.LoggedInUserInfo().Result.HcaId + "@hca.corpad.net";

                var body = "<b>Hi,</b><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This is the Case Management Audit Dispute and Resolution for Ticket #<b>" + auditMain.TicketID + "</b><br>";

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<html><Body>");
                stringBuilder.Append(body);
               // stringBuilder.Append("<br>Ticket Sub Category: " + (subCategory != null ? "<b>" + subCategory.SubCatgDescription + "</b>" : String.Empty));
                stringBuilder.Append("<br><br><table border='1' cellpadding='8'><tr bgcolor='#98C2DB'><th>Rank</th><th>Question Description</th>");
                stringBuilder.Append("<th>Correction Required</th><th>Comments</th><th>Grace Period</th><th>Over Turn</th><th>Dispute Comments</th></tr>");
                foreach (var item in auditMainResponse)
                {
                    var questionDesc = _auditToolContext.QuestionBank.Where(x => x.QuestionId == item.QuestionId).FirstOrDefault();
                    if (questionDesc != null)
                    {
                        var quesSeq = item.QuestionRank;
                        var quesDescription = questionDesc.QuestionDescription;
                        
                        //var correctionRequired = item.isCorrectionRequired == true ? "Yes" : "No";

                        //var comments = item.NonComplianceComments == null ? " " : item.NonComplianceComments;

                        //var graceperiod = item.GracePeriodId;

                        //var overturn = item.OverTurnId;

                        //var comments = item.Comments == null ? string.Empty : item.Comments;

                        //stringBuilder.Append(rowStartTag + cellStartTag + quesSeq + cellEndTag + cellStartTag + quesDescription);
                        //stringBuilder.Append(cellEndTag + cellStartTag + compliance + cellEndTag + cellStartTag + nonCompliance);
                        //stringBuilder.Append(cellEndTag + cellStartTag + notApplicable + cellEndTag + cellStartTag + correctionRequired);
                        //stringBuilder.Append(cellEndTag + cellStartTag + comments + cellEndTag + rowEndTag);
                    }
                }
                stringBuilder.Append(rowStartTag + cellStartTag + "Audit Comments:" + cellEndTag + "<td colspan=6>" + auditMain.AuditNotes + cellEndTag + "</table>");
                stringBuilder.Append("</Body></html>");

                var emailObject = new EmailTemplate
                {
                    SendFrom = sendFrom,
                    SendTo = sendTo,
                    ReplyTo = replyTo,
                    Subject = subject,
                    EmailBody = stringBuilder.ToString()
                };

                //EmailHelper emailHelper = new EmailHelper(_configuration);

               // emailHelper.SendEmailNotification(emailObject);

            }
        }
    }
}

