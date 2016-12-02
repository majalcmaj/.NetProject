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
	public partial class OrderItemsDialog : Window
	{
		public OrderItemsDialog (String itemName)
		{
			InitializeComponent();
			ItemName = itemName;
			this.OrderCount = null;
			IsAccepted = false;
		}


		private String ItemName;
		
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ItemNameLabel.Content = ItemName;
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			var orderCount = Int32.Parse(OrderCountTextbox.Text);
			if(orderCount != 0)
			{
				IsAccepted = true;
				OrderCount = orderCount;
			}
			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		public bool IsAccepted { get; private set;}
		public int? OrderCount { get; private set;}
	}
}
