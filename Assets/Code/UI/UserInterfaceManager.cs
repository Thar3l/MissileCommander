using GameUtils;
using UI.Windows;
using UnityEngine;

namespace UI
{
    public class UserInterfaceManager : Singleton<UserInterfaceManager>
    {
        [SerializeField] private MainMenu _mainMenu;

        public bool Initialize()
        {
            GameManager.Instance.OnGameStop += _mainMenu.Show;
            return true;
        }
    }
}
