using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des Abonnements
    /// </summary>
    public class Abonnement
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="datefinabonnement"></param>
        /// <param name="idrevue"></param>
        public Abonnement(string id, DateTime datefinabonnement, string idrevue)
        {
            this.Id = id;
            this.DateFinAbonnement = datefinabonnement;
            this.IdRevue = idrevue;
        }

        /// <summary>
        /// getter sur l'id 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// getter sur la date de fin d'abonnement
        /// </summary>
        public DateTime DateFinAbonnement { get; set; }
        /// <summary>
        /// getter sur l'id Revue
        /// </summary>
        public string IdRevue { get; set; }
    }
}
