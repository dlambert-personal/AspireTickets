namespace AspireTickets.ApiService.Models
{
    public class TicketItem
    {
        [Key]
        public Guid Id { get; set; }
        public string Subject { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime CreatedDate { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
    }
}
