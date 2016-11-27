using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PistollegroWCF.Entities
{
	public class WeaponCategory
	{
		[Key]
		public String CategoryName { get; set; }
		public virtual ICollection<WeaponOffer> WeaponOffers { get; set; }
	}
}