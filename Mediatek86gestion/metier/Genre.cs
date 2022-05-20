namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des Genre
    /// </summary>
    public class Genre : Categorie
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Genre(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
