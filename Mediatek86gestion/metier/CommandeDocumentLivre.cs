using System;

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des CommandeDocumentLivres
    /// </summary>
    public class CommandeDocumentLivre
    {
        private readonly string id;
        private readonly DateTime datecommande;
        private readonly double montant;
        private readonly int nbexemplaire;
        private readonly string idlivredvd;
        private readonly string idsuivi;
        private readonly string libelle;

        private readonly string isbn;
        private readonly string auteur;
        private readonly string collection;

        private readonly string titre;
        private readonly string genre;
        private readonly string typepublic;
        private readonly string rayon;
        private readonly string image;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="datecommande"></param>
        /// <param name="montant"></param>
        /// <param name="nbexemplaire"></param>
        /// <param name="idlivredvd"></param>
        /// <param name="idsuivi"></param>
        /// <param name="libelle"></param>
        /// <param name="isbn"></param>
        /// <param name="auteur"></param>
        /// <param name="collection"></param>
        /// <param name="titre"></param>
        /// <param name="genre"></param>
        /// <param name="typepublic"></param>
        /// <param name="rayon"></param>
        /// <param name="image"></param>
        public CommandeDocumentLivre(string id, DateTime datecommande, double montant, int nbexemplaire, string idlivredvd, string idsuivi, string libelle, string isbn, string auteur, string collection, string titre, string genre, string typepublic, string rayon, string image)
        {
            this.id = id;
            this.datecommande = datecommande;
            this.montant = montant;
            this.nbexemplaire = nbexemplaire;
            this.idlivredvd = idlivredvd;
            this.idsuivi = idsuivi;
            this.libelle = libelle;

            this.isbn = isbn;
            this.auteur = auteur;
            this.collection = collection;

            this.titre = titre;
            this.genre = genre;
            this.typepublic = typepublic;
            this.rayon = rayon;
            this.image = image;
        }

        /// <summary>
        /// getter sur l'id de la commande
        /// </summary>
        public string Id { get => id; }
        /// <summary>
        /// getter sur la date de la commande
        /// </summary>
        public DateTime DateCommande { get => datecommande; }
        /// <summary>
        /// getter sur le montant de la commande
        /// </summary>
        public double Montant { get => montant; }
        /// <summary>
        /// getter sur le nombre d'exemplaire de la commande
        /// </summary>
        public int NbExemplaire { get => nbexemplaire; }
        /// <summary>
        /// getter sur l'id du livre ou du dvd de la commande
        /// </summary>
        public string IdLivredvd { get => idlivredvd; }
        /// <summary>
        /// getter sur l'id du suivi de la commande
        /// </summary>
        public string IdSuivi { get => idsuivi; }
        /// <summary>
        /// getter sur le libelle du suivi de la commande
        /// </summary>
        public string Libelle { get => libelle; }

        /// <summary>
        /// getter sur le numéro isbn du livre lié à la commande
        /// </summary>
        public string Isbn { get => isbn; }
        /// <summary>
        /// getter sur l'auteur du livre lié à la commande
        /// </summary>
        public string Auteur { get => auteur; }
        /// <summary>
        /// getter sur la collection du livre lié à la commande
        /// </summary>
        public string Collection { get => collection; }

        /// <summary>
        /// getter sur le titre du livre lié à la commande
        /// </summary>
        public string Titre { get => titre; }
        /// <summary>
        /// getter sur le genre du livre lié à la commande
        /// </summary>
        public string Genre { get => genre; }
        /// <summary>
        /// getter sur le type de public du livre lié à la commande
        /// </summary>
        public string Typepublic { get => typepublic; }
        /// <summary>
        /// getter sur le rayon du livre lié à la commande
        /// </summary>
        public string Rayon { get => rayon; }
        /// <summary>
        /// getter sur l'image du livre lié à la commande
        /// </summary>
        public string Image { get => image; }
    }
}

