using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PistollegroWCF.Entities
{
	public class WeaponInfo
	{
		public WeaponInfo(int iD, int price, string name, string description, bool hasPicture, string categoryName, string organizationName)
		{
			ID = iD;
			Price = price;
			Name = name;
			Description = description;
			HasPicture = hasPicture;
			CategoryName = categoryName;
			OrganizationName = organizationName;
		}

		public WeaponInfo() { }

		public int ID { get; set; }
		public int Price { get; set; }
		public String Name { get; set; }
		public String Description { get; set; }
		public bool HasPicture { get; set; }
		public string CategoryName { get; set; }
		public string OrganizationName { get; set; }

		[Required]
		[ForeignKey("CategoryName")]
		public virtual WeaponCategory WeaponCategory { get; set; }

		[Required]
		[ForeignKey("OrganizationName")]
		public virtual Company Company { get; set; }
	}
}
