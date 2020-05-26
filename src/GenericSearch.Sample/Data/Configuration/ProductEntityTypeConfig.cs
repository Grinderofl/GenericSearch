using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenericSearch.Sample.Data.Entities;

namespace GenericSearch.Sample.Data.Configuration
{
	public class ProductEntityTypeConfig : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			//CREATE TABLE "Products" (
			builder.ToTable("Products");

			//	"ProductID" "int" IDENTITY (1, 1) NOT NULL ,
			//	CONSTRAINT "PK_Products" PRIMARY KEY  CLUSTERED
			//  (
			//  	"ProductID"
			//  ),
			builder.HasKey(m => m.Id).HasName("ProductID");
			builder.Property(m => m.Id).HasColumnName("ProductID").IsRequired();

			//"ProductName" nvarchar (40) NOT NULL ,
			//CREATE  INDEX "ProductName" ON "dbo"."Products"("ProductName")
			builder.Property(m => m.ProductName).HasMaxLength(40).IsRequired();
			builder.HasIndex(m => m.ProductName);

			//"SupplierID" "int" NULL ,
			//CONSTRAINT "FK_Products_Suppliers" FOREIGN KEY
			//(
			//	"SupplierID"
			//) REFERENCES "dbo"."Suppliers"(
			//	"SupplierID"
			//),
			builder.HasOne(m => m.Supplier)
				.WithMany(m=>m.Products)
				.HasForeignKey(m => m.SupplierId)
				.HasConstraintName("FK_Orders_Suppliers");
			builder.HasIndex(m => m.SupplierId).HasName("SuppliersProducts");

			//"CategoryID" "int" NULL ,
			//CONSTRAINT "FK_Products_Categories" FOREIGN KEY
			//			(
			//	"CategoryID"
			//) REFERENCES "dbo"."Categories"(
			//	"CategoryID"
			//),
			builder.HasOne(m => m.Category)
				.WithMany(m=>m.Products)
				.HasForeignKey(m => m.CategoryId)
				.HasConstraintName("FK_Orders_Categories");

			//CREATE INDEX "CategoriesProducts" ON "dbo"."Products"("CategoryID")
			builder.HasIndex(m => m.CategoryId).HasName("CategoriesProducts");

			//"QuantityPerUnit" nvarchar(20) NULL ,
			builder.Property(m => m.QuantityPerUnit).HasMaxLength(20);

			//"UnitPrice" "money" NULL CONSTRAINT "DF_Products_UnitPrice" DEFAULT(0),
			builder.Property(m => m.UnitPrice).HasDefaultValue(0.0);

			//"UnitsInStock" "smallint" NULL CONSTRAINT "DF_Products_UnitsInStock" DEFAULT(0),
			builder.Property(m => m.UnitsInStock).HasDefaultValue((short)0);

			//"UnitsOnOrder" "smallint" NULL CONSTRAINT "DF_Products_UnitsOnOrder" DEFAULT(0),
			builder.Property(m => m.UnitsOnOrder).HasDefaultValue((short)0);

			//"ReorderLevel" "smallint" NULL CONSTRAINT "DF_Products_ReorderLevel" DEFAULT(0),
			builder.Property(m => m.ReorderLevel).HasDefaultValue((short)0);

			//"Discontinued" "bit" NOT NULL CONSTRAINT "DF_Products_Discontinued" DEFAULT(0),
			builder.Property(m => m.Discontinued).HasDefaultValue(false);


