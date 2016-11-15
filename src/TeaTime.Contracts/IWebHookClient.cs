using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaTime.Contracts
{
    using Models.WebHook;

    public interface IWebHookClient
    {
        bool SendMessage(Message message);
    }
}
