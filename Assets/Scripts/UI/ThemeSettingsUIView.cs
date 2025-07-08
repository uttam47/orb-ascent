using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    public class ThemeSettingsUIView : UIView
    {

        private Button _lightThemeButton; 
        private Button _darkThemeButton;
        private UIEventChannel _uiEventchannel;
        private ThemeSettings _themeSettings; 

        public ThemeSettingsUIView(VisualElement visualElementRoot, ThemeSettings themeSettings) : base(visualElementRoot)
        {
            _themeSettings = themeSettings; 
            _lightThemeButton = root.Q<Button>("LightThemeButton"); 
            _darkThemeButton = root.Q<Button>("DarkThemeButton");
            _uiEventchannel = GameEventManager.GetEventChannel<UIEventChannel>();

            if(themeSettings.theme == Theme.Dark)
            {
                _darkThemeButton.AddToClassList("selectedItem"); 
            }
            else
            {
                _lightThemeButton.AddToClassList("selectedItem"); 
            }

            _lightThemeButton.clicked += OnLightThemeButtonClicked;
            _darkThemeButton.clicked += OnDarkThemeButtonnClicked ; 
        }

        private void OnDarkThemeButtonnClicked()
        {
            _darkThemeButton.AddToClassList("selectedItem");
            _lightThemeButton.RemoveFromClassList("selectedItem");
            _uiEventchannel.RaiseThemeChangeRequest(Theme.Dark);
        }

        private void OnLightThemeButtonClicked()
        {
            _darkThemeButton.RemoveFromClassList("selectedItem");
            _lightThemeButton.AddToClassList("selectedItem");
            _uiEventchannel.RaiseThemeChangeRequest(Theme.Light);
        }
    }
}
