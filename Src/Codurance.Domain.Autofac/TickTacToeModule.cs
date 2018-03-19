using Autofac;

namespace Codurance.Domain.Autofac
{
    public class TickTacToeModule : Module
    {
        private readonly Team playerOneTeam;

        public TickTacToeModule(Team playerOneTeam)
        {
            this.playerOneTeam = playerOneTeam;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new Board(playerOneTeam))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicTacToeGame>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
