using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HCAaudit.Service.Portal.AuditUI.Models;

namespace HCAaudit.Service.Portal.AuditUI.Services
{
    public interface IAuthService
    {
        public Task<bool> CheckUserGroups();

        public Task<LoggedInUserDetails> LoggedInUserInfo();
    }
}