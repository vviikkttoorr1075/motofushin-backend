using Microsoft.EntityFrameworkCore;
using Motofushin.Roadmap.Domain.DimRoute;
using Motofushin.Roadmap.Domain.DimRoute.Entities;

namespace Motofushin.Roadmap.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core based read access to <see cref="DimRoute"/> rows of the scraper data warehouse.
/// </summary>
/// <param name="context">The <see cref="ScraperDwhDbContext"/> used for querying.</param>
public class DimRouteRepository(ScraperDwhDbContext context) : IDimRouteRepository
{
	public async Task<IReadOnlyList<IDimRoute>> GetAllAsync(CancellationToken cancellationToken = default) =>
		await context.DimRoutes
			.AsNoTracking()
			.OrderBy(r => r.Id)
			.ToListAsync(cancellationToken);

	public async Task<IDimRoute?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
		await context.DimRoutes
			.AsNoTracking()
			.Include(r => r.Points)
			.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);

	public async Task<IDimRoute?> GetWithPointsByIdAsync(int id, CancellationToken cancellationToken = default) =>
		await context.DimRoutes
			.AsNoTracking()
			.Include(r => r.Points)
			.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);

	public async Task<IDimRoute?> GetBySourceAndExternalIdAsync(
		string source,
		string externalId,
		CancellationToken cancellationToken = default) =>
		await context.DimRoutes
			.AsNoTracking()
			.SingleOrDefaultAsync(
				r => r.Source == source && r.ExternalId == externalId,
				cancellationToken);
}
