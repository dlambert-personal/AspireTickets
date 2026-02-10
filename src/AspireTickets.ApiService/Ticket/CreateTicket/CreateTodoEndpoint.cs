using Mapster;
using TodoStates.Shared.CQRS;

namespace AspireTickets.ApiService.Ticket.CreateTicket;

public class CreateTodoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Ticketitems",
                async (
                    CreateTicketRequest request,
                    [FromServices] IDispatcher dispatcher,
                    CancellationToken cancellationToken) =>
                {
                    var command = request.Adapt<CreateTicketCommand>();
                    var result = await dispatcher.Send<CreateTicketCommand, CreateTicketResponse>(command, cancellationToken);

                    return Results.Created($"/Tickets/{result.Id}", result);
                })
            .WithName("CreateTicket")
            .Produces<CreateTicketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create ticket item")
            .WithDescription("Create ticket item");
    }
}
