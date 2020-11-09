using UnityEngine;
using MatchMakingCore;
using System.Text;

public class MatchmakingTest : MonoBehaviour
{
    private Container _container;
    private CreateJoinRequestSystem _createJoinRequestSystem;
    private CreateRoomSystem _createRoomSystem;
    private TurnSystem _turnSystem;
    private int _roomStartIndex;

    void Start()
    {
        _container = new Container();
        _container.InitComponentComparers();
        _container.InitMatchmakingCofig();
        _container.InitPlayerDataBase();

        _createJoinRequestSystem = new CreateJoinRequestSystem(_container.PlayerDatabaseSize);
        _turnSystem = new TurnSystem();
        _createRoomSystem = new CreateRoomSystem();

        _createJoinRequestSystem.Execute(_container);
        _roomStartIndex = 0;
    }

    public void OnNextTurnButtonClickd()
    {
        _createRoomSystem.Execute(_container);
        _turnSystem.Execute(_container);

        var builder = new StringBuilder();
        for(int i = _roomStartIndex; i < _container.RoomInfoComponentsCount; ++i)
        {
            if(_container.TryGetRoomInfoTeamAFromIndex(i, out int[] teamA) &&
                _container.TryGetRoomInfoTeamBFromIndex(i, out int[] teamB))
            {
                int entityId = _container.GetRoomInfoEntityId(i);
                builder.Append($"room {entityId} team A ");
                foreach(int key in teamA)
                {
                    PlayerData data = _container.GetPlayerData(key);
                    builder.Append($"{key} - {data.Name}; ");
                }

                builder.Append("team B ");
                foreach(int key in teamB)
                {
                    PlayerData data = _container.GetPlayerData(key);
                    builder.Append($"{key} - {data.Name}; ");
                }
            }
            Debug.Log(builder.ToString());
            builder.Clear();
        }

        _roomStartIndex = _container.RoomInfoComponentsCount;
    }
}
