using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Mikulati_bati
{

    //    SQL Tábla létrehozása
    //CREATE TABLE kalaplengeto_versenyzok(
    //id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    //nev VARCHAR(100) NOT NULL,
    //felhasznalonev VARCHAR(50) NOT NULL UNIQUE,
    //    aktiv BOOLEAN NOT NULL DEFAULT 1,
    //    salt CHAR(64) NOT NULL,
    //    hash CHAR(128) NOT NULL,
    //    szuletesiIdo DATE,
    //    fordulo1ido DECIMAL(5, 3),
    //    fordulo2ido DECIMAL(5, 3),
    //    fordulo3ido DECIMAL(5, 3)
    //   );
    internal class Program
    {

        static string[] menupontok = { "Adatbázis olvasása", "Adatbázishoz hozzádás", "Versenyző törlése" };
        static Action[] fuggvenyek = { Olvas, Hozza_ir, Torol };
        static int menupontok_szama = fuggvenyek.Length;
        static int aktualis_menu_pont = 0;
        static bool kivalasztva = false;
        static bool fut = true;
        static void Main(string[] args)
        {



            Console.Clear();
            while (fut) {

                try
                {
                   
                    Console.Clear();
                    for (int i = 0; i < menupontok_szama; i++)
                    {
                        if (aktualis_menu_pont == i)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(menupontok[i]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.WriteLine(menupontok[i]);

                        }
                    }
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Enter:
                            kivalasztva = true;
                            break;


                        case ConsoleKey.UpArrow:
                            if (aktualis_menu_pont > 0)
                            {
                                aktualis_menu_pont--;
                            }
                            else
                            {
                                aktualis_menu_pont = menupontok_szama - 1;
                            }
                            break;

                        case ConsoleKey.DownArrow:
                            if (aktualis_menu_pont < menupontok_szama - 1)
                            {

                                aktualis_menu_pont++;
                            }
                            else
                            {
                                aktualis_menu_pont = 0;
                            }
                            break;

                        default:
                            Console.Beep();
                            break;
                    }

                    if (kivalasztva)
                    {
                        fuggvenyek[aktualis_menu_pont]();
                        Console.ReadLine();
                        kivalasztva = false;
                        aktualis_menu_pont = 0;
                    }
                }

                catch (Exception)
                {
                    Console.WriteLine("Hiba történt!");
                    Console.WriteLine("Szeretné látni a hibát?");

                }
            }


        }



        static void Hozza_ir()
        {
            Console.Clear();

            string hozza_ir_sql =
            "INSERT INTO `kalaplengeto_versenyzok`" +
            "(`nev`, `felhasznalonev`, `aktiv`, `salt`, `hash`, `szuletesiIdo`, `fordulo1ido`, `fordulo2ido`, `fordulo3ido`) " +
            "VALUES (@nev, @fnev, @aktiv, @salt, @hash, @szido, @f1ido, @f2ido, @f3ido)";



            Console.Write("Kérem a versenyző nevét (string): ");
            string ujNev = Console.ReadLine() ?? "";

            Console.Write("Kérem a felhasználónevet (string): ");
            string ujFelhasznalonev = Console.ReadLine() ?? "";


            Console.Write("Kérem a SALT stringet: ");
            string salt = Console.ReadLine() ?? "";

            Console.Write("Kérem a HASH stringet: ");
            string hash = Console.ReadLine() ?? "";



            Console.Write("Aktív legyen a felhasználó? (I/N) ");
            bool aktiv = (Console.ReadLine()?.ToUpper() == "I");


            Console.Write("Kérem a születési dátumot (ÉÉÉÉ-HH-NN formátumban): ");
            DateTime szuletesiIdo;



            Console.WriteLine("Hibás dátumformátum! Alapértelmezett dátum: 1990.01.01.");
            szuletesiIdo = Convert.ToDateTime(Console.ReadLine());




            decimal f1Ido, f2Ido, f3Ido;


            Console.Write("Kérem az 1. forduló idejét (decimal, pl. 9.5): ");
            decimal.TryParse(Console.ReadLine()?.Replace(",", "."), out f1Ido);

            Console.Write("Kérem a 2. forduló idejét (decimal, pl. 10.1): ");
            decimal.TryParse(Console.ReadLine()?.Replace(",", "."), out f2Ido);

            Console.Write("Kérem a 3. forduló idejét (decimal, pl. 8.8): ");
            decimal.TryParse(Console.ReadLine()?.Replace(",", "."), out f3Ido);

            MySqlConnection connection = new MySqlConnection();
            string connectionString = "SERVER=localhost;DATABASE=csharp_gyakorlo;UID=root;PASSWORD=;";
            connection.ConnectionString = connectionString;

            connection.Open();







            using (MySqlCommand command = new MySqlCommand(hozza_ir_sql, connection))
            {

                command.Parameters.AddWithValue("@nev", ujNev);
                command.Parameters.AddWithValue("@fnev", ujFelhasznalonev);
                command.Parameters.AddWithValue("@aktiv", aktiv);
                command.Parameters.AddWithValue("@salt", salt);
                command.Parameters.AddWithValue("@hash", hash);
                command.Parameters.AddWithValue("@szido", szuletesiIdo);
                command.Parameters.AddWithValue("@f1ido", f1Ido);
                command.Parameters.AddWithValue("@f2ido", f2Ido);
                command.Parameters.AddWithValue("@f3ido", f3Ido);
                command.ExecuteNonQuery();
            }

            connection.Close();

            Console.WriteLine("Versenyző hozzáadva!");
      
        }


        static void Olvas()
        {
            Console.Clear();
            MySqlConnection connection = new MySqlConnection();
            string connectionString = "SERVER=localhost;DATABASE=csharp_gyakorlo;UID=root;PASSWORD=;";
            connection.ConnectionString = connectionString;

            connection.Open();
            string sql = "SELECT * FROM kalaplengeto_versenyzok";
            MySqlCommand cmd = new MySqlCommand(sql, connection);

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {


                int id = reader.GetInt32("id");
                string nev = reader.GetString("nev");
                string felhasznalonev = reader.GetString("felhasznalonev");
                bool aktiv = reader.GetBoolean("aktiv");
                string salt = reader.GetString("salt");
                string hash = reader.GetString("hash");
                DateTime szuletesiIdo = reader.GetDateTime("szuletesiIdo");
                decimal f1ido = reader.GetDecimal("fordulo1ido");
                decimal f2ido = reader.GetDecimal("fordulo2ido");
                decimal f3ido = reader.GetDecimal("fordulo3ido");



                Console.WriteLine($"ID: {id}, Név: {nev}");
                Console.WriteLine($"  Felhasználónév: {felhasznalonev}, Aktív: {aktiv}");
                Console.WriteLine($"  Szül. idő: {szuletesiIdo:yyyy.MM.dd.}");
                Console.WriteLine($"  Sót/Kivonat: {salt} {hash}");
                Console.WriteLine($"  Futamidők: F1={f1ido}s, F2={f2ido}s, F3={f3ido}s");
                Console.WriteLine("------------------------------------------");





            }
            reader.Close();
            connection.Close();


           
        }


        static void Torol()
        {

            Console.Clear();
            MySqlConnection connection = new MySqlConnection();
            string connectionString = "SERVER=localhost;DATABASE=csharp_gyakorlo;UID=root;PASSWORD=;";
            connection.ConnectionString = connectionString;

            connection.Open();
            string sql = "SELECT * FROM kalaplengeto_versenyzok";
            MySqlCommand cmd = new MySqlCommand(sql, connection);

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {


                int id = reader.GetInt32("id");
                string nev = reader.GetString("nev");
                string felhasznalonev = reader.GetString("felhasznalonev");
                bool aktiv = reader.GetBoolean("aktiv");
                string salt = reader.GetString("salt");
                string hash = reader.GetString("hash");
                DateTime szuletesiIdo = reader.GetDateTime("szuletesiIdo");
                decimal f1ido = reader.GetDecimal("fordulo1ido");
                decimal f2ido = reader.GetDecimal("fordulo2ido");
                decimal f3ido = reader.GetDecimal("fordulo3ido");



                Console.WriteLine($"ID: {id}, Név: {nev}");
                Console.WriteLine($"  Felhasználónév: {felhasznalonev}, Aktív: {aktiv}");
                Console.WriteLine($"  Szül. idő: {szuletesiIdo:yyyy.MM.dd.}");
                Console.WriteLine($"  Sót/Kivonat: {salt} {hash}");
                Console.WriteLine($"  Futamidők: F1={f1ido}s, F2={f2ido}s, F3={f3ido}s");
                Console.WriteLine("------------------------------------------");





            }
            reader.Close();





            Console.WriteLine("Adja meg melyiket szeretné törölni (nev):");
            string nev_t = Console.ReadLine();

            string sql_del = "DELETE FROM kalaplengeto_versenyzok WHERE hash = @nev";

            using (MySqlCommand command = new MySqlCommand(sql_del, connection))
            {
                command.Parameters.AddWithValue("@nev", nev_t);
                command.ExecuteNonQuery();
            }

        }

    }
}


