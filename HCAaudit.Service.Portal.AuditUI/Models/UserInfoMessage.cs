using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    /// <summary>
    /// PatientMessage structure for patient Information
    /// </summary>
    public class UserInfoMessage
    {
        #region Constructor
        /// <summary>
        /// Initializes a new CanonicalMessage
        /// </summary>
        public UserInfoMessage()
        {
            LoggedInUser = new LoggedInUserDetails();
            Requestor = new Requestor();
            PatientDetailsList = new List<UserDetails>();
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Payor/Physician
        /// </summary>
        public LoggedInUserDetails LoggedInUser { get; set; }

        /// <summary>
        /// Provider Details
        /// </summary>
        public Requestor Requestor { get; set; }

        /// <summary>
        /// Patient Details
        /// </summary>
        public List<UserDetails> PatientDetailsList { get; set; }

        #endregion
    }
}

