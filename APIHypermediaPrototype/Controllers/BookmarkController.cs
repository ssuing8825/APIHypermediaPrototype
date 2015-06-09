using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using APIHypermediaPrototype.Resources;

namespace APIHypermediaPrototype.Controllers
{
    public class BookmarkController : ApiController
    {
        public IHttpActionResult Get()
        {
            var href = new HrefLinkOnly(this.Url.Link("GetAccount", new {policyNumber = @"PolicyNumberToSearch" }));

            return this.Ok(href);
        }

    }
}
