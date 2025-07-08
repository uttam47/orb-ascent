using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    public class LevelWonScreen: CommonGameMenuButtonScreen
    {
        private Button _nextButton;
     
        protected override ScreenType screenType => ScreenType.LevelWonScreen;
     
        public override void Initialize()
        {
            base.Initialize(); 
            _nextButton = root.Q<Button>("NextButton"); 
            _nextButton.clicked += OnNextButtonClicked;
        }

        private void OnNextButtonClicked()
        {
            Hide();
            cloudCurtainCotnroller.HideCurtains(true); 
            levelEventChannel.RaiseLoadNextLevelRequest(); 
        }
    }
}
