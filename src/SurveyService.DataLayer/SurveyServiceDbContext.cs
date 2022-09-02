using LT.DigitalOffice.SurveyService.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LT.DigitalOffice.SurveyService.DataLayer;

public class SurveyServiceDbContext : DbContext
{
  public DbSet<DbQuestion> Questions { get; set; }
  public DbSet<DbOption> Options { get; set; }
  public DbSet<DbUserAnswer> UsersAnswers { get; set; }
  public DbSet<DbGroup> Groups { get; set; }

  public SurveyServiceDbContext(DbContextOptions<SurveyServiceDbContext> options)
    : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("LT.DigitalOffice.SurveyService.Models.Db"));
  }
}
