namespace CryptExApi.Models
{
    /// <summary>
    /// Payment Status
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// The transaction has not been processed yet.
        /// </summary>
        NotProcessed = -1,

        /// <summary>
        /// The transaction failed or was canceled.
        /// </summary>
        Failed = 0,

        /// <summary>
        /// The transaction was successful.
        /// </summary>
        Success = 1,

        /// <summary>
        /// The transaction is pending. (Most commonly this would indicate that the transaction was made using a payment method that takes time to process, like a bank wire)
        /// </summary>
        Pending = 2
    }
}
