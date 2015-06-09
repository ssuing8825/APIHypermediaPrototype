using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace APIHypermediaPrototype.Model
{
    public class Policy 
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the account name.
        /// </summary>
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Gets or sets the amount due.
        /// </summary>
        /// <value>
        /// The amount due.
        /// </value>
        public decimal AmountDue { get; set; }

        /// <summary>
        /// Gets or sets the next invoice date.
        /// </summary>
        /// <value>
        /// The next invoice date.
        /// </value>
        public DateTime NextInvoiceDate{ get; set; }

    }
}