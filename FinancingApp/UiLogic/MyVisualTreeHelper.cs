using System.Windows;
using System.Windows.Media;

namespace FinancingApp.UiLogic
{
    public static class MyVisualTreeHelper
    {
        public static T? GetFirstChildOfType<T>(DependencyObject? searchRoot) where T : DependencyObject
        {
            if (searchRoot is null)
            {
                return null;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(searchRoot); ++i)
            {
                var child = VisualTreeHelper.GetChild(searchRoot, i);

                var result = (child as T) ?? GetFirstChildOfType<T>(child);

                if (result is not null)
                {
                    return result;
                }
            }

            return null;
        }

        public static T? GetParentOfType<T>(DependencyObject? searchRoot) where T : DependencyObject
        {
            var element = searchRoot as UIElement;

            if (element is null)
            {
                return null;
            }

            do
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
            }
            while (element is not null and not T);

            return element as T;
        }
    }
}
