using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using APIHypermediaPrototype;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    /// <summary>
    /// The account controller tests.
    /// </summary>
    [TestClass]
    public class AccountControllerTests
    {
        /// <summary>
        /// The get account_ should succeed.
        /// </summary>
        [TestMethod]
        public void GetAccount_ShouldSucceed()
        {
            string baseAddress = "http://dummyname/";

            // Server
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            var server = new HttpServer(config);

            // Client
            var messageInvoker = new HttpMessageInvoker(new InMemoryHttpContentSerializationHandler(server));

            // order to be created
            // Order requestOrder = new Order() { OrderId = "A101", OrderValue = 125.00, OrderedDate = DateTime.Now.ToUniversalTime(), ShippedDate = DateTime.Now.AddDays(2).ToUniversalTime() };
            var request = new HttpRequestMessage();

            // request.Content = new ObjectContent<Order>(requestOrder, new JsonMediaTypeFormatter());
            request.RequestUri = new Uri(baseAddress + "Account");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            request.Method = HttpMethod.Get;

            var cts = new CancellationTokenSource();

            using (HttpResponseMessage response = messageInvoker.SendAsync(request, cts.Token).Result)
            {
                var t = response.Content.ReadAsAsync<List<string>>().Result;
                Console.Write(t);

                // Assert.NotNull(response.Content);
                // Assert.NotNull(response.Content.Headers.ContentType);
                // Assert.Equal<string>("application/xml; charset=utf-8", response.Content.Headers.ContentType.ToString());
                // Assert.Equal<Order>(requestOrder, response.Content.ReadAsAsync<Order>().Result);
            }
        }
    }

    /// <summary>
    /// The in memory http content serialization handler.
    /// </summary>
    public class InMemoryHttpContentSerializationHandler : DelegatingHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryHttpContentSerializationHandler"/> class.
        /// </summary>
        public InMemoryHttpContentSerializationHandler()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryHttpContentSerializationHandler"/> class.
        /// </summary>
        /// <param name="innerHandler">
        /// The inner handler.
        /// </param>
        public InMemoryHttpContentSerializationHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        /// <summary>
        /// The send async.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            // Replace the original content with a StreamContent before the request
            // passes through upper layers in the stack
            request.Content = this.ConvertToStreamContent(request.Content);

            return base.SendAsync(request, cancellationToken).ContinueWith(
                responseTask =>
                    {
                        HttpResponseMessage response = responseTask.Result;

                        // Replace the original content with a StreamContent before the response
                        // passes through lower layers in the stack
                        response.Content = this.ConvertToStreamContent(response.Content);

                        return response;
                    });
        }

        /// <summary>
        /// The convert to stream content.
        /// </summary>
        /// <param name="originalContent">
        /// The original content.
        /// </param>
        /// <returns>
        /// The <see cref="StreamContent"/>.
        /// </returns>
        private StreamContent ConvertToStreamContent(HttpContent originalContent)
        {
            if (originalContent == null)
            {
                return null;
            }

            var streamContent = originalContent as StreamContent;

            if (streamContent != null)
            {
                return streamContent;
            }

            var ms = new MemoryStream();

            // **** NOTE: ideally you should NOT be doing calling Wait() as its going to block this thread ****
            // if the original content is an ObjectContent, then this particular CopyToAsync() call would cause the MediaTypeFormatters to 
            // take part in Serialization of the ObjectContent and the result of this serialization is stored in the provided target memory stream.
            originalContent.CopyToAsync(ms).Wait();

            // Reset the stream position back to 0 as in the previous CopyToAsync() call,
            // a formatter for example, could have made the position to be at the end after serialization
            ms.Position = 0;

            streamContent = new StreamContent(ms);

            // copy headers from the original content
            foreach (KeyValuePair<string, IEnumerable<string>> header in originalContent.Headers)
            {
                streamContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return streamContent;
        }
    }
}