using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace DQ11
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFile(false);
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			SaveData.Instance().Save();
		}

		private void MenuItemFileImport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;
			SaveData.Instance().Import(dlg.FileName);
		}

		private void MenuItemFileExport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;
			SaveData.Instance().Export(dlg.FileName);
		}

		private void MenuItemFileExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ListBoxMenuItemAppend_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new ChoiceWindow();
			dlg.ShowDialog();

			if (String.IsNullOrWhiteSpace(dlg.ID)) return;
			var items = (DataContext as ViewModel)?.Items;
			items.Append(dlg.ID);
			items.Update();
			DataContext = new ViewModel();
		}

		private void ListBoxMenuItemDelete_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxItem.SelectedIndex == -1) return;
			var items = (DataContext as ViewModel)?.Items;
			items.Delete(ListBoxItem.SelectedIndex);
			items.Update();
			DataContext = new ViewModel();
		}

		private void ButtonItemChoice_Click(object sender, RoutedEventArgs e)
		{
			var item = (sender as Button)?.DataContext as Item;
			if (item == null) return;

			var dlg = new ChoiceWindow();
			dlg.ID = item.Name;
			dlg.ShowDialog();
			item.Name = dlg.ID;
		}

		private void OpenFile(bool force)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Open(dlg.FileName, force);
			DataContext = new ViewModel();
		}
	}
}