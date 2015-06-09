using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using APIHypermediaPrototype.Model;
using APIHypermediaPrototype.Resources;

using Account = APIHypermediaPrototype.Resources.Account;

namespace APIHypermediaPrototype.Controllers
{
    /// <summary>
    /// The account controller.
    /// </summary>
    [RoutePrefix("account")]
    public class AccountController : ApiControllerBase
    {
        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="IHttpActionResult"/>.
        /// </returns>
        [HttpGet]
        [Route("", Name = "GetAccount")]
        public IHttpActionResult Get([FromUri] string policynumber, [FromUri] string expand = "None")
        {

            if (string.IsNullOrEmpty(policynumber))
            {
                return this.NotFound();
            }

            var accounts = AccountRepository.GetAccountForPolicyNumber(policynumber);
            if (accounts == null)
            {
                return this.NotFound();
            }

            var resource = AccountMapper.MapToResource(accounts);
            resource._self = new HrefLinkOnly(this.Url.Link("GetAccountById", new { accountid = resource.Id }));

            if (expand == "Policy")
            {
                // Expand some resources
                resource.PolicyList = this.GetPolicyList(resource.Id);
            }
            else
            {
                //Just Hypermedia
                //   resource.PolicyList = this.GetPolicyListHypermedia(accountId);
                resource.PolicyList =
                    new HrefLinkOnly(this.Url.Link("GetPoliciesByAccountId", new { AccountId = resource.Id }));
            }


            return this.Ok(resource);
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="AccountId">The account id.</param>
        /// <param name="expand">The expand.</param>
        /// <returns>
        /// The <see cref="IHttpActionResult" />.
        /// </returns>
        [HttpGet]
        [Route("{accountid}", Name = "GetAccountById")]
        public IHttpActionResult Get(int accountId, [FromUri] string expand = "None")
        {
            // Get it from the Domain Model.
            var account = AccountRepository.GetAccount(accountId);
            if (account == null)
            {
                return this.NotFound(); // Returns a NotFoundResult
            }

            // Map to a resource
            var resource = AccountMapper.MapToResource(account);

            // Add Self
            // The self may need to be set on an action filter.
            resource._self = new HrefLinkOnly(this.Url.Link("GetAccountById", new { AccountId = resource.Id }));


            if (expand == "Policy")
            {
                // Expand some resources
                resource.PolicyList = this.GetPolicyList(accountId);
            }
            else
            {
                //Just Hypermedia
                //   resource.PolicyList = this.GetPolicyListHypermedia(accountId);
                resource.PolicyList =
                    new HrefLinkOnly(this.Url.Link("GetPoliciesByAccountId", new { AccountId = resource.Id }));
            }




            ;

            return this.Ok(resource); // Returns an OkNegotiatedContentResult
        }

        // POST api/values
        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public void Delete(int id)
        {
        }

        /// <summary>
        /// The get policy list.
        /// </summary>
        /// <param name="AccountId">
        /// The account id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<ResourceBase> GetPolicyList(int AccountId)
        {
            // get the domain model
            var policyList = AccountRepository.GetPolicyList(AccountId);

            // Map to resource
            var resource = PolicyController.PolicyMapper.MapToResource(policyList);

            // Add HyperMedia.
            foreach (var p in resource)
            {
                p._self = new HrefLinkOnly(this.Url.Link("GetPolicyByAccountIdAndPolicyId", new { AccountId, PolicyId = ((Resources.Policy)p).Id }));
            }

            return resource;
        }
    }

    /// <summary>
    /// The account mapper.
    /// </summary>
    public static class AccountMapper
    {
        internal static List<Account> MapToResource(List<Model.Account> accountList)
        {

            return accountList.Select(account => new Resources.Account() { Id = account.Id, AccountName = account.AccountName, TotalAmountDue = account.PolicyList.Sum(c => c.AmountDue) }).ToList();
        }

        /// <summary>
        /// The map to resource.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <returns>
        /// The <see cref="Account"/>.
        /// </returns>
        internal static Account MapToResource(Model.Account account)
        {
            var accountResource = new Account
                                      {
                                          AccountName = account.AccountName,
                                          Id = account.Id,
                                          PolicyCount = account.PolicyList.Count,
                                          TotalAmountDue = account.PolicyList.Sum(c => c.AmountDue)
                                      };

            return accountResource;
        }
    }
}