namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des Etats
    /// </summary>
    public class Etat
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Etat(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// getter setter sur l'id de l'état d'un exemplaire
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// getter setter sur le nom de l'état d'un exemplaire
        /// </summary>
        public string Libelle { get; set; }
    }
}
