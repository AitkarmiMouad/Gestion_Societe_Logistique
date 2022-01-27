using System;
using System.Data.SqlClient;
using System.Data;

namespace Gestion_Logistique
{
    internal class fonctionGeneral
    {
        public void presentation()
        {
            Console.Title = "TransConnect";
            Console.WriteLine("\t\t****************************************************\n");
            Console.WriteLine("\t\t*                                                  *\n");
            Console.WriteLine("\t\t*                   TransConnect                   *\n");
            Console.WriteLine("\t\t*             ------------------------             *\n");
            Console.WriteLine("\t\t*                                                  *\n");
            Console.WriteLine("\t\t****************************************************\n\n\n\n");
            Console.ReadKey();
        }

        // Fonction Pour verifier Login
        public bool Login(SqlConnection connection)
        {
            SqlCommand selectCommand;
            SqlDataReader reader;
            DataTable dt = new DataTable();
            string userName;
            string password;

            Console.Clear();
            Console.WriteLine("\t\t\t\t\t      Connexion \n");
            Console.WriteLine("\t\t\t--------------------------------------------------\n");

            Console.Write(" Entrer le userName     :\t");
            userName = Console.ReadLine();
            Console.Write(" Entrer le mot de passe :\t ");
            password = "";

            // Masquer le mot de passe en affichant *
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring(0, password.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }

            Console.WriteLine();
            // Requete de selection
            selectCommand = new SqlCommand("select * from Administrateur", connection);

            // Ouverture de connexion si fermer
            if (connection.State == ConnectionState.Closed) connection.Open();
            // Execution de requete
            reader = selectCommand.ExecuteReader();
            // Remplissage du DataTable
            dt.Load(reader);
            foreach (DataRow dr in dt.Rows)
            {
                if (userName == dr[1].ToString() && password == dr[2].ToString())
                {
                    connection.Close();
                    return true;
                }
            }

            connection.Close();
            return false;
        }

        // Fonction pour afficher le menu principale
        public int Menu()
        {
            Console.Clear();

            int op; //options du menu

            Console.Title = "TransConnect";
            Console.Clear();
            Console.WriteLine("\t\t\t\t\t       MENU \n");
            Console.WriteLine("\t\t\t--------------------------------------------------\n");
            Console.WriteLine("\t\t\t| 1>> Gestion des salariés                       |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 2>> Gestion des clients                        |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 3>> Gestion Des Commandes                      |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 4>> Statistique                                |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 5>> Quitter le programme                       |\n");
            Console.WriteLine("\t\t\t|------------------------------------------------|\n");

            Console.WriteLine("\n Entrer votre choix: ");
            op = int.Parse(Console.ReadLine());

            return op;
        }

        // Fonction pour traiter le resultat de fonction menu
        public void TraitementMenu(SqlConnection connection)
        {

            switch (Menu())
            {
                case 1:
                    Salarie s = new Salarie();
                    s.salarieMenu(connection);
                    break;
                case 2:
                    Client c = new Client();
                    c.clientMenu(connection);
                    break;
                case 3:
                    Commande commande = new Commande();
                    commande.commandeMenu(connection);
                    break;
                case 4:
                    Statistique statistique = new Statistique();
                    statistique.statistiqueMenu(connection);
                    break;
                case 5:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                    Console.ReadKey();
                    break;
            }
        }

    }
}
