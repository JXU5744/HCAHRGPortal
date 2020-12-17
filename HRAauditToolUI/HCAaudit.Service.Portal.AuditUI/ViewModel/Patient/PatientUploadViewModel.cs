using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel.Patient
{
    public class PatientUploadViewModel
    {
        [Required]
        [RegularExpression("[A-Za-z0-9 .'&-]*", ErrorMessage = "Invalid Requestor Name")]
        [MaxLength(100, ErrorMessage = "Requestor name cannot exceed 100 characters.")]
        public string RequestorName { get; set; }

        [Required]
        [RegularExpression("[A-Za-z0-9 .'&-]*", ErrorMessage = "Invalid Organization Name")]
        [MaxLength(200, ErrorMessage = "Organization name cannot exceed 200 characters.")]
        public string Organization { get; set; }

        [Required]

        public IFormFile PatientExcelFile { get; set; }

        public List<PatientViewModel> PatientViewModelList { get; set; }
        
        public bool IsExcelUpload { get; set; }

        public int SelectedPatientIndex { get; set; }

        public bool HasErrorRecord { get; set; }

        public string Message { get; set; }
       
        public string HcaId { get; set; }

        public string LoggedInFname { get; set; }

        public string LoggedInLname { get; set; }

        public string LoggedInIp { get; set; }

        public PatientUploadViewModel()
        {
            PatientViewModelList = new List<PatientViewModel>();
        }
    }
}
