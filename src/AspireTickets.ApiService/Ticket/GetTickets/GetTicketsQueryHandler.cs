using AspireTickets.ApiService.Data;
using AspireTickets.Shared.Pagination;
using TodoStates.Shared.CQRS;

namespace AspireTickets.ApiService.Ticket.GetTickets;
public class GetTicketsQueryHandler
(TicketContext context)
    : IQueryHandler<GetTicketsQuery, GetTicketsResponse>
{
    public async Task<GetTicketsResponse> Handle(GetTicketsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.Request.PageIndex;
        var pageSize = query.Request.PageSize;

        var totalCount = await context.TicketItems.LongCountAsync(cancellationToken);

        var tickets = await context.TicketItems
            .OrderBy(o => o.CreatedDate)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        //var customerDtos = tickets.ToTicketDtoList();


        return new GetTicketsResponse(
            new PaginatedResult<TicketItem>(
                pageIndex,
                pageSize,
                totalCount,
                tickets));   // not dtos yet
    }
}
