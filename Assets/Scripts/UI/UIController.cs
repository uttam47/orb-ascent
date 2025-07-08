using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class UIController : MonoBehaviour
    {
        private UIEventChannel _uiEventChannel;
        private Dictionary<ScreenType, UIScreen> _uiScreens;
        private Stack<UIScreen> _uiScreenStack;

        private void Awake()
        {
            _uiScreens = new Dictionary<ScreenType, UIScreen>();
            _uiScreenStack = new Stack<UIScreen>();
            List<UIScreen> uiScreens = GetComponentsInChildren<UIScreen>().ToList();

            foreach (UIScreen screen in uiScreens)
            {
                _uiScreens[screen.GetScreenTpye()] = screen;
            }

            _uiEventChannel = GameEventManager.GetEventChannel<UIEventChannel>();
        }

        private void OnEnable()
        {
            _uiEventChannel.OnPushUIScreen += OnPushUIScreen;
            _uiEventChannel.OnPopUIScreen += OnPopUIScreen;
        }

        private void OnDisable()
        {
            _uiEventChannel.OnPushUIScreen -= OnPushUIScreen;
            _uiEventChannel.OnPopUIScreen -= OnPopUIScreen;
        }

        private void OnPushUIScreen(ScreenType screenType)
        {
            if(_uiScreenStack.Count>0)
            {
                UIScreen uIScreen = _uiScreenStack.Peek();
                if(screenType == uIScreen.GetScreenTpye())
                {
                    return; 
                }
                uIScreen?.Hide(() => { ShowScreen(screenType); }); 
            }
            else
            {
                ShowScreen(screenType); 
            }   
        }

        private void ShowScreen(ScreenType screenType)
        {
            UIScreen uiScreen = _uiScreens[screenType];
            uiScreen.Show();
            _uiScreenStack.Push(uiScreen);
        }

        private void OnPopUIScreen()
        {
            if (_uiScreenStack.Count > 0)
            {
                UIScreen uiScreen = _uiScreenStack.Peek();
                _uiScreenStack.Pop();
                uiScreen.Hide(() => {
                    if (_uiScreenStack.Count > 0)
                    {
                        _uiScreenStack.Peek().Show();
                    }
                });
            }
        }
    }

}