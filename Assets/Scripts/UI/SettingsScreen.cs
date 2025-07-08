using UnityEngine;
using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    public class SettingsScreen: UIScreen
    {
        [SerializeField] CameraSettings cameraInputSensitivity;
        [SerializeField] AudioSettings audioSettings;
        [SerializeField] ThemeSettings themeSettings; 

        protected override ScreenType screenType => ScreenType.SettingsScreen;

        private Button _closeButton;
        private Button _cameraSettingsButton;
        private Button _audioSettingsButton;
        private Button _gameThemeButton; 

        private CameraSettingsUIView _cameraSettingsUIView;
        private AudioSettingsUIView _audioSettingsUIView;
        private ThemeSettingsUIView _themeSettingsView; 

        public override void Initialize()
        {
            base.Initialize();
           
            _closeButton = root.Q<Button>("CloseButton");
            _closeButton.clicked += OnCloseSettingsButtonClicked;

            _cameraSettingsButton = root.Q<Button>("CameraSettingsButton");
            _cameraSettingsButton.clicked += OnCameraSettingsButtonClicked;

            _audioSettingsButton = root.Q<Button>("AudioSettingsButton");
            _audioSettingsButton.clicked += OnAudioSettingsButtonClicked;

            _gameThemeButton = root.Q<Button>("GameThemeButton");
            _gameThemeButton.clicked += OnGameThemeButtonClicked;



            VisualElement cameraSettingsContainer = root.Q<VisualElement>("CameraSettings");
            _cameraSettingsUIView = new CameraSettingsUIView(cameraSettingsContainer, cameraInputSensitivity);

            VisualElement audioSettingsContainer = root.Q<VisualElement>("AudioSettings");
            _audioSettingsUIView = new AudioSettingsUIView(audioSettingsContainer, audioSettings);

            VisualElement gameThemeContainer = root.Q<VisualElement>("GameThemeSettings");

            _themeSettingsView = new ThemeSettingsUIView(gameThemeContainer, themeSettings); 

        }

        private void OnGameThemeButtonClicked()
        {
            _gameThemeButton.AddToClassList("selectedItem");
            _cameraSettingsButton.RemoveFromClassList("selectedItem");
            _audioSettingsButton.RemoveFromClassList("selectedItem");
            _audioSettingsUIView.Hide();
            _themeSettingsView.Show();
            _cameraSettingsUIView.Hide();
        }

        private void OnAudioSettingsButtonClicked()
        {
            _gameThemeButton.RemoveFromClassList("selectedItem"); 
            _cameraSettingsButton.RemoveFromClassList("selectedItem"); 
            _audioSettingsButton.AddToClassList("selectedItem"); 
            _audioSettingsUIView.Show();
            _themeSettingsView.Hide();
            _cameraSettingsUIView.Hide();
        }

        private void OnCameraSettingsButtonClicked()
        {
            _gameThemeButton.RemoveFromClassList("selectedItem");
            _cameraSettingsButton.AddToClassList("selectedItem");
            _audioSettingsButton.RemoveFromClassList("selectedItem");
            _audioSettingsUIView.Hide();
            _themeSettingsView.Hide();
            _cameraSettingsUIView.Show(); 
        }

        private void OnCloseSettingsButtonClicked()
        {
            cameraInputSensitivity.Save(); 
            audioSettings.Save();
            uiEventChannel.RaisePopUIScreen(); 
        }
    }
}
