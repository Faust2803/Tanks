using System;
using Common.Scripts.Network;
using Cysharp.Threading.Tasks;
using Managers;
using Tanks.Common.Scripts.Player;
using Unity.Netcode;
using UnityEngine;
using Util;
using Utils;
using Zenject;

namespace UI.Windows
{
    public class ConnectionWindowMediator :BaseWindowMediator <ConnectionWindowView, UIData>
    {
        
        private PlayerManager _playerManager;
        private SceneLoader _sceneLoader;
        private NetworkService _networkService;

        private string _connectionCode;
        
        [Inject]
        private void Construct(PlayerManager playerManager, SceneLoader sceneLoader, NetworkService networkService)
        {
            _playerManager = playerManager;
            _sceneLoader = sceneLoader; 
            _networkService = networkService;
        }

        protected override void ShowStart()
        {
            base.ShowStart();
            for (var i = 0; i < Target.TabToggle.Count; i++)
            {
                Target.TabToggle[i].onValueChanged.AddListener(OnToggleClicked);
            }
            Target.TabToggle[0].isOn = true;
            OnToggleClicked(true);
            
            var model = Target.ItemsLoader.LoadCurrent();

            var _rotator = new CharacterRotator(model, 25);
            Target.Rotator = _rotator;
            _rotator.StartRotate();
            
            Target.Play.onClick.AddListener(OnPlayClicked);
            Target.OkButtonConnect.onClick.AddListener(StartGame);
            Target.CancelButtonConnect.onClick.AddListener(DisconnectGame);
            ShowStartPanel();

        }

        private async void OnPlayClicked()
        {
            for (var i = 0; i < Target.TabToggle.Count; i++)
            {
                if (Target.TabToggle[i].isOn == true)
                {
                    switch (i)
                    {
                        case 0:
                            Debug.Log($"<color=green>[ConnectionWindowMediator] Start single player</color>");
                            SaveDataForGame(Target.BotsOptions.MapId, (int)Target.BotsOptions.CountPlayer);
                            OpenConnectPanel();
                            var resultSingle = await _sceneLoader.LoadScene(SceneName.Game.ToString(),
                                new Progress<float>(value =>
                                {
                                    Debug.Log($"Loading: {value:P0}");
                                })
                            );
                            
                            if (resultSingle)
                            {
                                StartWidthBots();
                                CloseSelf();
                                _uiManager.CloseAllPanels();
                            }
                            
                            break;
                        case 1:
                            Debug.Log($"<color=green>[ConnectionWindowMediator] Start multiplayer random player</color>");
                            SaveDataForGame(Target.RandomOptions.MapId, (int)Target.RandomOptions.CountPlayer);
                            OpenConnectPanel();
                            
                            var result = await _sceneLoader.LoadScene(SceneName.NetGame.ToString(),
                                new Progress<float>(value =>
                                {
                                    Debug.Log($"Loading: {value:P0}");
                                })
                            );
                            
                            if (result)
                            {
                                ConnectedToServer((int)Target.RandomOptions.CountPlayer+1 , Target.RandomOptions.MapId).Forget();
                            }
                            break;
                        case 2:

                            if (Target.CommandOptions.TypeMyltyplayer != 0 &&
                                Target.CommandOptions.FieldForCode == string.Empty)
                            {
                                return;
                            }
                            Debug.Log($"<color=green>[ConnectionWindowMediator] Start multiplayer commands player</color>");
                            SaveDataForGame(Target.RandomOptions.MapId, (int)Target.RandomOptions.CountPlayer);
                            OpenConnectPanel();
                            
                            var resultCommand = await _sceneLoader.LoadScene(SceneName.NetGame.ToString(),
                                new Progress<float>(value =>
                                {
                                    Debug.Log($"Loading: {value:P0}");
                                })
                            );
                            
                            if (resultCommand)
                            {
                                if (Target.CommandOptions.TypeMyltyplayer == 0)
                                {
                                    StartHost((int)Target.RandomOptions.CountPlayer , Target.RandomOptions.MapId).Forget();
                                }
                                else
                                {
                                    ConnectedToServer(Target.CommandOptions.FieldForCode);
                                }
                            }
                            break;
                        default:
                            Debug.Log($"<color=FF0000> [ConnectionWindowMediator] Error game type</color>");
                            break;
                    }
                }
            }
        }

        private void StartWidthBots()
        {
            Debug.Log($"<color=red>[ConnectionWindowMediator] NOT IMPLEMENTED !!!!! </color>");
        }

        

        private void SaveDataForGame(int mapId, int playerCount)
        {
            _playerManager.CurrentMapId =  mapId;
            _playerManager.CurrentPlayerCount = playerCount;
        }

        private async UniTask ConnectedToServer(int playerCount, int mapId)
        {
            var result = await _networkService.QuickJoinAsync(playerCount, mapId.ToString());
            Target.DescriptionTextConnect.text  = "We're looking for servers for the game";
            
            if (result)
            {
                Target.DescriptionTextConnect.text  = $"Connection successful. The match will start soon\n" +
                                                      $"Player connected: {NetworkManager.Singleton.ConnectedClients.Count}/{_networkService.MaxPlayers}";
                Debug.Log($"<color=green>[ConnectionWindowMediator] Connected to host compleated</color>");
                _uiManager.CloseAllPanels();
                SaveDataForGame(mapId, playerCount);
                
                Target.OkButtonConnect.gameObject.SetActive(false);
                Target.CancelButtonConnect.gameObject.SetActive(false);
                
                _networkService.ConnectedPlayersChanged += OnChangeClientCount;
                HideStartPanel();
            }
            else
            {
                StartHost(playerCount, mapId).Forget();
            }
        }
        
