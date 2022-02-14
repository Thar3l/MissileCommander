namespace UI.Windows
{
    public class MainMenu : BasicWindow
    {
        public void StartGame()
        {
            GameManager.Instance.StartGame();
            Hide();
        }
    }
}
