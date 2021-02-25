using HCAaudit.Service.Portal.AuditUI.Models;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.Services
{
    public interface IAuthService
    {
        public Task<bool> CheckAdminUserGroup();
        public Task<bool> CheckAuditorUserGroup();

        public Task<LoggedInUserDetails> LoggedInUserInfo();

        public Task<string> GetEmailFrom34ID(string input34id);
    }
}