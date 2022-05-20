using System.Collections.Generic;

namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des Suivis
    /// </summary>
    public class Suivi
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Suivi(string id, string libelle)
        {
            IdSuivi = id;
            Libelle = libelle;
        }

        /// <summary>
        /// getter sur l'id d'un suivi
        /// </summary>
        public string IdSuivi { get; }
        /// <summary>
        /// getter sur le libelle d'un suivi
        /// </summary>
        public string Libelle { get; }

        /// <summary>
        /// stock les états de commandes dans une liste
        /// </summary>
        public static List<Suivi> SuiviItems { get; set; }

        /// <summary>
        /// la commande possède un état ou non
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Suivi LibelleCmd(string id)
        {
            foreach (Suivi suivi in SuiviItems)
            {
                if (suivi.IdSuivi == id) return suivi;
            }
            return new Suivi("-1", "Erreur");
        }

        /// <summary>
        /// Retourne le libellé du suivi
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Libelle;
        }
    }
}
