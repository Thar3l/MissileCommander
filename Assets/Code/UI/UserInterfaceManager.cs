using GameUtils;
using UI.Windows;
using UnityEngine;

namespace UI
{
    public class UserInterfaceManager : Singleton<UserInterfaceManager>
    {
        [SerializeField] private MainMenu mainMenu;

        public bool Initialize()
        {
            GameManager.Instance.OnGameStop += mainMenu.Show;
            return true;
        }
    }
}
