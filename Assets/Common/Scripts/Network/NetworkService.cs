using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

namespace Common.Scripts.Network
{
    public class NetworkService : INetworkService
    {
        public event Action<NetworkState> StateChanged;
        public event Action<int> ConnectedPlayersChanged;

        private ISession _session;
        private bool _initialized;
        private bool _matchStarted;

        public NetworkState State { get; private set; } = NetworkState.Disconnected;
        public bool MatchStarted => _matchStarted;
        public int MaxPlayers => _session?.MaxPlayers ?? 0;

        public async UniTask<string> CreateHostAsync(int maxPlayers, string mapId)
        {
            try
            {
                SetState(NetworkState.Initializing);
                await InitializeAsync();
                SetState(NetworkState.CreatingSession);
                var options = new SessionOptions
                {
                    MaxPlayers = maxPlayers,
                    SessionProperties = new Dictionary<string, SessionProperty>
                    {
                        {
                            "mapId",
                            new SessionProperty(
                                mapId,
                                VisibilityPropertyOptions.Public,
                                PropertyIndex.String1)
                        }
                    }
                }.WithRelayNetwork();
                _session = await MultiplayerService.Instance.CreateSessionAsync(options);
                _matchStarted = false;
                SetState(NetworkState.Connected);
                Debug.Log($"<color=green>[NetworkService] Host created. Join Code: {_session.Code}</color>");
                SubscribeNetworkEvents();
                
                return _session.Code;
            }
            catch (Exception e)
            {
                SetState(NetworkState.Failed);
                Debug.LogError($"[NetworkService] Exception: CreateHostAsync code: {e}");
                return null;
            }
        }

        public async UniTask<NetworkConnectionResult> JoinAsync(string joinCode)
        {
            var result = new NetworkConnectionResult
            {
                Success = false
            };

            try
            {
                if (string.IsNullOrWhiteSpace(joinCode))
                {
                    Debug.Log("<color=FF0000>[NetworkService] Join code is empty.</color>");
                    SetState(NetworkState.Failed);
                    return result;
                }

                SetState(NetworkState.Initializing);
                await InitializeAsync();
                SetState(NetworkState.JoiningSession);
                _session = await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode);
                SetState(NetworkState.Connected);
                Debug.Log($"<color=green>[NetworkService] Joined session: {_session.Id}</color>");
                result.Success = true;
                result.CountPlayer = _session.MaxPlayers;

                if (_session.Properties.TryGetValue("mapId", out var mapProperty))
                {
                    if (int.TryParse(mapProperty.Value, out var mapId))
                    {
                        result.MapId = mapId;
                    }
                }
                SubscribeNetworkEvents();
                return result;
            }
            catch (Exception e)
            {
                SetState(NetworkState.Failed);
                Debug.LogError($"[NetworkService] Exception: JoinAsync code: {e}");
                return result;
            }
        }

        public async UniTask<bool> QuickJoinAsync(int maxPlayers, string mapId)
        {
            try
            {
                SetState(NetworkState.Initializing);
                await InitializeAsync();
                SetState(NetworkState.JoiningSession);
                var quickJoinOptions = new QuickJoinOptions
                {
                    Timeout = TimeSpan.FromSeconds(10),
                    CreateSession = false,

                    Filters = new List<FilterOption>
                    {
                        new FilterOption(
                            FilterField.StringIndex1,
                            mapId,
                            FilterOperation.Equal)
                    }
                };

                var sessionOptions = new SessionOptions
                {
                    MaxPlayers = maxPlayers
                }.WithRelayNetwork();

                _session = await MultiplayerService.Instance.MatchmakeSessionAsync(quickJoinOptions, sessionOptions);
                _matchStarted = false;
                SetState(NetworkState.Connected);
                Debug.Log($"<color=green>[NetworkService] Quick Joined session: {_session.Id}</color>");
                SubscribeNetworkEvents();
                return true;
            }
            catch (Exception e)
            {
                SetState(NetworkState.Failed);
                Debug.LogError($"[NetworkService] Exception: QuickJoinAsync code: {e}");
                return false;
            }
        }

        /// <summary>
        /// Запускает матч и блокирует присоединение новых игроков.
        /// Вызывать только на Host.
        /// </summary>
        public async UniTask<bool> StartMatchAsync()
        {
            try
            {
                if (_session == null)
                {
                    Debug.LogError("[NetworkService] Cannot start match. Session is null.");
                    return false;
                }

                if (_matchStarted)
                {
                    Debug.LogWarning("[NetworkService] Match has already started.");
                    return false;
                }
                var hostSession = _session.AsHost();
                
                hostSession.IsLocked = true;
                _matchStarted = true;
                SetState(NetworkState.Started);
                Debug.Log("<color=orange>[NetworkService] Host started match . New players are no longer allowed.</color>");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[NetworkService] Exception: StartMatchAsync: {e}");
                return false;
            }
        }

        /// <summary>
        /// Открывает Session обратно для новых игроков.
        /// Вызывать только на Host.
        /// </summary>
        public async UniTask<bool> OpenMatchmakingAsync()
        {
            try
            {
                if (_session == null)
                {
                    Debug.LogError("[NetworkService] Cannot open matchmaking. Session is null.");
                    return false;
                }

                var hostSession = _session.AsHost();
                hostSession.IsLocked = false;
                _matchStarted = false;

                Debug.Log("<color=green>[NetworkService] Matchmaking opened. New players can join again.</color>");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[NetworkService] Exception: OpenMatchmakingAsync: {e}");
                return false;
            }
        }

        public async UniTask DisconnectAsync()
        {
            try
            {
                if (_session != null)
                {
                    await _session.LeaveAsync();
                    _session = null;
                }
                _matchStarted = false;
            }
            catch (Exception e)
            {
                Debug.LogError($"[NetworkService] Exception: DisconnectAsync code: {e}");
            }
            finally
            {
                UnsubscribeNetworkEvents();
                SetState(NetworkState.Disconnected);
            }
        }
        private async UniTask InitializeAsync()
        {
            if (_initialized) return;
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            _initialized = true;
            Debug.Log($"<color=green>[NetworkService] Unity Services initialized. Player ID: {AuthenticationService.Instance.PlayerId}</color>");
        }

        private void SetState(NetworkState state)
        {
            if (State == state) return;
            State = state;
            Debug.Log($"<color=green>[NetworkService] SetState: {State}</color>");
            StateChanged?.Invoke(State);
        }
        
        private void OnClientConnected(ulong clientId)
        {
            int count = NetworkManager.Singleton.ConnectedClients.Count;

            Debug.Log($"[NetworkService] Client connected: {clientId}. " + $"Players: {count}/{_session?.MaxPlayers}");

            ConnectedPlayersChanged?.Invoke(count);
        }

        private void OnClientDisconnected(ulong clientId)
        {
            int count = NetworkManager.Singleton.ConnectedClients.Count;

            Debug.Log($"[NetworkService] Client disconnected: {clientId}. " + $"Players: {count}/{_session?.MaxPlayers}");

            ConnectedPlayersChanged?.Invoke(count);
        }
        
        private void SubscribeNetworkEvents()
        {
            if (NetworkManager.Singleton == null)
                return;

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        private void UnsubscribeNetworkEvents()
        {
            if (NetworkManager.Singleton == null)
                return;

            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    public class NetworkConnectionResult
    {
        public bool Success;
        public int MapId;
        public int CountPlayer;
    }
}