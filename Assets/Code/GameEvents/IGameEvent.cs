namespace GameEvents
{
    public interface IGameEvent
    {
        void Execute();
        bool IsRunning();
    }
}
