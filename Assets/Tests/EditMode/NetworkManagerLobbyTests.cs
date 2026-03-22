using NUnit.Framework;
using SharedCounter.Network;

public class NetworkManagerLobbyTests
{
    [Test]
    public void IsNameValid_ReturnsFalse_WhenEmpty()
    {
        var manager = new NetworkManagerLobby();

        bool result = manager.IsNameValid(null, "");

        Assert.IsFalse(result);
    }

    [Test]
    public void IsNameValid_ReturnsFalse_WhenDuplicate()
    {
        var manager = new NetworkManagerLobby();

        var player1 = new NetworkRoomPlayerLobby();
        player1.DisplayName = "Alex";

        var player2 = new NetworkRoomPlayerLobby();
        player2.DisplayName = "Alex";

        manager.RoomPlayers.Add(player1);
        manager.RoomPlayers.Add(player2);

        bool result = manager.IsNameValid(player2, "Alex");

        Assert.IsFalse(result);
    }

    [Test]
    public void IsNameValid_ReturnsTrue_WhenUnique()
    {
        var manager = new NetworkManagerLobby();

        var player1 = new NetworkRoomPlayerLobby();
        player1.DisplayName = "Alex";

        manager.RoomPlayers.Add(player1);

        bool result = manager.IsNameValid(player1, "Bob");

        Assert.IsTrue(result);
    }
}