public class GameRoundListener
{
    public bool State;

    public delegate void Notify();
    public event Notify OnNotify;

    public void NotifyChecker()
    {
        State = true;
        OnNotify?.Invoke();
    }
}