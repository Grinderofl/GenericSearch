using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Grinderofl.GenericSearch.Sample.Data.Entities;

namespace Grinderofl.GenericSearch.Sample.Data.Configuration
{
	public class EmployeeTerritoryEntityTypeConfig : IEntityTypeConfiguration<EmployeeTerritory>
	{
		public void Configure(EntityTypeBuilder<EmployeeTerritory> builder)
		{
			//CREATE TABLE [dbo].[EmployeeTerritories] 
			builder.ToTable("EmployeeTerritories");

			//	([EmployeeID] [int] NOT NULL,
			//[TerritoryID] [nvarchar] (20) NOT NULL
			builder.HasKey(m => new { m.EmployeeId, m.TerritoryId });

			builder.Property(m => m.EmployeeId).HasColumnName("EmployeeID").IsRequired();
			builder.Property(m => m.TerritoryId).HasColumnName("TerritoryID").IsRequired();

			builder.HasOne(m => m.Employee)
				.WithMany(m=> m.EmployeeTerritories)
				.HasForeignKey(m => m.EmployeeId)
				.IsRequired();

			builder.HasOne(m => m.Territory)
				.WithMany(m => m.EmployeeTerritories)
				.HasForeignKey(m => m.TerritoryId)
				.IsRequired();

			builder.HasData(
new EmployeeTerritory() { EmployeeId = 1, TerritoryId = "06897" },
new EmployeeTerritory() { EmployeeId = 1, TerritoryId = "19713" },
new EmployeeTerritory() { EmployeeId = 2, TerritoryId = "01581" },
new EmployeeTerritory() { EmployeeId = 2, TerritoryId = "01730" },
new EmployeeTerritory() { EmployeeId = 2, TerritoryId = "01833" },
new EmployeeTerritory() { EmployeeId = 2, TerritoryId = "02116" },
new EmployeeTerritory() { EmployeeId = 2, TerritoryId = "02139" },
new EmployeeTerritory() { EmployeeId = 2, TerritoryId = "02184" },
new EmployeeTerritory() { EmployeeId = 2, TerritoryId = "40222" },
new EmployeeTerritory() { EmployeeId = 3, TerritoryId = "30346" },
new EmployeeTerritory() { EmployeeId = 3, TerritoryId = "31406" },
new EmployeeTerritory() { EmployeeId = 3, TerritoryId = "32859" },
new EmployeeTerritory() { EmployeeId = 3, TerritoryId = "33607" },
new EmployeeTerritory() { EmployeeId = 4, TerritoryId = "20852" },
new EmployeeTerritory() { EmployeeId = 4, TerritoryId = "27403" },
new EmployeeTerritory() { EmployeeId = 4, TerritoryId = "27511" },
new EmployeeTerritory() { EmployeeId = 5, TerritoryId = "02903" },
new EmployeeTerritory() { EmployeeId = 5, TerritoryId = "07960" },
new EmployeeTerritory() { EmployeeId = 5, TerritoryId = "08837" },
new EmployeeTerritory() { EmployeeId = 5, TerritoryId = "10019" },
new EmployeeTerritory() { EmployeeId = 5, TerritoryId = "10038" },
new EmployeeTerritory() { EmployeeId = 5, TerritoryId = "11747" },
new EmployeeTerritory() { EmployeeId = 5, TerritoryId = "14450" },
new EmployeeTerritory() { EmployeeId = 6, TerritoryId = "85014" },
new EmployeeTerritory() { EmployeeId = 6, TerritoryId = "85251" },
new EmployeeTerritory() { EmployeeId = 6, TerritoryId = "98004" },
new EmployeeTerritory() { EmployeeId = 6, TerritoryId = "98052" },
new EmployeeTerritory() { EmployeeId = 6, TerritoryId = "98104" },
new EmployeeTerritory() { EmployeeId = 7, TerritoryId = "60179" },
new EmployeeTerritory() { EmployeeId = 7, TerritoryId = "60601" },
new EmployeeTerritory() { EmployeeId = 7, TerritoryId = "80202" },
new EmployeeTerritory() { EmployeeId = 7, TerritoryId = "80909" },
new EmployeeTerritory() { EmployeeId = 7, TerritoryId = "90405" },
new EmployeeTerritory() { EmployeeId = 7, TerritoryId = "94025" },
new EmployeeTerritory() { EmployeeId = 7, TerritoryId = "94105" },
new EmployeeTerritory() { EmployeeId = 7, TerritoryId = "95008" },
new EmployeeTerritory() { EmployeeId = 7, TerritoryId = "95054" },
new EmployeeTerritory() { EmployeeId = 7, TerritoryId = "95060" },
new EmployeeTerritory() { EmployeeId = 8, TerritoryId = "19428" },
new EmployeeTerritory() { EmployeeId = 8, TerritoryId = "44122" },
new EmployeeTerritory() { EmployeeId = 8, TerritoryId = "45839" },
new EmployeeTerritory() { EmployeeId = 8, TerritoryId = "53404" },
new EmployeeTerritory() { EmployeeId = 9, TerritoryId = "03049" },
new EmployeeTerritory() { EmployeeId = 9, TerritoryId = "03801" },
new EmployeeTerritory() { EmployeeId = 9, TerritoryId = "48075" },
new EmployeeTerritory() { EmployeeId = 9, TerritoryId = "48084" },
new EmployeeTerritory() { EmployeeId = 9, TerritoryId = "48304" },
new EmployeeTerritory() { EmployeeId = 9, TerritoryId = "55113" },
new EmployeeTerritory() { EmployeeId = 9, TerritoryId = "55439" }
);
		}
	}
}