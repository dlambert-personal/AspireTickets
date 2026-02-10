using AspireTickets.ApiService.Data;
using TodoStates.Shared.CQRS;

namespace AspireTickets.ApiService.Ticket.EditTicket;

internal class EditTicketCommandHandler(TicketContext context) : ICommandHandler<EditTicketCommand, EditTicketResponse>
{
    public async Task<EditTicketResponse> Handle(EditTicketCommand command, CancellationToken cancellationToken)
    {
        var existing = await context.TicketItems.FindAsync(new object[] { command.Id }, cancellationToken);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Ticket with id {command.Id} not found");
        }

        existing.Subject = command.Subject;
        existing.Description = command.Description;
        existing.CreatedBy = command.CreatedBy;

        context.TicketItems.Update(existing);
        await context.SaveChangesAsync(cancellationToken);

        return new EditTicketResponse(existing.Id);
    }
}
