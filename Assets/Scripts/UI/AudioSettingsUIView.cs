using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    internal class AudioSettingsUIView : UIView
    {
        private AudioSettings _audioSettings; 

        private Button _gameSoundButton;
        private Button _gameMusicButton;
        private Button _menuSoundButton;
        private Slider _masterVolumeSlider; 

        private AudioEventChannel _audioEventChannel;

        public AudioSettingsUIView(VisualElement visualElementRoot, AudioSettings audioSettings) : base(visualElementRoot)
        {

            _audioEventChannel = GameEventManager.GetEventChannel<AudioEventChannel>();
            _audioSettings = audioSettings;
            Init(); 
        }

        public void Init()
        {
            _gameSoundButton = root.Q<Button>("GameSoundButton");
            _gameMusicButton = root.Q<Button>("GameMusicButton");
            _menuSoundButton = root.Q<Button>("MenuSoundButton");
            _masterVolumeSlider = root.Q<Slider>("MasterSliderVolume");

            _gameMusicButton.clicked += OnGameMusicButtonClicked;
            _gameSoundButton.clicked += OnGameSoundButtonClicked;
            _menuSoundButton.clicked += OnMenuMusicButtonClicked;
            _masterVolumeSlider.RegisterValueChangedCallback(OnMasterVolumeChanged);

            _gameMusicButton.RemoveFromClassList(TOGGLE_ON_CLASS);  
            _gameSoundButton.RemoveFromClassList(TOGGLE_ON_CLASS); 
            _menuSoundButton.RemoveFromClassList(TOGGLE_ON_CLASS);

            _masterVolumeSlider.value = _audioSettings.masterVolume;
            _gameMusicButton.AddToClassList(_audioSettings.musicOn ? TOGGLE_ON_CLASS : TOGGLE_OFF_CLASS);
            _gameSoundButton.AddToClassList(_audioSettings.soundOn? TOGGLE_ON_CLASS : TOGGLE_OFF_CLASS);
            _menuSoundButton.AddToClassList(_audioSettings.menuSoundOn? TOGGLE_ON_CLASS : TOGGLE_OFF_CLASS);

        }

        private void OnMasterVolumeChanged(ChangeEvent<float> evt)
        {
            _audioSettings.masterVolume = evt.newValue;
            _audioEventChannel.RaiseAudioSettingsUpdated(_audioSettings); 
        }

        private void OnGameSoundButtonClicked()
        {
            _audioSettings.soundOn = !_audioSettings.soundOn;
            _gameSoundButton.RemoveFromClassList(TOGGLE_ON_CLASS); 
            _gameSoundButton.RemoveFromClassList(TOGGLE_OFF_CLASS); 
            _audioEventChannel.RaiseAudioSettingsUpdated(_audioSettings);
            _gameSoundButton.AddToClassList(_audioSettings.soundOn? TOGGLE_ON_CLASS : TOGGLE_OFF_CLASS);
            _audioEventChannel.RaiseAudioSettingsUpdated(_audioSettings); 
        }

        private void OnGameMusicButtonClicked()
        {
            _audioSettings.musicOn = !_audioSettings.musicOn;
            _gameMusicButton.RemoveFromClassList(TOGGLE_ON_CLASS);  
            _gameMusicButton.RemoveFromClassList(TOGGLE_OFF_CLASS);  
            _audioEventChannel.RaiseAudioSettingsUpdated(_audioSettings);
            _gameMusicButton.AddToClassList(_audioSettings.musicOn ? TOGGLE_ON_CLASS : TOGGLE_OFF_CLASS);
            _audioEventChannel.RaiseAudioSettingsUpdated(_audioSettings); 
        }

        private void OnMenuMusicButtonClicked()
        {
            _audioSettings.menuSoundOn = !_audioSettings.menuSoundOn;
            _menuSoundButton.RemoveFromClassList(TOGGLE_ON_CLASS); 
            _menuSoundButton.RemoveFromClassList(TOGGLE_OFF_CLASS); 
            _audioEventChannel.RaiseAudioSettingsUpdated(_audioSettings);
            _menuSoundButton.AddToClassList(_audioSettings.menuSoundOn? TOGGLE_ON_CLASS : TOGGLE_OFF_CLASS);
            _audioEventChannel.RaiseAudioSettingsUpdated(_audioSettings); 
        }
    }
}
