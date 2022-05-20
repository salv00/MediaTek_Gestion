using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des CommandeDocuments
    /// </summary>
    public class CommandeDocument
    {
        private readonly string id;
        private readonly int nbexemplaire;
        private readonly string idlivredvd;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nbexemplaire"></param>
        /// <param name="idlivredvd"></param>
        public CommandeDocument(string id, int nbexemplaire, string idlivredvd)
        {
            this.id = id;
            this.nbexemplaire = nbexemplaire;
            this.idlivredvd = idlivredvd;
        }

        /// <summary>
        /// getter sur l'id  de la commande
        /// </summary>
        public string Id { get => id; }
        /// <summary>
        /// getter sur le nombre d'exemplaire de cette commande
        /// </summary>
        public int NbExemplaire { get => nbexemplaire; }
        /// <summary>
        /// getter sur l'id du livre ou du dvd de cette commande
        /// </summary>
        public string IdLivredvd { get => idlivredvd; }
    }
}
