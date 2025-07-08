using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    public class CreditsScreen : CommonGameMenuButtonScreen
    {
        private Label headerLabel;
        private Button closeButton; 
        protected override ScreenType screenType => ScreenType.CreditScreen;

        public override void Initialize()
        {
            base.Initialize();

            closeButton = root.Q<Button>("CloseButton");
            headerLabel = root.Q<Label>("HeaderTitle"); 
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                headerLabel.text = "Orb Ascent";
                closeButton.clicked += OnCloseButtonClicked;
                menuButton.Hide();
                quitButton.Hide(); 

            }
            else
            {
                headerLabel.text = "Thanks For Playing";
                closeButton.Hide(); 
                Show();
            }
        }

        private void OnCloseButtonClicked()
        {
            uiEventChannel.RaisePopUIScreen(); 
        }
    }
}
