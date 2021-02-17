using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCAaudit.Service.Portal.AuditUI.Models;
namespace HCAaudit.Service.Portal.AuditUI.Services
{
    public interface IErrorLog
    {
       void  WriteErrorLog(LogItem item);
    }
}
