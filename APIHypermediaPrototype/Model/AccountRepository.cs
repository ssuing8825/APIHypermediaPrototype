using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using APIHypermediaPrototype.Resources;

namespace APIHypermediaPrototype.Model
{

    public static class AccountRepository
    {
        private static List<Account> accounts;
        private static List<Policy> policies;

        public static List<Account> GetAccounts()
        {
            return accounts;
        }
        public static List<Policy> GetPolicyList(int accountid)
        {
            return accounts.FirstOrDefault(c => c.Id == accountid).PolicyList;
        }
        public static Policy GetPolicy(int accountid, int policyid)
        {
            return accounts.FirstOrDefault(c => c.Id == accountid).PolicyList.FirstOrDefault(c => c.Id == policyid);
        }
        public static Account GetAccount(int accountid)
        {
            return accounts.FirstOrDefault(c => c.Id == accountid);
        }
        static AccountRepository()
        {



            policies = new List<Policy>
                           {
                               new Policy
                                             {
                                                 AmountDue = 123.23m,
                                                 Id = 3,
                                                 NextInvoiceDate = DateTime.Now,
                                                 PolicyNumber = "333333"
                                             },
                                              new Policy
                                             {
                                                 AmountDue = 123.23m,
                                                 Id = 4,
                                                 NextInvoiceDate = DateTime.Now,
                                                 PolicyNumber = "444444"
                                             } ,
                                             new Policy
                                             {
                                                 AmountDue = 123.23m,
                                                 Id = 1,
                                                 NextInvoiceDate = DateTime.Now,
                                                 PolicyNumber = "123456"
                                             },
                                              new Policy
                                             {
                                                 AmountDue = 123.23m,
                                                 Id = 2,
                                                 NextInvoiceDate = DateTime.Now,
                                                 PolicyNumber = "222222"
                                             }
                           };

            accounts = new List<Account>
                           {
                               new Account()
                                   {
                                       Id = 1,
                                       AccountName = "SteveAccount",
                                       TotalAmountDue = 233.43m,
                                       PolicyList = new List<Policy> { policies[0],   policies[1] }
                                   },
                               new Account()
                                   {
                                       Id = 2,
                                       AccountName = "TimAccount",
                                       TotalAmountDue = 233.43m,
                                        PolicyList = new List<Policy> { policies[2],   policies[3]}
                           }};
        }

        internal static Account GetAccountForPolicyNumber(string policynumber)
        {
            return accounts.FirstOrDefault(c => c.PolicyList.Any(q => q.PolicyNumber == policynumber));
        }
        internal static Policy GetPolicyById(int policyId)
        {
            return policies.FirstOrDefault(c => c.Id == policyId);
        }
    }
}