        private  void HideStartPanel()
        {
            Target.PanelConnect.SetActive(true);
            Target.PanelStart.SetActive(false);
        }
        
        private  void ShowStartPanel()
        {
            Target.PanelStart.SetActive(true);
            Target.PanelConnect.SetActive(false);
        }
        
        private void OpenConnectPanel()
        {
            Target.TitleTextConnect.text = "Game creation";
            Target.DescriptionTextConnect.text  = "Starting new game";
            Target.OkButtonConnect.gameObject.SetActive(false);
            Target.CancelButtonConnect.gameObject.SetActive(false);
            Target.PanelConnect.SetActive(true);
        }
        
        private async UniTask ConnectedToServer(string code)
        {
            var result = await _networkService.JoinAsync(code);
            Target.DescriptionTextConnect.text  = "We're looking for servers for the game";
            
            if (result.Success)
            {
                Target.DescriptionTextConnect.text  = $"Connection successful. The match will start soon\n" +
                                                      $"Player connected: {NetworkManager.Singleton.ConnectedClients.Count}/{_networkService.MaxPlayers}";
                Debug.Log($"<color=green>[ConnectionWindowMediator] Connected to host compleated</color>");
                SaveDataForGame(result.MapId, result.CountPlayer);
                Target.OkButtonConnect.gameObject.SetActive(false);
                Target.CancelButtonConnect.gameObject.SetActive(false);
                _networkService.ConnectedPlayersChanged += OnChangeClientCount;
                _uiManager.CloseAllPanels();
                HideStartPanel();
            }
            else
            {
                ErrorConnection();
            }
        }
        
        private async UniTask StartHost(int playerCount, int mapId)
        {
            var result = await _networkService.CreateHostAsync(playerCount, mapId.ToString());
            if (result != null)
            { 
                _connectionCode = result;
                Target.DescriptionTextConnect.text  = $"We create a new server and wait for other players to connect.\n" +
                                                      $"You can share this code with your friends so they can play with you.\n " +
                                                      $"<color=red>Connection code: {_connectionCode}</color>\n" +
                                                      $"Player connected: {NetworkManager.Singleton.ConnectedClients.Count}/{_networkService.MaxPlayers}";
                
                Target.OkButtonConnect.gameObject.SetActive(true);
                Target.CancelButtonConnect.gameObject.SetActive(true);

                SaveDataForGame(mapId, playerCount);
               
                _networkService.ConnectedPlayersChanged += OnChangeClientCount;
                Debug.Log($"<color=green>[ConnectionWindowMediator] Host started</color>");
                _uiManager.CloseAllPanels();
                HideStartPanel();
            }
            else
            {
                ErrorConnection();
            }
        }

        private void StartGame()
        {
            if (!_networkService.MatchStarted)
            {
                _networkService.StartMatchAsync();
            }
            CloseSelf();
        }
        
        private void DisconnectGame()
        {
            _networkService.DisconnectAsync();
            Target.TitleTextConnect.text = "Finish game";
            Target.DescriptionTextConnect.text  = "Game disconnected";
            Target.OkButtonConnect.gameObject.SetActive(false);
            Target.CancelButtonConnect.gameObject.SetActive(false);
            Debug.Log($"<color=green>[ConnectionWindowMediator] Game Disconnected</color>");
            _networkService.ConnectedPlayersChanged -= OnChangeClientCount;

            GoToLobby();
        }

        private void OnChangeClientCount(int count)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Target.DescriptionTextConnect.text  = $"We create a new server and wait for other players to connect.\n" +
                                                      $"You can share this code with your friends so they can play with you.\n " +
                                                      $"<color=red>Connection code: {_connectionCode}</color>\n" +
                                                      $"Player connected:{count}/{_networkService.MaxPlayers}";
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                Target.DescriptionTextConnect.text  = $"Connection successful. The match will start soon\n" +
                                                      $"Player connected:{count}/{_networkService.MaxPlayers}";
            }
        }

        private void ErrorConnection()
        {
            Target.TitleTextConnect.text = "Game error";
            Target.DescriptionTextConnect.text  = "Something went wrong, try again later";
            Debug.Log($"<color=FF0000>[ConnectionWindowMediator] Creation or connected game Error</color>");
            GoToLobby();
        }

        private async UniTask GoToLobby()
        {
            var load = await _sceneLoader.LoadScene(SceneName.Lobby.ToString(),
                new Progress<float>(value =>
                {
                    Debug.Log($"Loading: {value:P0}");
                })
            );
                
            if (load)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                CloseSelf();
            }
        }
        

        private void OnToggleClicked(bool isOn)
        {
            if (isOn)
            {
                for (var i = 0; i < Target.TabToggle.Count; i++)
                {
                    Target.Tabs[i].gameObject.SetActive(Target.TabToggle[i].isOn);
                }
            }
        }
        
        protected override void CloseStart()
        { 
            for (var i = 0; i < Target.TabToggle.Count; i++)
            {
                Target.TabToggle[i].onValueChanged.RemoveListener(OnToggleClicked);
            }
            Target.Play.onClick.RemoveListener(OnPlayClicked);
            Target.OkButtonConnect.onClick.AddListener(StartGame);
            Target.CancelButtonConnect.onClick.AddListener(DisconnectGame);
            base.CloseStart();
        }
    }
}