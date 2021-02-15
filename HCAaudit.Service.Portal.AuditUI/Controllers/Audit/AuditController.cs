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
    public class AuditController : Controller
    {
        private readonly ILogger<AuditController> _logger;
        private readonly IConfiguration config;
        private readonly IAuthService _authService;
        private readonly AuditToolContext _auditToolContext;
        private readonly bool isAuditor;

        public AuditController(ILogger<AuditController> logger, IConfiguration configuration, AuditToolContext audittoolc, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
            _authService = authService;
            isAuditor = _authService.CheckAuditorUserGroup().Result;
        }

        public IActionResult Index(String TicketId, String TicketStatus, String TicketDate,
            int ServiceCatId, int SubCatId, String EnvironmentType, String TicketSubStatus)
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
                var ticketSubStatus = TicketSubStatus;
                var auditTicketDate = TicketDate.Replace("%2F", "/");
                var recordDate = DateTime.Parse(auditTicketDate);
                var serviceCategory = ServiceCatId;
                var subCategory = SubCatId;
                var environmentType = EnvironmentType;

                var auditMain = _auditToolContext.AuditMain.Where(x => x.TicketID == TicketId
                    && x.AuditType == environmentType
                    && x.TicketDate == recordDate
                    && x.SubcategoryID == subCategory).FirstOrDefault();

                if (auditMain != null)
                    return RedirectToAction("Index", "Search");

                var auditViewModel = new AuditViewModel();

                var subcategory = _auditToolContext.SubCategories.Where(cat => cat.SubCatgID == subCategory &&
                cat.CatgID == serviceCategory && cat.IsActive == true).FirstOrDefault();
                var subCategoryDescription = subcategory != null ? subcategory.SubCatgDescription : String.Empty;

                var category = _auditToolContext.Categories.Where(cat => cat.CatgID == serviceCategory
                    && cat.IsActive == true).FirstOrDefault();
                var categoryDescription = category != null ? category.CatgDescription : String.Empty;

                if (ticketStatus == "0")
                {
                    var ssisTicket = _auditToolContext.SearchTicketDetail.Where(
                        ssis => ssis.TicketCode == ticketId
                        && ssis.TicketStatus == "0"
                        && ssis.ClosedDate == recordDate
                        && ssis.SubCategory == subCategoryDescription).FirstOrDefault();

                    if (ssisTicket != null)
                    {

                        auditViewModel.Agent34Id = String.IsNullOrWhiteSpace(ssisTicket.CloseUserId) ? "" : ssisTicket.CloseUserId.Substring(0, 7);

                        var hrProff = _auditToolContext.HROCRoster.Where(hrp => hrp.EmployeethreefourID == auditViewModel.Agent34Id).FirstOrDefault();
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
                }
                else if (ticketStatus == "1")
                {
                    var ssisTicket = _auditToolContext.SearchTicketDetail.Where(
                        ssis => ssis.TicketCode == ticketId
                        && ssis.TicketStatus == "1"
                        && ssis.CreateDate == recordDate
                        && ssis.SubCategory == subCategoryDescription).FirstOrDefault();

                    if (ssisTicket != null)
                    {

                        auditViewModel.Agent34Id = String.IsNullOrWhiteSpace(ssisTicket.CreatorUserId) ? "" : ssisTicket.CreatorUserId.Substring(0, 7);

                        var hrProff = _auditToolContext.HROCRoster.Where(hrp => hrp.EmployeethreefourID == auditViewModel.Agent34Id).FirstOrDefault();
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
                }

                return View(auditViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult SaveAudit(AuditViewModel audit)
        {
            if (isAuditor)
            {
                if (audit != null)
                {
                    AuditMain main = new AuditMain();
                    main.TicketID = audit.TicketId;
                    main.Agent34ID = audit.Agent34Id;
                    main.AgentName = audit.AgentName;
                    main.AuditNotes = audit.AuditNote;
                    main.AuditorName = audit.AuditorName;
                    main.AuditType = audit.EnvironmentType;
                    main.ModifiedDate = DateTime.Now;
                    main.ServiceGroupID = audit.ServiceCatId;
                    main.SubcategoryID = audit.SubCatId;
                    main.SubmitDT = DateTime.Now;
                    main.TicketDate = audit.TicketDate;
                    _auditToolContext.AuditMain.Add(main);
                    _auditToolContext.SaveChanges();


                    foreach (var item in audit.Question)
                    {
                        AuditMainResponse obj = new AuditMainResponse();

                        obj.QuestionId = item.QuestionId;
                        //Audit Main ID to be set in AuditMainresponse
                        obj.AuditMainID = main.ID;
                        obj.QuestionRank = item.QuestionSequence;
                        obj.TicketID = main.TicketID;
                        obj.isCompliant = item.Action.IsCompliance;
                        obj.isCorrectionRequired = item.CorrectionRequire;
                        obj.isHighNonComplianceImpact = item.Action.Impact.IsHighImpact;
                        obj.isLowNonComplianceImpact = item.Action.Impact.IsLowImpact;
                        obj.isNA = item.Action.IsNotApplicable;
                        obj.isNonCompliant = item.Action.IsNonCompliance;
                        obj.NonComplianceComments = item.Comments;
                        obj.ModifiedDate = DateTime.Now;
                        _auditToolContext.AuditMainResponse.Add(obj);
                        _auditToolContext.SaveChanges();
                    }
                }
                return RedirectToAction("Index", "Search");
            } 
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
    }
}

