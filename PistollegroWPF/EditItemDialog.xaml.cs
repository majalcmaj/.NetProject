using PistollegroWPF.PistollegroWCF;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PistollegroWPF
{
	/// <summary>
	/// Interaction logic for ApplyOfferDialog.xaml
	/// </summary>
	public partial class EditItemDialog : Window, INotifyPropertyChanged
	{
		private DbConnectionServiceClient service = new DbConnectionServiceClient();
		public EditItemDialog(ItemOnSalePresenter presenter)
		{
			InitializeComponent();
			ItemOnSale = presenter;
			IsAccepted = false;
		}


		private ItemOnSalePresenter ItemOnSale;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			BitmapImage src = new BitmapImage();
			src.BeginInit();
			src.UriSource = new Uri(ItemOnSale.ImagePath);
			src.CacheOption = BitmapCacheOption.OnLoad;
			src.EndInit();
			image.Source = src;
			NameTextBox.Text = ItemOnSale.Name;
			CategoryCombo.ItemsSource = service.GetCategories();
			CategoryCombo.SelectedItem = ItemOnSale.Category;
			DescriptionTextBox.Text = ItemOnSale.Description;
			PriceTextBox.Text = ItemOnSale.Price.ToString();
			ItemsCountTextBox.Text = ItemOnSale.ItemsAvailableCount.ToString();
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			bool changed = false;
			if (NameTextBox.Text != ItemOnSale.Name)
			{
				changed = true;
				ItemOnSale.Name = NameTextBox.Text;
			}

			if (DescriptionTextBox.Text != ItemOnSale.Description)
			{
				changed = true;
				ItemOnSale.Description = DescriptionTextBox.Text;
			}

			if ((String)CategoryCombo.SelectedItem != ItemOnSale.Category)
			{
				changed = true;
				ItemOnSale.Category = (String)CategoryCombo.SelectedItem;
			}

			if (changed)
			{
				//service.EditItemOnSale(ItemOnSale.OnSale);
				IsAccepted = true;
			}
			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		public bool IsAccepted { get; private set; }
	}
}
