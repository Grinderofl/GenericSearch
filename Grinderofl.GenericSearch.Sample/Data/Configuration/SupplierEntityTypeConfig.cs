﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Grinderofl.GenericSearch.Sample.Data.Entities;

namespace Grinderofl.GenericSearch.Sample.Data.Configuration
{
	public class SupplierEntityTypeConfig : IEntityTypeConfiguration<Supplier>
	{
		public void Configure(EntityTypeBuilder<Supplier> builder)
		{
			//CREATE TABLE "Suppliers" (
			builder.ToTable("Suppliers");

			//	"SupplierID" "int" IDENTITY (1, 1) NOT NULL ,
			//	CONSTRAINT "PK_Suppliers" PRIMARY KEY  CLUSTERED
			//    (
			//		"SupplierID"
			//	)
			builder.HasKey(m => m.Id).HasName("SupplierID");
			builder.Property(m => m.Id).HasColumnName("SupplierID").IsRequired();

			//	"CompanyName" nvarchar(40) NOT NULL,
			// CREATE  INDEX "CompanyName" ON "dbo"."Customers"("CompanyName")
			builder.Property(m => m.CompanyName).HasMaxLength(40).IsRequired();
			builder.HasIndex(m => m.CompanyName);

			//	"ContactName" nvarchar(30) NULL ,
			builder.Property(m => m.ContactName).HasMaxLength(30);

			//	"ContactTitle" nvarchar(30) NULL ,
			builder.Property(m => m.ContactTitle).HasMaxLength(30);

			//	"Address" nvarchar(60) NULL ,
			builder.Property(m => m.Address).HasMaxLength(60);

			//	"City" nvarchar(15) NULL ,
			builder.Property(m => m.City).HasMaxLength(15);

			//	"Region" nvarchar(15) NULL ,
			builder.Property(m => m.Region).HasMaxLength(15);

			//	"PostalCode" nvarchar(10) NULL ,
			// CREATE  INDEX "PostalCode" ON "dbo"."Customers"("PostalCode")
			builder.Property(m => m.PostalCode).HasMaxLength(10);
			builder.HasIndex(m => m.PostalCode);

			//	"Country" nvarchar(15) NULL ,
			builder.Property(m => m.Country).HasMaxLength(15);

			//	"Phone" nvarchar(24) NULL ,
			builder.Property(m => m.Phone).HasMaxLength(24);

			//	"Fax" nvarchar(24) NULL ,
			builder.Property(m => m.Fax).HasMaxLength(24);

			//	"HomePage" "ntext" NULL ,
			builder.Property(m => m.HomePage).HasMaxLength(int.MaxValue);

			builder.HasData(
				new Supplier() { Id = 1, CompanyName = "Exotic Liquids", ContactName = "Charlotte Cooper", ContactTitle = "Purchasing Manager", Address = "49 Gilbert St.", City = "London", Region = null, PostalCode = "EC1 4SD", Country = "UK", Phone = "(171) 555-2222", Fax = null, HomePage = null },
				new Supplier() { Id = 2, CompanyName = "New Orleans Cajun Delights", ContactName = "Shelley Burke", ContactTitle = "Order Administrator", Address = "P.O. Box 78934", City = "New Orleans", Region = "LA", PostalCode = "70117", Country = "USA", Phone = "(100) 555-4822", Fax = null, HomePage = "#CAJUN.HTM#" },
				new Supplier() { Id = 3, CompanyName = "Grandma Kelly's Homestead", ContactName = "Regina Murphy", ContactTitle = "Sales Representative", Address = "707 Oxford Rd.", City = "Ann Arbor", Region = "MI", PostalCode = "48104", Country = "USA", Phone = "(313) 555-5735", Fax = "(313) 555-3349", HomePage = null },
				new Supplier() { Id = 4, CompanyName = "Tokyo Traders", ContactName = "Yoshi Nagase", ContactTitle = "Marketing Manager", Address = "9-8 Sekimai Musashino-shi", City = "Tokyo", Region = null, PostalCode = "100", Country = "Japan", Phone = "(03) 3555-5011", Fax = null, HomePage = null },
				new Supplier() { Id = 5, CompanyName = "Cooperativa de Quesos 'Las Cabras'", ContactName = "Antonio del Valle Saavedra", ContactTitle = "Export Administrator", Address = "Calle del Rosal 4", City = "Oviedo", Region = "Asturias", PostalCode = "33007", Country = "Spain", Phone = "(98) 598 76 54", Fax = null, HomePage = null },
				new Supplier() { Id = 6, CompanyName = "Mayumi's", ContactName = "Mayumi Ohno", ContactTitle = "Marketing Representative", Address = "92 Setsuko Chuo-ku", City = "Osaka", Region = null, PostalCode = "545", Country = "Japan", Phone = "(06) 431-7877", Fax = null, HomePage = "Mayumi's (on the World Wide Web)#http://www.microsoft.com/accessdev/sampleapps/mayumi.htm#" },
				new Supplier() { Id = 7, CompanyName = "Pavlova, Ltd.", ContactName = "Ian Devling", ContactTitle = "Marketing Manager", Address = "74 Rose St. Moonie Ponds", City = "Melbourne", Region = "Victoria", PostalCode = "3058", Country = "Australia", Phone = "(03) 444-2343", Fax = "(03) 444-6588", HomePage = null },
				new Supplier() { Id = 8, CompanyName = "Specialty Biscuits, Ltd.", ContactName = "Peter Wilson", ContactTitle = "Sales Representative", Address = "29 King's Way", City = "Manchester", Region = null, PostalCode = "M14 GSD", Country = "UK", Phone = "(161) 555-4448", Fax = null, HomePage = null },
				new Supplier() { Id = 9, CompanyName = "PB Knäckebröd AB", ContactName = "Lars Peterson", ContactTitle = "Sales Agent", Address = "Kaloadagatan 13", City = "Göteborg", Region = null, PostalCode = "S-345 67", Country = "Sweden", Phone = "031-987 65 43", Fax = "031-987 65 91", HomePage = null },
				new Supplier() { Id = 10, CompanyName = "Refrescos Americanas LTDA", ContactName = "Carlos Diaz", ContactTitle = "Marketing Manager", Address = "Av. das Americanas 12.890", City = "Sao Paulo", Region = null, PostalCode = "5442", Country = "Brazil", Phone = "(11) 555 4640", Fax = null, HomePage = null },
				new Supplier() { Id = 11, CompanyName = "Heli Süßwaren GmbH & Co. KG", ContactName = "Petra Winkler", ContactTitle = "Sales Manager", Address = "Tiergartenstraße 5", City = "Berlin", Region = null, PostalCode = "10785", Country = "Germany", Phone = "(010) 9984510", Fax = null, HomePage = null },
				new Supplier() { Id = 12, CompanyName = "Plutzer Lebensmittelgroßmärkte AG", ContactName = "Martin Bein", ContactTitle = "International Marketing Mgr.", Address = "Bogenallee 51", City = "Frankfurt", Region = null, PostalCode = "60439", Country = "Germany", Phone = "(069) 992755", Fax = null, HomePage = "Plutzer (on the World Wide Web)#http://www.microsoft.com/accessdev/sampleapps/plutzer.htm#" },
				new Supplier() { Id = 13, CompanyName = "Nord-Ost-Fisch Handelsgesellschaft mbH", ContactName = "Sven Petersen", ContactTitle = "Coordinator Foreign Markets", Address = "Frahmredder 112a", City = "Cuxhaven", Region = null, PostalCode = "27478", Country = "Germany", Phone = "(04721) 8713", Fax = "(04721) 8714", HomePage = null },
				new Supplier() { Id = 14, CompanyName = "Formaggi Fortini s.r.l.", ContactName = "Elio Rossi", ContactTitle = "Sales Representative", Address = "Viale Dante, 75", City = "Ravenna", Region = null, PostalCode = "48100", Country = "Italy", Phone = "(0544) 60323", Fax = "(0544) 60603", HomePage = "#FORMAGGI.HTM#" },
				new Supplier() { Id = 15, CompanyName = "Norske Meierier", ContactName = "Beate Vileid", ContactTitle = "Marketing Manager", Address = "Hatlevegen 5", City = "Sandvika", Region = null, PostalCode = "1320", Country = "Norway", Phone = "(0)2-953010", Fax = null, HomePage = null },
				new Supplier() { Id = 16, CompanyName = "Bigfoot Breweries", ContactName = "Cheryl Saylor", ContactTitle = "Regional Account Rep.", Address = "3400 - 8th Avenue Suite 210", City = "Bend", Region = "OR", PostalCode = "97101", Country = "USA", Phone = "(503) 555-9931", Fax = null, HomePage = null },
				new Supplier() { Id = 17, CompanyName = "Svensk Sjöföda AB", ContactName = "Michael Björn", ContactTitle = "Sales Representative", Address = "Brovallavägen 231", City = "Stockholm", Region = null, PostalCode = "S-123 45", Country = "Sweden", Phone = "08-123 45 67", Fax = null, HomePage = null },
				new Supplier() { Id = 18, CompanyName = "Aux joyeux ecclésiastiques", ContactName = "Guylène Nodier", ContactTitle = "Sales Manager", Address = "203, Rue des Francs-Bourgeois", City = "Paris", Region = null, PostalCode = "75004", Country = "France", Phone = "(1) 03.83.00.68", Fax = "(1) 03.83.00.62", HomePage = null },
				new Supplier() { Id = 19, CompanyName = "New England Seafood Cannery", ContactName = "Robb Merchant", ContactTitle = "Wholesale Account Agent", Address = "Order Processing Dept. 2100 Paul Revere Blvd.", City = "Boston", Region = "MA", PostalCode = "02134", Country = "USA", Phone = "(617) 555-3267", Fax = "(617) 555-3389", HomePage = null },
				new Supplier() { Id = 20, CompanyName = "Leka Trading", ContactName = "Chandra Leka", ContactTitle = "Owner", Address = "471 Serangoon Loop, Suite #402", City = "Singapore", Region = null, PostalCode = "0512", Country = "Singapore", Phone = "555-8787", Fax = null, HomePage = null },
				new Supplier() { Id = 21, CompanyName = "Lyngbysild", ContactName = "Niels Petersen", ContactTitle = "Sales Manager", Address = "Lyngbysild Fiskebakken 10", City = "Lyngby", Region = null, PostalCode = "2800", Country = "Denmark", Phone = "43844108", Fax = "43844115", HomePage = null },
				new Supplier() { Id = 22, CompanyName = "Zaanse Snoepfabriek", ContactName = "Dirk Luchte", ContactTitle = "Accounting Manager", Address = "Verkoop Rijnweg 22", City = "Zaandam", Region = null, PostalCode = "9999 ZZ", Country = "Netherlands", Phone = "(12345) 1212", Fax = "(12345) 1210", HomePage = null },
				new Supplier() { Id = 23, CompanyName = "Karkki Oy", ContactName = "Anne Heikkonen", ContactTitle = "Product Manager", Address = "Valtakatu 12", City = "Lappeenranta", Region = null, PostalCode = "53120", Country = "Finland", Phone = "(953) 10956", Fax = null, HomePage = null },
				new Supplier() { Id = 24, CompanyName = "G'day, Mate", ContactName = "Wendy Mackenzie", ContactTitle = "Sales Representative", Address = "170 Prince Edward Parade Hunter's Hill", City = "Sydney", Region = "NSW", PostalCode = "2042", Country = "Australia", Phone = "(02) 555-5914", Fax = "(02) 555-4873", HomePage = "G'day Mate (on the World Wide Web)#http://www.microsoft.com/accessdev/sampleapps/gdaymate.htm#" },
				new Supplier() { Id = 25, CompanyName = "Ma Maison", ContactName = "Jean-Guy Lauzon", ContactTitle = "Marketing Manager", Address = "2960 Rue St. Laurent", City = "Montréal", Region = "Québec", PostalCode = "H1J 1C3", Country = "Canada", Phone = "(514) 555-9022", Fax = null, HomePage = null },
				new Supplier() { Id = 26, CompanyName = "Pasta Buttini s.r.l.", ContactName = "Giovanni Giudici", ContactTitle = "Order Administrator", Address = "Via dei Gelsomini, 153", City = "Salerno", Region = null, PostalCode = "84100", Country = "Italy", Phone = "(089) 6547665", Fax = "(089) 6547667", HomePage = null },
				new Supplier() { Id = 27, CompanyName = "Escargots Nouveaux", ContactName = "Marie Delamare", ContactTitle = "Sales Manager", Address = "22, rue H. Voiron", City = "Montceau", Region = null, PostalCode = "71300", Country = "France", Phone = "85.57.00.07", Fax = null, HomePage = null },
				new Supplier() { Id = 28, CompanyName = "Gai pâturage", ContactName = "Eliane Noz", ContactTitle = "Sales Representative", Address = "Bat. B 3, rue des Alpes", City = "Annecy", Region = null, PostalCode = "74000", Country = "France", Phone = "38.76.98.06", Fax = "38.76.98.58", HomePage = null },
				new Supplier() { Id = 29, CompanyName = "Forêts d'érables", ContactName = "Chantal Goulet", ContactTitle = "Accounting Manager", Address = "148 rue Chasseur", City = "Ste-Hyacinthe", Region = "Québec", PostalCode = "J2S 7S8", Country = "Canada", Phone = "(514) 555-2955", Fax = "(514) 555-2921", HomePage = null }
				);
		}
	}
}