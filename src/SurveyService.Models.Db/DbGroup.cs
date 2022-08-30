using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Models.Db;

public class DbGroup
{
  public const string TableName = "Groups";

  public Guid Id { get; set; }
  public string Subject { get; set; }
  public string Description { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public ICollection<DbQuestion> Questions { get; set; }

  public DbGroup()
  {
    Questions = new HashSet<DbQuestion>();
  }
}

public class GroupConfiguration : IEntityTypeConfiguration<DbGroup>
{
  public void Configure(EntityTypeBuilder<DbGroup> builder)
  {
    builder
      .ToTable(DbGroup.TableName);

    builder
      .HasKey(q => q.Id);

    builder
      .Property(g => g.Subject)
      .IsRequired();

    builder
      .HasMany(g => g.Questions)
      .WithOne(q => q.Group);
  }
}
