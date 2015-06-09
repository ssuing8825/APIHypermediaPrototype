using System.Collections.Generic;

namespace APIHypermediaPrototype.Model
{
    /// <summary>
    /// The account.
    /// </summary>
    public class Account 
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
        /// Gets or sets the policy.
        /// </summary>
        public List<Policy> PolicyList{ get; set; }
        
    }
}