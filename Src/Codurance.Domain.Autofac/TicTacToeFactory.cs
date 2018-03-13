using Autofac;

namespace Codurance.Domain.Autofac
{
    /// <remarks>
    /// The reason this exists, is to cater for WPF as there is no way
    /// to bootstrap IoC frameworks, so the factory method solves the 
    /// creation issue nicely
    /// </remarks>
    public class TicTacToeFactory
    {
        public static ITicTacToeGame Create(Team playerOneTeam)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TickTacToeModule(playerOneTeam));
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                return scope.Resolve<ITicTacToeGame>();
            }
        }
    }
}
