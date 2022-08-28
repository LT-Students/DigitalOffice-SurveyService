using LT.DigitalOffice.SurveyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.SurveyService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(SurveyServiceDbContext))]
  [Migration("20220826180800_InitialQuestionsTable")]
  class InitialQuestionsTable : Migration
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
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUTC = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: false),
          ModifiedAtUTC = table.Column<DateTime>(nullable: true),
          IsActive = table.Column<bool>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbQuestion.TableName}", x => x.Id);
        });
    }
  }
}
