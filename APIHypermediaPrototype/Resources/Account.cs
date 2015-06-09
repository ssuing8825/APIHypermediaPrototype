using System.Collections.Generic;

namespace APIHypermediaPrototype.Resources
{
    /// <summary>
    /// The account.
    /// </summary>
    public class Account : ResourceBase
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the account name.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the total amount due.
        /// </summary>
        public decimal TotalAmountDue { get; set; }

        /// <summary>
        /// Gets or sets the policy count.
        /// </summary>
        public int PolicyCount { get; set; }

        /// <summary>
        /// Gets or sets the policy.
        /// </summary>
        public object PolicyList { get; set; }
        
    }
}