using System;
using Cysharp.Threading.Tasks;

namespace Common.Scripts.Network
{
    public interface INetworkService
    {
        NetworkState State { get; }
        event Action<NetworkState> StateChanged;
        UniTask<string> CreateHostAsync(int maxPlayers, string mapId);
        UniTask<NetworkConnectionResult> JoinAsync(string joinCode);
        UniTask<bool> QuickJoinAsync(int maxPlayers, string mapId);
        UniTask<bool> StartMatchAsync();
        UniTask<bool> OpenMatchmakingAsync();
        
        UniTask DisconnectAsync();
    }
}