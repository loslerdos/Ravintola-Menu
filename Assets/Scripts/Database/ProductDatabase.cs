using System;
using System.Collections.Generic;
using System.IO;
using SQLite;
using UnityEngine;

public static class ProductDatabase
{
    private const string DatabaseFileName = "ravintola-menu.db";

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