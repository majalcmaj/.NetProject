using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PistollegroWCF.Entities
{
	public class ShipmentOrder
	{
		public int ID { get; set; }
		public int Count { get; set; }
		public int OnSaleId { get; set; }

		[Required]
		[ForeignKey("OnSaleId")]
		public virtual WeaponOnSale WeaponOnSale { get; set; }
	}
}
