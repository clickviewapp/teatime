using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaTime.ApiClients
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Contracts;
    using Models.WebHook;

    public class SlackWebHookClient : BaseApiClient, IWebHookClient
    {
        private static readonly Dictionary<string, string> _escapeDictionary = new Dictionary<string, string>
        {
            {"&", "&amp;"},
            {"<", "&lt;"},
            {">", "&gt;"}
        };

        public SlackWebHookClient(string endpoint) : base(endpoint) { }

        public bool SendMessage(Message message)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, string.Empty)
            {
                Content = new StringContent(EscapeSlackMessage(message), Encoding.UTF8, "application/json")
            };

            return this.Send(request).StatusCode == HttpStatusCode.OK;
        }

        private static string EscapeSlackMessage(Message message)
        {
            return _escapeDictionary.Aggregate(message.Text, (current, escpaedChar) => current.Replace(escpaedChar.Key, escpaedChar.Value));
        }
    }
}
