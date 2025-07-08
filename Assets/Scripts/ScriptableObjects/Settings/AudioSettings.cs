using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    [CreateAssetMenu(fileName = nameof(AudioSettings), menuName = "OrbAscent/" + nameof(AudioSettings))]
    public class AudioSettings : Settings
    {
        public float masterVolume;
        public bool musicOn;
        public bool menuSoundOn;
        public bool soundOn;
    }
}
