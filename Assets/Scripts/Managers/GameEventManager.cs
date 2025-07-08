using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnalyticalApproach.OrbAscent
{
    public static class GameEventManager
    {
        private static Dictionary<string, EventChannel> EVENTS_CHANNELS;
        private static bool initialized = false; 

        public static void InitEvents()
        {
            EVENTS_CHANNELS = new Dictionary<string, EventChannel>();
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            initialized = true; 
        }

        private static void OnSceneUnloaded(Scene arg0)
        {
            ResetAllEventChannels(); 
        }

        private static void ResetAllEventChannels()
        {
            foreach (var channel in EVENTS_CHANNELS.Values)
            {
                channel.ResetEvents();
            }
        } 

        public static T GetEventChannel<T>() where T: EventChannel
        {
            if (!initialized)
            {
                InitEvents(); 
            }

            if(!EVENTS_CHANNELS.ContainsKey(typeof(T).Name))
            {
               EventChannel eventChannel = ScriptableObject.CreateInstance<T>();
                EVENTS_CHANNELS[typeof(T).Name] = eventChannel;
                return (T)eventChannel; 
            }
            else
            {
                EventChannel x = EVENTS_CHANNELS[typeof(T).Name];
                return (T)EVENTS_CHANNELS[typeof(T).Name]; 
            }
        }
    }
}
