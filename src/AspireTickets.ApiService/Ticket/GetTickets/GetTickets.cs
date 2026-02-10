using AspireTickets.Shared.Pagination;
using TodoStates.Shared.CQRS;

namespace AspireTickets.ApiService.Ticket.GetTickets;
public class GetTicketsQuery(PaginationRequest request) : IQuery<GetTicketsResponse>
{
    public PaginationRequest Request { get; } = request;
}
// todo: consider use of DTO here
public record GetTicketsResponse(PaginatedResult<TicketItem> Tickets);