using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using APIHypermediaPrototype.Model;
using APIHypermediaPrototype.Resources;

namespace APIHypermediaPrototype.Controllers
{
    [RoutePrefix("account/{accountid}/policy")]
    public class PolicyController : ApiController
    {
        [HttpGet]
        [Route("", Name = "GetPoliciesByAccountId")]
        public IHttpActionResult GetPolicyList(int AccountId)
        {
            //get the domain model
            var policyList = AccountRepository.GetPolicyList(AccountId);
            if (policyList.Count == 0)
            {
                return NotFound(); // Returns a NotFoundResult
            }
            //Map to resource
            var resource = PolicyMapper.MapToResource(policyList);

            //Add HyperMedia.
            foreach (var p in resource)
            {
                var pp = (Resources.Policy)p;
                p._self = new HrefLinkOnly(Url.Link("GetPolicyByAccountIdAndPolicyId", new { accountId = AccountId, policyid = pp.Id }));
            }
    
            return Ok(resource);  // Returns an OkNegotiatedContentResult
        }

        [HttpGet]
        [Route("{policyid}", Name = "GetPolicyByAccountIdAndPolicyId")]
        public IHttpActionResult GetPolicy(int AccountId, int PolicyId)
        {
            var policy = AccountRepository.GetPolicyById(PolicyId);

       
            if (policy == null)
            {
                return NotFound(); // Returns a NotFoundResult
            }
            var resource = PolicyMapper.MapToResource(policy);
            resource._self = new HrefLinkOnly(Url.Link("GetPolicyByAccountIdAndPolicyId", new { accountId = AccountId, policyid = PolicyId }));

            return Ok(resource);  // Returns an OkNegotiatedContentResult
       

        }

        public static class PolicyMapper
        {
            internal static List<ResourceBase> MapToResource(List<Model.Policy> policyList)
            {
                return policyList.Select(policy =>  MapToResource(policy)).Cast<ResourceBase>().ToList();
            }
            internal static Resources.Policy MapToResource(Model.Policy policy)
            {
                return  new Resources.Policy() { Id = policy.Id, AmountDue = policy.AmountDue, NextInvoiceDate = policy.NextInvoiceDate, PolicyNumber = policy.PolicyNumber };
            }
        }
    }
}
