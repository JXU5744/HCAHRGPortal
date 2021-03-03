using HCAaudit.Service.Portal.AuditUI.Models;

namespace HCAaudit.Service.Portal.AuditUI.Services
{
    public class ErrorLogService : IErrorLog
    {
        private readonly AuditToolContext _auditToolContext;
        private readonly IAuthService _authService;

        public ErrorLogService(AuditToolContext context, IAuthService authService)
        {
            _auditToolContext = context;
            _authService = authService;
        }
        public void WriteErrorLog(LogItem item)
        {
            HRAuditErrorLog objSysLog = new HRAuditErrorLog
            {
                ErrorType = item.ErrorType,
                SourceLocation = item.ErrorSource,
                ErrorDescription = item.ErrorDiscription,
                CreatedBy = _authService.LoggedInUserInfo().Result.LoggedInFullName,
                CreatedDate = System.DateTime.Now
            };
            _auditToolContext.HRAuditErrorLog.Add(objSysLog);
            _auditToolContext.SaveChanges();
        }
    }
}
