namespace TeaTime.Models.SlashCommands
{
    using System.Windows.Input;
    using Commands.Models;

    public class SlashCommandResponse : ICommandResponse
    {
        public string response_type { get; set; }
        public string Text { get; set; }

        public SlashCommandResponse(bool isPrivate = false)
        {
            this.response_type = isPrivate ? string.Empty : "in_channel";
        }

        public SlashCommandResponse(string text, bool isPrivate = false): this(isPrivate)
        {
            this.Text = text;
        }
    }

    //public class Attachment
    //{
    //    public string Text { get; set; }
    //    public string Fallback { get; set; }
    //    public string CallBackId { get; set; }
    //    public string Color { get; set; }
    //    public string AttachmentType { get; set; }
    //    public IEnumerable<Action> Actions { get; set; }

    //}

    //public class Action
    //{
    //    public string Name { get; set; }
    //    public string Text { get; set; }
    //    public string Type { get; set; }
    //    public string Value { get; set; }
    //    public string Style { get; set; }
    //    public Confirm Confirm { get; set; }
    //}

    //public class Confirm
    //{
    //    public string Title { get; set; }
    //    public string Text { get; set; }
    //    public string OkText { get; set; }
    //    public string DismissText { get; set; }
    //}
    
}
