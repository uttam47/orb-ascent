using UnityEngine.UIElements;

namespace AnalyticalApproach
{
    public abstract class UIView
    {
        protected VisualElement root;

        protected const string TOGGLE_ON_CLASS = "toggleButtonOn";
        protected const string TOGGLE_OFF_CLASS = "toggleButtonOff";

        public UIView(VisualElement visualElementRoot)
        {
            root = visualElementRoot;
        }

        public void Show()
        {
            root.Show(); 
        }

        public void Hide()
        {

            root.Hide(); 
        }

        public void Show(bool show) 
        {
            root.Show(show); 
        }

    }
}
