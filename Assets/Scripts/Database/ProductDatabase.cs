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
        if (!TryGetCategoryNames(
                category,
                out string singularCategory,
                out string pluralCategory))
        {
            return new List<DatabaseProduct>();
        }

        using (SQLiteConnection connection =
               new SQLiteConnection(GetDatabasePath()))
        {
            return connection.Query<DatabaseProduct>(
                "SELECT * FROM Products " +
                "WHERE category = ? OR category = ? " +
                "ORDER BY id;",
                singularCategory,
                pluralCategory);
        }
    }

    public static List<DatabaseProduct> SearchProducts(
        string searchText,
        string category)
    {
        List<DatabaseProduct> products;

        if (string.IsNullOrWhiteSpace(category) ||
            category == "Kaikki Kategoriat")
        {
            products = GetAllProducts();
        }
        else
        {
            if (!TryGetCategoryNames(
                    category,
                    out string singularCategory,
                    out string pluralCategory))
            {
                return new List<DatabaseProduct>();
            }

            using (SQLiteConnection connection =
                   new SQLiteConnection(GetDatabasePath()))
            {
                products = connection.Query<DatabaseProduct>(
                    "SELECT * FROM Products " +
                    "WHERE category = ? OR category = ? " +
                    "ORDER BY id;",
                    singularCategory,
                    pluralCategory);
            }
        }

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return products;
        }

        string normalizedSearchText = searchText.Trim();
        products.RemoveAll(product =>
            string.IsNullOrEmpty(product.productName) ||
            product.productName.IndexOf(
                normalizedSearchText,
            StringComparison.OrdinalIgnoreCase) < 0);

        return products;
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
        int highestId = connection.ExecuteScalar<int>(
            "SELECT COALESCE(MAX(id), ?) FROM Products " +
            "WHERE id BETWEEN ? AND ?;",
            rangeStart - 1,
            rangeStart,
            rangeEnd);

        highestId = Math.Max(
            highestId,
            GetHighestProductIdFromOrderSnapshots(rangeStart, rangeEnd));

        int nextId = highestId + 1;

        return nextId <= rangeEnd ? nextId : 0;
    }
}

    private static int GetHighestProductIdFromOrderSnapshots(
        int rangeStart,
        int rangeEnd)
    {
        int highestId = rangeStart - 1;
        string persistentDataPath = Application.persistentDataPath;

        if (!Directory.Exists(persistentDataPath))
        {
            return highestId;
        }

        string[] orderPaths;

        try
        {
            orderPaths = Directory.GetFiles(
                persistentDataPath,
                "order_*.json",
                SearchOption.AllDirectories);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(
                "Tilaus-ID:itä ei voitu tarkistaa uusia tuotteita varten: "
                + exception.Message);
            return highestId;
        }

        foreach (string orderPath in orderPaths)
        {
            try
            {
                OrderData orderData = JsonUtility.FromJson<OrderData>(
                    File.ReadAllText(orderPath));

                if (orderData?.items == null)
                {
                    continue;
                }

                foreach (OrderLineData line in orderData.items)
                {
                    int productId = line?.menuItem?.id ?? 0;

                    if (productId >= rangeStart &&
                        productId <= rangeEnd &&
                        productId > highestId)
                    {
                        highestId = productId;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning(
                    "Tilaus-ID:tä ei voitu lukea tiedostosta "
                    + orderPath
                    + ": "
                    + exception.Message);
            }
        }

        return highestId;
    }

    public static bool InsertProduct(DatabaseProduct product)
    {
        if (product == null)
        {
            Debug.LogError("TUOTETTA EI VOITU LISÄTÄ: tuotetieto puuttuu.");
            return false;
        }

        if (!IsProductValid(product))
        {
            Debug.LogError(
                "TUOTETTA EI VOITU LISÄTÄ: virheellinen tuotetieto. " +
                $"id={product.id}, nimi='{product.productName}', " +
                $"kategoria='{product.category}', hinta={product.price}");
            return false;
        }

        if (!IsIdInCategoryRange(product.id, product.category))
        {
            Debug.LogError(
                "TUOTETTA EI VOITU LISÄTÄ: ID ei kuulu kategorian alueeseen. " +
                $"id={product.id}, kategoria='{product.category}'");
            return false;
        }

        using (SQLiteConnection connection =
               new SQLiteConnection(GetDatabasePath()))
        {
            int insertedRows = connection.Insert(product);
            Debug.Log(
                "PRODUCTS-LISÄYS: " +
                $"id={product.id}, kategoria='{product.category}', " +
                $"lisättyjä rivejä={insertedRows}");
            return insertedRows > 0;
        }
    }

    public static bool UpdateProduct(DatabaseProduct product)
    {
        if (!IsProductValid(product))
        {
            return false;
        }

        using (SQLiteConnection connection =
               new SQLiteConnection(GetDatabasePath()))
        {
            DatabaseProduct existingProduct =
                connection.Find<DatabaseProduct>(product.id);

            if (existingProduct == null)
            {
                return false;
            }

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

    private static bool TryGetCategoryNames(
        string category,
        out string singularCategory,
        out string pluralCategory)
    {
        switch (category)
        {
            case "Alkuruoka":
            case "Alkuruoat":
                singularCategory = "Alkuruoka";
                pluralCategory = "Alkuruoat";
                return true;
            case "Pääruoka":
            case "Pääruoat":
                singularCategory = "Pääruoka";
                pluralCategory = "Pääruoat";
                return true;
            case "Jälkiruoka":
            case "Jälkiruoat":
                singularCategory = "Jälkiruoka";
                pluralCategory = "Jälkiruoat";
                return true;
            case "Juoma":
            case "Juomat":
                singularCategory = "Juoma";
                pluralCategory = "Juomat";
                return true;
            default:
                singularCategory = null;
                pluralCategory = null;
                return false;
        }
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