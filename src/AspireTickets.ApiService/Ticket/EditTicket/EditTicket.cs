using TodoStates.Shared.CQRS;

namespace AspireTickets.ApiService.Ticket.EditTicket;

public record EditTicketRequest(string Subject, string Description, string CreatedBy);
public record EditTicketResponse(Guid Id);
public record EditTicketCommand(Guid Id, string Subject, string Description, string CreatedBy) : ICommand<EditTicketResponse>;
