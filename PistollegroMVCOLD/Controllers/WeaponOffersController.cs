using System.Net;
using System.Web;
using System.Web.Mvc;
using PistollegroMVC.Utils;
using System.Configuration;
using System.IO;
using PistollegroMVC.PistollegroWCF;

namespace PistollegroMVC.Controllers
{
	public class WeaponOffersController : Controller
	{
		private DbConnectionServiceClient service = new DbConnectionServiceClient();
		private ServiceSettingsManager config = (ServiceSettingsManager)ConfigurationManager.GetSection("pistollegroSettingsGroup/pistollegroSettings");
		// GET: WeaponOffers
		public ActionResult Index()
		{
			ViewBag.picturesDirectory = config.picturesDirectory;
			ViewBag.thumbmailSuffix = config.thumbmailSuffix;
			ViewBag.defaultPicture = config.defaultPhotoFilename;
			return View(service.GetAllOffers("name_desc"));
		}

		// GET: WeaponOffers/Create
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
			return View(weaponOffer);
		}

		// GET: WeaponOffers/Edit/5
		public ActionResult Edit(int? id)
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
			ViewBag.categories = service.GetCategories();
			return View(weaponOffer);
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
				int id = weaponOffer.ID;
				if (file != null && file.ContentLength > 0)
				{
					string photoDir = Path.Combine(Server.MapPath("~/"), config.picturesDirectory.Replace("/", @"\\"));
					var im = new ImageManager(file, photoDir, id);
					im.SavePictureAndThumbmail();
					weaponOffer.HasPicture = true;
				}
				weaponOffer.OrganizationName = User.Identity.Name;
				service.UpdateOffer(weaponOffer);
				return RedirectToAction("Index");
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
			return View(weaponOffer);
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

		public ActionResult UserOffers()
		{
			ViewBag.picturesDirectory = config.picturesDirectory;
			ViewBag.thumbmailSuffix = config.thumbmailSuffix;
			ViewBag.defaultPicture = config.defaultPhotoFilename;
			return View(service.GetOffersOfCompany(User.Identity.Name, "name_desc"));
		}



	}
}
