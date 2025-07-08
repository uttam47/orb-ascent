using UnityEngine;
using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    public abstract class CommonGameMenuButtonScreen : UIScreen
    {
        protected LevelEventChannel levelEventChannel;

        protected Button replayButton;
        protected Button menuButton;
        protected Button quitButton;
        protected CloudCurtainCotnroller cloudCurtainCotnroller;

        public override void Initialize()
        {
            levelEventChannel = GameEventManager.GetEventChannel<LevelEventChannel>();

            replayButton = root.Q<Button>("ReplayButton");
            quitButton = root.Q<Button>("QuitButton");
            menuButton = root.Q<Button>("MenuButton");

            if (replayButton != null)
            {
                replayButton.clicked += OnReplayButton;

            }
            quitButton.clicked += OnQuitButtonClicked;
            menuButton.clicked += OnMenuButtonClicked;

            cloudCurtainCotnroller = Camera.main.transform.Find("MenuCurtains").GetComponent<CloudCurtainCotnroller>();

            if (replayButton != null)
            {

                replayButton.RegisterCallback<MouseOverEvent>(MouseOnMeEvent);
            }

            quitButton.RegisterCallback<MouseOverEvent>(MouseOnMeEvent);
            menuButton.RegisterCallback<MouseOverEvent>(MouseOnMeEvent);
        }


        private void MouseOnMeEvent(MouseOverEvent evt)
        {
        }

        private void OnQuitButtonClicked()
        {
            levelEventChannel.RaiseLoadLevelRequest(-1);
        }

        private void OnReplayButton()
        {
            Hide();
            cloudCurtainCotnroller.HideCurtains(true);
            levelEventChannel.RaiseReloadLevelRequest();
        }

        private void OnMenuButtonClicked()
        {
            Hide();
            cloudCurtainCotnroller.HideCurtains(true);
            levelEventChannel.RaiseLoadLevelRequest(0);
        }

    }
}
