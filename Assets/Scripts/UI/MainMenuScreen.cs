using UnityEngine.UIElements;
using UnityEngine;
using System;
using Unity.Cinemachine;

namespace AnalyticalApproach.OrbAscent
{
    public class MainMenuScreen : UIScreen
    {
        protected override ScreenType screenType => ScreenType.MainMenuScreen;

        [SerializeField] CinemachineCamera subMenuCam; 
        private LevelEventChannel _levelEventChannel;
        private Button _playButton;
        private Button _infoButton;
        private Button _quitButton;
        private Button _settingsButton;
        private Button _creditsButton;


        private CloudCurtainCotnroller _cloudCurtainCotnroller; 

        public override void Initialize()
        {
            _cloudCurtainCotnroller = Camera.main.transform.GetComponentInChildren<CloudCurtainCotnroller>();   
            _levelEventChannel = GameEventManager.GetEventChannel<LevelEventChannel>();

            _playButton = root.Q<Button>("PlayButton");
            _infoButton = root.Q<Button>("InfoButton");
            _quitButton = root.Q<Button>("QuitButton");
            _settingsButton = root.Q<Button>("SettingsButton");

            _infoButton.clicked += OnInfoButtonClicked;
            _quitButton.clicked += OnQuitButtonClicked;
            _playButton.clicked += OnPlayButtonClicked;
            _settingsButton.clicked += OnSettingsButtonClicked;


            _creditsButton = root.Q<Button>("CreditsButton");
            _creditsButton.clicked += OnCreditsButtonClicked;

            subMenuCam.enabled = false; 
        }

        private void OnCreditsButtonClicked()
        {
            uiEventChannel.RaisePushUIScreen(ScreenType.CreditScreen);
        }

        private void Start()
        {
            root.style.display = DisplayStyle.None;
            uiEventChannel.RaisePushUIScreen(screenType); 
        }

        public override void Show(bool value, Action OnComplete = null)
        {
            subMenuCam.enabled = !value;

            _cloudCurtainCotnroller.HideCurtains(value, () => {
                base.Show(value, OnComplete); 
                OnComplete?.Invoke(); 
            });
        }

        public override void Hide(Action OnComplete = null)
        {
            subMenuCam.enabled = true;
            base.Hide(); 
            _cloudCurtainCotnroller.HideCurtains(false, OnComplete); 
        }

        public override void Show(Action OnComplete = null)
        {
            subMenuCam.enabled = false;
            _cloudCurtainCotnroller.HideCurtains(true, () =>
            {
                base.Show(OnComplete); 
            }); 
        }

        private void OnSettingsButtonClicked()
        {
            uiEventChannel.RaisePushUIScreen(ScreenType.SettingsScreen); 
        }

        private void OnPlayButtonClicked()
        {
            uiEventChannel.RaisePushUIScreen(ScreenType.LevelSelectionScreen);
        }

        private void OnQuitButtonClicked()
        {
            _levelEventChannel.RaiseLoadLevelRequest(-1); 
        }

        private void OnInfoButtonClicked()
        {
            uiEventChannel.RaisePushUIScreen(ScreenType.InstructionScreen); 
        }
    }
}
