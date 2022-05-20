namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des Revues
    /// </summary>
    public class Revue : Document
    {
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
        /// <param name="empruntable"></param>
        /// <param name="periodicite"></param>
        /// <param name="delaiMiseADispo"></param>
        public Revue(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon, 
            bool empruntable, string periodicite, int delaiMiseADispo)
             : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            Periodicite = periodicite;
            Empruntable = empruntable;
            DelaiMiseADispo = delaiMiseADispo;
        }

        /// <summary>
        /// getter setter sur la periodicite d'une revue
        /// </summary>
        public string Periodicite { get; set; }
        /// <summary>
        /// getter setter qui retourne vrai ou faux si la revue est empruntable
        /// </summary>
        public bool Empruntable { get; set; }
        /// <summary>
        /// getter setter sur le délai de mise à disposition d'une revue
        /// </summary>
        public int DelaiMiseADispo { get; set; }

        /// <summary>
        /// Retourne le titre d'une revue en format string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Titre.ToString();
        }
    }
}
