using PistollegroWCF.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PistollegroWCF.Entities
{
	public partial class Company
	{
		public Company(string organizationName, string address, string nip, string phone, string email, string password)
		{
			OrganizationName = organizationName;
			Address = address;
			NIP = nip;
			Phone = phone;
			Email = email;
			Password = password;
		}
		public Company() { }

		[Key]
		[Required]
		public String OrganizationName { get; set; }

		[StringLength(80)]
		public String Address { get; set; }

		[Required]
		public String NIP { get; set; }
		[Required,
		DataType(DataType.PhoneNumber)]
		public String Phone { get; set; }

		[Required,
		DataType(DataType.EmailAddress),
		 Display(Name = "Email")]
		public String Email { get; set; }

		[Required]
		public String Password { get; set; }

		public void setPassword(String passwd)
		{
			Password = Cryptography.Encrypt(passwd);
		}
		public virtual ICollection<WeaponOffer> WeaponOffers { get; set; }
	}
}
