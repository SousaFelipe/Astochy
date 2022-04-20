using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace VadenStock.View
{
    public static class Utils
    {
        public static T? GetChildOfType<T>(this DependencyObject dep) where T : DependencyObject
        {
            if (dep == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dep); i++)
            {
                var child = VisualTreeHelper.GetChild(dep, i);
                var result = (child as T) ?? GetChildOfType<T>(child);

                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
