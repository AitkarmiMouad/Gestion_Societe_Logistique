using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Gestion_Logistique
{
    
    internal class Program
    {

        static string MyConnectionString = ConfigurationManager.ConnectionStrings["DBMS_ConnectionString"].ConnectionString;
        static SqlConnection connection = new SqlConnection(MyConnectionString);


        [STAThreadAttribute]
        static void Main(string[] args)
        {
            fonctionGeneral f = new fonctionGeneral();
            f.presentation();
            while (!f.Login(connection))
            {
                Console.WriteLine("\n\nUserName ou Mot de passe Incorrect");
                Console.ReadKey();
            }

            f.TraitementMenu(connection);

        }

        
    }
}
