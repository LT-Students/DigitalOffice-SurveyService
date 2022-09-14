using LT.DigitalOffice.SurveyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.SurveyService.Data.Provider.MsSql.Ef.Migrations;

[DbContext(typeof(SurveyServiceDbContext))]
[Migration("20220826180800_InitialCreate")]
class InitialCreate : Migration
{
  protected override void Up(MigrationBuilder builder)
  {
    builder.CreateTable(
      name: DbQuestion.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        GroupId = table.Column<Guid>(nullable: true),
        Content = table.Column<string>(nullable: false),
        Deadline = table.Column<DateTime>(nullable: true),
        HasRealTimeResult = table.Column<bool>(nullable: false),
        IsAnonymous = table.Column<bool>(nullable: false),
        IsRevoteAvailable = table.Column<bool>(nullable: false),
        IsObligatory = table.Column<bool>(nullable: false),
        IsPrivate = table.Column<bool>(nullable: false),
        HasMultipleChoice = table.Column<bool>(nullable: false),
        HasCustomOptions = table.Column<bool>(nullable: false),
        IsActive = table.Column<bool>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedBy = table.Column<Guid>(nullable: true),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbQuestion.TableName}", x => x.Id);
      });

    builder.CreateTable(
      name: DbOption.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        QuestionId = table.Column<Guid>(nullable: false),
        Content = table.Column<string>(nullable: false),
        IsCustom = table.Column<bool>(nullable: false),
        IsActive = table.Column<bool>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedBy = table.Column<Guid>(nullable: true),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbOption.TableName}", x => x.Id);
      });

    builder.CreateTable(
      name: DbUserAnswer.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        UserId = table.Column<Guid>(nullable: false),
        OptionId = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbUserAnswer.TableName}", x => x.Id);
      });

    builder.CreateTable(
      name: DbGroup.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        Subject = table.Column<string>(nullable: false),
        Description = table.Column<string>(nullable: true),
        Deadline = table.Column<DateTime>(nullable: true),
        HasRealTimeResult = table.Column<bool>(nullable: false),
        IsActive = table.Column<bool>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedBy = table.Column<Guid>(nullable: true),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbGroup.TableName}", x => x.Id);
      });
  }
}
