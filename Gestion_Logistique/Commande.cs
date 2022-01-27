using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;
// NPOI package pour la gestion des fichier excel
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Gestion_Logistique
{
    internal class Commande
    {
        // Declaration
        private int Id_client;
        private string Livraison_pointA;
        private string Livraison_pointB;
        private int Prix;
        private int Num_SS; // Id chauffeur
        private string _Date;
        private int Id_Vehicule;
        private byte _status;
        private string _Description;

        // Fonction affichage du Menu de gestion des Commandes
        public void commandeMenu(SqlConnection connection)
        {
            Console.Clear();

            int op; //options du menu

            Console.Clear();
            Console.WriteLine("\t\t\t\t\t    Gestion des Commandes  \n");
            Console.WriteLine("\t\t\t|------------------------------------------------|\n");
            Console.WriteLine("\t\t\t| 1>> Ajouter une commande                       |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 2>> Modifier une commande                      |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 3>> Supprimer une commande                     |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 4>> Afficher une commande                      |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 5>> Afficher tous les commandes                |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 6>> Revenir au menu principal                  |\n");
            Console.WriteLine("\t\t\t|------------------------------------------------|\n");

            Console.WriteLine("\n Entrer votre choix: ");
            op = int.Parse(Console.ReadLine());
            do
            {
                switch (op)
                {
                    case 1:
                        // appel de fonction
                        ajouterCommande(connection);
                        Console.ReadKey();
                        // appel de fonction
                        commandeMenu(connection);
                        break;
                    case 2:
                        // appel de fonction
                        modifierCommande(connection);
                        Console.ReadKey();
                        // appel de fonction
                        commandeMenu(connection);
                        break;
                    case 3:
                        // appel de fonction
                        supprimerCommande(connection);
                        Console.ReadKey();
                        // appel de fonction
                        commandeMenu(connection);
                        break;
                    case 4:
                        // appel de fonction
                        afficherCommande(connection);
                        Console.ReadKey();
                        // appel de fonction
                        commandeMenu(connection);
                        break;
                    case 5:
                        // appel de fonction
                        afficherTousCommande(connection);
                        Console.ReadKey();
                        // appel de fonction
                        commandeMenu(connection);
                        break;
                    case 6:
                        // Creation d'objet
                        fonctionGeneral f = new fonctionGeneral();
                        // appel de fonction
                        f.TraitementMenu(connection);
                        break;
                    default:
                        Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                        Console.ReadKey();
                        break;
                }
            } while (op != 1 && op != 2 && op != 3 && op != 4 && op != 5 && op != 6);

        }

        // Fonction Pour Ajouter une Commande
        private void ajouterCommande(SqlConnection connection)
        {
            try
            {
                // Declaration
                Client c = new Client();
                Salarie salarie = new Salarie();
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();
                bool b = true;
                string path;
                XSSFWorkbook xssfwb;
                ISheet sheet;
                int idLigne;

                Console.Clear();
                Console.WriteLine("\t---------------------------- Ajout Commande ----------------------------\n\n");

                // Choisir Un Id Client Valid
                do
                {
                    b = true;
                    // appel de fonction
                    c.afficherTousClient(connection);
                    // Lecture Du id Client
                    Console.WriteLine("\nEntrer le id du client : ");
                    this.Id_client = int.Parse(Console.ReadLine());

                    // Requete de selection
                    selectCommand = new SqlCommand("select * from Client where Id_Client=@Id_Client", connection);

                    // Ouverture de connexion si fermer
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    // Ajout des parametre de requete sql
                    selectCommand.Parameters.AddWithValue("@Id_Client", Id_client);
                    reader = selectCommand.ExecuteReader();
                    // Remplissage du DataTable
                    dt.Load(reader);
                    if (dt.Rows.Count == 0)
                    {
                        Console.WriteLine("Ce client N'existe pas Veiller l'inserer d'avance ou choisir un Id valide");
                        b = false;
                    }

                } while (b == false);


                // Selectionner le fichier Distance.xlsx
                // la structure du fichier .xlsx doit etre | Ville depart | Ville d'arriver | distance | temps |

                Console.WriteLine("Veillez choisir le fichier des distance (cliquer sur une touche)");
                Console.ReadKey();
                Console.WriteLine();

                // Ouverture de FileDialog
                OpenFileDialog dialog = new OpenFileDialog();
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    // path du fichier selectionne
                    path = dialog.FileName;

                    // Affichage du fichier
                    Console.WriteLine(string.Format("{4,15} | {0,25} | {1,25} | {2,15} | {3,15}", "Ville Depart", "Ville d'arriver", "Distance", "Temps", "id"));
                    Console.WriteLine("\t-------------------------------------------------------------------------------------------------------");

                    using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        xssfwb = new XSSFWorkbook(file);
                    }

                    sheet = xssfwb.GetSheet("Feuil1");
                    for (int row = 0; row <= sheet.LastRowNum; row++)
                    {
                        if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                        {
                            Console.WriteLine(string.Format("{4,15} | {0,25} | {1,25} | {2,15} | {3,15}", sheet.GetRow(row).GetCell(0).StringCellValue, sheet.GetRow(row).GetCell(1).StringCellValue, sheet.GetRow(row).GetCell(2).NumericCellValue, sheet.GetRow(row).GetCell(3).StringCellValue, row));
                        }
                    }

                }

                // Lecture de Livraison_pointA et Livraison_pointB depuis les valeur dans fichier distance
                do
                {
                    path = dialog.FileName;
                    using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        xssfwb = new XSSFWorkbook(file);
                    }
                    sheet = xssfwb.GetSheet("Feuil1");

                    Console.WriteLine("Veillez choisir le id de la ligne :");
                    idLigne = int.Parse(Console.ReadLine());
                    if (sheet.GetRow(idLigne) != null)
                    {
                        Console.WriteLine(string.Format("{4,15} | {0,25} | {1,25} | {2,15} | {3,15}", sheet.GetRow(idLigne).GetCell(0).StringCellValue, sheet.GetRow(idLigne).GetCell(1).StringCellValue, sheet.GetRow(idLigne).GetCell(2).NumericCellValue, sheet.GetRow(idLigne).GetCell(3).StringCellValue, idLigne));
                        Livraison_pointA = sheet.GetRow(idLigne).GetCell(0).StringCellValue;
                        Livraison_pointB = sheet.GetRow(idLigne).GetCell(1).StringCellValue;
                    }

                } while (sheet.GetRow(idLigne) == null);

                // Lecture du Prix
                Console.WriteLine("Veillez Entrez le prix :");
                this.Prix = int.Parse(Console.ReadLine());

                // Lecture de date
                Console.WriteLine("Entrer la Date de Commande : (annee-mois-jour) ");
                this._Date = Console.ReadLine();

                // Choix d'un id de chauffeur valide
                do
                {
                    b = true;

                    // Requete de selection
                    selectCommand = new SqlCommand("select * from Salarie where Poste='Chauffeur' and Num_SS not in (select Num_SS from Commande where _Date = @_Date)", connection);
                    // Ajout des parametre de requete sql
                    selectCommand.Parameters.AddWithValue("@_Date", _Date);

                    Console.Write("\n\n\n");

                    // Ouverture de connexion si fermer
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    // Remplissage du DataTable
                    reader = selectCommand.ExecuteReader();
                    dt = new DataTable();
                    // Affichage du DataTable
                    dt.Load(reader);
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < 11; i++)
                        {
                            Console.Write(dr[i].ToString() + " ; ");

                        }
                        Console.Write("\n");

                    }

                    Console.WriteLine("\nEntrer le id du Chauffeur : ");
                    this.Num_SS = int.Parse(Console.ReadLine());

                    // Verifier si le Id Choisis existe dans la DataTable
                    if (!dt.AsEnumerable().Any(row => Num_SS == row.Field<int>(0)))
                    {
                        Console.WriteLine("\n\nCe Chauffeur N'existe pas Veiller choisir un Id valide");
                        b = false;
                    }

                } while (b == false);

                // choix d'un id vehicule valide
                do
                {
                    b = true;
                    // Requete de selection
                    selectCommand = new SqlCommand("select * from Vehicule", connection);

                    Console.Write("\n\n\n");
                    // Ouverture de connexion si fermer
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    // Remplissage du DataTable
                    reader = selectCommand.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(reader);
                    // Affichage du DataTable
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Console.Write(dr[i].ToString() + " ; ");

                        }
                        Console.Write("\n");

                    }

                    Console.WriteLine("\nEntrer le matricule de vehicule : ");
                    this.Id_Vehicule = int.Parse(Console.ReadLine());
                    // Verifier si le Id Choisis existe dans la DataTable
                    if (!dt.AsEnumerable().Any(row => Id_Vehicule == row.Field<int>(0)))
                    {
                        Console.WriteLine("\n\nCe vehicule  N'existe pas Veiller choisir un matricule valide");
                        b = false;
                    }

                } while (b == false);

                // Choix du status de la commande
                do
                {
                    b = true;
                    Console.WriteLine("Entrer le status de la Commande : \n1- Livré\n2 - Non Livré ");
                    int valeur = int.Parse(Console.ReadLine());
                    switch (valeur)
                    {
                        case 1:
                            this._status = 1;
                            break;
                        case 2:
                            this._status = 0;
                            break;
                        default:
                            Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                            Console.ReadKey();
                            b = false;
                            break;
                    }
                } while (b == false);

                // Lecture de la description de commande
                Console.WriteLine("\nEntrer Une description sur la Commande : ");
                this._Description = Console.ReadLine();

                // Requete d'insertion dans la base de donnees
                SqlCommand insertCommand = new SqlCommand("INSERT INTO Commande (Id_client,Livraison_pointA,Livraison_pointB,Prix,Num_SS,_Date,Id_Vehicule,_status,_Description) VALUES (@Id_client,@Livraison_pointA,@Livraison_pointB,@Prix,@Num_SS,@_Date,@Id_Vehicule,@_status,@_Description)", connection);

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();
                // Ajout des parametre de requete sql
                insertCommand.Parameters.AddWithValue("@Id_client", Id_client);
                insertCommand.Parameters.AddWithValue("@Livraison_pointA", Livraison_pointA);
                insertCommand.Parameters.AddWithValue("@Livraison_pointB", Livraison_pointB);
                insertCommand.Parameters.AddWithValue("@Prix", Prix);
                insertCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                insertCommand.Parameters.AddWithValue("@_Date", _Date);
                insertCommand.Parameters.AddWithValue("@Id_Vehicule", Id_Vehicule);
                insertCommand.Parameters.AddWithValue("@_status", _status);
                insertCommand.Parameters.AddWithValue("@_Description", _Description);
                // Execution de requete
                insertCommand.ExecuteNonQuery();
                // Message de Succes
                Console.WriteLine("\n\nAjouter Avec Succes");

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

        // Fonction Pour modifier une Commande
        private void modifierCommande(SqlConnection connection)
        {


            try
            {
                SqlCommand updateCommand;
                SqlCommand ExistedValue;

                Console.Clear();
                Console.WriteLine("\t---------------------------- Modifier Commande ----------------------------\n\n");

                // Lecture du id Commande
                Console.WriteLine("\nEntrer le Id de la Commande : ");
                int Id_Commande = int.Parse(Console.ReadLine());

                // Requete de selection
                ExistedValue = new SqlCommand("select count(Id_Commande) from Commande where Id_Commande=@Id_Commande", connection);
                // Ajout des parametre de requete sql
                ExistedValue.Parameters.AddWithValue("@Id_Commande", Id_Commande);
                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();

                if (Convert.ToInt32(ExistedValue.ExecuteScalar()) > 0)
                {
                    Console.WriteLine("\nEntrer le champ a modifier :\n1-Livraison point A\n2-Livraison point B\n3-Prix\n4-Chauffeur\n5-_Date\n6-Vehicule\n7-Status\n8-Description");
                    int valeur = int.Parse(Console.ReadLine());

                    bool b;
                    SqlDataReader reader;
                    DataTable dt;

                    do
                    {
                        switch (valeur)
                        {
                            case 1:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Commande set Livraison_pointA='@Livraison_pointA' where Id_Commande=@Id_Commande", connection);
                                Console.WriteLine("\nVeillez Entrer la ville de depart : ");
                                string Livraison_pointA = Console.ReadLine();
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@Livraison_pointA", Livraison_pointA);
                                updateCommand.Parameters.AddWithValue("@Id_Commande", Id_Commande);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();
                                break;
                            case 2:
                                updateCommand = new SqlCommand("update Commande set Livraison_pointB='@Livraison_pointB' where Id_Commande=@Id_Commande", connection);
                                Console.WriteLine("\nVeillez Entrer la ville d'arrivee : ");
                                string Livraison_pointB = Console.ReadLine();
                                updateCommand.Parameters.AddWithValue("@Livraison_pointA", Livraison_pointB);
                                updateCommand.Parameters.AddWithValue("@Id_Commande", Id_Commande);
                                updateCommand.ExecuteNonQuery();


                                break;
                            case 3:
                                updateCommand = new SqlCommand("update Commande set Prix='@Prix' where Id_Commande=@Id_Commande", connection);
                                Console.WriteLine("\nVeillez Entrer le nom : ");
                                string Prix = Console.ReadLine();
                                updateCommand.Parameters.AddWithValue("@Prix", Prix);
                                updateCommand.Parameters.AddWithValue("@Id_Commande", Id_Commande);
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 4:

                                do
                                {
                                    b = true;
                                    // Requete de selection des chauffeur
                                    SqlCommand selectCommand = new SqlCommand("select * from Salarie where Poste='Chauffeur' and Num_SS not in (select Num_SS from Commande where _Date in (select _Date from Commande where Id_Commande=@Id_Commande))", connection);
                                    selectCommand.Parameters.AddWithValue("@Id_Commande", Id_Commande);

                                    Console.Write("\n\n\n");
                                    // Ouverture de connexion si fermer
                                    if (connection.State == ConnectionState.Closed) connection.Open();
                                    // Remplissage du DataTable
                                    reader = selectCommand.ExecuteReader();
                                    dt = new DataTable();
                                    dt.Load(reader);
                                    // Affichage du DataTable
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        for (int i = 0; i < 11; i++)
                                        {
                                            Console.Write(dr[i].ToString() + " ; ");

                                        }
                                        Console.Write("\n");

                                    }

                                    Console.WriteLine("\nEntrer le id du Chauffeur : ");
                                    this.Num_SS = int.Parse(Console.ReadLine());
                                    // Verifier si le Id Choisis existe dans la DataTable
                                    if (!dt.AsEnumerable().Any(row => Num_SS == row.Field<int>(0)))
                                    {
                                        Console.WriteLine("\n\nCe Chauffeur N'existe pas Veiller choisir un Id valide");
                                        b = false;
                                    }

                                } while (b == false);

                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Commande set Num_SS='@Num_SS' where Id_Commande=@Id_Commande", connection);
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                updateCommand.Parameters.AddWithValue("@Id_Commande", Id_Commande);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;

                            case 5:
                                updateCommand = new SqlCommand("update Commande set _Date='@_Date' where Id_Commande=@Id_Commande", connection);
                                Console.WriteLine("\nVeillez Entrer la date (annee-mois-jour) : ");
                                string _Date = Console.ReadLine();
                                updateCommand.Parameters.AddWithValue("@_Date", _Date);
                                updateCommand.Parameters.AddWithValue("@Id_Commande", Id_Commande);
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 6:

                                do
                                {
                                    b = true;
                                    // Requete de selection
                                    SqlCommand selectCommand = new SqlCommand("select * from Vehicule", connection);
                                    Console.Write("\n\n\n");
                                    // Ouverture de connexion si fermer
                                    if (connection.State == ConnectionState.Closed) connection.Open();
                                    // Remplissage du DataTable
                                    reader = selectCommand.ExecuteReader();
                                    dt = new DataTable();
                                    dt.Load(reader);
                                    // Affichage du DataTable
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        for (int i = 0; i < 6; i++)
                                        {
                                            Console.Write(dr[i].ToString() + " ; ");

                                        }
                                        Console.Write("\n");

                                    }

                                    Console.WriteLine("\nEntrer le matricule de vehicule : ");
                                    this.Id_Vehicule = int.Parse(Console.ReadLine());
                                    // Verifier si le Id Choisis existe dans la DataTable
                                    if (!dt.AsEnumerable().Any(row => Id_Vehicule == row.Field<int>(0)))
                                    {
                                        Console.WriteLine("\n\nCe vehicule  N'existe pas Veiller choisir un matricule valide");
                                        b = false;
                                    }

                                } while (b == false);
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Commande set Id_Vehicule='@Id_Vehicule' where Id_Commande=@Id_Commande", connection);
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@Id_Vehicule", Id_Vehicule);
                                updateCommand.Parameters.AddWithValue("@Id_Commande", Id_Commande);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();
                                break;

                            case 7:

                                do
                                {
                                    b = true;
                                    Console.WriteLine("Entrer le status de la Commande : \n1- Livré\n2 - Non Livré ");
                                    int val = int.Parse(Console.ReadLine());
                                    switch (val)
                                    {
                                        case 1:
                                            this._status = 1;
                                            break;
                                        case 2:
                                            this._status = 0;
                                            break;
                                        default:
                                            Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                                            Console.ReadKey();
                                            b = false;
                                            break;
                                    }
                                } while (b == false);


                                updateCommand = new SqlCommand("update Commande set _status='@_status' where Id_Commande=@Id_Commande", connection);
                                updateCommand.Parameters.AddWithValue("@_status", _status);
                                updateCommand.Parameters.AddWithValue("@Id_Commande", Id_Commande);
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 8:
                                updateCommand = new SqlCommand("update Commande set _Description='@_Description' where Id_Commande=@Id_Commande", connection);
                                Console.WriteLine("\nVeillez Entrer la Description : ");
                                string _Description = Console.ReadLine();
                                updateCommand.Parameters.AddWithValue("@_Description", _Description);
                                updateCommand.Parameters.AddWithValue("@Id_Commande", Id_Commande);
                                updateCommand.ExecuteNonQuery();

                                break;

                            default:
                                Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                                Console.ReadKey();
                                break;
                        }
                    } while (valeur != 1 && valeur != 2 && valeur != 3 && valeur != 4 && valeur != 5 && valeur != 6 && valeur != 7 && valeur != 8);

                    // Message de Succes
                    Console.WriteLine("\n\nModifier Avec Succes");

                }
                else
                {
                    Console.WriteLine("\n\nCe client n'existe pas");
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

        // Fonction Pour supprimer une Commande
        private void supprimerCommande(SqlConnection connection)
        {

            try
            {
                // Requete de suppression
                SqlCommand deleteCommand = new SqlCommand("delete from Commande where Id_Commande=@Id_Commande", connection);
                Console.Clear();
                Console.WriteLine("\t---------------------------- Supprimer Commande ----------------------------\n\n");

                Console.WriteLine("\nEntrer le Id de la Commande : ");
                int Id_Commande = int.Parse(Console.ReadLine());
                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();
                // Ajout des parametre de requete sql
                deleteCommand.Parameters.AddWithValue("@Id_Commande", Id_Commande);
                // Execution de requete
                deleteCommand.ExecuteNonQuery();
                // Message de Succes
                Console.WriteLine("\n\nSupprimer Avec Succes");

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

        // Fonction Pour afficher une Commande
        private void afficherCommande(SqlConnection connection)
        {
            try
            {

                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();

                Console.Clear();
                Console.WriteLine("\t---------------------------- Afficher Commande ----------------------------\n\n");


                Console.WriteLine("\nEntrer le Id de la Commande : ");
                int id_Commande = int.Parse(Console.ReadLine());
                // Requete de selection
                selectCommand = new SqlCommand("select * from Commande where Id_Commande=@id_Commande", connection);

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();
                // Ajout des parametre de requete sql
                selectCommand.Parameters.AddWithValue("@id_Commande", id_Commande);
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

        // Fonction Pour afficher toute les Commandes
        private void afficherTousCommande(SqlConnection connection)
        {
            try
            {
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();

                Console.Clear();
                Console.WriteLine("\t---------------------------- Afficher Commande ----------------------------\n\n");

                // Requete de selection
                selectCommand = new SqlCommand("select * from Commande", connection);

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

    }
}
