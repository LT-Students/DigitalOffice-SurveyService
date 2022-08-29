using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Models.Db;

public class DbOption
{
  public const string TableName = "Options";

  public Guid Id { get; set; }
  public string Content { get; set; }
  public bool IsCustom { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }

  public DbQuestion Question { get; set; }
  public ICollection<DbAnswer> Answers { get; set; }

  public DbOption()
  {
    Answers = new HashSet<DbAnswer>();
  }
}

public class OptionsConfiguration : IEntityTypeConfiguration<DbOption>
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
      .HasMany(o => o.Answers)
      .WithOne(a => a.Option);
  }
}