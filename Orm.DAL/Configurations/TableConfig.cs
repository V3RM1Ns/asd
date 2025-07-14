using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orm.Core.Entities;

namespace Orm.DAL.Configurations;

public class TableConfig:IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.HasMany(t => t.Orders)
            .WithOne(x=>x.Table)
            .HasForeignKey(x=>x.TableId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.No).IsRequired();
        builder.Property(x=>x.IsOcupied).HasDefaultValue(false);
    }
}