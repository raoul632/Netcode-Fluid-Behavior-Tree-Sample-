using CleverCrow.Fluid.Utilities;
using Unity.Netcode;

public class PlayersManager : NetworkSingleton<PlayersManager>
{
    private NetworkVariable<int> _playersInGame = new NetworkVariable<int>();

    public int PlayersInGame{
        get
        {
            return _playersInGame.Value; 
        }
    }


    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                Logger.Instance.LogInfo($"{id} just connected... "); 
                _playersInGame.Value++; 
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Logger.Instance.LogInfo($"{id} just disconnected... ");
                _playersInGame.Value--;
            }
        };


    }

   
}
