namespace Codurance.Domain
{
    public interface IPlayer
    {
        bool IsActive { get; }
        Team Team { get; }
        string Name { get; }
    }
}