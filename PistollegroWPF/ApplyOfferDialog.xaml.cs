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
	public partial class ApplyOfferDialog : Window, INotifyPropertyChanged
	{
		private DbConnectionServiceClient service = new DbConnectionServiceClient();
		public ApplyOfferDialog(WeaponOfferPresenter presenter)
		{
			InitializeComponent();
			WeaponOffer = presenter;
			IsAccepted = false;
		}


		private WeaponOfferPresenter WeaponOffer;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public String OfferName
		{
			get
			{
				return WeaponOffer.Name;
			}
			set
			{
				WeaponOffer.Name = value;
			}
		}

		public int OfferPrice
		{
			get
			{
				return WeaponOffer.Price;
			}
			set
			{
				WeaponOffer.Price = value;
			}
		}

		public String OfferDescription
		{
			get
			{
				return WeaponOffer.Description;
			}
			set
			{
				WeaponOffer.Description = value;
			}
		}

		public String OfferCategory
		{
			get
			{
				return WeaponOffer.Category;
			}
			set
			{
				WeaponOffer.Name = value;
			}
		}

		public String OfferImage
		{
			get
			{
				return WeaponOffer.ImagePath;
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			BitmapImage src = new BitmapImage();
			src.BeginInit();
			src.UriSource = new Uri(OfferImage);
			src.CacheOption = BitmapCacheOption.OnLoad;
			src.EndInit();
			image.Source = src;
			NameTextBox.Text = OfferName;
			PriceTextBox.Text = OfferPrice.ToString();
			AddedPriceTextBox.Text = "0";
			CategoryCombo.ItemsSource = service.GetCategories();
			CategoryCombo.SelectedItem = OfferCategory;
			DescriptionTextBox.Text = OfferDescription;
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			bool changed = false;
			if (NameTextBox.Text != OfferName)
			{
				changed = true;
				OfferName = NameTextBox.Text;
			}

			int newOfferPrice = Int32.Parse(PriceTextBox.Text);
			int addedPrice = Int32.Parse(AddedPriceTextBox.Text); 
			if (newOfferPrice != OfferPrice || addedPrice != 0)
			{
				changed = true;
				OfferPrice = newOfferPrice + addedPrice;
			}

			if (DescriptionTextBox.Text != OfferDescription)
			{
				changed = true;
				OfferDescription = DescriptionTextBox.Text;
			}

			if ((String)CategoryCombo.SelectedItem != OfferCategory)
			{
				changed = true;
				OfferCategory = (String)CategoryCombo.SelectedItem;
			}

			if (changed)
			{
				service.UpdateOffer(WeaponOffer.WeaponOffer);
			}
			IsAccepted = true;
			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		public bool IsAccepted { get; private set; }
	}
}
