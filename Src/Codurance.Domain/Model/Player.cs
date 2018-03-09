using System.Collections.Generic;

namespace Codurance.Domain
{
    public struct Player : IPlayer, IEqualityComparer<Player>
    {
        public Player(Team team, bool isActive, string name)
        {
            Team = team;
            IsActive = isActive;
            Name = name;
        }

        public Team Team { get; }
        public bool IsActive { get; }
        public string Name { get; }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }
            return this.Equals(this, (Player)obj);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(this);
        }

        public bool Equals(Player x, Player y)
        {
            return x.Team == y.Team &&
                x.IsActive == y.IsActive &&
                x.Name == y.Name;
        }

        public int GetHashCode(Player player)
        {
            return player.Team.GetHashCode() ^
                player.IsActive.GetHashCode() ^
                player.Name.GetHashCode();
        }
    }
}
