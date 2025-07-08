
namespace AnalyticalApproach.OrbAscent
{
    public static class EventChannelPaths
    {
        private const string EVENT_BASE_PATH = "ScriptableObjects/EventChannels/";
        public const string PLAYER_EVENTS_PATH = EVENT_BASE_PATH +nameof(PlayerEventChannel); 
        public const string SELECTION_EVENTS_PATH = EVENT_BASE_PATH +nameof(SelectionEventChannel);  
        public const string CAMERA_EVENTS_PATH = EVENT_BASE_PATH +nameof(CameraEventsChannel);  
        public const string LEVEL_EVENTS_PATH = EVENT_BASE_PATH +nameof(LevelEventChannel);  
    }
}
