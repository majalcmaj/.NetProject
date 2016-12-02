//using PistollegroWPF.PistollegroWCF;
using PistollegroWPF.PistollegroWCF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PistollegroWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	public class WeaponOfferPresenter
	{
		private String ServerPath = @"http://localhost:62329/Content/Images/{0}_thumb.png";
		public WeaponOfferPresenter(WeaponOfferMV offer)
		{
			this.WeaponOffer = offer;
		}
		public WeaponOfferMV WeaponOffer { get; set; }
		public int Price { get { return WeaponOffer.Price; } set { WeaponOffer.Price = value; } }
		public String Name { get { return WeaponOffer.Name; } set { WeaponOffer.Name = value; } }
		public String Description { get { return WeaponOffer.Description; } set { WeaponOffer.Description = value; } }
		public String Category { get { return WeaponOffer.CategoryName; } set { WeaponOffer.CategoryName = value; } }
		public String ImagePath
		{
			get { return String.Format(ServerPath, WeaponOffer.ID); }
			set { /* Do Nothing */}
		}

	}

	public class ItemOnSalePresenter
	{
		private String ServerPath = @"http://localhost:62329/Content/Images/{0}_thumb.png";
		public ItemOnSalePresenter(WeaponOnSaleMV onSale)
		{
			this.OnSale = onSale;
		}
		public WeaponOnSaleMV OnSale { get; set; }
		public int Price { get { return OnSale.Price; } set { OnSale.Price = value; } }
		public String Name { get { return OnSale.Name; } set { OnSale.Name = value; } }
		public String Description { get { return OnSale.Description; } set { OnSale.Description = value; } }
		public String Category { get { return OnSale.CategoryName; } set { OnSale.CategoryName = value; } }
		public int ItemsAvailableCount { get { return OnSale.ItemsAvailableCount; } set { OnSale.ItemsAvailableCount = value; } }
		public String ImagePath
		{
			get { return String.Format(ServerPath, OnSale.ID); }
			set { /* Do Nothing */}
		}

	}
	public partial class MainWindow : Window
	{
		private DbConnectionServiceClient service = new DbConnectionServiceClient();
		public MainWindow()
		{
			InitializeComponent();
			LoadOffers();
		}

		private void LoadOffers()
		{
			CompanyOffers.Items.Clear();
			var offers = service.GetAllOffers(null);
			foreach (var offer in offers)
				CompanyOffers.Items.Add(new WeaponOfferPresenter(offer));
		}

		private void LoadOnSale()
		{
			ItemsOnSale.Items.Clear();
			var onSale = service.GetAllOnSale(null);
			foreach (var item in onSale)
				ItemsOnSale.Items.Add(new ItemOnSalePresenter(item));
		}

		private void AcceptOfferButton_Click(object sender, RoutedEventArgs e)
		{
			var offerToAccept = (WeaponOfferPresenter)CompanyOffers.SelectedItem;
			if (offerToAccept != null)
			{
				ApplyOfferDialog aod = new ApplyOfferDialog(offerToAccept);
				aod.ShowDialog();
				if (aod.IsAccepted)
				{
					service.ApplyWeaponOffer(offerToAccept.WeaponOffer.ID);
					LoadOffers();
				}
			}
		}

		private void RefreshOffers_Click(object sender, RoutedEventArgs e)
		{
			LoadOffers();
		}
		private void WeaponOffersTab_GotFocus(object sender, RoutedEventArgs e)
		{
			LoadOffers();
		}

		private void OnSale_GotFocus(object sender, RoutedEventArgs e)
		{
			LoadOnSale();
			ItemsOnSale.BringIntoView();
		}

		private void DeleteItems_Click(object sender, RoutedEventArgs e)
		{
			var itemSelected = (ItemOnSalePresenter)ItemsOnSale.SelectedItem;
			if(itemSelected != null)
			{
				//service.DeleteFromSaleById(itemSelected.OnSale.ID);
			}
		}

		private void OrderMore_Click(object sender, RoutedEventArgs e)
		{
			var itemSelected = (ItemOnSalePresenter)ItemsOnSale.SelectedItem;
			if(itemSelected != null) { 
				OrderItemsDialog aod = new OrderItemsDialog(itemSelected.Name);
				aod.ShowDialog();
				if (aod.IsAccepted)
				{
					int? orderCount = aod.OrderCount;
					//service.ApplyWeaponOffer(offerToAccept.WeaponOffer.ID);
					LoadOnSale();
				}
			}
		}

		private void EditItem_Click(object sender, RoutedEventArgs e)
		{
			var itemSelected = (ItemOnSalePresenter)ItemsOnSale.SelectedItem;
			if (itemSelected != null)
			{
				EditItemDialog eid = new EditItemDialog(itemSelected);
				eid.ShowDialog();
				if (eid.IsAccepted)
				{
					//service.EditOnSale(itemSelected.OnSale);
					LoadOffers();
				}
			}
		}
	}
}
