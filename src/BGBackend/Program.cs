using Microsoft.AspNetCore.Cors.Infrastructure;
using BrowserGameBackend.Data;
using Microsoft.EntityFrameworkCore;
using BrowserGameBackend.Services;
using BrowserGameBackend.Jobs;
using Quartz;
using BrowserGameBackend.Services.Game;
using BrowserGameBackend.Types.Options;
using BrowserGameBackend.Services.Utilities;

var builder = WebApplication.CreateBuilder(args);
//options add PascalCase instead on camelCase on responses / removed for now
builder.Services.AddControllersWithViews(); //.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddDbContext<GameContext>(options =>
                            options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionLocal")));
builder.Services.AddMemoryCache(
    options =>
    {    
        options.SizeLimit = 1000;
        options.CompactionPercentage = 0.1;
        options.TrackStatistics= true;
    });
builder.Services.Configure<GalaxyGenerationOptions>(
    builder.Configuration.GetSection(GalaxyGenerationOptions.GalaxyOptions));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IGalaxyMapService, GalaxyMapService>();
builder.Services.AddScoped<IGalaxyGenerationService, GalaxyGenerationService>();
builder.Services.AddScoped<IRandomGenerator, RandomGenerator>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IFactionService, FactionService> ();
builder.Services.AddScoped<ISpeciesService, SpeciesService>();

builder.Services.AddCors(
    options => 
    {
        options.AddPolicy(name: "CorsPolicy",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod() 
                        .AllowCredentials();
            });
    });
//QUARTZ
/*
builder.Services.AddQuartz(q =>
{
    q.UseDedicatedThreadPool(2);
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("CollectSensorData");
    q.AddJob<CollectSensorData>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("CollectSensorData-trigger")
        .WithCronSchedule("* * * * * ?")
    );
    var jobKey2 = new JobKey("CollectSensorData2");

    q.AddJob<CollectSensorData2>(opts => opts.WithIdentity(jobKey2));

    q.AddTrigger(opts => opts
        .ForJob(jobKey2)
        .WithIdentity("CollectSensorData2-trigger")
        .WithCronSchedule("* * * * * ?")
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
*/
var app = builder.Build();
app.UseHttpsRedirection();

app.UseStaticFiles(); 

app.UseRouting();
app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
