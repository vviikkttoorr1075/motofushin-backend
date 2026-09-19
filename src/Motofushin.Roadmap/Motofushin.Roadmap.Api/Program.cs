using Motofushin.Roadmap.Application.Features.DimRoute;
using Motofushin.Roadmap.Infrastructure.DependencyInjection;

namespace Motofushin.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();
			builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.ContentRootPath);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.MapGet("/routes/geojson", async Task<IResult> (
			IDimRouteGeoJsonService service,
			CancellationToken ct) =>
			{
				var documents = await service.GetAllAsync(ct);
				return TypedResults.Ok(documents);
			})
			.WithName("GetRouteGeoJsons");

		app.MapGet("/routes/{id:int}/geojson", async Task<IResult> (
			int id,
			IDimRouteGeoJsonService service,
			CancellationToken ct) =>
			{
				var document = await service.GetByIdAsync(id, ct);
				return document is null
						? TypedResults.NotFound($"Route '{id}' not found.")
						: TypedResults.Content(document, "application/geo+json");
			})
			.WithName("GetRouteGeoJsonById");

		app.Run();
		}
	}
}

