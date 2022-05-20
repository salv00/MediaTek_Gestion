using System.Collections.Generic;
using Mediatek86.modele;
using Mediatek86.metier;
using Mediatek86.vue;
using Serilog;


namespace Mediatek86.controleur
{
    /// <summary>
    /// Gère les interractions entre la vue et le modèle
    /// </summary>
    public class Controle
    {
        private readonly List<Livre> lesLivres;
        private readonly List<Dvd> lesDvd;
        private readonly List<Revue> lesRevues;
        private readonly List<Categorie> lesRayons;
        private readonly List<Categorie> lesPublics;
        private readonly List<Categorie> lesGenres;
        private readonly List<Suivi> lesSuivis;

        /// <summary>
        /// Ouverture de la fenêtre
        /// </summary>
        public Controle()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/logs.txt", 
                rollingInterval: RollingInterval.Day)               
                .CreateLogger();

            lesLivres = Dao.GetAllLivres();
            lesDvd = Dao.GetAllDvd();
            lesRevues = Dao.GetAllRevues();
            lesGenres = Dao.GetAllGenres();
            lesRayons = Dao.GetAllRayons();
            lesPublics = Dao.GetAllPublics();
            lesSuivis = Dao.GetAllSuivis();
            frmLogin frmLogin = new frmLogin(this);
            frmLogin.ShowDialog();
        }

        /// <summary>
        /// demande de vérification du login
        /// </summary>
        /// <param name="identifiant"></param>
        /// <param name="mdp"></param>
        /// <returns></returns>
        public Service ControleAuthentification(string identifiant, string mdp)
        {
            Service service = Dao.ControleAuthentification(identifiant, mdp);
            return service;
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Collection d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return lesGenres;
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Collection d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return lesLivres;
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Collection d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return lesDvd;
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Collection d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return lesRevues;
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return lesRayons;
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return lesPublics;
        }

        /// <summary>
        /// Récupère tout les suivis
        /// </summary>
        /// <returns>La liste contenant tout les suivis</returns>
        public List<Suivi> GetAllSuivis()
        {
            return lesSuivis;
        }

        /// <summary>
        /// Récupère les exemplaires d'une revue
        /// </summary>
        /// <returns>Collection d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return Dao.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue 
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return Dao.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// getter sur la liste des commandes de livres
        /// </summary>
        /// <returns>Collection d'objets Commandes</returns>
        public List<CommandeDocumentLivre> GetAllCommandesLivres()
        {
            List<CommandeDocumentLivre> lesCommandesLivres;
            lesCommandesLivres = Dao.GetAllCommandesLivres();
            return lesCommandesLivres;
        }

        /// <summary>
        /// Demande d'ajout d'une commande
        /// </summary>
        /// <param name="commande"></param>
        public void AddCommande(Commande commande)
        {
            Dao.AddCommande(commande);
        }

        /// <summary>
        /// Demande d'ajout d'une commande document
        /// </summary>
        /// <param name="commandedocument"></param>
        public void AddCommandeDocument(CommandeDocument commandedocument)
        {
            Dao.AddCommandeDocument(commandedocument);
        }

        /// <summary>
        /// Demande de modification d'une commande
        /// </summary>
        /// <param name="idCommande"></param>
        /// <param name="idSuivi"></param>
        public void EditCommande(string idCommande, string idSuivi)
        {
            Dao.EditCommande(idCommande, idSuivi);
        }

        /// <summary>
        /// Demande de suppression d'une commande
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCmd(string id)
        {
            Dao.DeleteCmdDoc(id);
            Dao.DeleteCmd(id);
        }

        /// <summary>
        /// getter sur la liste des commandes de dvd
        /// </summary>
        /// <returns>Collection d'objets Commandes</returns>
        public List<CommandeDocumentDvd> GetAllCommandesDvd()
        {
            List<CommandeDocumentDvd> lesCommandesDvd;
            lesCommandesDvd = Dao.GetAllCommandesDvd();
            return lesCommandesDvd;
        }

        /// <summary>
        /// getter sur la liste des commandes de revues
        /// </summary>
        /// <returns>Collection d'objets Commandes</returns>
        public List<CommandeRevue> GetAllCommandesRevues()
        {
            List<CommandeRevue> lesCommandesRevues;
            lesCommandesRevues = Dao.GetAllCommandesRevues();
            return lesCommandesRevues;
        }


        /// <summary>
        /// Demande d'ajout d'un abonnement
        /// </summary>
        /// <param name="abonnement"></param>
        public void AddAbonnementRevue(Abonnement abonnement)
        {
            Dao.AddAbonnementRevue(abonnement);
        }

        /// <summary>
        /// Demande de suppression d'un abonnement
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAbonnement(string id)
        {
            Dao.DeleteCmdAbonnement(id);
            Dao.DeleteCmd(id);
        }

        /// <summary>
        /// getter sur la liste des abonnements finissant dans 30jours
        /// </summary>
        /// <returns></returns>
        public string GetAbo30days()
        {
            string getabo30days = Dao.GetAbo30days();
            return getabo30days;
        }
    }

}

