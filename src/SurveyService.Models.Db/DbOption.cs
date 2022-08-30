using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Models.Db;

public class DbOption
{
  public const string TableName = "Options";

  public Guid Id { get; set; }
  public Guid QuestionId { get; set; }
  public string Content { get; set; }
  public bool IsCustom { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public DbQuestion Question { get; set; }
  public ICollection<DbUserAnswer> UsersAnswers { get; set; }

  public DbOption()
  {
    UsersAnswers = new HashSet<DbUserAnswer>();
  }
}

public class OptionConfiguration : IEntityTypeConfiguration<DbOption>
{
  public void Configure(EntityTypeBuilder<DbOption> builder)
  {
    builder
      .ToTable(DbOption.TableName);

    builder
      .HasKey(o => o.Id);

    builder
      .Property(o => o.Content)
      .IsRequired();

    builder
      .HasOne(o => o.Question)
      .WithMany(q => q.Options);

    builder
      .HasMany(o => o.UsersAnswers)
      .WithOne(ua => ua.Option);
  }
}