using Motofushin.Roadmap.Application.Features.DimRoute.Visitors;
using Motofushin.Roadmap.Domain.DimRoute;

namespace Motofushin.Roadmap.Application.Features.DimRoute;

/// <summary>
/// Loads <see cref="Domain.DimRoute.Entities.IDimRoute"/> data through
/// <see cref="IDimRouteRepository"/> and renders it as GeoJSON documents
/// using <see cref="RouteGeoJsonVisitor"/>.
/// </summary>
public sealed class DimRouteGeoJsonService(IDimRouteRepository routes) : IDimRouteGeoJsonService
{
	private readonly RouteGeoJsonVisitor _visitor = new();

	/// <inheritdoc/>
	public async Task<IReadOnlyList<string>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		var all = await routes.GetAllAsync(cancellationToken);
		return all.Select(route => _visitor.Visit(route)).ToList();
	}

	/// <inheritdoc/>
	public async Task<string?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		var route = await routes.GetWithPointsByIdAsync(id, cancellationToken);
		return route is null ? null : _visitor.Visit(route);
	}
}
