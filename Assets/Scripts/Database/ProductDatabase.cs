using System;
using System.Collections.Generic;
using System.IO;
using SQLite;
using UnityEngine;

public static class ProductDatabase
{
    private const string DatabaseFileName = "ravintola-menu.db";
    private const int AppetizerIdRangeStart = 100;
    private const int AppetizerIdRangeEnd = 199;
    private const int MainCourseIdRangeStart = 200;
    private const int MainCourseIdRangeEnd = 299;
    private const int DessertIdRangeStart = 300;
    private const int DessertIdRangeEnd = 399;
    private const int DrinkIdRangeStart = 400;
    private const int DrinkIdRangeEnd = 499;

    private static string GetDatabasePath()
    {
        return Path.Combine(
            Application.persistentDataPath,
            DatabaseFileName);
    }

    public static bool Initialize(List<MenuCategory> categories)
    {
        try
        {
            using (SQLiteConnection connection =
                   new SQLiteConnection(GetDatabasePath()))
            {
                connection.CreateTable<DatabaseProduct>();

                connection.Execute(
                    "CREATE INDEX IF NOT EXISTS idx_products_category " +
                    "ON Products(category);");

                int productCount = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Products;");

                if (productCount > 0)
                {
                    Debug.Log(
                        "PRODUCTS-TAULU ON JO OLEMASSA"
                        + "\nTuotteita: " + productCount);

                    return true;
                }

                int seededProductCount = SeedInitialProducts(
                    connection,
                    categories);

                Debug.Log(
                    "PRODUCTS-TAULU LUOTU JA ALOITUSDATA LISÄTTY"
                    + "\nLisättyjä tuotteita: " + seededProductCount);

                return true;
            }
        }
        catch (Exception exception)
        {
            Debug.LogError(
                "PRODUCTS-TIETOKANNAN ALUSTUS EPÄONNISTUI"
                + "\n" + exception.Message);

            return false;
        }
    }

    public static List<DatabaseProduct> GetAllProducts()
    {
        using (SQLiteConnection connection =
               new SQLiteConnection(GetDatabasePath()))
        {
            return connection.Query<DatabaseProduct>(
                "SELECT * FROM Products " +
                "ORDER BY id;");
        }
    }

    public static List<DatabaseProduct> GetProductsByCategory(string category)
    {
        using (SQLiteConnection connection =
               new SQLiteConnection(GetDatabasePath()))
        {
            return connection.Query<DatabaseProduct>(
                "SELECT * FROM Products " +
                "WHERE category = ? " +
                "ORDER BY id;",
                category);
        }
    }

    public static int GetNextProductId(string category)
    {
        if (!TryGetCategoryIdRange(
                category,
                out int rangeStart,
                out int rangeEnd))
        {
            return 0;
        }

        using (SQLiteConnection connection =
               new SQLiteConnection(GetDatabasePath()))
        {
            List<int> usedIds = connection.Query<int>(
                "SELECT id FROM Products " +
                "WHERE category = ? AND id BETWEEN ? AND ? " +
                "ORDER BY id;",
                category,
                rangeStart,
                rangeEnd);

            for (int id = rangeStart; id <= rangeEnd; id++)
            {
                if (!usedIds.Contains(id))
                {
                    return id;
                }
            }
        }

        return 0;
    }

    public static bool InsertProduct(DatabaseProduct product)
    {
        if (!IsProductValid(product) ||
            !IsIdInCategoryRange(product.id, product.category))
        {
            return false;
        }

        using (SQLiteConnection connection =
               new SQLiteConnection(GetDatabasePath()))
        {
            return connection.Insert(product) > 0;
        }
    }

    public static bool UpdateProduct(DatabaseProduct product)
    {
        if (!IsProductValid(product) ||
            !IsIdInCategoryRange(product.id, product.category))
        {
            return false;
        }

        using (SQLiteConnection connection =
               new SQLiteConnection(GetDatabasePath()))
        {
            return connection.Update(product) > 0;
        }
    }

    public static bool DeleteProduct(int id)
    {
        using (SQLiteConnection connection =
               new SQLiteConnection(GetDatabasePath()))
        {
            return connection.Delete<DatabaseProduct>(id) > 0;
        }
    }

    private static bool IsProductValid(DatabaseProduct product)
    {
        return product != null &&
               product.id > 0 &&
               !string.IsNullOrWhiteSpace(product.productName) &&
               !string.IsNullOrWhiteSpace(product.category) &&
               product.price >= 0f;
    }

    private static bool IsIdInCategoryRange(int id, string category)
    {
        return TryGetCategoryIdRange(
            category,
            out int rangeStart,
            out int rangeEnd) &&
            id >= rangeStart &&
            id <= rangeEnd;
    }

    private static bool TryGetCategoryIdRange(
        string category,
        out int rangeStart,
        out int rangeEnd)
    {
        switch (category)
        {
            case "Alkuruoka":
            case "Alkuruoat":
                rangeStart = AppetizerIdRangeStart;
                rangeEnd = AppetizerIdRangeEnd;
                return true;
            case "Pääruoka":
            case "Pääruoat":
                rangeStart = MainCourseIdRangeStart;
                rangeEnd = MainCourseIdRangeEnd;
                return true;
            case "Jälkiruoka":
            case "Jälkiruoat":
                rangeStart = DessertIdRangeStart;
                rangeEnd = DessertIdRangeEnd;
                return true;
            case "Juoma":
            case "Juomat":
                rangeStart = DrinkIdRangeStart;
                rangeEnd = DrinkIdRangeEnd;
                return true;
            default:
                rangeStart = 0;
                rangeEnd = 0;
                return false;
        }
    }

    private static int SeedInitialProducts(
        SQLiteConnection connection,
        List<MenuCategory> categories)
    {
        if (categories == null)
        {
            Debug.LogWarning(
                "Aloitusdataa ei voitu lisätä: kategoriat puuttuvat.");

            return 0;
        }

        int seededProductCount = 0;

        connection.RunInTransaction(() =>
        {
            foreach (MenuCategory menuCategory in categories)
            {
                if (menuCategory == null || menuCategory.menuItems == null)
                {
                    continue;
                }

                foreach (MenuItem menuItem in menuCategory.menuItems)
                {
                    if (menuItem == null)
                    {
                        continue;
                    }

                    DatabaseProduct databaseProduct = new DatabaseProduct
                    {
                        id = menuItem.id,
                        productName = menuItem.productName,
                        description = menuItem.description,
                        specialNote = menuItem.specialNote,
                        category = string.IsNullOrWhiteSpace(menuItem.category)
                            ? menuCategory.categoryName
                            : menuItem.category,
                        price = menuItem.price
                    };

                    connection.Insert(databaseProduct);
                    seededProductCount++;
                }
            }
        });

        return seededProductCount;
    }
}