using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Models.Db;

public class DbQuestion
{
  public const string TableName = "Questions";

  public Guid Id { get; set; }
  public Guid? GroupId { get; set; }
  public string Content { get; set; }
  public DateTime? Deadline { get; set; }
  public bool HasRealTimeResult { get; set; }
  public bool IsAnonymous { get; set; }
  public bool IsRevoteAvailable { get; set; }
  public bool IsObligatory { get; set; }
  public bool IsPrivate { get; set; }
  public bool HasMultipleChoice { get; set; }
  public bool HasCustomOptions { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public ICollection<DbOption> Options { get; set; }
  public DbGroup Group { get; set; }

  public DbQuestion()
  {
    Options = new HashSet<DbOption>();
  }
}

public class QuestionConfiguration : IEntityTypeConfiguration<DbQuestion>
{
  public void Configure(EntityTypeBuilder<DbQuestion> builder)
  {
    builder
      .ToTable(DbQuestion.TableName);

    builder
      .HasKey(q => q.Id);

    builder
      .Property(q => q.Content)
      .IsRequired();

    builder
      .HasMany(q => q.Options)
      .WithOne(o => o.Question);

    builder
      .HasOne(q => q.Group)
      .WithMany(g => g.Questions);
  }
}