			builder.HasData(
				new Product() { Id = 1, ProductName = "Chai", SupplierId = 1, CategoryId = 1, QuantityPerUnit = "10 boxes x 20 bags", UnitPrice = 18, UnitsInStock = 39, UnitsOnOrder = 0, ReorderLevel = 10, Discontinued = false },
				new Product() { Id = 2, ProductName = "Chang", SupplierId = 1, CategoryId = 1, QuantityPerUnit = "24 - 12 oz bottles", UnitPrice = 19, UnitsInStock = 17, UnitsOnOrder = 40, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 3, ProductName = "Aniseed Syrup", SupplierId = 1, CategoryId = 2, QuantityPerUnit = "12 - 550 ml bottles", UnitPrice = 10, UnitsInStock = 13, UnitsOnOrder = 70, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 4, ProductName = "Chef Anton''s Cajun Seasoning", SupplierId = 2, CategoryId = 2, QuantityPerUnit = "48 - 6 oz jars", UnitPrice = 22, UnitsInStock = 53, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 5, ProductName = "Chef Anton''s Gumbo Mix", SupplierId = 2, CategoryId = 2, QuantityPerUnit = "36 boxes", UnitPrice = 21.35, UnitsInStock = 0, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = true },
				new Product() { Id = 6, ProductName = "Grandma''s Boysenberry Spread", SupplierId = 3, CategoryId = 2, QuantityPerUnit = "12 - 8 oz jars", UnitPrice = 25, UnitsInStock = 120, UnitsOnOrder = 0, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 7, ProductName = "Uncle Bob''s Organic Dried Pears", SupplierId = 3, CategoryId = 7, QuantityPerUnit = "12 - 1 lb pkgs.", UnitPrice = 30, UnitsInStock = 15, UnitsOnOrder = 0, ReorderLevel = 10, Discontinued = false },
				new Product() { Id = 8, ProductName = "Northwoods Cranberry Sauce", SupplierId = 3, CategoryId = 2, QuantityPerUnit = "12 - 12 oz jars", UnitPrice = 40, UnitsInStock = 6, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 9, ProductName = "Mishi Kobe Niku", SupplierId = 4, CategoryId = 6, QuantityPerUnit = "18 - 500 g pkgs.", UnitPrice = 97, UnitsInStock = 29, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = true },
				new Product() { Id = 10, ProductName = "Ikura", SupplierId = 4, CategoryId = 8, QuantityPerUnit = "12 - 200 ml jars", UnitPrice = 31, UnitsInStock = 31, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 11, ProductName = "Queso Cabrales", SupplierId = 5, CategoryId = 4, QuantityPerUnit = "1 kg pkg.", UnitPrice = 21, UnitsInStock = 22, UnitsOnOrder = 30, ReorderLevel = 30, Discontinued = false },
				new Product() { Id = 12, ProductName = "Queso Manchego La Pastora", SupplierId = 5, CategoryId = 4, QuantityPerUnit = "10 - 500 g pkgs.", UnitPrice = 38, UnitsInStock = 86, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 13, ProductName = "Konbu", SupplierId = 6, CategoryId = 8, QuantityPerUnit = "2 kg box", UnitPrice = 6, UnitsInStock = 24, UnitsOnOrder = 0, ReorderLevel = 5, Discontinued = false },
				new Product() { Id = 14, ProductName = "Tofu", SupplierId = 6, CategoryId = 7, QuantityPerUnit = "40 - 100 g pkgs.", UnitPrice = 23.25, UnitsInStock = 35, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 15, ProductName = "Genen Shouyu", SupplierId = 6, CategoryId = 2, QuantityPerUnit = "24 - 250 ml bottles", UnitPrice = 15.5, UnitsInStock = 39, UnitsOnOrder = 0, ReorderLevel = 5, Discontinued = false },
				new Product() { Id = 16, ProductName = "Pavlova", SupplierId = 7, CategoryId = 3, QuantityPerUnit = "32 - 500 g boxes", UnitPrice = 17.45, UnitsInStock = 29, UnitsOnOrder = 0, ReorderLevel = 10, Discontinued = false },
				new Product() { Id = 17, ProductName = "Alice Mutton", SupplierId = 7, CategoryId = 6, QuantityPerUnit = "20 - 1 kg tins", UnitPrice = 39, UnitsInStock = 0, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = true },
				new Product() { Id = 18, ProductName = "Carnarvon Tigers", SupplierId = 7, CategoryId = 8, QuantityPerUnit = "16 kg pkg.", UnitPrice = 62.5, UnitsInStock = 42, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 19, ProductName = "Teatime Chocolate Biscuits", SupplierId = 8, CategoryId = 3, QuantityPerUnit = "10 boxes x 12 pieces", UnitPrice = 9.2, UnitsInStock = 25, UnitsOnOrder = 0, ReorderLevel = 5, Discontinued = false },
				new Product() { Id = 20, ProductName = "Sir Rodney''s Marmalade", SupplierId = 8, CategoryId = 3, QuantityPerUnit = "30 gift boxes", UnitPrice = 81, UnitsInStock = 40, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 21, ProductName = "Sir Rodney''s Scones", SupplierId = 8, CategoryId = 3, QuantityPerUnit = "24 pkgs. x 4 pieces", UnitPrice = 10, UnitsInStock = 3, UnitsOnOrder = 40, ReorderLevel = 5, Discontinued = false },
				new Product() { Id = 22, ProductName = "Gustaf''s Knäckebröd", SupplierId = 9, CategoryId = 5, QuantityPerUnit = "24 - 500 g pkgs.", UnitPrice = 21, UnitsInStock = 104, UnitsOnOrder = 0, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 23, ProductName = "Tunnbröd", SupplierId = 9, CategoryId = 5, QuantityPerUnit = "12 - 250 g pkgs.", UnitPrice = 9, UnitsInStock = 61, UnitsOnOrder = 0, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 24, ProductName = "Guaraná Fantástica", SupplierId = 10, CategoryId = 1, QuantityPerUnit = "12 - 355 ml cans", UnitPrice = 4.5, UnitsInStock = 20, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = true },
				new Product() { Id = 25, ProductName = "NuNuCa Nuß-Nougat-Creme", SupplierId = 11, CategoryId = 3, QuantityPerUnit = "20 - 450 g glasses", UnitPrice = 14, UnitsInStock = 76, UnitsOnOrder = 0, ReorderLevel = 30, Discontinued = false },
				new Product() { Id = 26, ProductName = "Gumbär Gummibärchen", SupplierId = 11, CategoryId = 3, QuantityPerUnit = "100 - 250 g bags", UnitPrice = 31.23, UnitsInStock = 15, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 27, ProductName = "Schoggi Schokolade", SupplierId = 11, CategoryId = 3, QuantityPerUnit = "100 - 100 g pieces", UnitPrice = 43.9, UnitsInStock = 49, UnitsOnOrder = 0, ReorderLevel = 30, Discontinued = false },
				new Product() { Id = 28, ProductName = "Rössle Sauerkraut", SupplierId = 12, CategoryId = 7, QuantityPerUnit = "25 - 825 g cans", UnitPrice = 45.6, UnitsInStock = 26, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = true },
				new Product() { Id = 29, ProductName = "Thüringer Rostbratwurst", SupplierId = 12, CategoryId = 6, QuantityPerUnit = "50 bags x 30 sausgs.", UnitPrice = 123.79, UnitsInStock = 0, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = true },
				new Product() { Id = 30, ProductName = "Nord-Ost Matjeshering", SupplierId = 13, CategoryId = 8, QuantityPerUnit = "10 - 200 g glasses", UnitPrice = 25.89, UnitsInStock = 10, UnitsOnOrder = 0, ReorderLevel = 15, Discontinued = false },
				new Product() { Id = 31, ProductName = "Gorgonzola Telino", SupplierId = 14, CategoryId = 4, QuantityPerUnit = "12 - 100 g pkgs", UnitPrice = 12.5, UnitsInStock = 0, UnitsOnOrder = 70, ReorderLevel = 20, Discontinued = false },
				new Product() { Id = 32, ProductName = "Mascarpone Fabioli", SupplierId = 14, CategoryId = 4, QuantityPerUnit = "24 - 200 g pkgs.", UnitPrice = 32, UnitsInStock = 9, UnitsOnOrder = 40, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 33, ProductName = "Geitost", SupplierId = 15, CategoryId = 4, QuantityPerUnit = "500 g", UnitPrice = 2.5, UnitsInStock = 112, UnitsOnOrder = 0, ReorderLevel = 20, Discontinued = false },
				new Product() { Id = 34, ProductName = "Sasquatch Ale", SupplierId = 16, CategoryId = 1, QuantityPerUnit = "24 - 12 oz bottles", UnitPrice = 14, UnitsInStock = 111, UnitsOnOrder = 0, ReorderLevel = 15, Discontinued = false },
				new Product() { Id = 35, ProductName = "Steeleye Stout", SupplierId = 16, CategoryId = 1, QuantityPerUnit = "24 - 12 oz bottles", UnitPrice = 18, UnitsInStock = 20, UnitsOnOrder = 0, ReorderLevel = 15, Discontinued = false },
				new Product() { Id = 36, ProductName = "Inlagd Sill", SupplierId = 17, CategoryId = 8, QuantityPerUnit = "24 - 250 g  jars", UnitPrice = 19, UnitsInStock = 112, UnitsOnOrder = 0, ReorderLevel = 20, Discontinued = false },
				new Product() { Id = 37, ProductName = "Gravad lax", SupplierId = 17, CategoryId = 8, QuantityPerUnit = "12 - 500 g pkgs.", UnitPrice = 26, UnitsInStock = 11, UnitsOnOrder = 50, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 38, ProductName = "Côte de Blaye", SupplierId = 18, CategoryId = 1, QuantityPerUnit = "12 - 75 cl bottles", UnitPrice = 263.5, UnitsInStock = 17, UnitsOnOrder = 0, ReorderLevel = 15, Discontinued = false },
				new Product() { Id = 39, ProductName = "Chartreuse verte", SupplierId = 18, CategoryId = 1, QuantityPerUnit = "750 cc per bottle", UnitPrice = 18, UnitsInStock = 69, UnitsOnOrder = 0, ReorderLevel = 5, Discontinued = false },
				new Product() { Id = 40, ProductName = "Boston Crab Meat", SupplierId = 19, CategoryId = 8, QuantityPerUnit = "24 - 4 oz tins", UnitPrice = 18.4, UnitsInStock = 123, UnitsOnOrder = 0, ReorderLevel = 30, Discontinued = false },
				new Product() { Id = 41, ProductName = "Jack''s New England Clam Chowder", SupplierId = 19, CategoryId = 8, QuantityPerUnit = "12 - 12 oz cans", UnitPrice = 9.65, UnitsInStock = 85, UnitsOnOrder = 0, ReorderLevel = 10, Discontinued = false },
				new Product() { Id = 42, ProductName = "Singaporean Hokkien Fried Mee", SupplierId = 20, CategoryId = 5, QuantityPerUnit = "32 - 1 kg pkgs.", UnitPrice = 14, UnitsInStock = 26, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = true },
				new Product() { Id = 43, ProductName = "Ipoh Coffee", SupplierId = 20, CategoryId = 1, QuantityPerUnit = "16 - 500 g tins", UnitPrice = 46, UnitsInStock = 17, UnitsOnOrder = 10, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 44, ProductName = "Gula Malacca", SupplierId = 20, CategoryId = 2, QuantityPerUnit = "20 - 2 kg bags", UnitPrice = 19.45, UnitsInStock = 27, UnitsOnOrder = 0, ReorderLevel = 15, Discontinued = false },
				new Product() { Id = 45, ProductName = "Rogede sild", SupplierId = 21, CategoryId = 8, QuantityPerUnit = "1k pkg.", UnitPrice = 9.5, UnitsInStock = 5, UnitsOnOrder = 70, ReorderLevel = 15, Discontinued = false },
				new Product() { Id = 46, ProductName = "Spegesild", SupplierId = 21, CategoryId = 8, QuantityPerUnit = "4 - 450 g glasses", UnitPrice = 12, UnitsInStock = 95, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 47, ProductName = "Zaanse koeken", SupplierId = 22, CategoryId = 3, QuantityPerUnit = "10 - 4 oz boxes", UnitPrice = 9.5, UnitsInStock = 36, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 48, ProductName = "Chocolade", SupplierId = 22, CategoryId = 3, QuantityPerUnit = "10 pkgs.", UnitPrice = 12.75, UnitsInStock = 15, UnitsOnOrder = 70, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 49, ProductName = "Maxilaku", SupplierId = 23, CategoryId = 3, QuantityPerUnit = "24 - 50 g pkgs.", UnitPrice = 20, UnitsInStock = 10, UnitsOnOrder = 60, ReorderLevel = 15, Discontinued = false },
				new Product() { Id = 50, ProductName = "Valkoinen suklaa", SupplierId = 23, CategoryId = 3, QuantityPerUnit = "12 - 100 g bars", UnitPrice = 16.25, UnitsInStock = 65, UnitsOnOrder = 0, ReorderLevel = 30, Discontinued = false },
				new Product() { Id = 51, ProductName = "Manjimup Dried Apples", SupplierId = 24, CategoryId = 7, QuantityPerUnit = "50 - 300 g pkgs.", UnitPrice = 53, UnitsInStock = 20, UnitsOnOrder = 0, ReorderLevel = 10, Discontinued = false },
				new Product() { Id = 52, ProductName = "Filo Mix", SupplierId = 24, CategoryId = 5, QuantityPerUnit = "16 - 2 kg boxes", UnitPrice = 7, UnitsInStock = 38, UnitsOnOrder = 0, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 53, ProductName = "Perth Pasties", SupplierId = 24, CategoryId = 6, QuantityPerUnit = "48 pieces", UnitPrice = 32.8, UnitsInStock = 0, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = true },
				new Product() { Id = 54, ProductName = "Tourtière", SupplierId = 25, CategoryId = 6, QuantityPerUnit = "16 pies", UnitPrice = 7.45, UnitsInStock = 21, UnitsOnOrder = 0, ReorderLevel = 10, Discontinued = false },
				new Product() { Id = 55, ProductName = "Pâté chinois", SupplierId = 25, CategoryId = 6, QuantityPerUnit = "24 boxes x 2 pies", UnitPrice = 24, UnitsInStock = 115, UnitsOnOrder = 0, ReorderLevel = 20, Discontinued = false },
				new Product() { Id = 56, ProductName = "Gnocchi di nonna Alice", SupplierId = 26, CategoryId = 5, QuantityPerUnit = "24 - 250 g pkgs.", UnitPrice = 38, UnitsInStock = 21, UnitsOnOrder = 10, ReorderLevel = 30, Discontinued = false },
				new Product() { Id = 57, ProductName = "Ravioli Angelo", SupplierId = 26, CategoryId = 5, QuantityPerUnit = "24 - 250 g pkgs.", UnitPrice = 19.5, UnitsInStock = 36, UnitsOnOrder = 0, ReorderLevel = 20, Discontinued = false },
				new Product() { Id = 58, ProductName = "Escargots de Bourgogne", SupplierId = 27, CategoryId = 8, QuantityPerUnit = "24 pieces", UnitPrice = 13.25, UnitsInStock = 62, UnitsOnOrder = 0, ReorderLevel = 20, Discontinued = false },
				new Product() { Id = 59, ProductName = "Raclette Courdavault", SupplierId = 28, CategoryId = 4, QuantityPerUnit = "5 kg pkg.", UnitPrice = 55, UnitsInStock = 79, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 60, ProductName = "Camembert Pierrot", SupplierId = 28, CategoryId = 4, QuantityPerUnit = "15 - 300 g rounds", UnitPrice = 34, UnitsInStock = 19, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 61, ProductName = "Sirop d''érable", SupplierId = 29, CategoryId = 2, QuantityPerUnit = "24 - 500 ml bottles", UnitPrice = 28.5, UnitsInStock = 113, UnitsOnOrder = 0, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 62, ProductName = "Tarte au sucre", SupplierId = 29, CategoryId = 3, QuantityPerUnit = "48 pies", UnitPrice = 49.3, UnitsInStock = 17, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 63, ProductName = "Vegie-spread", SupplierId = 7, CategoryId = 2, QuantityPerUnit = "15 - 625 g jars", UnitPrice = 43.9, UnitsInStock = 24, UnitsOnOrder = 0, ReorderLevel = 5, Discontinued = false },
				new Product() { Id = 64, ProductName = "Wimmers gute Semmelknödel", SupplierId = 12, CategoryId = 5, QuantityPerUnit = "20 bags x 4 pieces", UnitPrice = 33.25, UnitsInStock = 22, UnitsOnOrder = 80, ReorderLevel = 30, Discontinued = false },
				new Product() { Id = 65, ProductName = "Louisiana Fiery Hot Pepper Sauce", SupplierId = 2, CategoryId = 2, QuantityPerUnit = "32 - 8 oz bottles", UnitPrice = 21.05, UnitsInStock = 76, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 66, ProductName = "Louisiana Hot Spiced Okra", SupplierId = 2, CategoryId = 2, QuantityPerUnit = "24 - 8 oz jars", UnitPrice = 17, UnitsInStock = 4, UnitsOnOrder = 100, ReorderLevel = 20, Discontinued = false },
				new Product() { Id = 67, ProductName = "Laughing Lumberjack Lager", SupplierId = 16, CategoryId = 1, QuantityPerUnit = "24 - 12 oz bottles", UnitPrice = 14, UnitsInStock = 52, UnitsOnOrder = 0, ReorderLevel = 10, Discontinued = false },
				new Product() { Id = 68, ProductName = "Scottish Longbreads", SupplierId = 8, CategoryId = 3, QuantityPerUnit = "10 boxes x 8 pieces", UnitPrice = 12.5, UnitsInStock = 6, UnitsOnOrder = 10, ReorderLevel = 15, Discontinued = false },
				new Product() { Id = 69, ProductName = "Gudbrandsdalsost", SupplierId = 15, CategoryId = 4, QuantityPerUnit = "10 kg pkg.", UnitPrice = 36, UnitsInStock = 26, UnitsOnOrder = 0, ReorderLevel = 15, Discontinued = false },
				new Product() { Id = 70, ProductName = "Outback Lager", SupplierId = 7, CategoryId = 1, QuantityPerUnit = "24 - 355 ml bottles", UnitPrice = 15, UnitsInStock = 15, UnitsOnOrder = 10, ReorderLevel = 30, Discontinued = false },
				new Product() { Id = 71, ProductName = "Flotemysost", SupplierId = 15, CategoryId = 4, QuantityPerUnit = "10 - 500 g pkgs.", UnitPrice = 21.5, UnitsInStock = 26, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 72, ProductName = "Mozzarella di Giovanni", SupplierId = 14, CategoryId = 4, QuantityPerUnit = "24 - 200 g pkgs.", UnitPrice = 34.8, UnitsInStock = 14, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
				new Product() { Id = 73, ProductName = "Röd Kaviar", SupplierId = 17, CategoryId = 8, QuantityPerUnit = "24 - 150 g jars", UnitPrice = 15, UnitsInStock = 101, UnitsOnOrder = 0, ReorderLevel = 5, Discontinued = false },
				new Product() { Id = 74, ProductName = "Longlife Tofu", SupplierId = 4, CategoryId = 7, QuantityPerUnit = "5 kg pkg.", UnitPrice = 10, UnitsInStock = 4, UnitsOnOrder = 20, ReorderLevel = 5, Discontinued = false },
				new Product() { Id = 75, ProductName = "Rhönbräu Klosterbier", SupplierId = 12, CategoryId = 1, QuantityPerUnit = "24 - 0.5 l bottles", UnitPrice = 7.75, UnitsInStock = 125, UnitsOnOrder = 0, ReorderLevel = 25, Discontinued = false },
				new Product() { Id = 76, ProductName = "Lakkalikööri", SupplierId = 23, CategoryId = 1, QuantityPerUnit = "500 ml", UnitPrice = 18, UnitsInStock = 57, UnitsOnOrder = 0, ReorderLevel = 20, Discontinued = false },
				new Product() { Id = 77, ProductName = "Original Frankfurter grüne Soße", SupplierId = 12, CategoryId = 2, QuantityPerUnit = "12 boxes", UnitPrice = 13, UnitsInStock = 32, UnitsOnOrder = 0, ReorderLevel = 15, Discontinued = false });
		}
	}
}