using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        static void Main(string[] args)
        {

        


        }

        static void Hozza_ir(string ujNev, string ujFelhasznalonev, bool aktiv, string salt, string hash, DateTime szuletesiIdo, decimal f1Ido, decimal f2Ido, decimal f3Ido)
        {
           
 
            string hozza_ir_sql =
            "INSERT INTO `kalaplengeto_versenyzok`" +
            "(`nev`, `felhasznalonev`, `aktiv`, `salt`, `hash`, `szuletesiIdo`, `fordulo1ido`, `fordulo2ido`, `fordulo3ido`) " +
            "VALUES (@nev, @fnev, @aktiv, @salt, @hash, @szido, @f1ido, @f2ido, @f3ido)";

            MySqlConnection connection = new MySqlConnection();
            string connectionString = "SERVER=localhost;DATABASE=csharp_gyakorlo;UID=root;PASSWORD=;";
            connection.ConnectionString = connectionString;

            connection.Open();



            using (MySqlCommand command = new MySqlCommand(hozza_ir_sql, connection))
            {
                // 3. LÉPÉS: Hozzáadjuk a paramétereket (a C# változókat)
                // A @nev, @fnev, stb. helyfoglalókat kicseréli a valódi változók értékére.
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
        }


        static void Olvas()
        {

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
            connection.Close();
        }



    }
}


