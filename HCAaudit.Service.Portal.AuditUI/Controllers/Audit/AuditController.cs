﻿using HCAaudit.Service.Portal.AuditUI.Models;
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
    public class AuditController : Controller
    {
        private readonly ILogger<AuditController> _logger;
        private readonly IAuthService _authService;
        private readonly AuditToolContext _auditToolContext;
        private readonly bool isAuditor;
        private readonly IErrorLog _log;
        public IConfiguration _configuration { get; }

        public AuditController(ILogger<AuditController> logger, IErrorLog log, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            _configuration = configuration;
            _authService = authService;
            isAuditor = _authService.CheckAuditorUserGroup().Result;
            _log = log;
        }

        public IActionResult Index(String TicketId, String TicketStatus, String TicketDate,
            int ServiceCatId, int SubCatId, String EnvironmentType, String TicketSubStatus)
        {
            try
            {
                if (isAuditor)
                {
                    if (string.IsNullOrWhiteSpace(TicketId) ||
                        string.IsNullOrWhiteSpace(TicketStatus) ||
                        string.IsNullOrWhiteSpace(TicketDate) ||
                        ServiceCatId == 0 || SubCatId == 0)
                        return RedirectToAction("Index", "Search");

                    var ticketId = TicketId;
                    var ticketStatus = TicketStatus;
                    var auditTicketDate = TicketDate.Replace("%2F", "/");
                    var recordDate = DateTime.Parse(auditTicketDate);
                    var serviceCategory = ServiceCatId;
                    var subCategory = SubCatId;
                    var environmentType = EnvironmentType;

                    var auditMain = _auditToolContext.AuditMain.Where(x => x.TicketId == TicketId
                        && x.AuditType == environmentType
                        && x.TicketDate == recordDate
                        && x.SubcategoryId == subCategory).FirstOrDefault();

                    if (auditMain != null)
                        return RedirectToAction("Index", "Search");

                    var auditViewModel = new AuditViewModel();

                    var subcategory = _auditToolContext.SubCategory.Where(cat => cat.SubCatgId == subCategory &&
                    cat.CatgId == serviceCategory && cat.IsActive == true).FirstOrDefault();
                    var subCategoryDescription = subcategory != null ? subcategory.SubCatgDescription : String.Empty;

                    var category = _auditToolContext.Category.Where(cat => cat.CatgId == serviceCategory
                        && cat.IsActive == true).FirstOrDefault();
                    var categoryDescription = category != null ? category.CatgDescription : String.Empty;

                    if (ticketStatus == "0")
                    {
                        var ssisTicket = _auditToolContext.TicketsViaSSIS.Where(
                            ssis => ssis.TicketCode == ticketId
                            && ssis.TicketStatus == "0"
                            && ssis.ClosedDate == recordDate
                            && ssis.SubCategory == subCategoryDescription).FirstOrDefault();

                        if (ssisTicket != null)
                        {
                            auditViewModel.Agent34Id = String.IsNullOrWhiteSpace(ssisTicket.CloseUserId) ? "" : ssisTicket.CloseUserId.Substring(0, 7);

                            var hrProff = _auditToolContext.HROCRoster.Where(hrp => hrp.Employee34IdLowerCase == auditViewModel.Agent34Id).FirstOrDefault();
                            var hrProffName = hrProff != null ? hrProff.EmployeeFullName : String.Empty;

                            auditViewModel.TicketId = ssisTicket.TicketCode;
                            auditViewModel.AuditorName = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                            auditViewModel.EnvironmentType = environmentType;
                            auditViewModel.ServiceCatId = serviceCategory;
                            auditViewModel.SubCatName = subCategoryDescription;
                            auditViewModel.SupervisorName = hrProff != null ? hrProff.EmployeeFullName : String.Empty;
                            auditViewModel.ServiceGroupName = categoryDescription;
                            auditViewModel.SubCatId = subCategory;
                            auditViewModel.AgentName = hrProffName;
                            auditViewModel.TicketDate = (DateTime)ssisTicket.ClosedDate;

                            var listOfValues = _auditToolContext.ListOfValue.Where(x => x.IsActive == true &&
                                x.CodeType.Trim().ToLower() == "audit cancel reason").ToList();

                            var cancelReason = new List<SelectListItem>();
                            cancelReason.Add(new SelectListItem() { Text = "--Select--", Value = "0", Selected = true });
                            foreach (var item in listOfValues)
                            {
                                cancelReason.Add(new SelectListItem() { Text = item.Code, Value = item.Id.ToString() });
                            }

                            auditViewModel.CancellationReason = cancelReason;

                            var query = _auditToolContext.QuestionBank
                                .Join(
                                _auditToolContext.QuestionMapping.Where(a => a.SubCatgId == subCategory
                                && a.IsActive == true),
                            questionBanks => questionBanks.QuestionId,
                            questionMasters => questionMasters.QuestionId,
                            (questionBanks, questionMasters) => new
                            {
                                QuestionId = questionBanks.QuestionId,
                                QuestionName = questionBanks.QuestionName,
                                QuestionDescription = questionBanks.QuestionDescription,
                                SeqNumber = questionMasters.SeqNumber,
                                CatSubCatId = questionMasters.SubCatgId
                            })
                                .OrderBy(b => b.SeqNumber)
                                .Select(x => new QuestionConfigMappingJoinMast
                                {
                                    QuestionId = x.QuestionId,
                                    QuestionName = x.QuestionName,
                                    QuestionDescription = x.QuestionDescription,
                                    QuestionSeqNumber = x.SeqNumber
                                }).OrderBy(c => c.QuestionSeqNumber).ToList();

                            List<HCAaudit.Service.Portal.AuditUI.ViewModel.Question> lstQuestionList = new List<HCAaudit.Service.Portal.AuditUI.ViewModel.Question>();

                            foreach (var item in query)
                            {
                                ViewModel.Action objAction = new ViewModel.Action();
                                Impact objImpact = new Impact();
                                objImpact.IsHighImpact = false;
                                objImpact.IsLowImpact = false;
                                objAction.Impact = objImpact;
                                objAction.IsCompliance = false;
                                objAction.IsNonCompliance = false;
                                objAction.IsNotApplicable = false;

                                HCAaudit.Service.Portal.AuditUI.ViewModel.Question objQuestion = new HCAaudit.Service.Portal.AuditUI.ViewModel.Question();
                                objQuestion.QuestionId = item.QuestionId;
                                objQuestion.QuestionName = item.QuestionName;
                                objQuestion.QuestionDescription = item.QuestionDescription;
                                objQuestion.QuestionSequence = item.QuestionSeqNumber;
                                objQuestion.Action = objAction;
                                objQuestion.CorrectionRequire = true;
                                lstQuestionList.Add(objQuestion);
                            }
                            auditViewModel.Question = lstQuestionList;
                        }
                        else
                        {
                            return RedirectToAction("Index", "Search");
                        }
                    }
                    else if (ticketStatus == "1")
                    {
                        var ssisTicket = _auditToolContext.TicketsViaSSIS.Where(
                            ssis => ssis.TicketCode == ticketId
                            && ssis.TicketStatus == "1"
                            && ssis.CreateDate == recordDate
                            && ssis.SubCategory == subCategoryDescription).FirstOrDefault();

                        if (ssisTicket != null)
                        {
                            auditViewModel.Agent34Id = String.IsNullOrWhiteSpace(ssisTicket.CreatorUserId) ? "" : ssisTicket.CreatorUserId.Substring(0, 7);

                            var hrProff = _auditToolContext.HROCRoster.Where(hrp => hrp.Employee34IdLowerCase == auditViewModel.Agent34Id).FirstOrDefault();
                            var hrProffName = hrProff != null ? hrProff.EmployeeFullName : String.Empty;

                            auditViewModel.TicketId = ssisTicket.TicketCode;
                            auditViewModel.AuditorName = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                            auditViewModel.EnvironmentType = environmentType;
                            auditViewModel.ServiceCatId = serviceCategory;
                            auditViewModel.SubCatName = subCategoryDescription;
                            auditViewModel.ServiceGroupName = categoryDescription;
                            auditViewModel.ServiceCatId = serviceCategory;
                            auditViewModel.AgentName = hrProffName;
                            auditViewModel.TicketDate = (DateTime)ssisTicket.CreateDate;

                            var listOfValues = _auditToolContext.ListOfValue.Where(x => x.IsActive == true &&
                                x.CodeType.Trim().ToLower() == "audit cancel reason").ToList();

                            var cancelReason = new List<SelectListItem>();
                            cancelReason.Add(new SelectListItem() { Text = "--Select--", Value = "0", Selected = true });
                            foreach (var item in listOfValues)
                            {
                                cancelReason.Add(new SelectListItem() { Text = item.Code, Value = item.Id.ToString() });
                            }

                            auditViewModel.CancellationReason = cancelReason;

                            var query = _auditToolContext.QuestionBank
                                .Join(
                                _auditToolContext.QuestionMapping.Where(a => a.SubCatgId == serviceCategory
                                && a.IsActive == true),
                            questionBanks => questionBanks.QuestionId,
                            questionMasters => questionMasters.QuestionId,
                            (questionBanks, questionMasters) => new
                            {
                                QuestionId = questionBanks.QuestionId,
                                QuestionName = questionBanks.QuestionName,
                                QuestionDescription = questionBanks.QuestionDescription,
                                SeqNumber = questionMasters.SeqNumber,
                                CatSubCatId = questionMasters.SubCatgId
                            })
                                .OrderBy(b => b.SeqNumber)
                                .Select(x => new QuestionConfigMappingJoinMast
                                {
                                    QuestionId = x.QuestionId,
                                    QuestionName = x.QuestionName,
                                    QuestionDescription = x.QuestionDescription,
                                    QuestionSeqNumber = x.SeqNumber
                                }).ToList();

                            List<HCAaudit.Service.Portal.AuditUI.ViewModel.Question> lstQuestionList = new List<HCAaudit.Service.Portal.AuditUI.ViewModel.Question>();
                            foreach (var item in query)
                            {
                                ViewModel.Action objAction = new ViewModel.Action();
                                Impact objImpact = new Impact();
                                objImpact.IsHighImpact = false;
                                objImpact.IsLowImpact = false;
                                objAction.Impact = objImpact;
                                objAction.IsCompliance = false;
                                objAction.IsNonCompliance = false;
                                objAction.IsNotApplicable = false;

                                HCAaudit.Service.Portal.AuditUI.ViewModel.Question objQuestion = new HCAaudit.Service.Portal.AuditUI.ViewModel.Question();
                                objQuestion.QuestionId = item.QuestionId;
                                objQuestion.QuestionName = item.QuestionName;
                                objQuestion.QuestionDescription = item.QuestionDescription;
                                objQuestion.QuestionSequence = item.QuestionSeqNumber;
                                objQuestion.Action = objAction;
                                objQuestion.CorrectionRequire = true;
                                lstQuestionList.Add(objQuestion);
                            }
                            auditViewModel.Question = lstQuestionList;
                        }
                        else
                        {
                            return RedirectToAction("Index", "Search");
                        }
                    }
                    return View(auditViewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Index method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "AuditController_Index", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SaveAudit(AuditViewModel audit)
        {
            try
            {
                if (isAuditor)
                {
                    if (audit != null)
                    {
                        AuditMain main = new AuditMain();
                        main.TicketId = audit.TicketId;
                        main.Agent34Id = audit.Agent34Id;
                        main.AgentName = audit.AgentName;
                        main.AuditNotes = audit.AuditNote;
                        main.AuditorName = audit.AuditorName;
                        main.AuditType = audit.EnvironmentType;
                        main.ServiceGroupId = audit.ServiceCatId;
                        main.SubcategoryId = audit.SubCatId;
                        main.SubmitDt = DateTime.Now;
                        main.TicketDate = audit.TicketDate;
                        main.CreatedDate = DateTime.Now;
                        main.CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                        main.ModifiedDate = DateTime.Now;
                        main.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                        _auditToolContext.AuditMain.Add(main);
                        _auditToolContext.SaveChanges();

                        foreach (var item in audit.Question)
                        {
                            AuditMainResponse obj = new AuditMainResponse();

                            obj.QuestionId = item.QuestionId;
                            obj.AuditMainId = main.Id;
                            obj.QuestionRank = item.QuestionSequence;
                            obj.TicketId = main.TicketId;
                            obj.IsCompliant = item.Action.IsCompliance;
                            obj.IsCorrectionRequired = item.CorrectionRequire;
                            obj.IsHighNonComplianceImpact = item.Action.Impact.IsHighImpact;
                            obj.IsLowNonComplianceImpact = item.Action.Impact.IsLowImpact;
                            obj.IsNa = item.Action.IsNotApplicable;
                            obj.IsNonCompliant = item.Action.IsNonCompliance;
                            obj.NonComplianceComments = item.Comments;
                            obj.CreatedDate = DateTime.Now;
                            obj.CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                            obj.ModifiedDate = DateTime.Now;
                            obj.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;

                            _auditToolContext.AuditMainResponse.Add(obj);
                            _auditToolContext.SaveChanges();
                        }

                        FormatAndSendEmail(main.Id);
                    }
                    return RedirectToAction("Index", "Search");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in SaveAudit method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "AuditController_SaveAudit", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult CancelAudit(AuditViewModel audit)
        {
            try
            {
                object response = "";
                if (isAuditor)
                {
                    if (audit != null)
                    {
                        int tempreasonid = 0;
                        if (audit.AuditorQuitReasonId != null && int.TryParse(audit.AuditorQuitReasonId, out tempreasonid))
                        {
                            if (tempreasonid > 0)
                            {
                                var cancelreason = _auditToolContext.ListOfValue.Where(x => x.Id == Int32.Parse(audit.AuditorQuitReasonId)).FirstOrDefault();

                                AuditMain main = new AuditMain();

                                main.TicketId = audit.TicketId;
                                main.Agent34Id = audit.Agent34Id;
                                main.AgentName = audit.AgentName;
                                main.AuditorName = audit.AuditorName;
                                main.AuditType = audit.EnvironmentType;
                                main.ServiceGroupId = audit.ServiceCatId;
                                main.SubcategoryId = audit.SubCatId;
                                main.SubmitDt = DateTime.Now;
                                main.AuditorQuit = "Quit";
                                main.AuditorQuitReason = cancelreason.Code;
                                main.TicketDate = audit.TicketDate;
                                main.CreatedDate = DateTime.Now;
                                main.CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                                main.ModifiedDate = DateTime.Now;
                                main.ModifiedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName;
                                _auditToolContext.AuditMain.Add(main);
                                _auditToolContext.SaveChanges();
                            }
                            else
                            {
                                response = "1";
                            }
                        }
                        else
                        {
                            response = "1";
                        }
                    }
                    else
                    {
                        response = "1";
                    }
                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in CancelAudit method");
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "AuditController_CancelAudit", ErrorDiscription = ex.Message });
            }
            return RedirectToAction("Index", "Home");
        }

        private void FormatAndSendEmail(int mainID)
        {
            var rowStartTag = "<tr>";
            var rowEndTag = "</tr>";
            var cellStartTag = "<td>";
            var cellEndTag = "</td>";

            try
            {
                var auditMain = _auditToolContext.AuditMain.Where(x => x.Id == mainID).FirstOrDefault();
                if (auditMain != null)
                {
                    var auditMainResponse = _auditToolContext.AuditMainResponse.Where(X => X.AuditMainId == mainID).ToList();
                    var subCategory = _auditToolContext.SubCategory.Where(x => x.SubCatgId == auditMain.SubcategoryId).FirstOrDefault();
                    var environment = auditMain.AuditType.Equals("Production") ? string.Empty : "[Training] ";
                    var subject = environment + "Case Management Audit Ticket #" + auditMain.TicketId;
                    
                    var sendTo = _authService.LoggedInUserInfo().Result.HcaId + "@hca.corpad.net"; // To be removed while going into production.
                    var sendFrom = _authService.LoggedInUserInfo().Result.HcaId + "@hca.corpad.net";
                    var replyTo = _authService.LoggedInUserInfo().Result.HcaId + "@hca.corpad.net";

                    var body = "<b>Hi,</b><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Attached you will find a Case Management Audit for Ticket #<b>" + auditMain.TicketId + "</b><br>";

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("<html><Body>");
                    stringBuilder.Append(body);
                    stringBuilder.Append("<br>Ticket Sub Category: " + (subCategory != null ? "<b>" + subCategory.SubCatgDescription + "</b>" : String.Empty));
                    stringBuilder.Append("<br><br><table border='1' cellpadding='8'><tr bgcolor='#98C2DB'><th>Question Sequence</th><th>Question Description</th><th>Compliant</th><th>Non Compliant</th><th>");
                    stringBuilder.Append("Not Applicable</th><th>Correction Required</th><th>Comments</th></tr>");
                    foreach (var item in auditMainResponse)
                    {
                        var questionDesc = _auditToolContext.QuestionBank.Where(x => x.QuestionId == item.QuestionId).FirstOrDefault();
                        if (questionDesc != null)
                        {
                            var quesSeq = item.QuestionRank;
                            var quesDescription = questionDesc.QuestionDescription;
                            var compliance = item.IsCompliant == true ? "Yes" : "No";
                            var nonCompliance = item.IsNonCompliant == true ? "Yes" : "No";
                            var lowNonCompliant = item.IsLowNonComplianceImpact == true ? " (Low)" : string.Empty;
                            var impact = item.IsHighNonComplianceImpact == true ? " (High)" : lowNonCompliant;
                            nonCompliance += impact;
                            var correctionRequired = item.IsCorrectionRequired == true ? "Yes" : "No";
                            var notApplicable = item.IsNa == true ? "Yes" : "No";
                            var comments = item.NonComplianceComments == null ? " " : item.NonComplianceComments;

                            stringBuilder.Append(rowStartTag + cellStartTag + quesSeq + cellEndTag + cellStartTag + quesDescription);
                            stringBuilder.Append(cellEndTag + cellStartTag + compliance + cellEndTag + cellStartTag + nonCompliance);
                            stringBuilder.Append(cellEndTag + cellStartTag + notApplicable + cellEndTag + cellStartTag + correctionRequired);
                            stringBuilder.Append(cellEndTag + cellStartTag + comments + cellEndTag + rowEndTag);
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
                        SendFromName = _authService.LoggedInUserInfo().Result.LoggedInFullName,
                        EmailBody = stringBuilder.ToString()
                    };
                    EmailHelper emailHelper = new EmailHelper(_configuration, _log);
                    emailHelper.SendEmailNotification(emailObject);
                }
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "AuditController_FormatAndSendEmail", ErrorDiscription = ex.InnerException.ToString() });
            }
        }
    }
}