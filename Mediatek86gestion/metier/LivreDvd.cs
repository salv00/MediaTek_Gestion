﻿namespace Mediatek86.metier
{
    /// <summary>
    /// Classe qui gère le constructeur des LivreDvd
    /// </summary>
    public abstract class LivreDvd : Document
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
        protected LivreDvd(string id, string titre, string image, string idGenre, string genre, 
            string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
        }

    }
}

