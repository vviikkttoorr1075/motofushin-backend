using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Motofushin.App.Application.Features.Gpx;
using Motofushin.App.Infrastructure.Persistence;
using Motofushin.App.Infrastructure.Services;

namespace Motofushin.App.Infrastructure.DependencyInjection;

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

		services.AddSingleton<IGpxTrackStore>(sp =>
				new FileSystemGpxTrackStore(assetsPath));

		services.AddDbContext<ScraperDwhDbContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("Postgres")));

		return services;
	}
}
