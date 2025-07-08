using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    [CreateAssetMenu(fileName = nameof(AudioRegistry), menuName = "OrbAscent/" + nameof(AudioRegistry))]
    public class AudioRegistry : ScriptableObject
    {
        [Serializable]
        public struct AudioData
        {
            public AudioType audioType;
            public AudioClip audioClip;
        }
        private Dictionary<AudioType, AudioData> _audioRegistery;
        private bool _registryInitialized = false;

        public List<AudioData> audioData;

        private void Init()
        {
            _audioRegistery = new Dictionary<AudioType, AudioData>();
            foreach (AudioData audioData in audioData)
            {
                _audioRegistery[audioData.audioType] = audioData;
            }
            _registryInitialized = true;
        }

        private void OnDisable()
        {
            _registryInitialized = false; 
        }

        public AudioClip GetAudioClip(AudioType audioType)
        {
            if(!_registryInitialized)
            {
                Init(); 
            }
         
            if(_audioRegistery.ContainsKey(audioType))
            {
                return _audioRegistery[audioType].audioClip;
            }
        
            return null; 
        }
    }
}
