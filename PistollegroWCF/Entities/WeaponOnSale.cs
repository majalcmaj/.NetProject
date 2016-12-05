using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PistollegroWCF.Entities
{
	public class WeaponOnSale : WeaponInfo
	{
		public WeaponOnSale() { }

		public WeaponOnSale(int iD, int price, string name, string description, bool hasPicture, string categoryName, string organizationName, int countAvailable) : 
			base(iD, price, name, description, hasPicture, categoryName, organizationName)
		{
			this.CountAvailable = countAvailable;
		}
		public int CountAvailable { get; set; }

		public virtual ICollection<ShipmentOrder> ShipmentOrders { get; set; }
	}
}
