using BambooCard.Plugin.Misc.AssessmentTasks.Domains;
using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Data;

[NopMigration("2025/05/30 19:50:00:0000000", "BambooCard.Plugin.Misc.AssessmentTasks base schema", MigrationProcessType.NoMatter)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<JwtRefreshToken>();
    }
}

