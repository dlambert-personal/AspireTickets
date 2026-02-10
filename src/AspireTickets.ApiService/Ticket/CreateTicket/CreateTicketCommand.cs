using AspireTickets.ApiService.Data;
using TodoStates.Shared.CQRS;

namespace AspireTickets.ApiService.Ticket.CreateTicket;

internal class CreateTicketCommandHandler(TicketContext context) : ICommandHandler<CreateTicketCommand, CreateTicketResponse>
{
    public async Task<CreateTicketResponse> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        //todo: implement mapster
        var ticketItem = new TicketItem
        {
            Id = Guid.NewGuid(),
            Subject = command.Subject,
            Description = command.Description,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = command.CreatedBy   
        };

        context.TicketItems.Add(ticketItem);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateTicketResponse(ticketItem.Id);
    }
}
