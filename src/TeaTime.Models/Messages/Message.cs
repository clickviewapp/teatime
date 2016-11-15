namespace TeaTime.Models.WebHook
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class Message
    {
        public string HookUrl { get; set; }
        public string Text { get; set; }
        public string IconUrl { get; set; }
        public string UserName { get; set; }
        private string _channel;

        public string Channel
        {
            get { return _channel; }
            set { _channel = value.StartsWith("#") || value.StartsWith("@") ? value : "#" + value; }
        }

        public bool UnfurlLinks { get; set; }
        
        public List<Attachment> Attachments { get; set; }
    }
}