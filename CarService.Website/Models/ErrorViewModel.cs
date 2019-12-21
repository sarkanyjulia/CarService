using System;

namespace CarService.Website.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrorMessage { get; set; }
    }
}