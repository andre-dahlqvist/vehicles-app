using Microsoft.EntityFrameworkCore;
using VehiclesApi.Models;

namespace VehiclesApi.Data
{
	public class VehiclesDbContext : DbContext
	{
		public VehiclesDbContext(DbContextOptions<VehiclesDbContext> options) : base(options)
		{
		}

		public DbSet<Vehicle> Vehicles { get; set; }

		public DbSet<Equipment> Equipment { get; set; }

		public DbSet<VehicleEquipment> VehicleEquipment { get; set; }

		public DbSet<Brand> Brands { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<VehicleEquipment>()
				.HasKey(ve => new { ve.Vin, ve.EquipmentId });

			modelBuilder.Entity<VehicleEquipment>()
				.HasOne(ve => ve.Vehicle)
				.WithMany(v => v.VehicleEquipment)
				.HasForeignKey(ve => ve.Vin);

			modelBuilder.Entity<VehicleEquipment>()
				.HasOne(ve => ve.Equipment)
				.WithMany()
				.HasForeignKey(ve => ve.EquipmentId);

			// Seed some common car brands and equipment just so we always have some data
			// in a freshly created database.
			modelBuilder.Entity<Brand>().HasData(
				  new Brand { Id = 1, Name = "Toyota" },
				  new Brand { Id = 2, Name = "General Motors" },
				  new Brand { Id = 3, Name = "BMW" },
				  new Brand { Id = 4, Name = "Tesla" },
				  new Brand { Id = 5, Name = "Hyundai" }
			);

			modelBuilder.Entity<Equipment>().HasData(
				  new Equipment { Id = 1, Name = "Parking sensor" },
				  new Equipment { Id = 2, Name = "Rear view camera" },
				  new Equipment { Id = 3, Name = "Leather seats" }
			);
		}
	}
}
