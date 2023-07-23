namespace CitiesApp.Application.Commands
{
    public class CommandBase : ICommand
    {
        public Guid Id { get; set; }

        public CommandBase()
        {
            Id = Guid.NewGuid();
        }

        protected CommandBase(Guid id)
        {
            Id = id;
        }
    }

    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        public Guid Id { get; set; }

        protected CommandBase()
        {
            Id = Guid.NewGuid();
        }
        protected CommandBase(Guid id)
        {
            Id = id;
        }
    }
}
