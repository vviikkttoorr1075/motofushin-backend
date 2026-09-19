using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Motofushin.Roadmap.Application.Features.DimRoute;
using Motofushin.Roadmap.Domain.DimRoute;
using Motofushin.Roadmap.Infrastructure.Persistence;
using Motofushin.Roadmap.Infrastructure.Persistence.Repositories;

namespace Motofushin.Roadmap.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
	/// <summary>
	/// Registers file-system based implementations of application services
	/// and the PostgreSQL <c>scraper_dwh</c> database access.
	/// 
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="configuration">The application configuration (reads <c>ConnectionStrings:Postgres</c>).</param>
	/// <param name="assetsPath">Absolute path of the directory holding GPX assets.</param>
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration,
		string assetsPath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(assetsPath);

		services.AddScoped<IDimRouteGeoJsonService, DimRouteGeoJsonService>();

		services.AddDbContext<ScraperDwhDbContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("Postgres")));

		services.AddScoped<IDimRouteRepository, DimRouteRepository>();

		return services;
	}
}
