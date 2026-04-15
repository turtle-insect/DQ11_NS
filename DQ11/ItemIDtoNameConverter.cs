using System.Globalization;
using System.Windows.Data;

namespace DQ11
{
	class ItemIDtoNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (Info.Instance().Items.ContainsKey((String)value) == false) return value;
			return Info.Instance().Items[(String)value];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
