using System;
using System.IO;
using SQLite;
using UnityEngine;

public class SQLiteConnectionTest : MonoBehaviour
{
    private const string DatabaseFileName = "ravintola-menu.db";

    private void Start()
    {
        string databasePath = Path.Combine(
            Application.persistentDataPath,
            DatabaseFileName);

        try
        {
            using (SQLiteConnection connection = new SQLiteConnection(databasePath))
            {
                string sqliteVersion = connection.ExecuteScalar<string>(
                    "SELECT sqlite_version();");

                Debug.Log(
                    "SQLITE-YHTEYS TOIMII"
                    + "\nTietokanta: " + databasePath
                    + "\nSQLite-versio: " + sqliteVersion);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError(
                "SQLITE-YHTEYS EPÄONNISTUI"
                + "\n" + exception.Message);
        }
    }
}