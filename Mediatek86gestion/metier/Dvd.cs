namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des LivreDvd
    /// </summary>
    public class Dvd : LivreDvd
    {

        private readonly int duree;
        private readonly string realisateur;
        private readonly string synopsis;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="image"></param>
        /// <param name="duree"></param>
        /// <param name="realisateur"></param>
        /// <param name="synopsis"></param>
        /// <param name="idGenre"></param>
        /// <param name="genre"></param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic"></param>
        /// <param name="idRayon"></param>
        /// <param name="rayon"></param>
        public Dvd(string id, string titre, string image, int duree, string realisateur, string synopsis,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.duree = duree;
            this.realisateur = realisateur;
            this.synopsis = synopsis;
        }

        /// <summary>
        /// getter sur la durée d'un dvd
        /// </summary>
        public int Duree { get => duree; }
        /// <summary>
        /// getter sur le réalisateur d'un dvd
        /// </summary>
        public string Realisateur { get => realisateur; }
        /// <summary>
        /// getter sur le synopsis d'un dvd
        /// </summary>
        public string Synopsis { get => synopsis; }

        /// <summary>
        /// Retourne le titre du dvd en format string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Titre.ToString();
        }
    }
}
