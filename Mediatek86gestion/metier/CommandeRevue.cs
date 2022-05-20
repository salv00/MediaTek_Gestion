using System;

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des CommandeRevue
    /// </summary>
    public class CommandeRevue
    {
        private readonly string id;
        private readonly DateTime datecommande;
        private readonly double montant;

        private readonly DateTime datefinabo;
        private readonly string idRevue;

        private readonly bool empruntable;
        private readonly string periodicite;
        private readonly int delaiMiseADispo;

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
        /// <param name="datefinabo"></param>
        /// <param name="idRevue"></param>
        /// <param name="empruntable"></param>
        /// <param name="periodicite"></param>
        /// <param name="delaiMiseADispo"></param>
        /// <param name="titre"></param>
        /// <param name="genre"></param>
        /// <param name="typepublic"></param>
        /// <param name="rayon"></param>
        /// <param name="image"></param>
        public CommandeRevue(string id, DateTime datecommande, double montant, DateTime datefinabo, string idRevue, bool empruntable, string periodicite, int delaiMiseADispo, string titre, string genre, string typepublic, string rayon, string image)
        {
            this.id = id;
            this.datecommande = datecommande;
            this.montant = montant;

            this.datefinabo = datefinabo;
            this.idRevue = idRevue;

            this.empruntable = empruntable;
            this.periodicite = periodicite;
            this.delaiMiseADispo = delaiMiseADispo;

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
        /// getter sur la date de fin de l'abonnement
        /// </summary>
        public DateTime DateFinAbo { get => datefinabo; }
        /// <summary>
        /// getter sur l'id de la revue
        /// </summary>
        public string IdRevue { get => idRevue; }

        /// <summary>
        /// getter qui retourne vrai ou faux si une revue est empruntable
        /// </summary>
        public bool Empruntable { get => empruntable; }
        /// <summary>
        /// getter sur la périodicité de la revue
        /// </summary>
        public string Periodicite { get => periodicite; }
        /// <summary>
        /// getter sur le délai de la mise à disposition de la revue
        /// </summary>
        public int DelaiMiseADispo { get => delaiMiseADispo; }

        /// <summary>
        /// getter sur le titre de la revue lié à la commande
        /// </summary>
        public string Titre { get => titre; }
        /// <summary>
        /// getter sur le genre de la revue lié à la commande
        /// </summary>
        public string Genre { get => genre; }
        /// <summary>
        /// getter sur le type de public de la revue lié à la commande
        /// </summary>
        public string Typepublic { get => typepublic; }
        /// <summary>
        /// getter sur le rayon de la revue lié à la commande
        /// </summary>
        public string Rayon { get => rayon; }
        /// <summary>
        /// getter sur l'image de la revue lié à la commande
        /// </summary>
        public string Image { get => image; }
    }
}
