using System;

namespace AnalyticalApproach.OrbAscent
{
    public class UIEventChannel : EventChannel
    {
        public event Action<ScreenType> OnPushUIScreen; 
        public void RaisePushUIScreen(ScreenType screenType)
        {
            OnPushUIScreen?.Invoke(screenType); 
        }

        public event Action OnPopUIScreen;
        public void RaisePopUIScreen()
        {
            OnPopUIScreen?.Invoke();
        }

        public event Action<Theme> OnThemeChanged; 
        public void RaiseThemeChangeRequest(Theme theme)
        {
            OnThemeChanged?.Invoke(theme); 
        }
               
        public override void ResetEvents()
        {
            OnPushUIScreen = null;
            OnPopUIScreen = null; 
        }
    }
}
