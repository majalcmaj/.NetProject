using PistollegroWCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PistollegroWCF
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IDbConnectionService
	{
		// Account - related contracts
		[OperationContract]
		CompanyMV loginOperation(string email, string password);
		[OperationContract]
		bool changePassword(string organizationName, string oldPasswd, string newPasswd);
		[OperationContract]
		bool registerCompany(CompanyMV cmpny);
		[OperationContract]
		CompanyMV getCompany(string organizationName);

		[OperationContract]
		CompanyMV[] getCompanies();
		// Shop operations - related contracts
		[OperationContract]
		string[] GetCategories();

		[OperationContract]
		WeaponOfferMV[] GetAllOffers(string orderSort=null);

		[OperationContract]
		WeaponOfferMV[] GetOffersOfCategories(string[] categoryNames);

		[OperationContract]
		WeaponOfferMV[] GetOffersOfCompany(string companyName, string orderSort=null);
		[OperationContract]
		WeaponOfferMV GetOffer(int? id);

		[OperationContract]
		int? AddOffer(WeaponOfferMV offerToAdd);

		[OperationContract]
		void UpdateOffer(WeaponOfferMV offerToUpdate);

		[OperationContract]
		void DeleteOffer(WeaponOfferMV offerToDelete);

		[OperationContract]
		void DeleteOfferById(int? offerToDeleteID);

		[OperationContract]
		ShipmentOrderMV[] GetShipmentsForCompany(String CompanyName);

		[OperationContract]
		ShipmentOrderMV[] GetAllShipments();

		[OperationContract]
		void FulfillShipment(String CompanyName, int shipmentId);

		[OperationContract]
		void DeleteShipmentOfferById(int shipmentOfferId);

		//Seller side
		[OperationContract]
		void ApplyWeaponOffer(int WeaponOfferID);

		[OperationContract]
		WeaponOnSaleMV[] GetAllOnSale(string orderSort = null);

		[OperationContract]
		WeaponOnSaleMV[] GetOnSaleOfCompany(string companyName);

		[OperationContract]
		WeaponOnSaleMV GetOnSale(int ID);

		[OperationContract]
		void EditOnSale(WeaponOnSaleMV onSale);

		[OperationContract]
		void OrderMore(int onSaleId, int? orderCount);

		[OperationContract]
		void DeleteFromSaleById(int onSaleId);

	}

	[DataContract]
	public class WeaponOfferMV
	{
		public WeaponOfferMV(int iD, int price, string name, string description, bool hasPicture, string categoryName, string orgniazationName)
		{
			ID = iD;
			Price = price;
			Name = name;
			Description = description;
			HasPicture = hasPicture;
			CategoryName = categoryName;
			OrganizationName = orgniazationName;

		}

		public WeaponOfferMV() { }

		[DataMember]
		public int ID { get; set; }
		[DataMember]
		public int Price { get; set; }
		[DataMember]
		public String Name { get; set; }
		[DataMember]
		public String Description { get; set; }
		[DataMember]
		public bool HasPicture { get; set; }
		[DataMember]
		public string CategoryName { get; set; }
		[DataMember]
		public string OrganizationName { get; set; }
	}


	[DataContract]
	public class CompanyMV
	{
		public CompanyMV() { }
		public CompanyMV(string organizationName, string address, string nip, string phone, string email, string password)
		{
			OrganizationName = organizationName;
			Address = address;
			NIP = nip;
			Phone = phone;
			Email = email;
			Password = password;

		}
		[DataMember]
		public string OrganizationName { get; set; }

		[DataMember]
		public string Address { get; set; }

		[DataMember]
		public string NIP { get; set; }

		[DataMember]
		public string Phone { get; set; }

		[DataMember]
		public string Email { get; set; }

		[DataMember]
		public string Password { get; set; }

	}

	[DataContract]
	public class WeaponOnSaleMV
	{
		public WeaponOnSaleMV(int iD, int price, string name, string description, bool hasPicture, string categoryName, string orgniazationName, int availableCount)
		{
			ID = iD;
			Price = price;
			Name = name;
			Description = description;
			HasPicture = hasPicture;
			CategoryName = categoryName;
			OrganizationName = orgniazationName;
			ItemsAvailableCount = availableCount;
		}

		public WeaponOnSaleMV() { }

		[DataMember]
		public int ID { get; set; }
		[DataMember]
		public int Price { get; set; }
		[DataMember]
		public String Name { get; set; }
		[DataMember]
		public String Description { get; set; }
		[DataMember]
		public bool HasPicture { get; set; }
		[DataMember]
		public string CategoryName { get; set; }
		[DataMember]
		public string OrganizationName { get; set; }
		[DataMember]
		public int ItemsAvailableCount { get; set; }
	}

	[DataContract]
	public class ShipmentOrderMV
	{
		public ShipmentOrderMV(WeaponOnSaleMV weaponOnSale, int count)
		{
			this.WeaponOnSale = weaponOnSale;
			this.Count = count;
		}

		public ShipmentOrderMV() { }

		[DataMember]
		public int ID { get; set; }
		[DataMember]
		public WeaponOnSaleMV WeaponOnSale { get; set; }
		[DataMember]
		public int Count { get; set; }
	}
}
