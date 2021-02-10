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
    //[Authorize]
    public class DisputeController : Controller
    {
        private readonly ILogger<DisputeController> _logger;
        private readonly IConfiguration config;
        private IAuthService _authService;
        List<CategoryMast> masterCategory = null;
        private AuditToolContext _auditToolContext;
        public DisputeController(ILogger<DisputeController> logger, IConfiguration configuration, AuditToolContext audittoolc)//, IAuthService authService)
        {
            _auditToolContext = audittoolc;
            _logger = logger;
            config = configuration;
        }

        [HttpGet]
        public IActionResult Index(int AuditMainId)
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
                var questionText = _auditToolContext.QuestionBank.Where(x => x.QuestionID == auditRes.QuestionId).FirstOrDefault();

                if (questionText != null) {

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


        [HttpPost]
        public IActionResult GetData([FromBody]List<AuditNonComplianceModel> model)
        {
            //Not handled error and logs
            var ticketId = model.First().TicketId;
            var auditMain = _auditToolContext.AuditMain.FirstOrDefault(x => x.TicketID == ticketId);
            auditMain.isDisputed = true;
            auditMain.DisputeDate = DateTime.Now;
            //auditMain.DisputeAuditor34ID = "";

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
                        CreatedOn = DateTime.Now
                    });
                }

            }

            _auditToolContext.AuditMain.Update(auditMain);
            _auditToolContext.AuditMainResponse.UpdateRange(auditRes);
            _auditToolContext.AuditDispute.AddRange(dispute);

            var result = _auditToolContext.SaveChanges();

            return Json(result); 

        }
    }
}

