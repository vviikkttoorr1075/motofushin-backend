using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motofushin.Roadmap.Domain.DimRoute.Entities;

namespace Motofushin.Roadmap.Infrastructure.Persistence;

/// <summary>
/// EF Core context for the <c>scraper_dwh</c> schema.
/// </summary>
public class ScraperDwhDbContext : DbContext
{
	public ScraperDwhDbContext(DbContextOptions<ScraperDwhDbContext> options)
		: base(options)
	{
	}

	public DbSet<DimRoute> DimRoutes => Set<DimRoute>();

	public DbSet<RoutePoint> RoutePoints => Set<RoutePoint>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("scraper_dwh");

		modelBuilder.Entity<DimRoute>(ConfigureDimRoute);
		modelBuilder.Entity<RoutePoint>(ConfigureRoutePoint);
	}

	private static void ConfigureDimRoute(EntityTypeBuilder<DimRoute> builder)
	{
		builder.ToTable("dim_route");

		builder.HasKey(r => r.Id);

		builder.Property(r => r.Id)
			.ValueGeneratedNever();

		builder.Property(r => r.Source)
			.IsRequired()
			.HasColumnType("text");

		builder.Property(r => r.ExternalId)
			.IsRequired()
			.HasColumnType("text");

		builder.Property(r => r.Name)
			.HasColumnType("text");

		builder.Property(r => r.LengthM)
			.HasColumnType("numeric");

		builder.Property(r => r.PointCount);

		builder.Property(r => r.CreatedAt)
			.IsRequired()
			.HasColumnType("timestamptz")
			.HasDefaultValueSql("now()");

		builder.HasIndex(r => new { r.Source, r.ExternalId })
			.IsUnique()
			.HasDatabaseName("ux_dim_route_source_external_id");
	}

	private static void ConfigureRoutePoint(EntityTypeBuilder<RoutePoint> builder)
	{
		builder.ToTable("route_point");

		builder.HasKey(p => new { p.Id, p.Seq });

		builder.ComplexProperty(p => p.Position, position =>
		{
			position.Property(c => c.Lat)
				.HasColumnName("Lat")
				.IsRequired()
				.HasColumnType("numeric(9,6)");

			position.Property(c => c.Lon)
				.HasColumnName("Lon")
				.IsRequired()
				.HasColumnType("numeric(9,6)");

			position.Property(c => c.ElevationM)
				.HasColumnName("ElevationM")
				.HasColumnType("numeric(7,1)");
		});

		builder.Property(p => p.RecordedAt)
			.HasColumnType("timestamptz");

		builder.HasOne(p => p.Route)
			.WithMany(r => r.Points)
			.HasForeignKey(p => p.Id)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
