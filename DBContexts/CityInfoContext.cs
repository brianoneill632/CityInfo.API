using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DBContexts
{
    public class CityInfoContext(DbContextOptions<CityInfoContext> options) : DbContext(options)
    {
        public DbSet<Entities.City> Cities { get; set; } = null!;
        public DbSet<Entities.PointOfInterest> PointsOfInterest { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.City>().HasData(
                new Entities.City("New York City")
                {
                    Id = 1,
                    Description = "The one with that big park."
                },
                new Entities.City("Antwerp")
                {
                    Id = 2,
                    Description = "The one with the cathedral that was never really finished."
                },
                new Entities.City("Paris")
                {
                    Id = 3,
                    Description = "The one with that big tower."
                }
            );
            modelBuilder.Entity<Entities.PointOfInterest>().HasData(
                new Entities.PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "The most visited urban park in the United States."
                },
                new Entities.PointOfInterest("Empire State Building")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "A 102-story skyscraper located in Midtown Manhattan."
                },
                new Entities.PointOfInterest("Cathedral of Our Lady")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                },
                new Entities.PointOfInterest("Antwerp Central Station")
                {
                    Id = 4,
                    CityId = 2,
                    Description = "The the finest example of railway architecture in Belgium."
                },
                new Entities.PointOfInterest("Eiffel Tower")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
                },
                new Entities.PointOfInterest("The Louvre")
                {
                    Id = 6,
                    CityId = 3,
                    Description = "The world's largest museum."
                }
            );
        }
    }
}
