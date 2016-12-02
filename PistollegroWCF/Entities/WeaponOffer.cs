using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PistollegroWCF.Entities
{
	public class WeaponOffer: WeaponInfo
	{
		public WeaponOffer(int iD, int price, string name, string description, bool hasPicture, string categoryName, string organizationName) : 
			base(iD, price, name, description, hasPicture, categoryName, organizationName)
		{
			
		}

		public WeaponOffer() { }
	}
}
