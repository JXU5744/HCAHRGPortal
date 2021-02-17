using HCAaudit.Service.Portal.AuditUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.Services
{
    public class ErrorLogService:IErrorLog
    {
        private AuditToolContext _auditToolContext;

       public ErrorLogService(AuditToolContext context)
        {
            _auditToolContext = context;
        }
        public void WriteErrorLog(LogItem item)
        {
            ErrorDetail objSysLog = new ErrorDetail();
            objSysLog.ErrorType = item.ErrorType;
            objSysLog.SourceLocation = item.ErrorSource;
            objSysLog.ErrorDescription = item.ErrorDiscription;
            objSysLog.CreatedBy = Environment.UserName;
            objSysLog.CreatedDate = System.DateTime.Now;
            _auditToolContext.ErrorDetails.Add(objSysLog);
            _auditToolContext.SaveChanges();
        }
    }
}
