using BambooCard.Plugin.Misc.AssessmentTasks.Domains;
using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Data.Builders;

public class JwtRefreshTokenEntityBuilder : NopEntityBuilder<JwtRefreshToken>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(JwtRefreshToken.Token))
            .AsGuid().NotNullable()
            .WithColumn(nameof(JwtRefreshToken.CustomerId))
            .AsInt32().ForeignKey<Customer>()
            .WithColumn(nameof(JwtRefreshToken.ValidTill))
            .AsDateTime2().NotNullable()
            .WithColumn(nameof(JwtRefreshToken.Active))
            .AsBoolean().NotNullable();
    }
}
