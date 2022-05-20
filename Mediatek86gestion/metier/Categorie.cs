namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le contructeur des Catégories
    /// </summary>
    public abstract class Categorie
    {
        private readonly string id;
        private readonly string libelle;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        protected Categorie(string id, string libelle)
        {
            this.id = id;
            this.libelle = libelle;
        }

        /// <summary>
        /// getter sur l'id
        /// </summary>
        public string Id { get => id; }
        /// <summary>
        /// getter sur le libelle de la categorie
        /// </summary>
        public string Libelle { get => libelle; }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.libelle;
        }

    }
}
