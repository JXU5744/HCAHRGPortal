using HCAaudit.Service.Portal.AuditUI.Models;
namespace HCAaudit.Service.Portal.AuditUI.Services
{
    public interface IErrorLog
    {
        void WriteErrorLog(LogItem item);
    }
}
