using Mapster;
using TodoStates.Shared.CQRS;

namespace AspireTickets.ApiService.Ticket.EditTicket;

public class EditTicketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/Ticketitems/{id}",
                async (
                    Guid id,
                    EditTicketRequest request,
                    [FromServices] IDispatcher dispatcher,
                    CancellationToken cancellationToken) =>
                {
                    var command = new EditTicketCommand(id, request.Subject, request.Description, request.CreatedBy);
                    var result = await dispatcher.Send<EditTicketCommand, EditTicketResponse>(command, cancellationToken);

                    return Results.Ok(result);
                })
            .WithName("EditTicket")
            .Produces<EditTicketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Edit ticket item")
            .WithDescription("Edit ticket item");
    }
}
