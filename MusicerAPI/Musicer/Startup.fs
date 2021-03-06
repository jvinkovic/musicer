namespace Musicer

open Microsoft.AspNetCore.Cors.Infrastructure

module ConfigurationCors =
    let ConfigureCors(corsBuilder: CorsPolicyBuilder): unit =        
        corsBuilder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod() |> ignore



open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

open Microsoft.OpenApi.Models
open Musicer.Repositories

open ConfigurationCors


type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        let info = OpenApiInfo()
        info.Title <- "Musicer API V1"
        info.Version <- "v1"
        services.AddSwaggerGen(fun config -> config.SwaggerDoc("v1", info)) |> ignore
        // Add framework services.
        services.AddControllers() |> ignore

        // register DI services
        services.AddScoped<SongsRepository>() |> ignore
        services.AddScoped<CommentsRepository>() |> ignore
        services.AddScoped<RatingsRepository>() |> ignore

        // initialize database if needed
        SetupDB.initDB

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
            app.UseSwagger() |> ignore
            app.UseSwaggerUI(fun config -> config.SwaggerEndpoint("/swagger/v1/swagger.json", "Musicer API V1")) |> ignore

        app.UseCors() |> ignore
        app.UseCors(ConfigureCors) |> ignore

        app.UseRouting() |> ignore

        app.UseEndpoints(fun endpoints ->
            endpoints.MapControllers() |> ignore
            ) |> ignore

    member val Configuration : IConfiguration = null with get, set
