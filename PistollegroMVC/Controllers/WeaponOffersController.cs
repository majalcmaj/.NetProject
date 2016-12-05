using System.Net;
using System.Web;
using System.Web.Mvc;
using PistollegroMVC.Utils;
using System.Configuration;
using System.IO;
using PistollegroMVC.PistollegroWCF;
using System;

namespace PistollegroMVC.Controllers
{
	public class WeaponOffersController : Controller
	{
		private DbConnectionServiceClient service = new DbConnectionServiceClient();
		private ServiceSettingsManager config = (ServiceSettingsManager)ConfigurationManager.GetSection("pistollegroSettingsGroup/pistollegroSettings");
		// GET: WeaponOffers
		public ActionResult Index(string sortOrder)
		{
			ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.PriceSortParam = sortOrder == "Price" ? "price_desc" : "Price";
			ViewBag.DescriptionSortParam = sortOrder == "Description" ? "description_desc" : "Description";
			ViewBag.CategorySortParam = sortOrder == "Category" ? "category_desc" : "Category";


			ViewBag.picturesDirectory = config.picturesDirectory;
			ViewBag.thumbmailSuffix = config.thumbmailSuffix;
			ViewBag.defaultPicture = config.defaultPhotoFilename;
			var offers = service.GetAllOffers(sortOrder);
			if (offers != null)
				return View(offers);
			else
				return HttpNotFound();
		}

		// GET: WeaponOffers/Create
		[Authorize]
		public ActionResult Create()
		{
			ViewBag.categories = service.GetCategories();
			return View();
		}

		// POST: WeaponOffers/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]

		public ActionResult Create([Bind(Include = "ID,Price,Name,Description,CategoryName")] WeaponOfferMV weaponOffer, HttpPostedFileBase file)
		{
			if (ModelState.IsValid)
			{
				bool HasPicture = file != null && file.ContentLength > 0;
				weaponOffer.HasPicture = HasPicture;
				weaponOffer.OrganizationName = User.Identity.Name;
				int? id = service.AddOffer(weaponOffer);
				if (HasPicture)
				{
					string photoDir = Path.Combine(Server.MapPath("~/"), config.picturesDirectory.Replace("/", @"\\"));
					var im = new ImageManager(file, photoDir, id);
					im.SavePictureAndThumbmail();
				}
				return RedirectToAction("Index");
			}

			ViewBag.categories = service.GetCategories();
			if (weaponOffer != null)
				return View(weaponOffer);
			else
				return HttpNotFound();
		}

		// GET: WeaponOffers/Edit/5
		public ActionResult Edit(int? id)
		{
			ViewBag.categories = service.GetCategories();
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			WeaponOfferMV weaponOffer = service.GetOffer(id);
			if (weaponOffer == null)
			{
				return HttpNotFound();
			}
			if (weaponOffer.OrganizationName.Equals(User.Identity.Name))
				return View(weaponOffer);
			else
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
		}

		// POST: WeaponOffers/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,Price,Name,Description,CategoryName,HasPicture")] WeaponOfferMV weaponOffer, HttpPostedFileBase file)
		{
			if (ModelState.IsValid)
			{
				var managedOffer = service.GetOffer(weaponOffer.ID);
				if (managedOffer.OrganizationName.Equals(User.Identity.Name))
				{
					int id = managedOffer.ID;
					if (file != null && file.ContentLength > 0)
					{
						string photoDir = Path.Combine(Server.MapPath("~/"), config.picturesDirectory.Replace("/", @"\\"));
						var im = new ImageManager(file, photoDir, id);
						im.SavePictureAndThumbmail();
						managedOffer.HasPicture = true;
					}
					//weaponOffer.OrganizationName = User.Identity.Name;
					managedOffer.Price = weaponOffer.Price;
					managedOffer.Name = weaponOffer.Name;
					managedOffer.Description = weaponOffer.Description;
					managedOffer.CategoryName = weaponOffer.CategoryName;
					service.UpdateOffer(managedOffer);
					return RedirectToAction("Index");
				}
				else return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}

			ViewBag.categories = service.GetCategories();
			return View(weaponOffer);
		}

		// GET: WeaponOffers/Delete/5
		public ActionResult Delete(int? id)
		{

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			WeaponOfferMV weaponOffer = service.GetOffer(id);
			if (weaponOffer == null)
			{
				return HttpNotFound();
			}
			if (weaponOffer.OrganizationName.Equals(User.Identity.Name))
				return View(weaponOffer);
			else return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
		}

		// POST: WeaponOffers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			service.DeleteOfferById(id);
			return RedirectToAction("Index");
		}


		[Authorize]
		public ActionResult UserOffers(string sortOrder)
		{
			ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.PriceSortParam = sortOrder == "Price" ? "price_desc" : "Price";
			ViewBag.DescriptionSortParam = sortOrder == "Description" ? "description_desc" : "Description";
			ViewBag.CategorySortParam = sortOrder == "Category" ? "category_desc" : "Category";



			ViewBag.picturesDirectory = config.picturesDirectory;
			ViewBag.thumbmailSuffix = config.thumbmailSuffix;
			ViewBag.defaultPicture = config.defaultPhotoFilename;
			var offers = service.GetOffersOfCompany(User.Identity.Name, sortOrder);
			if (offers != null)
				return View(offers);
			else
				return HttpNotFound();
		}

		[Authorize]
		public ActionResult UserContracts()
		{
			var onSale = service.GetOnSaleOfCompany(User.Identity.Name);
			if (onSale != null)
			{
				ViewBag.picturesDirectory = config.picturesDirectory;
				ViewBag.thumbmailSuffix = config.thumbmailSuffix;
				ViewBag.defaultPicture = config.defaultPhotoFilename;
				return View(onSale);
			}
			else
				return HttpNotFound();
		}

		[Authorize]
		public ActionResult UserShipmentOrders()
		{
			var Shipments = service.GetShipmentsForCompany(User.Identity.Name);
			if (Shipments != null)
			{
				return View(Shipments);
			}
			else
				return HttpNotFound();
		}

		public ActionResult FulfillOrder(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			service.FulfillShipment(User.Identity.Name, (int)id);
			return RedirectToAction("Index");
		}
	}
}
