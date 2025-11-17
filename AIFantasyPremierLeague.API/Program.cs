using AIFantasyPremierLeague.API.DataGatherers;
using AIFantasyPremierLeague.API.Exceptions;
using AIFantasyPremierLeague.API.Prediction;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Config;
using AIFantasyPremierLeague.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ML;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    )
);


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<NotFoundFilter>();
});

builder.Services.AddScoped<IPlayerPerformanceRepository, PlayerPerformanceRepository>();
builder.Services.AddScoped<AverageGoalsCalculator>();
builder.Services.AddScoped<AverageAssistsCalculator>();
builder.Services.AddScoped<AveragePointsCalculator>();
builder.Services.AddScoped<AverageMinsPlayedCalculator>();
builder.Services.AddScoped<AverageBonusCalculator>();
builder.Services.AddScoped<AverageCleanSheetsCalculator>();
builder.Services.AddScoped<AverageGoalsConcededCalculator>();
builder.Services.AddScoped<AverageYellowCardsCalculator>();
builder.Services.AddScoped<AverageRedCardsCalculator>();
builder.Services.AddScoped<AverageSavesCalculator>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IPlayerPerformanceService, PlayerPerformanceService>();
builder.Services.AddScoped<IFPLDataService, FPLDataService>();
builder.Services.AddScoped<IPredictionService, PredictionService>();


builder.Services.AddHttpClient("FPLApi", client =>
{
    client.BaseAddress = new Uri("https://fantasy.premierleague.com");
    client.Timeout = TimeSpan.FromSeconds(15);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddPredictionEnginePool<PlayerTrainingData, PlayerPrediction>()
    .FromFile("fplModel.zip", watchForChanges: true);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}
app.MapControllers();

app.Run();
