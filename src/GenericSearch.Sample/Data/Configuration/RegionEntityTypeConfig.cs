using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenericSearch.Sample.Data.Entities;

namespace GenericSearch.Sample.Data.Configuration
{
	public class RegionEntityTypeConfig : IEntityTypeConfiguration<Region>
	{
		public void Configure(EntityTypeBuilder<Region> builder)
		{
			//CREATE TABLE "Region" (
			builder.ToTable("Region");

			//	[RegionID] [int] NOT NULL ,
			builder.HasKey(m => m.Id).HasName("RegionID");
			builder.Property(m => m.Id).HasColumnName("RegionID").IsRequired();

			//	[RegionDescription] [nchar] (50) NOT NULL 
			builder.Property(m => m.RegionDescription).HasMaxLength(50).IsRequired();


			builder.HasData(
				new Region() { Id = 1, RegionDescription = "Eastern" },
				new Region() { Id = 2, RegionDescription = "Western" },
				new Region() { Id = 3, RegionDescription = "Northern" },
				new Region() { Id = 4, RegionDescription = "Southern" });
		}
	}
}