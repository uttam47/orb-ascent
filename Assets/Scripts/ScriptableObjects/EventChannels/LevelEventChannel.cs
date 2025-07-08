using System;

namespace AnalyticalApproach.OrbAscent
{
    public class LevelEventChannel : EventChannel
    {
        public event Action OnLoadNextLevel;
        public void RaiseLoadNextLevelRequest()
        {
            OnLoadNextLevel?.Invoke(); 
        }

        public event Action<int> OnLoadLevel;
        public void RaiseLoadLevelRequest(int levelIndex)
        {
            OnLoadLevel?.Invoke(levelIndex); 
        }

        public event Action OnReloadLevel;
        public void RaiseReloadLevelRequest()
        {
            OnReloadLevel?.Invoke();
        }

        public event Action OnPauseLevel; 
        public void RaisePauseLevelRequest()
        {
            OnPauseLevel?.Invoke(); 
        }

        public event Action OnResumeLevel;
        public void RaiseResumeLevelRequest()
        {
            OnResumeLevel?.Invoke(); 
        }

        public override void ResetEvents()
        {
            OnLoadNextLevel = null; 
            OnLoadLevel = null;
            OnReloadLevel = null; 
        }

    }
}
