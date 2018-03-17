namespace Codurance.Domain
{
    public enum Team
    {
        None,
        Zero,
        Cross
    }

    public static class TeamExtensions
    {
        public static char AsChar(this Team source)
        {
            switch (source)
            {
                case Team.Cross: return 'X';
                case Team.Zero: return '0';
                default: return '-';
            }
        }
    }
}
