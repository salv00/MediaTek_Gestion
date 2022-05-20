using Mediatek86.metier;
using System.Collections.Generic;
using Mediatek86.bdd;
using System;
using Serilog;

namespace Mediatek86.modele
{
    /// <summary>
    /// Classe qui permet de gérer toute demandes qui concerne les données
    /// </summary>
    public static class Dao
    {
        private static int nb = 0;
        // connexion à la BDD
        private static readonly string server = "localhost";
        private static readonly string userid = "root";
        private static readonly string password = "";
        private static readonly string database = "mediatek86";
        private static readonly string connectionString = "server="+server+";user id="+userid+";password="+password+";database="+database+";SslMode=none";

        /// <summary>
        /// Contrôle si l'user à le droit de se connecter (login password)
        /// </summary>
        /// <param name="identifiant"></param>
        /// <param name="mdp"></param>
        /// <returns></returns>
        public static Service ControleAuthentification(string identifiant, string mdp)
        {
            string req = "SELECT identifiant, service, s.nom FROM user u ";
            req += "LEFT JOIN service s on s.id = u.service ";
            req += "WHERE u.identifiant = @identifiant AND u.mdp = SHA2(@mdp, 256)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@identifiant", identifiant);
            parameters.Add("@mdp", mdp);
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);
            if (curs.Read())
            {
                Service service = new Service((string)curs.Field("identifiant"), (int)curs.Field("service"), (string)curs.Field("nom"));
                Log.Information("L'utilisateur {0} s'est connecté, il fait appartient au service {1}", service.Utilisateur, service.Nom);
                curs.Close();
                return service;
            }
            else
            {
                Log.Information("Echec de la connexion");
                curs.Close();
                return null;
            }
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public static List<Categorie> GetAllGenres()
        {
            List<Categorie> lesGenres = new List<Categorie>();
            string req = "Select * from genre order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Genre genre = new Genre((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesGenres.Add(genre);
            }
            curs.Close();
            return lesGenres;
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public static List<Categorie> GetAllRayons()
        {
            List<Categorie> lesRayons = new List<Categorie>();
            string req = "Select * from rayon order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Rayon rayon = new Rayon((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesRayons.Add(rayon);
            }
            curs.Close();
            return lesRayons;
        }

        /// <summary>
        /// Retourne toutes les types de public à partir de la BDD
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public static List<Categorie> GetAllPublics()
        {
            List<Categorie> lesPublics = new List<Categorie>();
            string req = "Select * from public order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Public lePublic = new Public((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesPublics.Add(lePublic);
            }
            curs.Close();
            return lesPublics;
        }

        /// <summary>
        /// Retourne tout les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public static List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = new List<Livre>();
            string req = "Select l.id, l.ISBN, l.auteur, d.titre, d.image, l.collection, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from livre l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                string isbn = (string)curs.Field("ISBN");
                string auteur = (string)curs.Field("auteur");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                string collection = (string)curs.Field("collection");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idgenre, genre, 
                    idpublic, lepublic, idrayon, rayon);
                lesLivres.Add(livre);
            }
            curs.Close();

            return lesLivres;
        }

        /// <summary>
        /// Retourne tout les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public static List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = new List<Dvd>();
            string req = "Select l.id, l.duree, l.realisateur, d.titre, d.image, l.synopsis, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from dvd l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                int duree = (int)curs.Field("duree");
                string realisateur = (string)curs.Field("realisateur");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                string synopsis = (string)curs.Field("synopsis");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon);
                lesDvd.Add(dvd);
            }
            curs.Close();

            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public static List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = new List<Revue>();
            string req = "Select l.id, l.empruntable, l.periodicite, d.titre, d.image, l.delaiMiseADispo, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from revue l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                bool empruntable = (bool)curs.Field("empruntable");
                string periodicite = (string)curs.Field("periodicite");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                int delaiMiseADispo = (int)curs.Field("delaimiseadispo");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Revue revue = new Revue(id, titre, image, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon, empruntable, periodicite, delaiMiseADispo);
                lesRevues.Add(revue);
            }
            curs.Close();

            return lesRevues;
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <returns>Liste d'objets Exemplaire</returns>
        public static List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            List<Exemplaire> lesExemplaires = new List<Exemplaire>();
            string req = "Select e.id, e.numero, e.dateAchat, e.photo, e.idEtat ";
            req += "from exemplaire e join document d on e.id=d.id ";
            req += "where e.id = @id ";
            req += "order by e.dateAchat DESC";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);

            while (curs.Read())
            {
                string idDocuement = (string)curs.Field("id");
                int numero = (int)curs.Field("numero");
                DateTime dateAchat = (DateTime)curs.Field("dateAchat");
                string photo = (string)curs.Field("photo");
                string idEtat = (string)curs.Field("idEtat");
                Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocuement);
                lesExemplaires.Add(exemplaire);
            }
            curs.Close();

            return lesExemplaires;
        }

        /// <summary>
        /// Retourne tout les suivis
        /// </summary>
        /// <returns></returns>
        public static List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = null;
            try
            {
                lesSuivis = new List<Suivi>();
                string req = "SELECT * FROM `suivi` ORDER BY libelle;";
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, null);

                while (curs.Read())
                {
                    Suivi suivi = new Suivi(
                        (string)curs.Field("id"),
                        (string)curs.Field("libelle")
                        );
                    lesSuivis.Add(suivi);
                }
                curs.Close();
                return lesSuivis;
            }
            catch (Exception e)
            {
                Log.Information("Echec de la récupération des suivis de la BDD : {0}", e);
                return lesSuivis;
            }
        }

        /// <summary>
        /// Ecriture d'un exemplaire dans la BDD
        /// </summary>
        /// <param name="exemplaire"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public static bool CreerExemplaire(Exemplaire exemplaire)
        {
            try
            {
                string req = "insert into exemplaire values (@idDocument,@numero,@dateAchat,@photo,@idEtat)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@idDocument", exemplaire.IdDocument},
                    { "@numero", exemplaire.Numero},
                    { "@dateAchat", exemplaire.DateAchat},
                    { "@photo", exemplaire.Photo},
                    { "@idEtat",exemplaire.IdEtat}
                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }catch (Exception e)
            {
                Log.Information("Echec de l'insertion d'un exemplaire dans la bdd : {0}", e);
                return false;
            }
        }

        /// <summary>
        /// Retourne toutes les commandes de livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Commandes</returns>
        public static List<CommandeDocumentLivre> GetAllCommandesLivres()
        {
            List<CommandeDocumentLivre> lesCommandesLivres = null;
            try
            {
                lesCommandesLivres = new List<CommandeDocumentLivre>();
                string req = "SELECT c.id as id_commande, c.dateCommande as dateCommande, c.montant as montant, cd.nbExemplaire as nbExemplaire, cd.idLivreDvd as idLivre, s.id as id_etat, s.libelle as libelle, l.isbn as isbn, l.auteur as auteur, l.collection as collection, d.titre as titre, g.libelle as genre, p.libelle as public, r.libelle as rayon, d.image as image ";
                req += "FROM `commande`c LEFT JOIN `commandedocument` cd USING(id) ";
                req += "LEFT JOIN `suivi` s ON s.id = cd.idSuivi ";
                req += "LEFT JOIN `livre` l ON l.id = cd.idLivreDvd ";
                req += "LEFT JOIN `document` d ON d.id = cd.idLivreDvd ";
                req += "JOIN rayon r on r.id=d.idRayon ";
                req += "JOIN genre g on g.id=d.idGenre ";
                req += "JOIN public p on p.id=d.idPublic ";
                req += "WHERE cd.idLivreDvd = l.id ORDER BY c.dateCommande DESC";                
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, null);

                while (curs.Read())
                {
                    CommandeDocumentLivre commandeDocument = new CommandeDocumentLivre(
                        (string)curs.Field("id_commande"),
                        (DateTime)curs.Field("dateCommande"),
                        (double)curs.Field("montant"),
                        (int)curs.Field("nbExemplaire"),
                        (string)curs.Field("idLivre"),
                        (string)curs.Field("id_etat"),
                        (string)curs.Field("libelle"),
                        (string)curs.Field("isbn"),
                        (string)curs.Field("auteur"),
                        (string)curs.Field("collection"),
                        (string)curs.Field("titre"),
                        (string)curs.Field("genre"),
                        (string)curs.Field("public"),
                        (string)curs.Field("rayon"),
                        (string)curs.Field("image")
                        );
                    lesCommandesLivres.Add(commandeDocument);
                }
                curs.Close();
                return lesCommandesLivres;
            }
            catch (Exception e)
            {
                Log.Information("Echec de la récupération des commandes de livres dans la bdd : {0}", e);
                return lesCommandesLivres;
            }
        }

        /// <summary>
        /// Demande d'ajout d'une commande dans la BDD
        /// </summary>
        /// <param name="commande"></param>
        public static void AddCommande(Commande commande)
        {
            string req = "INSERT INTO commande(id,dateCommande,montant) ";
            req += "values (@id, @dateCommande, @montant);";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", commande.Id);
            parameters.Add("@dateCommande", commande.DateCommande);
            parameters.Add("@montant", commande.Montant);
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqUpdate(req, parameters);
        }

        /// <summary>
        /// Demande d'ajout d'une commandedocument dans la BDD
        /// </summary>
        /// <param name="commandedocument"></param>
        public static void AddCommandeDocument(CommandeDocument commandedocument)
        {
            string req = "INSERT INTO commandedocument(id, nbExemplaire, idLivreDvd, idSuivi)";
            req += "values (@id, @nbExemplaire, @idLivreDvd, @idSuivi);";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", commandedocument.Id);
            parameters.Add("@nbExemplaire", commandedocument.NbExemplaire);
            parameters.Add("@idLivreDvd", commandedocument.IdLivredvd);
            parameters.Add("@idSuivi", "00001");
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqUpdate(req, parameters);
        }

        /// <summary>
        /// Demande de modification du suivi d'une commande dans la BDD
        /// </summary>
        /// <param name="idcommande"></param>
        /// <param name="idSuivi"></param>
        public static void EditCommande(string idcommande, string idSuivi)
        {
            string req = "UPDATE commandedocument SET idSuivi = @idSuivi WHERE id = @idCommande";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@idSuivi", idSuivi },
                    { "@idCommande", idcommande}
                };
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqUpdate(req, parameters);
        }

        /// <summary>
        /// Demande de suppression d'une commandedocument dans la BDD
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteCmdDoc(string id)
        {
            string req = "DELETE FROM commandedocument WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqUpdate(req, parameters);
        }

        /// <summary>
        /// Demande de suppresion d'une commande dans la BDD
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteCmd(string id)
        {
            string req = "DELETE FROM commande WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqUpdate(req, parameters);
        }

        /// <summary>
        /// Retourne toutes les commandes de Dvd dans la BDD
        /// </summary>
        /// <returns>Liste d'objets Commandes</returns>
        public static List<CommandeDocumentDvd> GetAllCommandesDvd()
        {
            List<CommandeDocumentDvd> lesCommandesDvd = null;
            try
            {
                lesCommandesDvd = new List<CommandeDocumentDvd>();
                string req = "SELECT c.id as id_commande, c.dateCommande as dateCommande, c.montant as montant, cd.nbExemplaire as nbExemplaire, cd.idLivreDvd as idLivre, s.id as id_etat, s.libelle as libelle, dvd.synopsis as synopsis, dvd.realisateur as realisateur, dvd.duree as duree, d.titre as titre, g.libelle as genre, p.libelle as public, r.libelle as rayon, d.image as image ";
                req += "FROM `commande`c LEFT JOIN `commandedocument` cd USING(id) ";
                req += "LEFT JOIN `suivi` s ON s.id = cd.idSuivi ";
                req += "LEFT JOIN `dvd` dvd ON dvd.id = cd.idLivreDvd ";
                req += "LEFT JOIN `document` d ON d.id = cd.idLivreDvd ";
                req += "JOIN rayon r on r.id=d.idRayon ";
                req += "JOIN genre g on g.id=d.idGenre ";
                req += "JOIN public p on p.id=d.idPublic ";
                req += "WHERE cd.idLivreDvd = dvd.id ORDER BY c.dateCommande DESC";
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, null);

                while (curs.Read())
                {
                    CommandeDocumentDvd commandeDocumentDvd = new CommandeDocumentDvd(
                        (string)curs.Field("id_commande"),
                        (DateTime)curs.Field("dateCommande"),
                        (double)curs.Field("montant"),
                        (int)curs.Field("nbExemplaire"),
                        (string)curs.Field("idLivre"),
                        (string)curs.Field("id_etat"),
                        (string)curs.Field("libelle"),
                        (string)curs.Field("synopsis"),
                        (string)curs.Field("realisateur"),
                        (int)curs.Field("duree"),
                        (string)curs.Field("titre"),
                        (string)curs.Field("genre"),
                        (string)curs.Field("public"),
                        (string)curs.Field("rayon"),
                        (string)curs.Field("image")
                        );
                    lesCommandesDvd.Add(commandeDocumentDvd);
                }
                curs.Close();
                return lesCommandesDvd;
            }
            catch (Exception e)
            {
                Log.Information("Echec de la récupération des commandes de dvd dans la bdd : {0}", e);
                return lesCommandesDvd;
            }
        }

        /// <summary>
        /// Retourne toutes commandes de revues dans la BDD
        /// </summary>
        /// <returns>Liste d'objets Commandes</returns>
        public static List<CommandeRevue> GetAllCommandesRevues()
        {
            List<CommandeRevue> lesCommandesRevues = null;
            try
            {
                lesCommandesRevues = new List<CommandeRevue>();
                string req = "SELECT c.id, c.dateCommande, a.dateFinAbonnement, a.idRevue, re.empruntable, d.titre, re.periodicite, re.delaiMiseADispo as delaidispo, g.libelle as genre, p.libelle as public, r.libelle as rayon, d.image, c.montant ";
                req += "FROM commande c ";
                req += "LEFT JOIN abonnement a USING(id) ";
                req += "LEFT JOIN revue re ON a.idRevue = re.id ";
                req += "LEFT JOIN document d ON a.idRevue = d.id ";
                req += "JOIN genre g ON d.idGenre = g.id ";
                req += "JOIN public p ON d.idPublic = p.id ";
                req += "JOIN rayon r ON d.idRayon = r.id ";
                req += "WHERE a.idRevue = re.id ";
                req += "ORDER BY c.dateCommande DESC";
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, null);

                while (curs.Read())
                {
                    CommandeRevue commandeRevue = new CommandeRevue(
                        (string)curs.Field("id"),
                        (DateTime)curs.Field("dateCommande"),
                        (double)curs.Field("montant"),
                        (DateTime)curs.Field("dateFinAbonnement"),
                        (string)curs.Field("idRevue"),                    
                        (bool)curs.Field("empruntable"),
                        (string)curs.Field("periodicite"),
                        (int)curs.Field("delaidispo"),
                        (string)curs.Field("titre"),
                        (string)curs.Field("genre"),
                        (string)curs.Field("public"),
                        (string)curs.Field("rayon"),
                        (string)curs.Field("image")
                        );
                    lesCommandesRevues.Add(commandeRevue);
                }
                curs.Close();
                return lesCommandesRevues;
            }
            catch (Exception e)
            {
                Log.Information("Echec de la récupération des commandes de revues dans la bdd : {0}", e);
                return lesCommandesRevues;
            }
        }

        /// <summary>
        /// Ajoute un abonnement dans la BDD
        /// </summary>
        /// <param name="abonnement">Abonnement à ajouter.</param>
        /// <returns>True si l'opération est un succès, false sinon.</returns>
        public static bool AddAbonnementRevue(Abonnement abonnement)
        {
            try
            {
                string req = "INSERT INTO abonnement VALUES (@id, @dateFin, @idRevue);";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                  { "@id", abonnement.Id},
                  { "@dateFin", abonnement.DateFinAbonnement},
                  { "@idRevue", abonnement.IdRevue},
                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }
            catch (Exception e)
            {
                Log.Information("Echec de l'ajout d'un abonnement dans la BDD : {0}", e);
                return false;
            }
        }

        /// <summary>
        /// Demande de suppression d'un abonnement dans la BDD
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteCmdAbonnement(string id)
        {
            string req = "DELETE FROM abonnement WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqUpdate(req, parameters);
        }

        /// <summary>
        /// Demande d'éxecuter la fonction qui va récuperer 
        /// tout les abonnements finnissant dans moins de 31jours
        /// </summary>
        /// <returns></returns>
        public static string GetAbo30days()
        {   
            // Permet d'afficher qu'une seule fois la pop-up au démarrage
            if (nb == 0)
            {
                try
                {
                    string req = "SELECT aboBelow30Days() AS string;";
                    BddMySql curs = BddMySql.GetInstance(connectionString);
                    curs.ReqSelect(req, null);
                    string procedure = "";
                    while (curs.Read())
                    {
                        procedure = (string)curs.Field("string");
                        procedure = procedure.Replace(" blank ", "\n");
                    }
                    nb++;
                    return procedure;
                }
                catch
                {
                    nb++;
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
