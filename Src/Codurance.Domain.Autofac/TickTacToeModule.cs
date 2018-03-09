using Autofac;

namespace Codurance.Domain.Autofac
{
    public class TickTacToeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TicTacToeGame>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<IBoard>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
