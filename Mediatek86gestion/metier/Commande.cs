using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des Commandes
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="montant"></param>
        public Commande(string id, DateTime date, int montant)
        {
            this.Id = id;
            this.DateCommande = date;
            this.Montant = montant;
        }

        /// <summary>
        /// getter sur l'id  de la commande
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// getter sur la date de commande correspondant à l'id
        /// </summary>
        public DateTime DateCommande { get; set; }
        /// <summary>
        /// getter sur le montant de la commande correspondant à l'id
        /// </summary>
        public int Montant { get; set; }
    }
}