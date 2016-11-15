namespace TeaTime.Commands.Models
{
    public class StringCommandResponse : ICommandResponse
    {
        private readonly string _message;

        public StringCommandResponse(string message)
        {
            _message = message;
        }
    }
}
