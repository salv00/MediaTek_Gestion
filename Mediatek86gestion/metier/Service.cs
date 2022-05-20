namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le contructeur des services
    /// </summary>
    public class Service
    {
        private readonly string utilisateur;
        private readonly int service;
        private readonly string nom;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="utilisateur"></param>
        /// <param name="service"></param>
        /// <param name="nom"></param>
        public Service(string utilisateur, int service, string nom)
        {
            this.utilisateur = utilisateur;
            this.service = service;
            this.nom = nom;
        }

        /// <summary>
        /// getter sur l'utilisateur
        /// </summary>
        public string Utilisateur { get => utilisateur; }
        /// <summary>
        /// getter sur l'id du service
        /// </summary>
        public int ServiceInt { get => service; }
        /// <summary>
        /// getter sur le nom du service
        /// </summary>
        public string Nom { get => nom; }
    }
}
