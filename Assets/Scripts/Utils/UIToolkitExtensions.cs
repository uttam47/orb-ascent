using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AnalyticalApproach
{
    public static class UIToolkitExtensions
    {
        public static List<T> GetAllChilderenOfType<T>(this VisualElement root) where T : VisualElement
        {
            List<T> queriedTypeList = new List<T>();
            ScanHierarchy<T>(root, queriedTypeList); 
            return queriedTypeList;
        }

        private static void ScanHierarchy<T>(VisualElement element, List<T> queriedTypeList) where T: VisualElement   
        {
            if (element is T button)
            {
                queriedTypeList.Add(button);
            }

            foreach (var child in element.Children())
            {
                ScanHierarchy(child, queriedTypeList);
            }
        }

        public static void Show(this VisualElement element)
        {
            element.style.display = DisplayStyle.Flex; 
        }

        public static void Hide(this VisualElement element)
        {
            element.style.display = DisplayStyle.None; 
        }

        public static void Show(this VisualElement element, bool value)
        {
            element.style.display = value ? DisplayStyle.Flex: DisplayStyle.None; 
        }
    }
}
