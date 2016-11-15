namespace TeaTime.Contracts
{
    using Models.WebHook;


    public interface IMessageService
    {
        bool SendMessage(Message message);
    }
}
