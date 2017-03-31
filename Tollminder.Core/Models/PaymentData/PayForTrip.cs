using System;
namespace Tollminder.Core.Models.PaymentData
{
    public class PayForTrip
    {
        public string UserId { get; set; }
        public string PaymentProfileId { get; set; }
        public string Amount { get; set; }
    }
}
