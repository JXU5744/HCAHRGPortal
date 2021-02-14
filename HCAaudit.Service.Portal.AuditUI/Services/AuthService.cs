using HCAaudit.Service.Portal.AuditUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;


namespace HCAaudit.Service.Portal.AuditUI.Services
{
    public class AuthService : IAuthService
    {
        private IHttpContextAccessor contextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            contextAccessor = httpContextAccessor;
        }

        #region

        public async Task<bool> CheckAdminUserGroup()
        {
            var token = await GetIdToken();
            var group = token.Claims.FirstOrDefault(claim => claim.Type == "group") != null? token.Claims.FirstOrDefault(claim => claim.Type == "group").Value.ToLower():"";
            if (group.ToLower().Equals("corp_hr_hraudit_admin"))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckAuditorUserGroup()
        {
            var token = await GetIdToken();
            var group = token.Claims.FirstOrDefault(claim => claim.Type == "group") != null ? token.Claims.FirstOrDefault(claim => claim.Type == "group").Value.ToLower() : "";
            if (group.ToLower().Equals("corp_hr_hraudit_user"))
            {
                return true;
            }
            return false;
        }

        public async Task<LoggedInUserDetails> LoggedInUserInfo()
        {
            var token = await GetToken();
            var firstName = token.Claims?.FirstOrDefault(claim => claim.Type == "firstName")?.Value;
            var lastName = token.Claims?.FirstOrDefault(claim => claim.Type == "lastName")?.Value;
            var hcdId = token.Claims?.FirstOrDefault(claim => claim.Type == "subject")?.Value;
            return new LoggedInUserDetails()
            {
                LoggedInFname = firstName,
                LoggedInLname = lastName,
                LoggedInFullName = firstName + " " + lastName,
                Initials = GetUserInitials(firstName, lastName),
                HcaId = hcdId,
                LoggedInIp = Convert.ToString(contextAccessor.HttpContext.Connection.RemoteIpAddress)
            };

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get User Inititals
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private string GetUserInitials(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && firstName.Length > 1 && !string.IsNullOrEmpty(lastName) && lastName.Length > 1)
            {
                return firstName.Substring(0, 1) + lastName.Substring(0, 1);
            }
            return String.Empty;
        }

        /// <summary>
        /// Get Token
        /// </summary>
        /// <returns></returns>
        private async Task<JwtSecurityToken> GetToken()
        {
            var token_string = await contextAccessor.HttpContext.GetTokenAsync("access_token");
            return new JwtSecurityTokenHandler().ReadJwtToken(token_string);
        }

        /// <summary>
        /// Get Token
        /// </summary>
        /// <returns></returns>
        private async Task<JwtSecurityToken> GetIdToken()
        {
            var token_string = await contextAccessor.HttpContext.GetTokenAsync("id_token");
            
            return new JwtSecurityTokenHandler().ReadJwtToken(token_string);
        }
        #endregion
    }
}