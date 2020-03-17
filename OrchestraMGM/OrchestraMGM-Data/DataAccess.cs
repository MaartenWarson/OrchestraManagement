using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using OrchestraMGM_Business;
using OrchestraMGM_Data;
using Windows.Storage;

namespace OrchestraMGM_Data
{
    public static class DataAccess
    {
        private static string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "orchestradb.db3");
        private const int ID_COLUMN = 0;
        private const int VOORNAAM_COLUMN = 1;
        private const int NAAM_COLUMN = 2;
        private const int ADRES_COLUMN = 3;
        private const int POSTCODE_COLUMN = 4;
        private const int GEMEENTE_COLUMN = 5;
        private const int INSTRUMENT_COLUMN = 6;
        private static long id = 0;

        // DATABASE MAKEN EN OPSLAAN
        public static void InitializeDatabase()
        {
            using (SqliteConnection db = new SqliteConnection("Data Source=" + path))
            {
                db.Open();

                string createCommand = "CREATE TABLE IF NOT EXISTS tableOrchestra (" +
                    "ID INT(5), " +
                    "VOORNAAM VARCHAR(50), " +
                    "NAAM VARCHAR(50), " +
                    "ADRES VARCHAR(100), " +
                    "POSTCODE INT(4), " +
                    "GEMEENTE VARCHAR(100), " +
                    "INSTRUMENT VARCHAR(50));";

                SqliteCommand createTable = new SqliteCommand(createCommand, db);

                createTable.ExecuteReader();
            }
        }

        // DATA VERKRIJGEN VAN DE DATABASE
        public static List<Musician> GetData(int columnNumber)
        {
            string selectStatement = "";
            SqliteConnection db = new SqliteConnection("Data Source=" + path);

            switch (columnNumber)
            {
                case 1:
                    selectStatement = "SELECT * FROM tableOrchestra ORDER BY Id";
                    break;
                case 2:
                    selectStatement = "SELECT * FROM tableOrchestra ORDER BY Voornaam";
                    break;
                case 3:
                    selectStatement = "SELECT * FROM tableOrchestra ORDER BY Naam";
                    break;
                case 4:
                    selectStatement = "SELECT * FROM tableOrchestra ORDER BY Adres";
                    break;
                case 5:
                    selectStatement = "SELECT * FROM tableOrchestra ORDER BY Postcode";
                    break;
                case 6:
                    selectStatement = "SELECT * FROM tableOrchestra ORDER BY Gemeente";
                    break;
                case 7:
                    selectStatement = "SELECT * FROM tableOrchestra ORDER BY Instrument";
                    break;
            }

            SqliteCommand selectCommand = new SqliteCommand(selectStatement, db);

            List<Musician> musicianList = new List<Musician>();
            SqliteDataReader reader = null;

            try
            {
                db.Open();
                reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    Musician musician = new Musician();

                    // Hulpmethode om null-waardes te voorkomen
                    ConsiderNullValues(musician, reader);

                    musicianList.Add(musician);
                }
            }
            catch (SqliteException)
            {
            }
            finally
            {
                db.Close();
            }

