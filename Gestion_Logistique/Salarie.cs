using System;
using System.Data.SqlClient;
using System.Data;

namespace Gestion_Logistique
{

    internal class Salarie
    {
        // Declaration des variable
        private string Nom;
        private string Prenom;
        private string DateNaissance;
        private string AdressePostale;
        private string Mail;
        private string Telephone;
        private string DateEntree;
        private string Poste;
        private int Salaire;
        private int Id_Chef;

        // Fonction affichage du Menu de gestion des salarie
        public void salarieMenu(SqlConnection connection)
        {
            Console.Clear();

            int op; //options du menu

            Console.Clear();
            Console.WriteLine("\t\t\t\t\t    Gestion des salarie  \n");
            Console.WriteLine("\t\t\t|------------------------------------------------|\n");
            Console.WriteLine("\t\t\t| 1>> Embaucher un salarie                       |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 2>> Modifier un salarie                        |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 3>> Licencier un salarie                       |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 4>> Afficher un salarie                        |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 5>> Afficher tous les salarie                  |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 6>> Afficher l'organigramme                    |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 7>> Revenir au menu principal                  |\n");
            Console.WriteLine("\t\t\t|------------------------------------------------|\n");

            Console.WriteLine("\n Entrer votre choix: ");
            op = int.Parse(Console.ReadLine());
            do
            {
                switch (op)
                {
                    case 1:
                        // Appel de la fonction
                        embaucherSalarie(connection);
                        Console.ReadKey();
                        // Appel de la fonction 
                        salarieMenu(connection);
                        break;
                    case 2:
                        // Appel de la fonction
                        modifierSalarie(connection);
                        Console.ReadKey();
                        // Appel de la fonction
                        salarieMenu(connection);
                        break;
                    case 3:
                        // Appel de la fonction
                        licencierSalarie(connection);
                        Console.ReadKey();
                        // Appel de la fonction
                        salarieMenu(connection);
                        break;
                    case 4:
                        // Appel de la fonction
                        afficherSalarie(connection);
                        Console.ReadKey();
                        // Appel de la fonction
                        salarieMenu(connection);
                        break;
                    case 5:
                        // Appel de la fonction
                        afficherTousSalarie(connection);
                        Console.ReadKey();
                        // Appel de la fonction
                        salarieMenu(connection);
                        break;
                    case 6:
                        // Appel de la fonction
                        Organigramme(connection);
                        Console.ReadKey();
                        // Appel de la fonction
                        salarieMenu(connection);
                        break;
                    case 7:
                        // Creation d'object de la classe fonctionGeneral
                        fonctionGeneral f = new fonctionGeneral();
                        // Appel de la fonction
                        f.TraitementMenu(connection);
                        break;
                    default:
                        Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                        Console.ReadKey();
                        break;
                }

            } while (op != 1 && op != 2 && op != 3 && op != 4 && op != 5 && op != 6 && op != 7);
        }

