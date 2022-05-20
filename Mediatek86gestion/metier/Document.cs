namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des Documents
    /// </summary>
    public class Document
    {

        private readonly string id;
        private readonly string titre;
        private readonly string image;
        private readonly string idGenre;
        private readonly string genre;
        private readonly string idPublic;
        private readonly string lePublic;
        private readonly string idRayon;
        private readonly string rayon;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="image"></param>
        /// <param name="idGenre"></param>
        /// <param name="genre"></param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic"></param>
        /// <param name="idRayon"></param>
        /// <param name="rayon"></param>
        public Document(string id, string titre, string image, string idGenre, string genre, 
            string idPublic, string lePublic, string idRayon, string rayon)
        {
            this.id = id;
            this.titre = titre;
            this.image = image;
            this.idGenre = idGenre;
            this.genre = genre;
            this.idPublic = idPublic;
            this.lePublic = lePublic;
            this.idRayon = idRayon;
            this.rayon = rayon;
        }

        /// <summary>
        /// getter sur l'id du document
        /// </summary>
        public string Id { get => id; }
        /// <summary>
        /// getter sur le titre du document
        /// </summary>
        public string Titre { get => titre; }
        /// <summary>
        /// getter sur l'image du document
        /// </summary>
        public string Image { get => image; }
        /// <summary>
        /// getter sur l'id du genre du document
        /// </summary>
        public string IdGenre { get => idGenre; }
        /// <summary>
        /// getter sur le nom du genre du document
        /// </summary>
        public string Genre { get => genre; }
        /// <summary>
        /// getter sur l'id du type de public du document
        /// </summary>
        public string IdPublic { get => idPublic; }
        /// <summary>
        /// getter sur le nom du type de public du document
        /// </summary>
        public string Public { get => lePublic; }
        /// <summary>
        /// getter sur l'id du rayon du document
        /// </summary>
        public string IdRayon { get => idRayon; }
        /// <summary>
        /// getter sur le nom du rayon du document
        /// </summary>
        public string Rayon { get => rayon; }

    }


}
