using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace cp2.HealthChecks;

public class RemoteHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;

    public RemoteHealthCheck(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("http://localhost:5042/", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("Componente remoto está respondendo corretamente.");
            }
            else
            {
                return HealthCheckResult.Unhealthy("Falha ao se conectar ao componente remoto.");
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Exceção ao tentar se conectar ao componente remoto.", ex);
        }
    }
}
