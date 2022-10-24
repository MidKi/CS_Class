using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace MyBrick
{
    class Brick
    {
        public int Id { get; set; } //primary key

        [MaxLength(250)]
        public string Title { get; set; } = String.Empty;

        public Color? Color { get; set; } //peut être null apparemment

        //une brique peut avoir 0 ou plusieurs tags
        //reçoit un pointeur
        public List<Tag> Tags { get; set; } = new (); //pas besoin de préciser List<Tag> car c# sait

        public List<BrickAvailability> Availabilities { get; set; } = new ();
    }

    class BrickContext : DbContext
    {
        public BrickContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Brick> Bricks { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<BrickAvailability> BrickAvailabilities { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  //baseplate er minifighead héritent de brick (coté db)
        {
            modelBuilder.Entity<BasePlate>().HasBaseType<Brick>();
            modelBuilder.Entity<MinifigHead>().HasBaseType<Brick>();
        }
    }

    class BrickContextFabric : IDesignTimeDbContextFactory<BrickContext>
    {
        public BrickContext CreateDbContext(string[]? args = null)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var optionsBuilder = new DbContextOptionsBuilder<BrickContext>();

            optionsBuilder
                .UseLoggerFactory(LoggerFactory.Create(optionsBuilder => optionsBuilder.AddConsole()))
                .UseSqlServer(config["ConnectionStrings:DefaultConnection"]);

            return new BrickContext(optionsBuilder.Options);
        }
    }
}
