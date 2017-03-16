using System;
using System.Threading.Tasks;
using Tollminder.Core.Models.PaymentData;

namespace Tollminder.Core.Services.ScanCrediCard
{
    public interface ICreditCardScanService
    {
        /// <summary>
        /// Scans the card info asynchronously.
        /// </summary>
        /// <returns>The scanned credit card's info.</returns>
        /// <param name="creditCardScanOptions"> Options for the scan screen. </param>
        Task<AddCreditCard> ScanCardInfoAsync(CreditCardScanOptions creditCardScanOptions = null);

        /// <summary>
        /// Scans the card info.
        /// </summary>
        /// <param name="callback">Callback called once the card has been scanned.</param>
        /// <param name="creditCardScanOptions"> Options for the scan screen.</param>
        void ScanCardInfo(Action<AddCreditCard> callback, CreditCardScanOptions creditCardScanOptions = null);
    }
}