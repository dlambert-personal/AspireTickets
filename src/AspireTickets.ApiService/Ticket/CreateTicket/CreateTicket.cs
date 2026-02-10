using TodoStates.Shared.CQRS;

namespace AspireTickets.ApiService.Ticket.CreateTicket;

public record CreateTicketRequest(string Subject, string Description, string CreatedBy);
public record CreateTicketResponse(Guid Id);
public record CreateTicketCommand(string Subject, string Description, string CreatedBy) : ICommand<CreateTicketResponse>;