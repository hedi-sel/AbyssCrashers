using System.Collections.Generic;

public interface IEntityLayer
{
    protected List<PlayerControl> Players { get; }

    public IReadOnlyList<PlayerControl> GetPlayers => Players;
    void RegisterPlayer(PlayerControl player) => Players.Add(player);
    public void UnregisterPlayer(PlayerControl player) => Players.Remove(player);
}