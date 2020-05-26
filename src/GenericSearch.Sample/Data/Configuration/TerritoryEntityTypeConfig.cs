using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenericSearch.Sample.Data.Entities;

namespace GenericSearch.Sample.Data.Configuration
{
	public class TerritoryEntityTypeConfig : IEntityTypeConfiguration<Territory>
	{
		public void Configure(EntityTypeBuilder<Territory> builder)
		{
			//CREATE TABLE [dbo].[Territories]
			builder.ToTable("Territories");

			//	([TerritoryID] [nvarchar] (20) NOT NULL ,
			builder.HasKey(m => m.Id).HasName("TerritoryID");
			builder.Property(m => m.Id).HasColumnName("TerritoryID").HasMaxLength(20).IsRequired();

			//	[TerritoryDescription] [nchar] (50) NOT NULL ,
			builder.Property(m => m.TerritoryDescription).HasMaxLength(50).IsRequired();

			//[RegionID] [int] NOT NULL
			builder.Property(m => m.RegionId).HasColumnName("RegionID").IsRequired();
			builder.HasOne(m => m.Region)
				.WithMany(m => m.Territories)
				.HasForeignKey(m => m.RegionId)
				.IsRequired();

			builder.HasData(
				new Territory() { Id = "01581", TerritoryDescription = "Westboro", RegionId = 1 },
new Territory() { Id = "01730", TerritoryDescription = "Bedford", RegionId = 1 },
new Territory() { Id = "01833", TerritoryDescription = "Georgetow", RegionId = 1 },
new Territory() { Id = "02116", TerritoryDescription = "Boston", RegionId = 1 },
new Territory() { Id = "02139", TerritoryDescription = "Cambridge", RegionId = 1 },
new Territory() { Id = "02184", TerritoryDescription = "Braintree", RegionId = 1 },
new Territory() { Id = "02903", TerritoryDescription = "Providence", RegionId = 1 },
new Territory() { Id = "03049", TerritoryDescription = "Hollis", RegionId = 3 },
new Territory() { Id = "03801", TerritoryDescription = "Portsmouth", RegionId = 3 },
new Territory() { Id = "06897", TerritoryDescription = "Wilton", RegionId = 1 },
new Territory() { Id = "07960", TerritoryDescription = "Morristown", RegionId = 1 },
new Territory() { Id = "08837", TerritoryDescription = "Edison", RegionId = 1 },
new Territory() { Id = "10019", TerritoryDescription = "New York", RegionId = 1 },
new Territory() { Id = "10038", TerritoryDescription = "New York", RegionId = 1 },
new Territory() { Id = "11747", TerritoryDescription = "Mellvile", RegionId = 1 },
new Territory() { Id = "14450", TerritoryDescription = "Fairport", RegionId = 1 },
new Territory() { Id = "19428", TerritoryDescription = "Philadelphia", RegionId = 3 },
new Territory() { Id = "19713", TerritoryDescription = "Neward", RegionId = 1 },
new Territory() { Id = "20852", TerritoryDescription = "Rockville", RegionId = 1 },
new Territory() { Id = "27403", TerritoryDescription = "Greensboro", RegionId = 1 },
new Territory() { Id = "27511", TerritoryDescription = "Cary", RegionId = 1 },
new Territory() { Id = "29202", TerritoryDescription = "Columbia", RegionId = 4 },
new Territory() { Id = "30346", TerritoryDescription = "Atlanta", RegionId = 4 },
new Territory() { Id = "31406", TerritoryDescription = "Savannah", RegionId = 4 },
new Territory() { Id = "32859", TerritoryDescription = "Orlando", RegionId = 4 },
new Territory() { Id = "33607", TerritoryDescription = "Tampa", RegionId = 4 },
new Territory() { Id = "40222", TerritoryDescription = "Louisville", RegionId = 1 },
new Territory() { Id = "44122", TerritoryDescription = "Beachwood", RegionId = 3 },
new Territory() { Id = "45839", TerritoryDescription = "Findlay", RegionId = 3 },
new Territory() { Id = "48075", TerritoryDescription = "Southfield", RegionId = 3 },
new Territory() { Id = "48084", TerritoryDescription = "Troy", RegionId = 3 },
new Territory() { Id = "48304", TerritoryDescription = "Bloomfield Hills", RegionId = 3 },
new Territory() { Id = "53404", TerritoryDescription = "Racine", RegionId = 3 },
new Territory() { Id = "55113", TerritoryDescription = "Roseville", RegionId = 3 },
new Territory() { Id = "55439", TerritoryDescription = "Minneapolis", RegionId = 3 },
new Territory() { Id = "60179", TerritoryDescription = "Hoffman Estates", RegionId = 2 },
new Territory() { Id = "60601", TerritoryDescription = "Chicago", RegionId = 2 },
new Territory() { Id = "72716", TerritoryDescription = "Bentonville", RegionId = 4 },
new Territory() { Id = "75234", TerritoryDescription = "Dallas", RegionId = 4 },
new Territory() { Id = "78759", TerritoryDescription = "Austin", RegionId = 4 },
new Territory() { Id = "80202", TerritoryDescription = "Denver", RegionId = 2 },
new Territory() { Id = "80909", TerritoryDescription = "Colorado Springs", RegionId = 2 },
new Territory() { Id = "85014", TerritoryDescription = "Phoenix", RegionId = 2 },
new Territory() { Id = "85251", TerritoryDescription = "Scottsdale", RegionId = 2 },
new Territory() { Id = "90405", TerritoryDescription = "Santa Monica", RegionId = 2 },
new Territory() { Id = "94025", TerritoryDescription = "Menlo Park", RegionId = 2 },
new Territory() { Id = "94105", TerritoryDescription = "San Francisco", RegionId = 2 },
new Territory() { Id = "95008", TerritoryDescription = "Campbell", RegionId = 2 },
new Territory() { Id = "95054", TerritoryDescription = "Santa Clara", RegionId = 2 },
new Territory() { Id = "95060", TerritoryDescription = "Santa Cruz", RegionId = 2 },
new Territory() { Id = "98004", TerritoryDescription = "Bellevue", RegionId = 2 },
new Territory() { Id = "98052", TerritoryDescription = "Redmond", RegionId = 2 },
new Territory() { Id = "98104", TerritoryDescription = "Seattle", RegionId = 2 }
				);
		}
	}
}