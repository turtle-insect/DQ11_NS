using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DQ11
{
	/// <summary>
	/// ChoiceWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class ChoiceWindow : Window
	{
		public String ID { get; set; }

		public ChoiceWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			CreateItemList("");
			TextBoxFilter.Focus();
		}

		private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CreateItemList(TextBoxFilter.Text);
		}

		private void ListBoxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ButtonDecision.IsEnabled = ListBoxItem.SelectedIndex >= 0;
		}

		private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ButtonDecision_Click(sender, null);
		}

		private void ButtonDecision_Click(object sender, RoutedEventArgs e)
		{
			if (!(ListBoxItem.SelectedItem is KeyValuePair<String, String> item)) return;
			ID = item.Key;
			Close();
		}

		private void CreateItemList(String filter)
		{
			ListBoxItem.Items.Clear();
			var items = Info.Instance().Items;

			foreach (var item in items)
			{
				if (item.Key.Length == ID.Length && (String.IsNullOrEmpty(filter) || item.Value.IndexOf(filter) >= 0))
				{
					ListBoxItem.Items.Add(item);
				}
			}
		}
	}
}
