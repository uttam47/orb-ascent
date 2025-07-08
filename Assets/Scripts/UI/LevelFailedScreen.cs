
using System;
using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    public class LevelFailedScreen: CommonGameMenuButtonScreen
    {
        private Button _settingsButton;

        protected override ScreenType screenType => ScreenType.LevelFailedScreen;
     
        public override void Initialize()
        {
            base.Initialize();
            replayButton.text = "Retry";
            _settingsButton = root.Q<Button>("SettingsButton");
            _settingsButton.clicked += OnSettingsButtonClicked;
        }

        private void OnSettingsButtonClicked()
        {
            uiEventChannel.RaisePushUIScreen(ScreenType.SettingsScreen);
        }
    }
}
