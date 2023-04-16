using Microsoft.AspNetCore.Cors.Infrastructure;
using BrowserGameBackend.Data;
using Microsoft.EntityFrameworkCore;
using BrowserGameBackend.Services;
using BrowserGameBackend.Jobs;
using Quartz;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GameContext>(options  =>
                            options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionLocal")));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();



builder.Services.AddCors(options => {options.AddPolicy(name: "CorsPolicy",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
