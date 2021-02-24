using System;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class UserDetails
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public Guid RequestId { get; set; }
        /// <summary>
        /// Patient FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Patient MiddleName
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Patient LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Patient DateOfBirth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Patient Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Patient Address1
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Patient Address2
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Patient City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Patient State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Patient ZipCode
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Patient Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Patient PhoneNumber
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// SourceMemIdList
        /// </summary>
        public string SourceMemIdList { get; set; }

        /// <summary>
        /// HasValidScore
        /// </summary>
        public bool HasValidScore { get; set; }

        /// <summary>
        /// Record StartDate
        /// </summary>
        public DateTime RecordStartDate { get; set; }

        /// <summary>
        /// Record EndDate
        /// </summary>
        public DateTime RecordEndDate { get; set; }

        /// <summary>
        /// DocumentType
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// StatusDescription
        /// </summary>
        public string StatusDescription { get; set; }

    }
}
