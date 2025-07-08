using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    [CreateAssetMenu(fileName = nameof(ThemeSettings), menuName = "OrbAscent/" + nameof(ThemeSettings))]
    public class ThemeSettings: Settings
    {
        public Theme theme;
    }
}
