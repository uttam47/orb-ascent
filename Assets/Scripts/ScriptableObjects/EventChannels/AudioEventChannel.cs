using System;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class AudioEventChannel : EventChannel
    {
        public event Action<AudioSettings> OnAudioSettingsUpdated;
        public void RaiseAudioSettingsUpdated(AudioSettings audioSettings)
        {
            OnAudioSettingsUpdated?.Invoke(audioSettings); 
        }

        public event Action<AudioType> OnPlayMenuAudio;
        public void RaisePlayMenuAudio(AudioType audioType)
        {
            OnPlayMenuAudio?.Invoke(audioType); 
        }

        public event Action<AudioSource, AudioType> OnPlayAudioWithAudioSource;
        public event Action<AudioSource, AudioType> OnPlayAudioWithMainThemeDimmed; 
        public void RaisePlayAudioWithAudioSource(AudioSource audioSource, AudioType audioType, bool dimMainTheme  =false)
        {
            if(dimMainTheme)
            {
                OnPlayAudioWithMainThemeDimmed?.Invoke(audioSource, audioType);
            }
            else
            {
                OnPlayAudioWithAudioSource?.Invoke(audioSource, audioType);
            }
        }

        public override void ResetEvents()
        {
            OnAudioSettingsUpdated = null;
        }
    }
}
