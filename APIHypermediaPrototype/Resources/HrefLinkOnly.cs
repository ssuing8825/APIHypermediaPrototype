using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIHypermediaPrototype.Resources
{
    public class HrefLinkOnly
    {
        public HrefLinkOnly(string link)
        {
            this.Href = link;
        }

        public string Href { get; set; }
    }
}