namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des Livres
    /// </summary>
    public class Livre : LivreDvd
    {
        private readonly string isbn;
        private readonly string auteur;
        private readonly string collection;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="image"></param>
        /// <param name="isbn"></param>
        /// <param name="auteur"></param>
        /// <param name="collection"></param>
        /// <param name="idGenre"></param>
        /// <param name="genre"></param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic"></param>
        /// <param name="idRayon"></param>
        /// <param name="rayon"></param>
        public Livre(string id, string titre, string image, string isbn, string auteur, string collection, 
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            :base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.isbn = isbn;
            this.auteur = auteur;
            this.collection = collection;
        }

        /// <summary>
        /// getter sur le numéro isbn d'un livre
        /// </summary>
        public string Isbn { get => isbn; }
        /// <summary>
        /// getter sur l'auteur d'un livre
        /// </summary>
        public string Auteur { get => auteur; }
        /// <summary>
        /// getter sur la collection d'un livre
        /// </summary>
        public string Collection { get => collection; }

        /// <summary>
        /// Retourne le titre du livre en format string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Titre.ToString();
        }

    }
}
