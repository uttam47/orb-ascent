using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    internal class LevelSelectionScreen : UIScreen
    {
        protected override ScreenType screenType => ScreenType.LevelSelectionScreen;

        private int _totalLevels;
        private int _currentPage = 0;
        private const int LevelsPerPage = 10;

        private Button _closeButton;
        private Button _prevButton;
        private Button _nextButton;

        private const string LEVEL_BUTTON_CLASS = "levelButton";
        private const string LEVEL_LOCKED_CLASS = "levelLockedButton";
        private LevelEventChannel _levelEventChannel;

        private int _clearedLevel;
        private CloudCurtainCotnroller _cloudCurtainCotnroller;
        private const string LEVEL_CLEARED_KEY = "LEVEL_CLEARED_KEY";

        private VisualElement _levelBlock1;
        private VisualElement _levelBlock2;

        public override void Initialize()
        {
            base.Initialize();
            _totalLevels = SceneManager.sceneCountInBuildSettings - 2;

            // Fetch UI Elements
            _closeButton = root.Q<Button>("CloseButton");
            _prevButton = root.Q<Button>("PrevButton");
            _nextButton = root.Q<Button>("NextButton");
            _levelBlock1 = root.Q<VisualElement>("LevelBlock1");
            _levelBlock2 = root.Q<VisualElement>("LevelBlock2");

            // Button event bindings
            _closeButton.clicked += OnCloseButtonClicked;
            _prevButton.clicked += OnPrevButtonClicked;
            _nextButton.clicked += OnNextButtonClicked;

            _levelEventChannel = GameEventManager.GetEventChannel<LevelEventChannel>();
            _clearedLevel = PlayerPrefs.GetInt(LEVEL_CLEARED_KEY);

            // Initial setup
            _cloudCurtainCotnroller = Camera.main.transform.Find("MenuCurtains").GetComponent<CloudCurtainCotnroller>();
            UpdateLevelButtons();
        }

        private void OnCloseButtonClicked()
        {
            uiEventChannel.RaisePopUIScreen();
        }

        private void OnPrevButtonClicked()
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                UpdateLevelButtons();
            }
        }

        private void OnNextButtonClicked()
        {
            if ((_currentPage + 1) * LevelsPerPage < _totalLevels)
            {
                _currentPage++;
                UpdateLevelButtons();
            }
        }

        private void UpdateLevelButtons()
        {
            // Clear the previous buttons
            _levelBlock1.Clear();
            _levelBlock2.Clear();

            // Calculate level indices for the current page
            int startLevel = _currentPage * LevelsPerPage;
            int endLevel = Mathf.Min(startLevel + LevelsPerPage, _totalLevels);

            // Add buttons to level blocks
            for (int i = startLevel; i < endLevel; i++)
            {
                VisualElement buttonElement; 
                int currentLevel = i + 1;

                // Ensure that level one is always unlocked
                if (currentLevel == 1 || i <= _clearedLevel)
                {
                    Button levelButton = new Button();
                    buttonElement = levelButton;
                    levelButton.text = currentLevel.ToString();
                    int levelNum = i;
                    levelButton.clicked += () =>
                    {
                        Hide();
                        _cloudCurtainCotnroller.HideCurtains(true, () =>
                        {
                            _levelEventChannel.RaiseLoadLevelRequest(currentLevel);
                        });
                    };
                }
                else
                {
                    buttonElement = new VisualElement(); 
                    buttonElement.AddToClassList(LEVEL_LOCKED_CLASS);
                }
              
                buttonElement.AddToClassList(LEVEL_BUTTON_CLASS);

                // Add buttons to LevelBlock1 or LevelBlock2
                if (i - startLevel < 5)
                {
                    _levelBlock1.Add(buttonElement);
                }
                else
                {
                    _levelBlock2.Add(buttonElement);
                }
            }

            // Enable/disable paging buttons based on current page
            _prevButton.Show(_currentPage > 0);
            _nextButton.Show((_currentPage + 1) * LevelsPerPage < _totalLevels);
        }
    }
}
