using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnalyticalApproach.OrbAscent
{
    public class LevelWonTrigger : MonoBehaviour
    {
        private UIEventChannel _uiEventChannel;
        private AudioEventChannel _audioEventChannel;
        private const string LEVEL_CLEARED_KEY = "LEVEL_CLEARED_KEY";
        private void Awake()
        { 
            _uiEventChannel = GameEventManager.GetEventChannel<UIEventChannel>();   
            _audioEventChannel = GameEventManager.GetEventChannel<AudioEventChannel>();   
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlaybleUnit"))
            {

                int levelCleared = PlayerPrefs.GetInt(LEVEL_CLEARED_KEY);

                if (levelCleared < SceneManager.GetActiveScene().buildIndex)
                {
                    PlayerPrefs.SetInt(LEVEL_CLEARED_KEY, SceneManager.GetActiveScene().buildIndex);
                    PlayerPrefs.Save();
                }

                other.gameObject.GetComponent<PlaybleUnit>().Despose(true);


            }
        }
    }
}