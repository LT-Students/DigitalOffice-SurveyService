using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.SurveyService.Models.Db;

public class DbUserAnswer
{
  public const string TableName = "UsersAnswers";

  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid OptionId { get; set; }
  public DateTime CreatedAtUtc { get; set; }

  public DbOption Option { get; set; }
}

public class UserAnswerConfiguration : IEntityTypeConfiguration<DbUserAnswer>
{
  public void Configure(EntityTypeBuilder<DbUserAnswer> builder)
  {
    builder
      .ToTable(DbUserAnswer.TableName);

    builder
      .HasKey(ua => ua.Id);

    builder
      .HasOne(ua => ua.Option)
      .WithMany(o => o.UsersAnswers);
  }
}
