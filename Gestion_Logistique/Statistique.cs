using System;
using System.Data.SqlClient;
using System.Data;

namespace Gestion_Logistique
{
    internal class Statistique
    {
        // Fonction affichage du Menu de Statistique et bilan
        public void statistiqueMenu(SqlConnection connection)
        {
            Console.Clear();

            int op; //options du menu

            Console.Clear();
            Console.WriteLine("\t\t\t\t\t    Bilan  \n");
            Console.WriteLine("\t\t\t|---------------------------------------------------------|\n");
            Console.WriteLine("\t\t\t| 1>> Afficher Nombre de livraison par chauffeur          |\n");
            Console.WriteLine("\t\t\t|                                                         |\n");
            Console.WriteLine("\t\t\t| 2>> Afficher le commande selon une periode de temps     |\n");
            Console.WriteLine("\t\t\t|                                                         |\n");
            Console.WriteLine("\t\t\t| 3>> Afficher la moyenne des prix des commande           |\n");
            Console.WriteLine("\t\t\t|                                                         |\n");
            Console.WriteLine("\t\t\t| 4>> Afficher la moyenne des compte clients              |\n");
            Console.WriteLine("\t\t\t|                                                         |\n");
            Console.WriteLine("\t\t\t| 5>> Afficher la liste des commande pour un client       |\n");
            Console.WriteLine("\t\t\t|                                                         |\n");
            Console.WriteLine("\t\t\t| 6>> Revenir au menu principal                           |\n");
            Console.WriteLine("\t\t\t|---------------------------------------------------------|\n");

            Console.WriteLine("\n Entrer votre choix: ");
            op = int.Parse(Console.ReadLine());
            do
            {
                switch (op)
                {
                    case 1:
                        // appel de Fonction
                        livraisonParChauffeur(connection);
                        Console.ReadKey();
                        // appel de Fonction
                        statistiqueMenu(connection);
                        break;
                    case 2:
                        // appel de Fonction
                        commandeEntreDate(connection);
                        Console.ReadKey();
                        // appel de Fonction
                        statistiqueMenu(connection);
                        break;
                    case 3:
                        // appel de Fonction
                        moyennePrixCommande(connection);
                        Console.ReadKey();
                        // appel de Fonction
                        statistiqueMenu(connection);
                        break;
                    case 4:
                        // appel de Fonction
                        moyenneClient(connection);
                        Console.ReadKey();
                        // appel de Fonction
                        statistiqueMenu(connection);
                        break;
                    case 5:
                        // appel de Fonction
                        commandeClient(connection);
                        Console.ReadKey();
                        // appel de Fonction
                        statistiqueMenu(connection);
                        break;
                    case 6:
                        // Creation d'objet
                        fonctionGeneral f = new fonctionGeneral();
                        // appel de Fonction
                        f.TraitementMenu(connection);
                        break;
                    default:
                        Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                        Console.ReadKey();
                        break;
                }

            } while (op != 1 && op != 2 && op != 3 && op != 4 && op != 5 && op != 6);


        }

