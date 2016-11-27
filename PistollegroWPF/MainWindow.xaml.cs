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
		public int Price { get { return WeaponOffer.Price; } }
		public String Name { get { return WeaponOffer.Name; } }
		public String Description { get { return WeaponOffer.Description; } }
		public String Category { get { return WeaponOffer.CategoryName; } }
		public String ImagePath {
			get { return String.Format(ServerPath, WeaponOffer.ID); }
		}
	}

	public partial class MainWindow : Window
	{
		private DbConnectionServiceClient service = new DbConnectionServiceClient();
		public MainWindow()
		{
			InitializeComponent();
			var offers = service.GetAllOffers(null);
			foreach(var offer in offers)
				CompanyOffers.Items.Add(new WeaponOfferPresenter(offer));
		}

		private void CompanyOffers_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
