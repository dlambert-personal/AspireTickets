

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireTickets_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    // keep the swagger deep-link entries
    .WithUrlForEndpoint("https", ep => new() { Url = "/swagger", DisplayText = "Swagger" })
    .WithUrlForEndpoint("http",  ep => new() { Url = "/swagger", DisplayText = "Swagger" })
    // then remove the auto-generated base URLs and keep only the swagger entries
    .WithUrls(c =>
    {
        var keep = c.Urls.Where(u =>
            string.Equals(u.DisplayText, "Swagger", StringComparison.OrdinalIgnoreCase)
            || (u.Url is not null && u.Url.EndsWith("/swagger", StringComparison.OrdinalIgnoreCase)))
            .ToList();

        c.Urls.Clear();
        foreach (var u in keep)
        {
            c.Urls.Add(u);
        }
    });

builder.AddProject<Projects.AspireTickets_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
