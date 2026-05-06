using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MEFExampleApp.Modules.LoanCalculator
{
    /// <summary>
    /// Returns Visibility.Visible when the bound string is non-empty, Collapsed otherwise.
    /// Used in LoanCalculatorView to show the results panel and validation message only
    /// when they have content — all driven from the ViewModel, no code-behind required.
    /// </summary>
    [ValueConversion(typeof(string), typeof(Visibility))]
    public sealed class StringToVisibilityConverter : IValueConverter
    {
        /// <summary>Singleton instance referenced from XAML via {x:Static …}.</summary>
        public static readonly StringToVisibilityConverter Instance = new StringToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
