using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HCAaudit.Service.Portal.AuditUI.ViewModel.ModelValidator;
using Microsoft.VisualBasic;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel.Patient
{
    public class PatientViewModel
    {
        /// <summary>
        /// Patient FirstName
        /// </summary>
        /// 
        public Guid RequestId { get; set; }

        [Required]
        [RegularExpression("[A-Za-z0-9 .'&-]*", ErrorMessage = "Invalid First Name")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Patient MiddleName
        /// </summary>
        [RegularExpression("[A-Za-z0-9 .'&\0-]*", ErrorMessage = "Invalid Middle Name")]
        [MaxLength(50, ErrorMessage = "Middle Name cannot exceed 50 characters.")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Patient LastName
        /// </summary>
        [Required]
        [RegularExpression("[A-Za-z0-9 .'&-]*", ErrorMessage = "Invalid Last Name")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        /// <summary>
        /// Patient DateOfBirth
        /// </summary>
        [Required(ErrorMessage = "Invalid Date of Birth")]
        [DateLessThanCurrentDate]
        public DateTime DateOfBirth { get; set; }

        public string DateOfBirthForUi { get; set; }

        /// <summary>
        /// Patient Gender
        /// </summary>
        [Required(ErrorMessage = "Select Gender")]
        [RegularExpression("[A-Za-z]*", ErrorMessage = "Select Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Patient Address1
        /// </summary>
        [Required]
        [RegularExpression("[A-Za-z0-9,.# -]*", ErrorMessage = "Invalid Address")]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        public string Address1 { get; set; }

        /// <summary>
        /// Patient Address2
        /// </summary>
        [RegularExpression("[A-Za-z0-9,.#\0 -]*", ErrorMessage = "Invalid Address")]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        public string Address2 { get; set; }

        /// <summary>
        /// Patient City
        /// </summary>
        [Required]
        [RegularExpression("[A-Za-z0-9 -]*", ErrorMessage = "Invalid City")]
        [MaxLength(50, ErrorMessage = "City Name cannot exceed 50 characters.")]
        public string City { get; set; }

        /// <summary>
        /// Patient State
        /// </summary>
        /// <summary>
        /// Patient City
        /// </summary>
        [Required(ErrorMessage = "Select State")]
        [RegularExpression("[A-Za-z]*", ErrorMessage = "Invalid State")]
        public string State { get; set; }

        /// <summary>
        /// Patient ZipCode
        /// </summary>
        [Required]
        [RegularExpression("[A-Za-z0-9-]*", ErrorMessage = "Invalid Zip Code")]
        [MaxLength(12, ErrorMessage = "Zip code cannot exceed 12 characters.")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Patient PhoneNumber
        /// </summary>
        [Required]
        [RegularExpression("[0-9(). -]*", ErrorMessage = "Invalid Phone Number")]
        [MaxLength(14, ErrorMessage = "Invalid Phone Number")]
        [LengthValidator]
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

        [Required(ErrorMessage = "Invalid Record Start Date")]
        [DateLessThanCurrentDate]
        [DateTimeValidatorForStartDate]
        public DateTime RecordStartDate { get; set; }

        public string RecordStartDateForUi { get; set; }

        /// <summary>
        /// Record EndDate
        /// </summary>
        [Required(ErrorMessage = "Invalid Record End Date")]
        [DateTimeNotLessThan("RecordStartDate")]
        [DateLessThanCurrentDate]
        public DateTime RecordEndDate { get; set; }

        public string RecordEndDateForUi { get; set; }

        /// <summary>
        /// DocumentType
        /// </summary>
        [Required(ErrorMessage = "Select Document Type")]
        public string RecordFormat { get; set; }

        /// <summary>
        /// Npi
        /// </summary>
        [RegularExpression("[0-9]*", ErrorMessage = "Invalid NPI")]
        [LengthValidator]
        [CompareValidator("DirectAddress")]
        public string Npi { get; set; }

        /// <summary>
        /// Direct Address
        /// </summary>
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid Direct Address")]
        [MaxLength(365, ErrorMessage = "DirectAddress cannot exceed 200 characters.")]
        public string DirectAddress { get; set; }

        /// <summary>
        /// Verify Direct Address
        /// </summary>
        [Compare("DirectAddress", ErrorMessage = "Direct Address and Confirm Direct Address should be same")]
        [MaxLength(365, ErrorMessage = "DirectAddress cannot exceed 200 characters.")]
        public string VerifyDirectAddress { get; set; }

        /// <summary>
        /// RequestorType
        /// </summary>
        public string RequestorType { get; set; }

        /// <summary>
        /// Requestor Full Name
        /// </summary>
        [Required]
        [RegularExpression("[A-Za-z0-9 .'&-]*", ErrorMessage = "Invalid Requestor Name")]
        [MaxLength(100, ErrorMessage = "Requestor name cannot exceed 100 characters.")]
        public string RequestorFullName { get; set; }

        /// <summary>
        /// Requestor Organization
        /// </summary>
        [Required]
        [RegularExpression("[A-Za-z0-9 .'&-]*", ErrorMessage = "Invalid Organization Name")]
        [MaxLength(200, ErrorMessage = "Organization name cannot exceed 200 characters.")]
        public string RequestorOrganization { get; set; }

        public bool IsValidRecord { get; set; }
    }
}
