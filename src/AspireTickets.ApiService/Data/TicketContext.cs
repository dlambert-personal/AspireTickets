

namespace AspireTickets.ApiService.Data
{
    public class TicketContext : DbContext
    {
        public string DbPath { get; }
        public DbSet<TicketItem> TicketItems { get; set; } = default!;

        public TicketContext(DbContextOptions<TicketContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "ticket.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        //    modelBuilder.Entity<TicketItem>().HasData(
        //    new TicketItem
        //    {
        //        Id = Guid.Parse("e455f48f-35d1-4fa4-aaf1-4f7fcf5da22a"),
        //        Subject = "Problem with order #1234",
        //        Description = "I experienced an issue with my recent order.",
        //        CreatedDate = DateTime.UtcNow,
        //        CreatedBy = "Sam Sample"
        //    },
        //    new TicketItem
        //    {
        //        Id = Guid.Parse("0c30022b-8617-47ec-8e2d-6f327f507084"),
        //        Subject = "Account issue",
        //        Description = "Jane Smith's account has issues.",
        //        CreatedDate = DateTime.UtcNow,
        //        CreatedBy = "Sam Sample"
        //    }
        //);
        }
    }
}
