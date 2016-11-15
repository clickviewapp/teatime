namespace TeaTime.Models.WebHook
{
    using System.Collections.Generic;

    public class Attachment
    {
        public string Fallback { get; set; }
        public string Text { get; set; }
        public string PreText { get; set; }
        public string Color { get; set; }
        public List<AttachmentField> Fields { get; set; }

        public Attachment()
        {
            this.Fields = new List<AttachmentField>();
        }
    }
}