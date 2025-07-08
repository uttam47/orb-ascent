using Unity.Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AnalyticalApproach.OrbAscent
{
    public class GameScreen : UIScreen
    {
        private LevelEventChannel _levelEventChannel;
        private PlayerEventChannel _playerEventChannel;
        private SelectionEventChannel _selectionEventChannel;
        private CloudCurtainCotnroller _cloudCurtainCotnroller;
        private CameraEventsChannel _cameraEventChannel;
        private GameMode _gameMode;

        protected override ScreenType screenType => ScreenType.GameScreen;

        [SerializeField] private ThemeSettings themeSettings;
        [SerializeField] private CameraSettings cameraSettings;
        [SerializeField] private CinemachineCamera menuCam;
        [SerializeField] private Button selecButton;
        [SerializeField] private Button jumpButton;
        [SerializeField] private Button deselectButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button helpButton;
        [SerializeField] private Button reloadButton;
        [SerializeField] private Image leftStickContainer;
        [SerializeField] private GameObject cursorImage;

        void Awake()
        {
            FetchEventChannels();
            _cloudCurtainCotnroller = Camera.main.transform.Find("MenuCurtains").GetComponent<CloudCurtainCotnroller>();
            menuCam.enabled = false;

            _playerEventChannel.OnGameModeUpdate += OnGameModeUpdated;
            uiEventChannel.OnThemeChanged += OnThemeChanged;
            _cameraEventChannel.OnCameraSettingsUpdated += CameraSettingsUpdated;
            OnThemeChanged(themeSettings.theme);
        }


        private void CameraSettingsUpdated()
        {
            if (_gameMode == GameMode.InspectMode)
            {
                leftStickContainer.gameObject.SetActive(cameraSettings.trackedCameraControlType == TrackedCameraControlType.Joystick);
            }
        }

        private void FetchEventChannels()
        {
            _playerEventChannel = GameEventManager.GetEventChannel<PlayerEventChannel>();
            _levelEventChannel = GameEventManager.GetEventChannel<LevelEventChannel>();
            _selectionEventChannel = GameEventManager.GetEventChannel<SelectionEventChannel>();
            uiEventChannel = GameEventManager.GetEventChannel<UIEventChannel>();
            audioEventChannel = GameEventManager.GetEventChannel<AudioEventChannel>();
            _cameraEventChannel = GameEventManager.GetEventChannel<CameraEventsChannel>();
        }

        private void OnThemeChanged(Theme theme)
        {
            leftStickContainer.color = theme == Theme.Dark ? Color.white : Color.black;
        }


        private void Start()
        {
            gameObject.SetActive(false);
            uiEventChannel.RaisePushUIScreen(screenType);
            OnThemeChanged(themeSettings.theme);
        }

        public override void Show(bool value, Action OnComplete = null)
        {
            menuCam.enabled = !value;
            _cloudCurtainCotnroller.HideCurtains(value, () =>
            {
                gameObject.SetActive(value);
                OnComplete?.Invoke();
            });
        }

        public override void Hide(Action OnComplete = null)
        {
            menuCam.enabled = true;
            gameObject.SetActive(false);
            _levelEventChannel.RaisePauseLevelRequest();
            _cloudCurtainCotnroller.HideCurtains(false, OnComplete);
        }

        public override void Show(Action OnComplete = null)
        {
            menuCam.enabled = false;
            _cloudCurtainCotnroller.HideCurtains(true, () =>
            {
                _levelEventChannel.RaiseResumeLevelRequest();
                gameObject.SetActive(true);
                OnComplete?.Invoke();
            });
        }

        private void OnEnable()
        {
            selecButton.onClick.AddListener(OnSelectButtonClicked);
            menuButton.onClick.AddListener(OnMenuButtonClicked);
            deselectButton.onClick.AddListener(OnDeselectButtonClicked);
          //  jumpButton.onClick.AddListener(OnJumpButtonClicked);
            jumpButton.GetComponent<ButtonEvents>().OnPointerDownEvent += OnJumpButtonClicked;
            reloadButton.onClick.AddListener(OnReloadButtonClicked);
            helpButton.onClick.AddListener(OnHelpButtonClicked);
        }

        private void OnDisable()
        {
            selecButton.onClick.RemoveAllListeners();
            menuButton.onClick.RemoveAllListeners();
            deselectButton.onClick.RemoveAllListeners();
            //jumpButton.onClick.RemoveAllListeners();
            jumpButton.GetComponent<ButtonEvents>().OnPointerDownEvent -= OnJumpButtonClicked;
            reloadButton.onClick.RemoveAllListeners();
            helpButton.onClick.RemoveAllListeners();
        }

        private void OnHelpButtonClicked()
        {
            uiEventChannel.RaisePushUIScreen(ScreenType.InstructionScreen); 
        }

        private void OnReloadButtonClicked()
        {
            gameObject.SetActive(false);
            audioEventChannel.RaisePlayMenuAudio(AudioType.MenuButton);
            _levelEventChannel.RaiseReloadLevelRequest();
        }

        private void OnGameModeUpdated(GameMode gameMode)
        {
            _gameMode = gameMode;
            switch (gameMode)
            {
                case GameMode.InspectMode:
                    cursorImage.SetActive(true); 
                    SetTrackedCamLook();
                    break;
                case GameMode.PlayableSelected:
                    cursorImage.SetActive(false); 
                    SetSelectedLook();
                    break;
            }
        }

        private void SetSelectedLook()
        {
            leftStickContainer.gameObject.SetActive(true);
            selecButton.gameObject.SetActive(false);
            deselectButton.gameObject.SetActive(true);
            jumpButton.gameObject.SetActive(true);
        }

        private void SetTrackedCamLook()
        {
            leftStickContainer.gameObject.SetActive(cameraSettings.trackedCameraControlType == TrackedCameraControlType.Joystick);
            selecButton.gameObject.SetActive(true);
            deselectButton.gameObject.SetActive(false);
            jumpButton.gameObject.SetActive(false);
        }

        private void OnMenuButtonClicked()
        {
            audioEventChannel.RaisePlayMenuAudio(AudioType.MenuButton);
            uiEventChannel.RaisePushUIScreen(ScreenType.LevelPauseScreen);
        }

        private void OnDeselectButtonClicked()
        {
            _selectionEventChannel.RaiseDeselectActionPerformed();
        }

        private void OnJumpButtonClicked()
        {
            _playerEventChannel.RaisePlayerJumpEvent(true);
        }

        private void OnSelectButtonClicked()
        {
            _selectionEventChannel.RaiseSelectActionPerformed();
        }

        private void OnDestroy()
        {
            _playerEventChannel.OnGameModeUpdate -= OnGameModeUpdated;
            uiEventChannel.OnThemeChanged -= OnThemeChanged;

        }
    }

}