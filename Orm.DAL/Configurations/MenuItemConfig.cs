using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orm.Core.Entities;

namespace Orm.DAL.Configurations;

public class MenuItemConfig:IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x=>x.Category).IsRequired();
        
    }
}