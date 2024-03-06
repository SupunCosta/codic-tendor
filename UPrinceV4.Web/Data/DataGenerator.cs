namespace UPrinceV4.Web.Data;

public static class DataGenerator
{
    public static void GenerateData(this ApplicationDbContext dbContext, int tenantId)
    {
        if (tenantId == 1) GenerateForTenant1(dbContext);

        if (tenantId == 2) GenerateForTenant2(dbContext);

        if (tenantId == 3) GenerateForTenant3(dbContext);
    }

    private static void GenerateForTenant1(ApplicationDbContext dbContext)
    {
        var tvCategory = new ProductCategory { Name = "TV" };
        var computersCategory = new ProductCategory { Name = "Computers" };


        dbContext.SaveChanges();
    }

    private static void GenerateForTenant2(ApplicationDbContext dbContext)
    {
        var actionCategory = new ProductCategory { Name = "Action" };
        var romanceCategory = new ProductCategory { Name = "Romance" };


        dbContext.SaveChanges();
    }

    private static void GenerateForTenant3(ApplicationDbContext dbContext)
    {
        var winesCategory = new ProductCategory { Name = "Wines" };
        var beersCategory = new ProductCategory { Name = "Beers" };


        dbContext.SaveChanges();
    }
}