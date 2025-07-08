using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class SceneSettingsController : MonoBehaviour
    {
        [SerializeField] private ThemeSettings themeSettings;
        [SerializeField] private Material lightSkybox;
        [SerializeField] private Material darkSkybox;

        private UIEventChannel _uiEventChannel; 
             
        void Awake()
        {
            themeSettings.Load(); 
            _uiEventChannel = GameEventManager.GetEventChannel<UIEventChannel>();
            SetEnv();
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

            _uiEventChannel.OnThemeChanged += OnThemeChanged; 
        }

        private void OnThemeChanged(Theme changedTheme)
        {
            themeSettings.theme = changedTheme;
            SetEnv();
            themeSettings.Save();
        }

        public void SetEnv()
        {
            RenderSettings.skybox = themeSettings.theme == Theme.Dark ? darkSkybox : lightSkybox;
            RenderSettings.ambientLight = themeSettings.theme == Theme.Dark ? Color.white/4 : Color.white/4;
            DynamicGI.UpdateEnvironment(); 
        }
    }

}