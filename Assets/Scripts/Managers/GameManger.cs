using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnalyticalApproach.OrbAscent
{
    public class GameManger : MonoBehaviour
    {
        private LevelEventChannel _levelEventChannel;
        private CloudCurtainCotnroller _reloadCurtains;
        private PlayerInputHandler _playerInput;
        private AudioManager _audioManager;

        [SerializeField] private CameraSettings cameraSettings;
        [SerializeField] private AudioSettings audioSettings;

        private void Awake()
        {
            cameraSettings.Load();
            audioSettings.Load();

            _playerInput = GetComponent<PlayerInputHandler>();
            _audioManager = GetComponentInChildren<AudioManager>();
            _reloadCurtains = Camera.main.transform.Find("ReloadCurtains").GetComponent<CloudCurtainCotnroller>();

            _levelEventChannel = GameEventManager.GetEventChannel<LevelEventChannel>();

            RegisterLevelEvents();
            EnableCursor(false);
            SceneManager.sceneLoaded += RemoveCurtains;
        }
        
        private void RemoveCurtains(Scene arg0, LoadSceneMode arg1)
        {
            _reloadCurtains.HideCurtains(true, null);
        }

        private void RegisterLevelEvents()
        {
            _levelEventChannel.OnReloadLevel += Reload;
            _levelEventChannel.OnLoadNextLevel += LoadNextLevel;
            _levelEventChannel.OnLoadLevel += LoadLevel;
            _levelEventChannel.OnPauseLevel += PauseGame;
            _levelEventChannel.OnResumeLevel += ResumeGame;
        }

        private void ResumeGame()
        {
            _playerInput.Enable();
        }

        private void PauseGame()
        {
            _playerInput.Disable();
        }

        private void LoadLevel(int levelIndex)
        {
            if (levelIndex == -1)
            {
                Quit();
                return;
            }

            EnableCursor(levelIndex == 0);            
            _audioManager.StopMainTheme();
            
            _reloadCurtains.HideCurtains(false, () =>
            {
                SceneManager.LoadScene(levelIndex);
            });
        }

        private void Reload()
        {
            EnableCursor(false);
            _audioManager.StopMainTheme(); 

            _reloadCurtains.HideCurtains(false, () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void LoadNextLevel()
        {
            EnableCursor(false);
            _audioManager.StopMainTheme(); 

            _reloadCurtains.HideCurtains(false, () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            });

        }

        private void EnableCursor(bool value)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            /*   Cursor.lockState = value? CursorLockMode.None: CursorLockMode.Locked;
               Cursor.visible = value;*/
#endif
        }

        private void OnDestroy()
        {
            _levelEventChannel.OnReloadLevel -= Reload;
            _levelEventChannel.OnLoadNextLevel -= LoadNextLevel;
            _levelEventChannel.OnLoadLevel -= LoadLevel;
            _levelEventChannel.OnPauseLevel -= PauseGame;
            _levelEventChannel.OnResumeLevel -= ResumeGame;
            SceneManager.sceneLoaded -= RemoveCurtains;
        }
    }

}