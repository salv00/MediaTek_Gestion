using System;

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des Exemplaires
    /// </summary>
    public class Exemplaire
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="dateAchat"></param>
        /// <param name="photo"></param>
        /// <param name="idEtat"></param>
        /// <param name="idDocument"></param>
        public Exemplaire(int numero, DateTime dateAchat, string photo,string idEtat, string idDocument)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.IdDocument = idDocument;
        }

        /// <summary>
        /// getter setter sur le numéro d'un exemplaire
        /// </summary>
        public int Numero { get; set; }
        /// <summary>
        /// getter setter sur le lien d'une photo d'un exemplaire
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// getter setter sur la date d'achat d'un exemplaire
        /// </summary>
        public DateTime DateAchat { get; set; }
        /// <summary>
        /// getter setter sur l'id de l'état d'un exemplaire
        /// </summary>
        public string IdEtat { get; set; }
        /// <summary>
        /// getter setter sur l'id du document
        /// </summary>
        public string IdDocument { get; set; }
    }
}
