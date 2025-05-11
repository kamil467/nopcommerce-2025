using System;
using FluentMigrator;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Migrations.TestMigration;
[NopSchemaMigration("2025-05-10 00:00:00", "Category. Add SomeNewProperty")]
public class CategoryMigrationTest : ForwardOnlyMigration
{
    public override void Up()
    {
        var categoryTableName = nameof(Category);
        if(!Schema.Table(categoryTableName).Column(nameof(Category.SomeNewProperty)).Exists())
        {
          Alter.Table(categoryTableName)
                .AddColumn(nameof(Category.SomeNewProperty))
                .AsString(255).Nullable();
        }
    }
}
