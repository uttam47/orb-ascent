using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    public class LevelPauseScreen: CommonGameMenuButtonScreen
    {
        private Button _settingsButton;

        protected override ScreenType screenType => ScreenType.LevelPauseScreen;
     
        public override void Initialize()
        {
            base.Initialize();
            _settingsButton = root.Q<Button>("SettingsButton");
            _settingsButton.clicked += OnSettingsButtonClicked;

            replayButton.clickable = null;
            replayButton.text = "Resume";
            replayButton.clicked += OnResumeGameButtonClicked; 
        }

        private void OnResumeGameButtonClicked()
        {
            uiEventChannel.RaisePopUIScreen();  
        }

        private void OnSettingsButtonClicked()
        {
            uiEventChannel.RaisePushUIScreen(ScreenType.SettingsScreen); 
        }
    }
}