        // Fonction Pour ajouter un salarie (Embaucher)
        private void embaucherSalarie(SqlConnection connection)
        {
            try
            {
                Console.Clear();
                // Requete d'insertion dans la base de donnees
                SqlCommand insertCommand = new SqlCommand("INSERT INTO Salarie (Nom,Prenom,DateNaissance,AdressePostale,Mail,Telephone,DateEntree,Poste,Salaire,Id_Chef) VALUES(@Nom,@Prenom,@DateNaissance,@AdressePostale,@Mail,@Telephone,@DateEntree,@Poste,@Salaire,@Id_Chef)", connection);

                Console.WriteLine("\t---------------------------- Ajout Salarie ----------------------------\n\n");
                // Lecture de Donnees a inserer
                Console.WriteLine("\nEntrer le Nom : ");
                this.Nom = Console.ReadLine();
                Console.WriteLine("\nEntrer le Prenom : ");
                this.Prenom = Console.ReadLine();
                Console.WriteLine("\nEntrer la Date de Naissance : (annee-mois-jour) ");
                this.DateNaissance = Console.ReadLine();
                Console.WriteLine("\nEntrer l'adresse Postale : ");
                this.AdressePostale = Console.ReadLine();
                Console.WriteLine("\nEntrer le Mail : ");
                this.Mail = Console.ReadLine();
                Console.WriteLine("\nEntrer le Telephone : ");
                this.Telephone = Console.ReadLine();
                Console.WriteLine("\nEntrer la Date d' Entree : (annee-mois-jour) ");
                this.DateEntree = Console.ReadLine();
                Console.WriteLine("\nEntrer l'intitule du poste : ");
                this.Poste = Console.ReadLine();
                Console.WriteLine("\nEntrer le Salaire : ");
                this.Salaire = int.Parse(Console.ReadLine());
                Console.WriteLine("\nEntrer le Id du Superviseur : ");
                this.Id_Chef = int.Parse(Console.ReadLine());

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();

                // Ajout des parametre de requete sql
                insertCommand.Parameters.AddWithValue("@Nom", Nom);
                insertCommand.Parameters.AddWithValue("@Prenom", Prenom);
                insertCommand.Parameters.AddWithValue("@DateNaissance", DateNaissance);
                insertCommand.Parameters.AddWithValue("@AdressePostale", AdressePostale);
                insertCommand.Parameters.AddWithValue("@Mail", Mail);
                insertCommand.Parameters.AddWithValue("@Telephone", Telephone);
                insertCommand.Parameters.AddWithValue("@DateEntree", DateEntree);
                insertCommand.Parameters.AddWithValue("@Poste", Poste);
                insertCommand.Parameters.AddWithValue("@Salaire", Salaire);
                insertCommand.Parameters.AddWithValue("@Id_Chef", Id_Chef);

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

        // Fonction Pour modifier un salarie 
        private void modifierSalarie(SqlConnection connection)
        {

            try
            {
                SqlCommand updateCommand;
                SqlCommand ExistedValue;

                Console.Clear();
                Console.WriteLine("\t---------------------------- Modifier Salarie ----------------------------\n\n");

                // Lecture de Id du salarie
                Console.WriteLine("\nEntrer le Id du Salarie : ");
                int Num_SS = int.Parse(Console.ReadLine());

                // Requete sql pour retourner le nombre de salarie ayant le Id
                ExistedValue = new SqlCommand("select count(Num_SS) from Client where Num_SS=@Num_SS", connection);
                // Ajout des parametre de requete sql
                ExistedValue.Parameters.AddWithValue("@Num_SS", Num_SS);

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();

                // Verification d'existence de ID
                if (Convert.ToInt32(ExistedValue.ExecuteScalar()) > 0)
                {
                    // Choisir le champ a modifier
                    Console.WriteLine("\nEntrer le champ a modifier :\n1-Nom\n2-Prenom\n3-DateNaissance\n4-AdressePostale\n5-Mail\n6-Telephone\n7-DateEntree\n8-Poste\n9-Salaire\n10-Superviseur");
                    int valeur = int.Parse(Console.ReadLine());

                    do
                    {
                        switch (valeur)
                        {
                            case 1:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Salarie set Nom='@Nom' where Num_SS=@Num_SS", connection);
                                Console.WriteLine("\nVeillez Entrer le nom : ");
                                string nom = Console.ReadLine();
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@Nom", nom);
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 2:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Salarie set Prenom='@Prenom' where Num_SS=@Num_SS", connection);
                                Console.WriteLine("\nVeillez Entrer le prenom : ");
                                string prenom = Console.ReadLine();
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@Prenom", prenom);
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 3:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Salarie set DateNaissance='@dateNaissance' where Num_SS=@Num_SS", connection);
                                Console.WriteLine("\nVeillez Entrer la Date de Naissance : ");
                                string dateNaissance = Console.ReadLine();
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@DateNaissance", dateNaissance);
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 4:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Salarie set AdressePostale='@AdressePostale' where Num_SS=@Num_SS", connection);
                                Console.WriteLine("\nVeillez Entrer l'adresse Postale : ");
                                string adressePostale = Console.ReadLine();
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@AdressePostale", adressePostale);
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 5:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Salarie set Mail='@Mail' where Num_SS=@Num_SS", connection);
                                Console.WriteLine("\nVeillez Entrer le mail : ");
                                string mail = Console.ReadLine();
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@Mail", mail);
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 6:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Salarie set Telephone='@Telephone' where Num_SS=@Num_SS", connection);
                                Console.WriteLine("\nVeillez Entrer le telephone : ");
                                string telephone = Console.ReadLine();
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@Telephone", telephone);
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 7:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Salarie set DateEntree='@DateEntree' where Num_SS=@Num_SS", connection);
                                Console.WriteLine("\nVeillez Entrer la Date d'entree : ");
                                string dateEntree = Console.ReadLine();
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@DateEntree", dateEntree);
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 8:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Salarie set Poste='@Poste' where Num_SS=@Num_SS", connection);
                                Console.WriteLine("\nVeillez Entrer l'intitule du poste : ");
                                string poste = Console.ReadLine();
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@Poste", poste);
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 9:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Salarie set Salaire='@Salaire' where Num_SS=@Num_SS", connection);
                                Console.WriteLine("\nVeillez Entrer le salaire : ");
                                int salaire = int.Parse(Console.ReadLine());
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@Salaire", salaire);
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;
                            case 10:
                                // Requete de modification dans la base de donnes
                                updateCommand = new SqlCommand("update Salarie set Id_Chef='@Id_Chef' where Num_SS=@Num_SS", connection);
                                Console.WriteLine("\nVeillez Entrer la Date d'entree : ");
                                string id_Chef = Console.ReadLine();
                                // Ajout des parametre de requete sql
                                updateCommand.Parameters.AddWithValue("@Id_Chef", id_Chef);
                                updateCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                                // Execution de requete
                                updateCommand.ExecuteNonQuery();

                                break;
                            default:
                                Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                                Console.ReadKey();
                                break;
                        }
                    } while (valeur != 1 && valeur != 2 && valeur != 3 && valeur != 4 && valeur != 5 && valeur != 6 && valeur != 7 && valeur != 8 && valeur != 9 && valeur != 10);

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

        // Fonction Pour supprimer un salarie (Licencier)
        private void licencierSalarie(SqlConnection connection)
        {
            try
            {
                // Requete de suppression de la base de donnees
                SqlCommand deleteCommand = new SqlCommand("delete from Salarie where Num_SS=@Num_SS", connection);

                Console.Clear();
                Console.WriteLine("\t---------------------------- Licencier Salarie ----------------------------\n\n");

                // Lecture de Id a supprimer
                Console.WriteLine("\nEntrer le Id du Salarie : ");
                int Num_SS = int.Parse(Console.ReadLine());

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();

                // Ajout des parametre de requete sql
                deleteCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                // Execution de requete
                deleteCommand.ExecuteNonQuery();

                // Message de Succes
                Console.WriteLine("\n\nLicencier Avec Succes");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\nErreur : " + ex);
            }
            finally
            {
                connection.Close();
            }
        }

        // Fonction Pour afficher un salarie par son id
        private void afficherSalarie(SqlConnection connection)
        {
            try
            {
                // Declaration
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();

                Console.Clear();
                Console.WriteLine("\t---------------------------- Afficher Salarie ----------------------------\n\n");

                // Lecture du Id de salarie
                Console.WriteLine("\nEntrer le Id du Salarie : ");
                int Num_SS = int.Parse(Console.ReadLine());

                // Requete de selection 
                selectCommand = new SqlCommand("select * from Salarie where Num_SS=@Num_SS", connection);

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();

                // Ajout des parametre de requete sql
                selectCommand.Parameters.AddWithValue("@Num_SS", Num_SS);
                // Execution de requete
                reader = selectCommand.ExecuteReader();
                // Remplissage de DataTable
                dt.Load(reader);

                // Affichage de DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < 11; i++)
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

        // Fonction Pour afficher Tous les Salarie
        private void afficherTousSalarie(SqlConnection connection)
        {
            try
            {
                // Declaration
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();

                Console.Clear();
                Console.WriteLine("\t---------------------------- Afficher Salarie ----------------------------\n\n");

                // Requete de selection
                selectCommand = new SqlCommand("select * from Salarie", connection);

                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();

                // Remplissage du DataTable
                reader = selectCommand.ExecuteReader();
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

        // Fonction Pour afficher Tous les lien d'organigramme de la societe
        private void Organigramme(SqlConnection connection)
        {
            try
            {
                // Declaration
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();
                string str = "";

                Console.Clear();
                Console.WriteLine("\t---------------------------- Organigramme ----------------------------\n\n");

                // Requete de selection
                selectCommand = new SqlCommand("select * from Salarie", connection);
                // Ouverture de connexion si fermer
                if (connection.State == ConnectionState.Closed) connection.Open();
                // Remplissage du DataTable
                reader = selectCommand.ExecuteReader();
                dt.Load(reader);
                // Affichage du DataTable
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Console.Write(dt.Rows[i][1]);

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        if (dt.Rows[i][0].ToString() == dt.Rows[j][10].ToString())
                        {
                            Console.Write("\n\t -> " + str + dt.Rows[j][1]);
                        }

                    }
                    Console.WriteLine("\n\n-------------------------\n");
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
