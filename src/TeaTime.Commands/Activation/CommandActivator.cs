namespace TeaTime.Commands.Activation
{
    using System;

    public interface ICommandActivator
    {
        object Create(Type type);
    }
}
