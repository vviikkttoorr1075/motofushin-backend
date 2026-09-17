using Microsoft.Extensions.DependencyInjection;
using Motofushin.Application.Features.Gpx;
using Motofushin.Infrastructure.Services;

namespace Motofushin.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
	/// <summary>
	/// Registers file-system based implementations of application services.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="assetsPath">Absolute path of the directory holding GPX assets.</param>
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, string assetsPath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(assetsPath);

		services.AddSingleton<IGpxTrackStore>(sp =>
				new FileSystemGpxTrackStore(assetsPath));

		return services;
	}
}