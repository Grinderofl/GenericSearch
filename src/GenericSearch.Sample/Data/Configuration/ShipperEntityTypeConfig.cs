using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenericSearch.Sample.Data.Entities;

namespace GenericSearch.Sample.Data.Configuration
{
	public class ShipperEntityTypeConfig : IEntityTypeConfiguration<Shipper>
	{
		public void Configure(EntityTypeBuilder<Shipper> builder)
		{
			//CREATE TABLE "Shippers" (
			builder.ToTable("Shippers");

			//	"ShipperID" "int" IDENTITY (1, 1) NOT NULL ,
			//	CONSTRAINT "PK_Shippers" PRIMARY KEY  CLUSTERED
			//	(
			//		"ShipperID"
			//	),
			builder.HasKey(m => m.Id).HasName("ShipperID");
			builder.Property(m => m.Id).HasColumnName("ShipperID").IsRequired();

			//	"CompanyName" nvarchar (40) NOT NULL ,
			builder.Property(m => m.CompanyName).HasMaxLength(40).IsRequired();

			//  "Phone" nvarchar (24) NULL ,
			builder.Property(m => m.Phone).HasMaxLength(24);

			builder.HasData(
				new Shipper() { Id = 1, CompanyName = "Speedy Express", Phone = "(503) 555-9831" },
				new Shipper() { Id = 2, CompanyName = "United Package", Phone = "(503) 555-3199" },
				new Shipper() { Id = 3, CompanyName = "Federal Shipping", Phone = "(503) 555-9931" });
		}
	}
}