            return musicianList;
        }

        // DATA TOEVOEGEN AAN DE DATABASE
        public static void AddData(string voornaam, string naam, string adres, int postcode, string gemeente, string instrument)
        {
            using (SqliteConnection db = new SqliteConnection("Data Source=" + path))
            {
                id = GetIdValue() + 1;

                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO tableOrchestra VALUES (@Id, @Voornaam, @Naam, " +
                    "@Adres, @Postcode, @Gemeente, @Instrument);";
                insertCommand.Parameters.AddWithValue("@Id", id);
                insertCommand.Parameters.AddWithValue("@Voornaam", voornaam);
                insertCommand.Parameters.AddWithValue("@Naam", naam);
                insertCommand.Parameters.AddWithValue("@Adres", adres);
                insertCommand.Parameters.AddWithValue("@Postcode", postcode);
                insertCommand.Parameters.AddWithValue("@Gemeente", gemeente);
                insertCommand.Parameters.AddWithValue("@Instrument", instrument);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        // DATA AANPASSEN IN DATABASE (ervan uitgaande dat voornaam en naam NIET aangepast kunnen worden)
        public static void UpdateData(long id, string voornaam, string naam, string adres, int postcode, string gemeente, string instrument)
        {
            using (SqliteConnection db = new SqliteConnection("Data Source=" + path))
            {
                 db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                updateCommand.CommandText = "UPDATE tableOrchestra SET Voornaam=@Voornaam, Naam=@Naam, Adres=@Adres, Postcode=@Postcode, Gemeente=@Gemeente, Instrument=@Instrument " +
                    "WHERE Id=@Id;";
                updateCommand.Parameters.AddWithValue("@Id", id);
                updateCommand.Parameters.AddWithValue("@Voornaam", voornaam);
                updateCommand.Parameters.AddWithValue("@Naam", naam);
                updateCommand.Parameters.AddWithValue("@Adres", adres);
                updateCommand.Parameters.AddWithValue("@Postcode", postcode);
                updateCommand.Parameters.AddWithValue("@Gemeente", gemeente);
                updateCommand.Parameters.AddWithValue("@Instrument", instrument);

                updateCommand.ExecuteReader();

                db.Close();
            }
        }

        // DATA VERWIJDEREN UIT DATABASE
        public static void DeleteData(long id)
        {
            using (SqliteConnection db = new SqliteConnection("Data Source=" + path))
            {
                db.Open();

                SqliteCommand deleteCommand = new SqliteCommand();
                deleteCommand.Connection = db;

                deleteCommand.CommandText = "DELETE FROM tableOrchestra WHERE Id=@Id;";
                deleteCommand.Parameters.AddWithValue("@Id", id);
 
                deleteCommand.ExecuteReader();

                db.Close();
            }
        }
        // HULPMETHODES
        private static void ConsiderNullValues(Musician musician, SqliteDataReader reader)
        {
            musician.Id = reader.GetInt64(ID_COLUMN);

            if (reader.IsDBNull(VOORNAAM_COLUMN))
            {
                musician.Voornaam = null;
            }
            else
            {
                musician.Voornaam = reader.GetString(VOORNAAM_COLUMN);
            }

            if (reader.IsDBNull(NAAM_COLUMN))
            {
                musician.Naam = null;
            }
            else
            {
                musician.Naam = reader.GetString(NAAM_COLUMN);
            }

            if (reader.IsDBNull(ADRES_COLUMN))
            {
                musician.Adres = null;
            }
            else
            {
                musician.Adres = reader.GetString(ADRES_COLUMN);
            }

            if (reader.IsDBNull(POSTCODE_COLUMN))
            {
                musician.Postcode = 0;
            }
            else
            {
                musician.Postcode = reader.GetInt32(POSTCODE_COLUMN);
            }

            if (reader.IsDBNull(GEMEENTE_COLUMN))
            {
                musician.Gemeente = null;
            }
            else
            {
                musician.Gemeente = reader.GetString(GEMEENTE_COLUMN);
            }

            if (reader.IsDBNull(INSTRUMENT_COLUMN))
            {
                musician.Instrument = null;
            }
            else
            {
                musician.Instrument = reader.GetString(INSTRUMENT_COLUMN);
            }
        }

        private static long GetIdValue()
        {
            long id = 0;
            string selectStatement = "SELECT Id FROM tableOrchestra;";
            SqliteConnection db = new SqliteConnection("Data Source=" + path);
            SqliteCommand selectCommand = new SqliteCommand(selectStatement, db);

            SqliteDataReader reader = null;

            try
            {
                db.Open();
                reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    id = reader.GetInt64(ID_COLUMN);
                }
            }
            catch (SqliteException)
            {
            }
            finally
            {
                db.Close();
            }

            return id;
        }
    }
}
