using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace View.ValueConverters {
	public class InverseVisibilityConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is Visibility visibilityValue) {
				return (visibilityValue == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is Visibility visibilityValue) {
				return (visibilityValue == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
			}
			return value;
		}
	}
}
