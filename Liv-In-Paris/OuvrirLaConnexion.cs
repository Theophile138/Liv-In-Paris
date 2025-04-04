using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    internal class OuvrirLaConnexion
    {
        public static MySqlConnection ObtenirConnexion()
        {
            string connectionString = "server=localhost;user=root;password=root;database=restaurant;";
            var conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de connexion à la base de données :" + ex.Message);
                return null;
            }
        }
    }
}