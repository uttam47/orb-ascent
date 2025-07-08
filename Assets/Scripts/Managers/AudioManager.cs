using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class AudioManager : MonoBehaviour
    {
        private List<AudioSource> _audioSources;
        private AudioEventChannel _audioEventChannel;

        [Header("Audio Settings")]
        [SerializeField] AudioSettings audioSettings;
        [SerializeField] AudioRegistry audioRegistry;

        public AudioClip mainThemeAudioClip;

        private AudioSource _menuAudioSource;
        private AudioSource _mainThemeAudioSource;
        private List<AudioSource> _vfxAudioSources;

        [Header("Fade Settings")]
        [SerializeField] private float fadeDuration = 2f;

        private Coroutine _fadeAudioCoroutine;

        void Awake()
        {
            _vfxAudioSources = new List<AudioSource>();
            _audioEventChannel = GameEventManager.GetEventChannel<AudioEventChannel>();
            _audioSources = FindObjectsOfType<AudioSource>().ToList();
            _mainThemeAudioSource = GetComponent<AudioSource>();

            _menuAudioSource = GameObject.FindGameObjectWithTag("MenuAudioSource").GetComponent<AudioSource>();

            SortVfxSources();
            SetMasterVolume(audioSettings.masterVolume);

            if (mainThemeAudioClip != null)
            {
                _mainThemeAudioSource.clip = mainThemeAudioClip;
            }
            else
            {
                _mainThemeAudioSource.clip = audioRegistry.GetAudioClip(AudioType.MainTheme);
            }

            PlayMainTheme();
        }

        private void OnEnable()
        {
            _audioEventChannel.OnAudioSettingsUpdated += OnAudioSettingsUpdated;
            _audioEventChannel.OnPlayAudioWithAudioSource += PlayVfxSound;
            _audioEventChannel.OnPlayAudioWithMainThemeDimmed += PlayVfxSoundDimmingMainTheme;
            _audioEventChannel.OnPlayMenuAudio += PlayMenuAudio;
        }

        private void OnDisable()
        {
            _audioEventChannel.OnAudioSettingsUpdated -= OnAudioSettingsUpdated;
            _audioEventChannel.OnPlayAudioWithAudioSource -= PlayVfxSound;
            _audioEventChannel.OnPlayAudioWithMainThemeDimmed -= PlayVfxSoundDimmingMainTheme;
            _audioEventChannel.OnPlayMenuAudio -= PlayMenuAudio;
        }

        private void PlayVfxSoundDimmingMainTheme(AudioSource source, AudioType type)
        {
            SmoothedPlay(_mainThemeAudioSource, 0, .3f, false, null);
            PlayVfxSound(source, type);
            StartCoroutine(DelayCoroutine(audioRegistry.GetAudioClip(type).length, () =>
            {
                SmoothedPlay(_mainThemeAudioSource, audioSettings.masterVolume, 2, true && audioSettings.musicOn);
            }));
        }

        private void SmoothedPlay(AudioSource audioSource, float targetVolume, float duration, bool play, Action onComplete = null)
        {
            if (_fadeAudioCoroutine != null)
            {
                StopCoroutine(_fadeAudioCoroutine);
            }

            _fadeAudioCoroutine = StartCoroutine(PlaySmooth(audioSource, targetVolume, duration, play, onComplete));
        }

        private IEnumerator DelayCoroutine(float duration, Action onComplete)
        {
            yield return new WaitForSeconds(duration);
            onComplete?.Invoke();
        }

        private void PlayMenuAudio(AudioType type)
        {
            if (!audioSettings.menuSoundOn)
            {
                return;
            }
            _menuAudioSource.PlayOneShot(audioRegistry.GetAudioClip(type));
        }

        private void OnAudioSettingsUpdated(AudioSettings settings)
        {
            SetMasterVolume(settings.masterVolume);
            PlayMainTheme();
            UpdateMenuSoundSettings();
        }

        private void UpdateMenuSoundSettings()
        {
            _menuAudioSource.enabled = audioSettings.menuSoundOn;
        }

        private void SetMasterVolume(float masterVolume)
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                if(audioSource == null)
                {
                    continue; 
                }
                audioSource.volume = masterVolume;
            }
        }


        private void SortVfxSources()
        {
            foreach (AudioSource source in _audioSources)
            {
                if (source == null)
                {
                    continue;
                }
                if (source != _menuAudioSource && source != _mainThemeAudioSource)
                {
                    _vfxAudioSources.Add(source);
                }
            }
        }

        private void PlayVfxSound(AudioSource source, AudioType type)
        {
            if(!audioSettings.soundOn)
            {
                return; 
            }

            source.PlayOneShot(audioRegistry.GetAudioClip(type));
        }

        private void PlayMainTheme()
        {
            if (audioSettings.musicOn && !_mainThemeAudioSource.isPlaying)
            {
                SmoothedPlay(_mainThemeAudioSource, audioSettings.masterVolume, fadeDuration, true);
            }
            else if (!audioSettings.musicOn && _mainThemeAudioSource.isPlaying)
            {
                SmoothedPlay(_mainThemeAudioSource, 0f, fadeDuration, false);
            }
        }

        public void StopMainTheme()
        {
            if (audioSettings.musicOn)
            {
                SmoothedPlay(_mainThemeAudioSource, 0f, .5f, false);
            }
        }


        private IEnumerator PlaySmooth(AudioSource audioSource, float targetVolume, float duration, bool play, Action onComplete = null)
        {
            if (play)
            {
                audioSource.enabled = true;
                audioSource.volume = 0f;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }

            float startVolume = audioSource.volume;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = targetVolume;

            if (!play)
            {
                audioSource.Stop();
                audioSource.enabled = false;
            }

            onComplete?.Invoke();
        }
    }
}
