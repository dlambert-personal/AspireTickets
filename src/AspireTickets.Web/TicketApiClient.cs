using AspireTickets.Shared.Pagination;
using AspireTickets.Web.Components.Pages;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace AspireTickets.Web;

public class TicketApiClient(HttpClient httpClient, ILogger<TicketApiClient> logger)
{
    public async Task<TicketItem[]> GetTicketsAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<TicketItem>? tickets = null;
        var rawRes = await httpClient.GetAsync($"/tickets?PageNumber=0&PageSize={maxItems}", cancellationToken);
        var rawContent = await rawRes.Content.ReadAsStringAsync(cancellationToken);

        logger.LogDebug("GET /tickets returned {StatusCode}. Payload: {Payload}", rawRes.StatusCode, rawContent);

        if (!rawRes.IsSuccessStatusCode)
        {
            logger.LogWarning("Tickets API returned non-success status {StatusCode}", rawRes.StatusCode);
            // decide whether to return empty or throw
        }

        GetTicketsResponse? pagedRes = null;
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            pagedRes = JsonSerializer.Deserialize<GetTicketsResponse>(rawContent, options);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize GetTicketsResponse. Response JSON: {Json}", rawContent);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during deserialization of PaginatedResult<Ticket>. Response JSON: {Json}", rawContent);
            throw;
        }

        if (pagedRes?.Tickets is not null)
        {
            foreach (var ticket in pagedRes.Tickets.Data)
            {
                if (tickets?.Count >= maxItems)
                {
                    break;
                }
                if (ticket is not null)
                {
                    tickets ??= [];
                    tickets.Add(ticket);
                }
            }
        }

        return tickets?.ToArray() ?? [];
    }
}

public record GetTicketsResponse(PaginatedResult<TicketItem> Tickets);
public record TicketItem(Guid Id, string Subject, string Description, DateTime CreatedDate, string CreatedBy)
{
    // {"tickets":{"pageIndex":0,"pageSize":10,"count":1,"data":[{"id":"43c25b92-9684-4164-b1f2-483e4fed9eb1","subject":"New subject","description":"ticket description","createdDate":"2026-02-04T20:00:46.3055421","createdBy":"dlambert"}]}}
}
