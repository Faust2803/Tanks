
using System.Collections.Generic;
using Common.Scripts.Network;
using Managers;
using SO.Scripts;
using Tanks.Common.Scripts.Player;
using Tanks.Game.Scripts;
using Tanks.NetGame.Scripts;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class NetGameScenesManager : NetworkBehaviour
{
    private UiManager _uiManager;
    private NetworkService _networkService;
    private  LevelConfig _levelConfig;
    // [Inject] protected  ColorsConfig _colorsConfig;
     [Inject] protected  TanksConfig _tankConfig;
    private  PlayerManager _playerManager;
    
    private DiContainer _container;
    private List<Transform> _spawnPoints;
    
    public NetworkVariable<bool> GameStarted = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        GameStarted.OnValueChanged += OnGameStartedChanged;

        // На случай, если значение уже true к моменту Spawn
        OnGameStartedChanged(false, GameStarted.Value);
        _networkService.StateChanged += OnNetworkService;
    }

    public override void OnNetworkDespawn()
    {
        GameStarted.OnValueChanged -= OnGameStartedChanged;
        _networkService.StateChanged -= OnNetworkService;
    }

    [Inject]
    public void Construct(DiContainer container,
        NetworkService networkService,
        LevelConfig levelConfig,
        PlayerManager playerManager,
        UiManager uiManager)
    {
        _container = container;
        _networkService = networkService;
        _levelConfig = levelConfig;
        _playerManager = playerManager;
        _uiManager = uiManager;
    }

    private void OnNetworkService(NetworkState obj)
    {
        switch (obj)
        {
            case NetworkState.Disconnected:
                break;
            case NetworkState.Connected:
                var levelMap = _levelConfig.GetLevel(_playerManager.CurrentMapId).LevelPrefab;
                _spawnPoints = Instantiate(levelMap).GetComponent<LevelMap>().SpawnPoints;


                
                
                break;
            case NetworkState.Started:
                GameStarted.Value = true;
                break;
            case NetworkState.Initializing:
            case NetworkState.CreatingSession:
            case NetworkState.JoiningSession:
            case NetworkState.Connecting:
            case NetworkState.Failed:
            default:
                
                break;
            //throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
        }
       
    }
    
    private void OnGameStartedChanged(bool previousValue, bool newValue)
    {
        if (newValue)
        {
            PlaceAllPlayers();
            
            Debug.Log("<color=orange>[NetGameScenesManager] Host started match . All player must be started.</color>");
            _uiManager.CloseAllWindows();
        }
    }
    
    private void PlaceAllPlayers()
    {
        var clients = NetworkManager.Singleton.ConnectedClientsList;
        for (var i = 0; i < clients.Count; i++)
        {
            var playerObject = clients[i].PlayerObject;
            if (playerObject == null)
            {
                Debug.LogWarning($"[NetGameScenesManager] PlayerObject для ClientId {clients[i].ClientId} ещё не создан.");
                continue;
            } 
            if (i >= _spawnPoints.Count)
            {
                Debug.LogWarning("[NetGameScenesManager ]Не хватает SpawnPoints для всех игроков.");
                break;
            }
            
            playerObject.TryGetComponent(out NetPlayer netPlayer);
            if (netPlayer != null)
            {
               
                Debug.Log($"<color=blue>[NetGameScenesManager] Player: {clients[i].ClientId} </color>");
                if (!IsOwner)
                {
                    _container.InjectGameObject(playerObject.gameObject);
                }
                netPlayer.PrepareForGame();
            }
            else
            {
                Debug.Log($"<color=FF0000>[NetGameScenesManager] NetPlayer error not geted component Player {clients[i].ClientId}</color>");
            }
            
            if (playerObject == NetworkManager.Singleton.LocalClient.PlayerObject)
            {
                var spawnPoint = _spawnPoints[i];
                playerObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
                Debug.Log($"[NetGameScenesManager] Player {clients[i].ClientId} -> {spawnPoint.name}");
            }
        }
    }
    
}
