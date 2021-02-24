namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class LoggedInUserDetails
    {
        /// <summary>
        /// HCA 3/4 Id
        /// </summary>
        public string HcaId { get; set; }

        /// <summary>
        /// LoggedIn User First name
        /// </summary>
        public string LoggedInFname { get; set; }

        /// <summary>
        /// LoggedIn User Last name
        /// </summary>
        public string LoggedInLname { get; set; }

        /// <summary>
        /// LoggedIn User full name
        /// </summary>
        public string LoggedInFullName { get; set; }


        /// <summary>
        /// LoggedIn User IpAddress
        /// </summary>
        public string LoggedInIp { get; set; }

        /// <summary>
        /// LoggedIn User Inititals
        /// </summary>
        public string Initials { get; set; }


    }
}
