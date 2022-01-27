using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace Gestion_Logistique
{
    internal class Client
    {
        // Declaration
        private string nom;
        private string prenom;
        private string adressePostale;
        private string mail;
        private string telephone;

        // Fonction affichage du Menu de gestion des clients
        public void clientMenu(SqlConnection connection)
        {
            Console.Clear();

            int op; //options du menu

            Console.Clear();
            Console.WriteLine("\t\t\t\t\t    Gestion des client  \n");
            Console.WriteLine("\t\t\t|------------------------------------------------|\n");
            Console.WriteLine("\t\t\t| 1>> Ajouter un client                          |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 2>> Modifier un client                         |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 3>> Supprimer un client                        |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 4>> Afficher un client                         |\n");
            Console.WriteLine("\t\t\t|                                                |\n");
            Console.WriteLine("\t\t\t| 5>> Afficher tous les client                   |\n");
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
                        // appel fonction
                        ajouterClient(connection);
                        Console.ReadKey();
                        // appel fonction
                        clientMenu(connection);
                        break;
                    case 2:
                        modifierClient(connection);
                        Console.ReadKey();
                        clientMenu(connection);
                        break;
                    case 3:
                        supprimerClient(connection);
                        Console.ReadKey();
                        clientMenu(connection);
                        break;
                    case 4:
                        afficherClient(connection);
                        Console.ReadKey();
                        clientMenu(connection);
                        break;
                    case 5:
                        afficherTousClient(connection);
                        Console.ReadKey();
                        clientMenu(connection);
                        break;
                    case 6:
                        // Creation d'objet
                        fonctionGeneral f = new fonctionGeneral();
                        // appel fonction
                        f.TraitementMenu(connection);
                        break;
                    default:
                        Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                        Console.ReadKey();
                        break;
                }
            } while (op != 1 && op != 2 && op != 3 && op != 4 && op != 5 && op != 6);

        }

        // Fonction pour ajouter client
        private void ajouterClient(SqlConnection connection)
        {
            try
            {
                int op;
                // Requete d'insertion dans la base de donnees
                SqlCommand insertCommand = new SqlCommand("INSERT INTO Client (Nom,Prenom,AdressePostale,Mail,Telephone) VALUES(@Nom,@Prenom,@AdressePostale,@Mail,@Telephone)", connection);
                Console.Clear();
                Console.WriteLine("\t---------------------------- Ajout Client ----------------------------\n\n");


                do
                {
                    Console.WriteLine("Veillez entrer la maniere du traitement : \n1)\tConsole\n2)\tFichier");
                    op = int.Parse(Console.ReadLine());
                    switch (op)
                    {
                        // Console
                        case 1:
                            Console.WriteLine("\nEntrer le Nom : ");
                            this.nom = Console.ReadLine();
                            Console.WriteLine("\nEntrer le Prenom : ");
                            this.prenom = Console.ReadLine();
                            Console.WriteLine("\nEntrer l'adresse Postale : ");
                            this.adressePostale = Console.ReadLine();
                            Console.WriteLine("\nEntrer le Mail : ");
                            this.mail = Console.ReadLine();
                            Console.WriteLine("\nEntrer le Telephone : ");
                            this.telephone = Console.ReadLine();
                            // Ouverture de connexion si fermer
                            if (connection.State == ConnectionState.Closed) connection.Open();
                            // Ajout des parametre de requete sql
                            insertCommand.Parameters.AddWithValue("@Nom", nom);
                            insertCommand.Parameters.AddWithValue("@Prenom", prenom);
                            insertCommand.Parameters.AddWithValue("@AdressePostale", adressePostale);
                            insertCommand.Parameters.AddWithValue("@Mail", mail);
                            insertCommand.Parameters.AddWithValue("@Telephone", telephone);
                            // Execution de requete
                            insertCommand.ExecuteNonQuery();
                            // Message de Succes
                            Console.WriteLine("\n\nAjouter Avec Succes");


                            break;

                        //fichier
                        case 2:
                            // le fichier doit etre en cette format -> nom;prenom;adressePostale;Mail;Telephone
                            OpenFileDialog dialog = new OpenFileDialog();
                            if (DialogResult.OK == dialog.ShowDialog())
                            {
                                string path = dialog.FileName;

                                int counter = 0;
                                List<string> listClient = new List<string>();

                                // Lecture line par line et stocker dans List
                                foreach (string line in System.IO.File.ReadLines(path))
                                {
                                    listClient.Add(line);
                                    counter++;
                                }

                                // insertion a partir du List
                                if (counter > 0)
                                {

                                    if (connection.State == ConnectionState.Closed) connection.Open();

                                    foreach (string line in listClient)
                                    {
                                        insertCommand.Parameters.Clear();
                                        insertCommand.Parameters.AddWithValue("@Nom", line.Split(';')[0]);
                                        insertCommand.Parameters.AddWithValue("@Prenom", line.Split(';')[1]);
                                        insertCommand.Parameters.AddWithValue("@AdressePostale", line.Split(';')[2]);
                                        insertCommand.Parameters.AddWithValue("@Mail", line.Split(';')[3]);
                                        insertCommand.Parameters.AddWithValue("@Telephone", line.Split(';')[4]);

                                        insertCommand.ExecuteNonQuery();
                                    }
                                    // Message de Succes
                                    Console.WriteLine("\n\nAjouter Avec Succes");

                                }
                                else
                                {
                                    Console.WriteLine("\n\nVotre fichier est vide");
                                }
                            }

                            break;
                        default:
                            Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                            Console.ReadKey();
                            break;
                    }
                } while (op != 1 && op != 2);



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

        // Fonction Pour modifier client
        private void modifierClient(SqlConnection connection)
        {
            try
            {
                int op;
                SqlCommand updateCommand;
                SqlCommand ExistedValue;

                Console.Clear();
                Console.WriteLine("\t---------------------------- Modifier Client ----------------------------\n\n");
                Console.WriteLine("Veillez entrer la maniere du traitement : \n1)\tConsole\n2)\tFichier");
                op = int.Parse(Console.ReadLine());

                do
                {
                    switch (op)
                    {
                        case 1:
                            // Lecture id Client a modifier 
                            Console.WriteLine("\nEntrer le Id du client : ");
                            int Id_Client = int.Parse(Console.ReadLine());

                            ExistedValue = new SqlCommand("select count(Id_Client) from Client where Id_Client=@Id_Client", connection);
                            ExistedValue.Parameters.AddWithValue("@Id_Client", Id_Client);
                            // Ouverture de connexion si fermer
                            if (connection.State == ConnectionState.Closed) connection.Open();

                            if (Convert.ToInt32(ExistedValue.ExecuteScalar()) > 0)
                            {
                                Console.WriteLine("\nEntrer le champ a modifier :\n1-Nom\n2-Prenom\n3-Adresse Postale\n4-Mail\n5-Telephone");

                                int valeur = int.Parse(Console.ReadLine());
                                do
                                {
                                    switch (valeur)
                                    {
                                        case 1:
                                            // Requete de modification dans la base de donnes
                                            updateCommand = new SqlCommand("update Client set Nom='@Nom' where Id_Client=@Id_Client", connection);
                                            Console.WriteLine("\nVeillez Entrer le nom : ");
                                            string nom = Console.ReadLine();
                                            // Ajout des parametre de requete sql
                                            updateCommand.Parameters.AddWithValue("@Nom", nom);
                                            updateCommand.Parameters.AddWithValue("@Id_Client", Id_Client);
                                            // Execution de requete
                                            updateCommand.ExecuteNonQuery();

                                            break;
                                        case 2:
                                            updateCommand = new SqlCommand("update Client set Prenom='@Prenom' where Id_Client=@Id_Client", connection);
                                            Console.WriteLine("\nVeillez Entrer le prenom : ");
                                            string prenom = Console.ReadLine();
                                            updateCommand.Parameters.AddWithValue("@Prenom", prenom);
                                            updateCommand.Parameters.AddWithValue("@Id_Client", Id_Client);
                                            updateCommand.ExecuteNonQuery();


                                            break;
                                        case 3:
                                            updateCommand = new SqlCommand("update Client set AdressePostale='@AdressePostale' where Id_Client=@Id_Client", connection);
                                            Console.WriteLine("\nVeillez Entrer l'adresse Postale : ");
                                            string adressePostale = Console.ReadLine();
                                            updateCommand.Parameters.AddWithValue("@AdressePostale", adressePostale);
                                            updateCommand.Parameters.AddWithValue("@Id_Client", Id_Client);
                                            updateCommand.ExecuteNonQuery();

                                            break;
                                        case 4:
                                            updateCommand = new SqlCommand("update Client set Mail='@Mail' where Id_Client=@Id_Client", connection);
                                            Console.WriteLine("\nVeillez Entrer le mail : ");
                                            string mail = Console.ReadLine();
                                            updateCommand.Parameters.AddWithValue("@Mail", mail);
                                            updateCommand.Parameters.AddWithValue("@Id_Client", Id_Client);
                                            updateCommand.ExecuteNonQuery();

                                            break;
                                        case 5:
                                            updateCommand = new SqlCommand("update Client set Telephone='@Telephone' where Id_Client=@Id_Client", connection);
                                            Console.WriteLine("\nVeillez Entrer le telephone : ");
                                            string telephone = Console.ReadLine();
                                            updateCommand.Parameters.AddWithValue("@Telephone", telephone);
                                            updateCommand.Parameters.AddWithValue("@Id_Client", Id_Client);
                                            updateCommand.ExecuteNonQuery();

                                            break;
                                    }
                                } while (valeur != 1 && valeur != 2 && valeur != 3 && valeur != 4 && valeur != 5);




                            }
                            else
                            {
                                Console.WriteLine("\n\nCe client n'existe pas");
                            }

                            break;


                        case 2:
                            // le fichier doit etre en cette format -> id;nom;prenom;adressePostale;Mail;Telephone
                            OpenFileDialog dialog = new OpenFileDialog();
                            List<string> ClientInexistant = new List<string>();
                            if (DialogResult.OK == dialog.ShowDialog())
                            {
                                string path = dialog.FileName;

                                int counter = 0;
                                List<string> listClient = new List<string>();

                                // Lecture line par line et stocker dans List
                                foreach (string line in System.IO.File.ReadLines(path))
                                {
                                    listClient.Add(line);
                                    counter++;
                                }

                                if (counter > 0)
                                {
                                    // Ouverture de connexion si fermer
                                    if (connection.State == ConnectionState.Closed) connection.Open();

                                    // insertion a partir du List
                                    foreach (string line in listClient)
                                    {
                                        ExistedValue = new SqlCommand("select count(Id_Client) from Client where Id_Client=@Id_Client", connection);
                                        ExistedValue.Parameters.AddWithValue("@Id_Client", line.Split(';')[0]);

                                        if (Convert.ToInt32(ExistedValue.ExecuteScalar()) > 0)
                                        {
                                            updateCommand = new SqlCommand("update Client set Nom='@Nom',Prenom='@Prenom',AdressePostale='@AdressePostale',Mail='@Mail'Telephone='@Telephone' where Id_Client=@Id_Client", connection);
                                            updateCommand.Parameters.Clear();
                                            updateCommand.Parameters.AddWithValue("@Id_Client", line.Split(';')[0]);
                                            updateCommand.Parameters.AddWithValue("@Nom", line.Split(';')[1]);
                                            updateCommand.Parameters.AddWithValue("@Prenom", line.Split(';')[2]);
                                            updateCommand.Parameters.AddWithValue("@AdressePostale", line.Split(';')[3]);
                                            updateCommand.Parameters.AddWithValue("@Mail", line.Split(';')[4]);
                                            updateCommand.Parameters.AddWithValue("@Telephone", line.Split(';')[5]);

                                            updateCommand.ExecuteNonQuery();


                                        }
                                        else
                                        {
                                            ClientInexistant.Add(line.Split(';')[0]);
                                        }



                                    }

                                    // Afficher les client qui n'existent pas
                                    foreach (string c in ClientInexistant)
                                    {
                                        Console.WriteLine("Ce client n'existe pas : " + c);

                                    }
                                    // Message de Succes
                                    Console.WriteLine("\n\nModifier Avec Succes");

                                }
                                else
                                {
                                    Console.WriteLine("\n\nVotre fichier est vide");
                                }



                            }

                            break;
                        default:
                            Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                            Console.ReadKey();
                            break;
                    }
                } while (op != 1 && op != 2);






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

        // Fonction Pour supprimer client
        private void supprimerClient(SqlConnection connection)
        {

            try
            {
                int op;
                // Requete de suppression
                SqlCommand deleteCommand = new SqlCommand("delete from Client where Id_Client=@Id_Client", connection);
                Console.Clear();
                Console.WriteLine("\t---------------------------- Supprimer Client ----------------------------\n\n");


                do
                {
                    Console.WriteLine("Veillez entrer la maniere du traitement : \n1)\tConsole\n2)\tFichier");
                    op = int.Parse(Console.ReadLine());
                    switch (op)
                    {
                        case 1:
                            Console.WriteLine("\nEntrer le Id du Client : ");
                            int idClient = int.Parse(Console.ReadLine());

                            if (connection.State == ConnectionState.Closed) connection.Open();

                            deleteCommand.Parameters.AddWithValue("@Id_Client", idClient);
                            deleteCommand.ExecuteNonQuery();
                            Console.WriteLine("\n\nSupprimer Avec Succes");


                            break;
                        case 2:
                            // le fichier doit etre en cette format -> id
                            OpenFileDialog dialog = new OpenFileDialog();
                            if (DialogResult.OK == dialog.ShowDialog())
                            {
                                string path = dialog.FileName;

                                int counter = 0;
                                List<string> listClient = new List<string>();

                                // Lecture line par line et stocker dans List
                                foreach (string line in System.IO.File.ReadLines(path))
                                {
                                    listClient.Add(line);
                                    counter++;
                                }

                                if (counter > 0)
                                {

                                    if (connection.State == ConnectionState.Closed) connection.Open();
                                    // suppression a partir du List
                                    foreach (string line in listClient)
                                    {
                                        deleteCommand.Parameters.Clear();

                                        deleteCommand.Parameters.AddWithValue("@Nom", line.Trim());

                                        deleteCommand.ExecuteNonQuery();
                                    }
                                    Console.WriteLine("\n\nSupprimer Avec Succes");

                                }
                                else
                                {
                                    Console.WriteLine("\n\nVotre fichier est vide");
                                }



                            }

                            break;
                        default:
                            Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                            Console.ReadKey();
                            break;
                    }
                } while (op != 1 && op != 2);



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

        // Fonction Pour afficher client
        private void afficherClient(SqlConnection connection)
        {
            try
            {
                int op;
                SqlCommand selectCommand;
                SqlDataReader reader;
                DataTable dt = new DataTable();

                Console.Clear();
                Console.WriteLine("\t---------------------------- Afficher Client ----------------------------\n\n");


                do
                {
                    Console.WriteLine("Veillez entrer la maniere du traitement : \n1)\tConsole\n2)\tFichier");
                    op = int.Parse(Console.ReadLine());
                    switch (op)
                    {
                        case 1:
                            Console.WriteLine("\nEntrer le Id du Client : ");
                            int idClient = int.Parse(Console.ReadLine());
                            // Requete de selection
                            selectCommand = new SqlCommand("select * from Client where Id_Client=@Id_Client", connection);
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
                                for (int i = 0; i < 6; i++)
                                {
                                    Console.Write(dr[i].ToString() + " ; ");

                                }

                                Console.Write("\n");
                            }



                            break;
                        case 2:
                            // le fichier doit etre en cette format -> id
                            OpenFileDialog dialog = new OpenFileDialog();
                            if (DialogResult.OK == dialog.ShowDialog())
                            {
                                string path = dialog.FileName;

                                int counter = 0;
                                List<string> listClient = new List<string>();

                                // Lecture line par line et stocker dans List
                                foreach (string line in System.IO.File.ReadLines(path))
                                {
                                    listClient.Add(line);
                                    counter++;
                                }

                                if (counter > 0)
                                {

                                    if (connection.State == ConnectionState.Closed) connection.Open();
                                    // affichage a partir du List
                                    foreach (string line in listClient)
                                    {
                                        selectCommand = new SqlCommand("select * from Client where Id_Client=@Id_Client", connection);
                                        selectCommand.Parameters.Clear();

                                        selectCommand.Parameters.AddWithValue("@Id_Client", line.Trim());
                                        reader = selectCommand.ExecuteReader();

                                        dt.Load(reader);
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            for (int i = 0; i < 6; i++)
                                            {
                                                Console.Write(dr[i].ToString() + " ; ");

                                            }

                                            Console.Write("\n");
                                        }


                                    }
                                    // Message de Succes
                                    Console.WriteLine("\n\nSupprimer Avec Succes");

                                }
                                else
                                {
                                    Console.WriteLine("\n\nVotre fichier est vide");
                                }



                            }

                            break;
                        default:
                            Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                            Console.ReadKey();
                            break;
                    }
                } while (op != 1 && op != 2);



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

        // Fonction Pour afficher tous les clients
        public void afficherTousClient(SqlConnection connection)
        {

            try
            {
                int op;
                SqlCommand selectCommand;
                SqlDataReader reader;

                Console.Clear();
                Console.WriteLine("\t---------------------------- Afficher Client ----------------------------\n\n");

                do
                {
                    DataTable dt = new DataTable();

                    Console.WriteLine("Veillez entrer le trie a effectuer : \n1)\tAlphabetique\n2)\tVille\n3)\tMontant");
                    op = int.Parse(Console.ReadLine());
                    switch (op)
                    {
                        case 1:
                            // Requete de selection
                            selectCommand = new SqlCommand("select * from Client order by Nom", connection);
                            // Ouverture de connexion si fermer
                            if (connection.State == ConnectionState.Closed) connection.Open();
                            reader = selectCommand.ExecuteReader();
                            // Remplissage du DataTable
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

                            break;
                        case 2:

                            selectCommand = new SqlCommand("select * from Client order by AdressePostale", connection);


                            if (connection.State == ConnectionState.Closed) connection.Open();
                            reader = selectCommand.ExecuteReader();
                            dt.Load(reader);
                            foreach (DataRow dr in dt.Rows)
                            {
                                for (int i = 0; i < 6; i++)
                                {
                                    Console.Write(dr[i].ToString() + " ; ");

                                }

                                Console.Write("\n");
                            }

                            break;

                        case 3:

                            selectCommand = new SqlCommand("select cl.Nom,cl.Prenom,cl.AdressePostale,cl.Mail,cl.Telephone,count(com.Id_Commande) as 'Montant' from Client cl,Commande com where cl.Id_Client=com.Id_client group by Nom,Prenom,AdressePostale,Mail,Telephone order by Montant", connection);

                            if (connection.State == ConnectionState.Closed) connection.Open();
                            reader = selectCommand.ExecuteReader();
                            dt.Load(reader);
                            foreach (DataRow dr in dt.Rows)
                            {
                                for (int i = 0; i < 6; i++)
                                {
                                    Console.Write(dr[i].ToString() + " ; ");

                                }

                                Console.Write("\n");
                            }

                            break;
                        default:
                            Console.WriteLine("\n\nVeillez Entrer un chiffre de la liste");
                            Console.ReadKey();
                            break;
                    }
                } while (op != 1 && op != 2 && op != 3);



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
