using System.Data.Entity;

namespace PistollegroWCF.Entities
{
	public class PistollegroDBContext : DbContext
	{
		// public WeaponShopContext(): base("name=DefaultConnection") { } // TODO: nazwa dbContext zmienic na PistollegroDBContext
		public DbSet<Company> Companies { get; set; }
		public DbSet<WeaponOffer> WeaponOffers { get; set; }
		public DbSet<WeaponCategory> WeaponCategories { get; set; }
	}
}