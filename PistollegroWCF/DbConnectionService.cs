using PistollegroWCF.Entities;
using PistollegroWCF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PistollegroWCF
{
	public class DbConnectionService : IDbConnectionService
	{

		private PistollegroDBContext db = new PistollegroDBContext();
		public CompanyMV loginOperation(string email, string password)
		{
			Company company = db.Companies.FirstOrDefault(cmp => cmp.Email.Equals(email));
			if (company != null)
			{
				string hashPasswd = company.Password;
				string decryptedPasswd = Cryptography.Decrypt(hashPasswd);
				if (decryptedPasswd.Equals(password))
					return convert(company);
				else
					return null;
			}
			else
				return null;

		}

		public bool changePassword(string organizationName, string oldPasswd, string newPasswd)
		{

			var original = db.Companies.Find(organizationName);
			if (original != null)
			{
				var updatedCompany = original;
				string hashPasswd = original.Password;
				string decryptedPasswd = Cryptography.Decrypt(hashPasswd);
				if (Cryptography.Decrypt(original.Password).Equals(oldPasswd))
				{
					updatedCompany.setPassword(newPasswd);
					db.Entry(original).CurrentValues.SetValues(updatedCompany);
					db.SaveChanges();
					return true;
				}
				else
					return false;
			}
			else
				return false;

		}

		public bool registerCompany(CompanyMV cmpny)
		{

			try
			{
				string passHash = Cryptography.Encrypt(cmpny.Password);
				Company company = new Company(
								cmpny.OrganizationName,
								cmpny.Address,
								cmpny.NIP,
								cmpny.Phone,
								cmpny.Email,
								passHash
								);
				db.Companies.Add(company);
				db.SaveChanges();
				return true;
			}

			catch (Exception e)
			{
				return false;
			}

		}

		public CompanyMV[] getCompanies()
		{
			CompanyMV[] companies;
			var cmpny = from company in db.Companies
						orderby company.OrganizationName
						select new CompanyMV()
						{
							OrganizationName = company.OrganizationName,
							Address = company.Address,
							NIP = company.NIP,
							Phone = company.Phone,
							Email = company.Email,
							Password = company.Password,

						};

			companies = cmpny.ToArray();
			return companies;

			//  ICollection<WeaponOffer> weaponOffers = db.WeaponOffers.Where(x => x.OrganiationName == company.OrganizationName);

			// company.WeaponOffers = weaponOffers;
		}
		public CompanyMV getCompany(string organizationName)
		{
			CompanyMV company = null;

			if (organizationName != null)
			{
				var offer = db.Companies.Find(organizationName);
				company = new CompanyMV(
								offer.OrganizationName,
								offer.Address,
								offer.NIP,
								offer.Phone,
								offer.Email,
								offer.Password);
			}
			return company;
		}

		private CompanyMV convert(Company company)
		{
			if (company != null)
			{
				return new CompanyMV(company.OrganizationName,
									  company.Address,
									  company.NIP,
									  company.Phone,
									  company.Email,
									  company.Password);
			}
			else return null;
		}

		public string[] GetCategories()
		{
			string[] categories;

			var db_cats = from category in db.WeaponCategories select category;
			var categoryList = new List<string>();
			foreach (var cat in db_cats)
			{
				categoryList.Add(cat.CategoryName);
			}
			categories = categoryList.ToArray();

			return categories;
		}

		public WeaponOfferMV[] GetAllOffers(string sortOrder = null)
		{
			WeaponOfferMV[] result;

			var off = from offer in db.WeaponOffers
					  orderby offer.Name
					  select new WeaponOfferMV()
					  {
						  CategoryName = offer.CategoryName,
						  OrganizationName = offer.OrganizationName,
						  Description = offer.Description,
						  ID = offer.ID,
						  HasPicture = offer.HasPicture,
						  Name = offer.Name,
						  Price = offer.Price
					  };

			switch (sortOrder)
			{
				case "name_desc":
					off = off.OrderByDescending(x => x.Name);
					break;
				case "Price":
					off = off.OrderBy(x => x.Price);
					break;
				case "price_desc":
					off = off.OrderByDescending(x => x.Price);
					break;
				case "category_desc":
					off = off.OrderByDescending(x => x.CategoryName);
					break;
				case "Category":
					off = off.OrderBy(x => x.CategoryName);
					break;
				case "description_desc":
					off = off.OrderByDescending(x => x.Description);
					break;
				case "Description":
					off = off.OrderBy(x => x.Description);
					break;

				default:
					off = off.OrderBy(x => x.Name);
					break;
			}

			result = off.ToArray();

			return result;
		}

		public WeaponOfferMV[] GetOffersOfCategories(string[] categoryNames)
		{
			WeaponOfferMV[] result;

			var off = from offer in db.WeaponOffers
					  orderby offer.Name
					  where categoryNames.Contains(offer.CategoryName)
					  select new WeaponOfferMV(
						  offer.ID,
						  offer.Price,
						  offer.Name,
						  offer.Description,
						  offer.HasPicture,
						  offer.CategoryName,
						  offer.OrganizationName
					  );


			result = off.ToArray();

			return result;
		}

		public WeaponOfferMV[] GetOffersOfCompany(string companyName, string sortOrder = null)
		{
			WeaponOfferMV[] result;
			var offs = db.WeaponOffers
							.Where(x => x.OrganizationName == companyName)
							.ToList()
							.Select(x => new WeaponOfferMV
							(x.ID,
								x.Price,
								x.Name,
								x.Description,
								x.HasPicture,
								x.CategoryName,
								x.OrganizationName
							));

			switch (sortOrder)
			{
				case "name_desc":
					offs = offs.OrderByDescending(x => x.Name);
					break;
				case "Price":
					offs = offs.OrderBy(x => x.Price);
					break;
				case "price_desc":
					offs = offs.OrderByDescending(x => x.Price);
					break;
				case "category_desc":
					offs = offs.OrderByDescending(x => x.CategoryName);
					break;
				case "Category":
					offs = offs.OrderBy(x => x.CategoryName);
					break;
				case "description_desc":
					offs = offs.OrderByDescending(x => x.Description);
					break;
				case "Description":
					offs = offs.OrderBy(x => x.Description);
					break;

				default:
					offs = offs.OrderBy(x => x.Name);
					break;
			}
			result = offs.ToArray();

			return result;
		}




		public WeaponOfferMV GetOffer(int? ID)
		{
			WeaponOfferMV result = null;
			if (ID != null)
			{
				var offer = db.WeaponOffers.Find(ID);
				if (offer != null)
				{
					result = new WeaponOfferMV(offer.ID, offer.Price, offer.Name, offer.Description, offer.HasPicture, offer.CategoryName, offer.OrganizationName);
				}
			}
			return result;
		}

		public int? AddOffer(WeaponOfferMV toAdd)
		{
			try
			{
				WeaponOffer offer = new WeaponOffer(
								  toAdd.ID,
								  toAdd.Price,
								  toAdd.Name,
								  toAdd.Description,
								  toAdd.HasPicture,
								  toAdd.CategoryName,
								  toAdd.OrganizationName
							  );

				var cat = db.WeaponCategories.Where(x => x.CategoryName == offer.CategoryName).First();
				offer.WeaponCategory = cat;
				var company = db.Companies.Where(x => x.OrganizationName == offer.OrganizationName).First();
				offer.Company = company;
				db.WeaponOffers.Add(offer);
				db.SaveChanges();
				return offer.ID;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public void UpdateOffer(WeaponOfferMV update)
		{
			var offer = new WeaponOffer(
							  update.ID,
							  update.Price,
							  update.Name,
							  update.Description,
							  update.HasPicture,
							  update.CategoryName,
							  update.OrganizationName
						  );

			var currentOffer = db.WeaponOffers.AsNoTracking().Where(x => x.ID == update.ID).First();
			db.WeaponOffers.Attach(offer);

			var cat = db.WeaponCategories.Where(x => x.CategoryName == offer.CategoryName).First();
			offer.WeaponCategory = cat;

			var company = db.Companies.Where(x => x.OrganizationName == offer.OrganizationName).First();
			offer.Company = company;
			db.Entry(offer).State = EntityState.Modified;
			db.SaveChanges();
		}

		public void DeleteOffer(WeaponOfferMV toDelete)
		{
			DeleteOfferById(toDelete.ID);
		}

		public void DeleteOfferById(int? offerToDeleteID)
		{
			if (offerToDeleteID != null)
			{
				var offer = db.WeaponOffers.Attach(new WeaponOffer() { ID = (int)offerToDeleteID });
				db.WeaponOffers.Remove(offer);
				db.SaveChanges();
			}
		}

		public void ApplyWeaponOffer(int WeaponOfferID)
		{
			var x = db.WeaponOffers.Find(WeaponOfferID);
			var onSale = new WeaponOnSale()
			{
				ID = x.ID,
				Price = x.Price,
				Name = x.Name,
				Description = x.Description,
				HasPicture = x.HasPicture,
				CategoryName = x.CategoryName,
				WeaponCategory = x.WeaponCategory,
				OrganizationName = x.OrganizationName,
				CountAvailable = 0,
				Company = x.Company
			};
			db.WeaponOffers.Remove(x);
			db.WeaponsOnSale.Add(onSale);
			db.SaveChanges();
		}

		public WeaponOnSaleMV[] GetAllOnSale(string orderSort = null)
		{
			WeaponOnSaleMV[] result;
			var offs = db.WeaponsOnSale
							.Select(x => new WeaponOnSaleMV()
							{
								ID = x.ID,
								Price = x.Price,
								Name = x.Name,
								Description = x.Description,
								HasPicture = x.HasPicture,
								CategoryName = x.CategoryName,
								OrganizationName = x.OrganizationName,
								ItemsAvailableCount = x.CountAvailable
							});

			switch (orderSort)
			{
				case "name_desc":
					offs = offs.OrderByDescending(x => x.Name);
					break;
				case "Price":
					offs = offs.OrderBy(x => x.Price);
					break;
				case "price_desc":
					offs = offs.OrderByDescending(x => x.Price);
					break;
				case "category_desc":
					offs = offs.OrderByDescending(x => x.CategoryName);
					break;
				case "Category":
					offs = offs.OrderBy(x => x.CategoryName);
					break;
				case "description_desc":
					offs = offs.OrderByDescending(x => x.Description);
					break;
				case "Description":
					offs = offs.OrderBy(x => x.Description);
					break;

				default:
					offs = offs.OrderBy(x => x.Name);
					break;
			}
			result = offs.ToArray();

			return result;
		}

		public WeaponOnSaleMV GetOnSale(int ID)
		{
			var x = db.WeaponsOnSale.Find(ID);
			return new WeaponOnSaleMV()
			{
				ID = x.ID,
				Price = x.Price,
				Name = x.Name,
				Description = x.Description,
				HasPicture = x.HasPicture,
				CategoryName = x.CategoryName,
				OrganizationName = x.OrganizationName,
				ItemsAvailableCount = x.CountAvailable
			};
		}

		public void EditOnSale(WeaponOnSaleMV onSale)
		{
			var managed = db.WeaponsOnSale.Find(onSale.ID);
			managed.Name = onSale.Name;
			managed.Price = onSale.Price;
			if (managed.CategoryName != onSale.CategoryName)
			{
				managed.CategoryName = onSale.CategoryName;
				managed.WeaponCategory = db.WeaponCategories.Find(managed.CategoryName);
			}
			managed.Description = onSale.Description;
			managed.CountAvailable = onSale.ItemsAvailableCount;
			db.Entry(managed).State = EntityState.Modified;
			db.SaveChanges();
		}

		public void OrderMore(int onSaleId, int? orderCount)
		{
			if (orderCount != null && orderCount > 0)
			{
				ShipmentOrder so = new ShipmentOrder()
				{
					Count = (int)orderCount,
					OnSaleId = onSaleId,
					WeaponOnSale = db.WeaponsOnSale.Find(onSaleId)
				};
				db.ShipmentOrders.Add(so);
				db.SaveChanges();
			}
		}

		public void DeleteFromSaleById(int onSaleId)
		{
			var onSale = db.WeaponsOnSale.Attach(new WeaponOnSale() { ID = onSaleId });
			db.WeaponsOnSale.Remove(onSale);
			db.SaveChanges();
		}

		public ShipmentOrderMV[] GetShipmentsForCompany(String CompanyName)
		{
			return db.ShipmentOrders
				.Where(x => x.WeaponOnSale.OrganizationName == CompanyName)
				.Select(x => new ShipmentOrderMV()
				{
					Count = x.Count,
					ID = x.ID,
					WeaponOnSale = new WeaponOnSaleMV()
					{
						ID = x.WeaponOnSale.ID,
						Price = x.WeaponOnSale.Price,
						Name = x.WeaponOnSale.Name,
						Description = x.WeaponOnSale.Description,
						HasPicture = x.WeaponOnSale.HasPicture,
						CategoryName = x.WeaponOnSale.CategoryName,
						OrganizationName = x.WeaponOnSale.OrganizationName,
						ItemsAvailableCount = x.WeaponOnSale.CountAvailable
					},
				})
				.ToArray();
		}

		public void FulfillShipment(string CompanyName, int shipmentId)
		{
			var shipment = db.ShipmentOrders
				.Find(shipmentId);
			if (shipment != null)
			{
				var weaponOnSale = shipment.WeaponOnSale;
				if (weaponOnSale.OrganizationName == CompanyName)
				{
					//var weaponOnSale = db.WeaponsOnSale.Attach(new WeaponOnSale()
					//{
					//	CountAvailable = shipment.WeaponOnSale.CountAvailable + shipment.Count,
					//	ID = shipment.WeaponOnSale.ID
					//});
					db.Entry(weaponOnSale).Reference(x => x.WeaponCategory).Load();
					db.Entry(weaponOnSale).Reference(x => x.Company).Load();
					weaponOnSale.CountAvailable += shipment.Count;
					db.Entry(weaponOnSale).Property(x => x.CountAvailable).IsModified = true;
					db.ShipmentOrders.Remove(shipment);
					db.SaveChanges();
				}
			}
		}

		public ShipmentOrderMV[] GetAllShipments()
		{
			return db.ShipmentOrders
				.Select(x => new ShipmentOrderMV()
				{
					Count = x.Count,
					ID = x.ID,
					WeaponOnSale = new WeaponOnSaleMV()
					{
						ID = x.WeaponOnSale.ID,
						Price = x.WeaponOnSale.Price,
						Name = x.WeaponOnSale.Name,
						Description = x.WeaponOnSale.Description,
						HasPicture = x.WeaponOnSale.HasPicture,
						CategoryName = x.WeaponOnSale.CategoryName,
						OrganizationName = x.WeaponOnSale.OrganizationName,
						ItemsAvailableCount = x.WeaponOnSale.CountAvailable
					},
				})
				.ToArray();
		}

		public void DeleteShipmentOfferById(int shipmentOfferId)
		{
			try
			{
				var shipmentOrder = db.ShipmentOrders.Attach(new ShipmentOrder() { ID = shipmentOfferId });
				db.ShipmentOrders.Remove(shipmentOrder);
				db.SaveChanges();
			}
			catch (Exception e)
			{
				//TODO
			}
		}

		public WeaponOnSaleMV[] GetOnSaleOfCompany(string companyName)
		{
			return db.WeaponsOnSale
				.Where(x => x.Company.OrganizationName == companyName)
							.Select(x => new WeaponOnSaleMV()
							{
								ID = x.ID,
								Price = x.Price,
								Name = x.Name,
								Description = x.Description,
								HasPicture = x.HasPicture,
								CategoryName = x.CategoryName,
								OrganizationName = x.OrganizationName,
								ItemsAvailableCount = x.CountAvailable
							}).ToArray();
		}

		~DbConnectionService()
		{
			db.Dispose();
		}
	}
}
