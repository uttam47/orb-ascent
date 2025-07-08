using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    internal class InstructionScreen : UIScreen
    {
        protected override ScreenType screenType => ScreenType.InstructionScreen;
        private Button closeButton;


        public override void Initialize()
        {
            base.Initialize();

            closeButton = root.Q<Button>("CloseButton");
            closeButton.clicked += OnCloseButtonClicked;
        }

        private void OnCloseButtonClicked()
        {
            uiEventChannel.RaisePopUIScreen();
        }

    }
}
