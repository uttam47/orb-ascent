using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public abstract class InputProcessor: MonoBehaviour
    {
        protected Player_ActionMap inputActions;
        protected PlayerEventChannel playerEventChannel;
        protected CameraEventsChannel cameraEventsChannel;
        protected SelectionEventChannel selectionEventChannel;

        protected virtual void Awake()
        {
            playerEventChannel = GameEventManager.GetEventChannel<PlayerEventChannel>();
            selectionEventChannel = GameEventManager.GetEventChannel<SelectionEventChannel>();
            cameraEventsChannel = GameEventManager.GetEventChannel<CameraEventsChannel>();
        }

        public abstract void SetInputActionMap(Player_ActionMap inputActions); 
    }
}