        // Fonction Pour afficher les livraison de chaque chauffeur
        private void livraisonParChauffeur(SqlConnection connection)
        {
            try
            {
                // Declaration
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();

                Console.Clear();
                Console.WriteLine("\t---------------------------- Livraison Par Chauffeur ----------------------------\n\n");

                // Requete de selection
                selectCommand = new SqlCommand("select * from Salarie where Poste = 'Chauffeur' ", connection);
                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();
                // Remplissage du DataTable
                reader = selectCommand.ExecuteReader();
                dt.Load(reader);
                // Affichage du DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    Console.WriteLine("\nChauffeur : " + dr[1] + " " + dr[2] + "\n");

                    selectCommand = new SqlCommand("select * from Commande where Num_SS = @Num_SS ", connection);
                    int Num_SS = int.Parse(dr[0].ToString());
                    selectCommand.Parameters.AddWithValue("@Num_SS", Num_SS);

                    reader = selectCommand.ExecuteReader();
                    dt2.Load(reader);

                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        Console.WriteLine("\t\t\t");
                        for (int i = 0; i < dt2.Columns.Count; i++)
                        {
                            Console.Write(dr2[i] + " ; ");

                        }
                        Console.WriteLine("\n");
                    }

                    Console.Write("\n");
                    dt2.Clear();
                }

            }
            catch (Exception ex)
            {
                // affichage d'erreur
                Console.WriteLine("\n\nErreur : " + ex);
            }
            finally
            {
                // Fermeture de connexion
                connection.Close();
            }
        }

        // Fonction Pour Afficher les commande entre deux date
        private void commandeEntreDate(SqlConnection connection)
        {
            try
            {
                // Declaration
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();

                Console.Clear();
                Console.WriteLine("\t---------------------------- Afficher Commande ----------------------------\n\n");

                // Requete de selection
                selectCommand = new SqlCommand("select * from Commande where _Date between @_date1 and @_date2", connection);

                // Lecture des deux date
                Console.WriteLine("Entrer la premiere Date : (annee-mois-jour) ");
                string _date1 = Console.ReadLine();
                Console.WriteLine("Entrer la deuxieme Date : (annee-mois-jour) ");
                string _date2 = Console.ReadLine();

                // Ajout des parametre de requete sql
                selectCommand.Parameters.AddWithValue("@_date1", _date1);
                selectCommand.Parameters.AddWithValue("@_date2", _date2);

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();
                // Remplissage du DataTable
                reader = selectCommand.ExecuteReader();
                dt.Load(reader);
                // Affichage du DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        Console.Write(dr[i].ToString() + " ; ");
                    }

                    Console.Write("\n");
                }
            }
            catch (Exception ex)
            {
                // affichage d'erreur
                Console.WriteLine("\n\nErreur : " + ex);
            }
            finally
            {
                // Fermeture de connexion
                connection.Close();
            }
        }

        // Fonction Pour Afficher la moyenne des prix de tous les commandes
        private void moyennePrixCommande(SqlConnection connection)
        {
            try
            {
                // Declaration
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();

                Console.Clear();
                Console.WriteLine("\t---------------------------- Prix Moyenne des Commandes ----------------------------\n\n");

                // Requete de selection
                selectCommand = new SqlCommand("select avg(Prix) from Commande ", connection);

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();

                // Remplissage du DataTable
                reader = selectCommand.ExecuteReader();
                dt.Load(reader);
                // Affichage du DataTable
                Console.Write("Prix Moyenne des Commandes : " + dt.Rows[0][0].ToString());


            }
            catch (Exception ex)
            {
                // affichage d'erreur
                Console.WriteLine("\n\nErreur : " + ex);
            }
            finally
            {
                // Fermeture de connexion
                connection.Close();
            }
        }

        // Fonction Pour Affiche La moyenne de prix de commande d'un client
        private void moyenneClient(SqlConnection connection)
        {
            try
            {
                // Declaration
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();

                Console.Clear();
                Console.WriteLine("\t---------------------------- Moyenne des comptes clients ----------------------------\n\n");

                // Requete de selection
                selectCommand = new SqlCommand("select * from Client ", connection);

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();

                // Remplissage du DataTable
                reader = selectCommand.ExecuteReader();
                dt.Load(reader);
                // Affichage du DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    Console.Write("\nClient : " + dr[1] + " " + dr[2]);

                    selectCommand = new SqlCommand("select avg(Prix),Id_client from Commande where Id_client = @Id_client group by Id_client ", connection);
                    int Id_client = int.Parse(dr[0].ToString());
                    selectCommand.Parameters.AddWithValue("@Id_client", Id_client);

                    reader = selectCommand.ExecuteReader();
                    dt2.Load(reader);
                    if (dt2.Rows.Count != 0)
                    {
                        Console.Write("\t\tMoyenne : " + dt2.Rows[0][0]);
                    }
                    else
                    {
                        Console.Write("\t\tMoyenne : 0 ");
                    }
                    Console.Write("\n");
                    dt2.Clear();
                }

            }
            catch (Exception ex)
            {
                // affichage d'erreur
                Console.WriteLine("\n\nErreur : " + ex);
            }
            finally
            {
                // Fermeture de connexion
                connection.Close();
            }
        }

        // Fonction Pour Afficher les commande d'un client
        private void commandeClient(SqlConnection connection)
        {
            try
            {
                // Declaration
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();

                Console.Clear();
                Console.WriteLine("\t---------------------------- Afficher Commande d'un Client ----------------------------\n\n");

                // Lecture du Id Client
                Console.WriteLine("\nEntrer le Id du Client : ");
                int idClient = int.Parse(Console.ReadLine());

                // Requete de selection
                selectCommand = new SqlCommand("select * from Commande where Id_client=@Id_Client", connection);

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();
                // Ajout des parametre de requete sql
                selectCommand.Parameters.AddWithValue("@Id_Client", idClient);
                // Remplissage du DataTable
                reader = selectCommand.ExecuteReader();
                dt.Load(reader);
                // Affichage du DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        Console.Write(dr[i].ToString() + " ; ");
                    }
                    Console.Write("\n");
                }

            }
            catch (Exception ex)
            {
                // affichage d'erreur
                Console.WriteLine("\n\nErreur : " + ex);
            }
            finally
            {
                // Fermeture de connexion
                connection.Close();
            }
        }

    }
}
