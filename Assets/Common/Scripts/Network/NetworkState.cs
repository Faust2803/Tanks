namespace Common.Scripts.Network
{
    public enum NetworkState
    {
        Disconnected,
        Initializing,
        CreatingSession,
        JoiningSession,
        Connecting,
        Connected,
        Failed,
        Started
    }
}