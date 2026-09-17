
using Motofushin.Roadmap.Application.Features.Gpx;
using Motofushin.Roadmap.Infrastructure.DependencyInjection;

namespace Motofushin.Roadmap.Api
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

			app.MapGet("/gpx", async Task<IResult> (
				IGpxTrackStore store,
				CancellationToken ct) =>
				{
					var names = await store.GetNamesAsync(ct);
					return names is null
						? TypedResults.NotFound("GPX directory not found.")
						: TypedResults.Ok(names);
				})
			.WithName("GetGpxTracks");

			app.MapGet("/gpx/{name}", async Task<IResult> (
				string name,
				IGpxTrackStore store,
				CancellationToken ct) =>
				{
					var content = await store.GetByNameAsync(name, ct);
					return content is null
							? TypedResults.NotFound($"GPX track '{name}' not found.")
							: TypedResults.Content(content, "application/gpx+xml");
				})
				.WithName("GetGpxTrackByName");

			app.Run();
		}
	}
}

