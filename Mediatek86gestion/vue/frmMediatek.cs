using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mediatek86.metier;
using Mediatek86.controleur;
using System.Drawing;
using System.Linq;

namespace Mediatek86.vue
{
    /// <summary>
    /// Fenêtre qui contient toutes les données de Mediatek
    /// </summary>
    public partial class FrmMediatek : Form
    {

        #region Variables globales

        private readonly Controle controle;
        const string ETATNEUF = "00001";

        private readonly BindingSource bdgLivresListe = new BindingSource();
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private readonly BindingSource bdgExemplairesListe = new BindingSource();

        private readonly BindingSource bdgLivresListeCmd = new BindingSource();
        private readonly BindingSource bdgDvdListeCmd = new BindingSource();
        private readonly BindingSource bdgRevueListeCmd = new BindingSource();
        private readonly BindingSource bdgStatutListe = new BindingSource();

        private List<Livre> lesLivres = new List<Livre>();
        private List<Dvd> lesDvd = new List<Dvd>();
        private List<Revue> lesRevues = new List<Revue>();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        private Service service;
        private List<CommandeDocumentLivre> lesCommandesLivres = new List<CommandeDocumentLivre>();
        private List<CommandeDocumentDvd> lesCommandesDvd = new List<CommandeDocumentDvd>();
        private List<CommandeRevue> lesCommandesRevues = new List<CommandeRevue>();

        private bool AddCmdLivre = false;
        private bool EditCmdLivre = false;
        private bool AddCmdDvd = false;
        private bool EditCmdDvd = false;

        #endregion

        /// <summary>
        /// initialisation du form
        /// </summary>
        /// <param name="controle"></param>
        /// <param name="service"></param>
        public FrmMediatek(Controle controle, Service service)
        {
            InitializeComponent();
            this.controle = controle;
            this.service = service;
        }


        #region modules communs

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories"></param>
        /// <param name="bdg"></param>
        /// <param name="cbx"></param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        #endregion


        #region Revues
        //-----------------------------------------------------------
        // ONGLET "Revues"
        //------------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["empruntable"].Visible = false;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>();
                    revues.Add(revue);
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            chkRevuesEmpruntable.Checked = revue.Empruntable;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;     
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch 
            { 
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            chkRevuesEmpruntable.Checked = false;
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }

        #endregion


        #region Livres

        //-----------------------------------------------------------
        // ONGLET "LIVRES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            // Si le service est pret alors on enleve des onglets
            if (service.ServiceInt == 2)
            {
                tabOngletsApplication.TabPages.RemoveByKey("tabReceptionRevue");
                tabOngletsApplication.TabPages.RemoveByKey("tabCmdLivres");
                tabOngletsApplication.TabPages.RemoveByKey("tabCmdDvd");
                tabOngletsApplication.TabPages.RemoveByKey("tabCmdRevues");
            }
            lesLivres = controle.GetAllLivres();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
            // Si le service est admin alors affichage du pop-up qui montre les abonnements 
            // finnissant dans moins de 31jours
            if (service.ServiceInt == 1)
            {
                string getabo30days = controle.GetAbo30days();
                if (getabo30days != "")
                {
                    MessageBox.Show(getabo30days, "Abonnements finissants dans moins de 30 jours (Titre-Date de fin) :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>();
                    livres.Add(livre);
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0 
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre"></param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;      
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch 
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        #endregion


        #region Dvd
        //-----------------------------------------------------------
        // ONGLET "DVD"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controle.GetAllDvd();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>();
                    Dvd.Add(dvd);
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd"></param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString() ;
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch 
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        #endregion


        #region Réception Exemplaire de presse
        //-----------------------------------------------------------
        // ONGLET "RECEPTION DE REVUES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet : blocage en saisie des champs de saisie des infos de l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues();
            accesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            bdgExemplairesListe.DataSource = exemplaires;
            dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
            dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
            dgvReceptionExemplairesListe.Columns["idDocument"].Visible = false;
            dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
            dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    VideReceptionRevueInfos();
                }
            }
            else
            {
                VideReceptionRevueInfos();
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            accesReceptionExemplaireGroupBox(false);
            VideReceptionRevueInfos();
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            chkReceptionRevueEmpruntable.Checked = revue.Empruntable;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;         
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch 
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            afficheReceptionExemplairesRevue();
            // accès à la zone d'ajout d'un exemplaire
            accesReceptionExemplaireGroupBox(true);
        }

        private void afficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controle.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
        }

        /// <summary>
        /// Vide les zones d'affchage des informations de la revue
        /// </summary>
        private void VideReceptionRevueInfos()
        {
            txbReceptionRevuePeriodicite.Text = "";
            chkReceptionRevueEmpruntable.Checked = false;
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            lesExemplaires = new List<Exemplaire>();
            RemplirReceptionExemplairesListe(lesExemplaires);
            accesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de l'exemplaire
        /// </summary>
        private void VideReceptionExemplaireInfos()
        {
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces"></param>
        private void accesReceptionExemplaireGroupBox(bool acces)
        {
            VideReceptionExemplaireInfos();
            grpReceptionExemplaire.Enabled = acces;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;         
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch 
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controle.CreerExemplaire(exemplaire))
                    {
                        VideReceptionExemplaireInfos();
                        afficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// Sélection d'une ligne complète et affichage de l'image sz l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        #endregion

        #region Commandes de livres
        //-----------------------------------------------------------
        // ONGLET "COMMANDES DE LIVRES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Commandes de Livres : 
        /// appel des méthodes pour remplir le datagrid des commandes de livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCmdLivres_Enter(object sender, EventArgs e)
        {
            lesCommandesLivres = controle.GetAllCommandesLivres();
            InitDataGridViewLivre();
            RemplirCbLivre();
            RemplirCbSuivi();
            DisableAddEditCmdLivres();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void InitDataGridViewLivre()
        {
            List<CommandeDocumentLivre> livres = controle.GetAllCommandesLivres();
            bdgLivresListeCmd.DataSource = livres;
            dgvListeCmdLivres.DataSource = bdgLivresListeCmd;
            // dgvListeCmdLivres.Columns["id"].Visible = false;
            dgvListeCmdLivres.Columns["idlivredvd"].Visible = false;
            dgvListeCmdLivres.Columns["idsuivi"].Visible = false;
            dgvListeCmdLivres.Columns["auteur"].Visible = false;
            dgvListeCmdLivres.Columns["isbn"].Visible = false;
            dgvListeCmdLivres.Columns["collection"].Visible = false;
            dgvListeCmdLivres.Columns["genre"].Visible = false;
            dgvListeCmdLivres.Columns["rayon"].Visible = false;
            dgvListeCmdLivres.Columns["typepublic"].Visible = false;
            dgvListeCmdLivres.Columns["titre"].Visible = false;
            dgvListeCmdLivres.Columns["image"].Visible = false;

            dgvListeCmdLivres.Columns["montant"].HeaderText = "Montant (€)";
            dgvListeCmdLivres.Columns["nbexemplaire"].HeaderText = "Nombre d'exemplaire";
            dgvListeCmdLivres.Columns["libelle"].HeaderText = "Etat du Suivi";
            dgvListeCmdLivres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Remplit le dategrid avec l'id d'un livre reçue en paramètre
        /// </summary>
        private void InitDataGridViewLivreRecherche(List<CommandeDocumentLivre> livres)
        {
            bdgLivresListeCmd.DataSource = livres;
            dgvListeCmdLivres.DataSource = bdgLivresListeCmd;
            // dgvListeCmdLivres.Columns["id"].Visible = false;
            dgvListeCmdLivres.Columns["idlivredvd"].Visible = false;
            dgvListeCmdLivres.Columns["idsuivi"].Visible = false;
            dgvListeCmdLivres.Columns["auteur"].Visible = false;
            dgvListeCmdLivres.Columns["isbn"].Visible = false;
            dgvListeCmdLivres.Columns["collection"].Visible = false;
            dgvListeCmdLivres.Columns["genre"].Visible = false;
            dgvListeCmdLivres.Columns["rayon"].Visible = false;
            dgvListeCmdLivres.Columns["typepublic"].Visible = false;
            dgvListeCmdLivres.Columns["titre"].Visible = false;
            dgvListeCmdLivres.Columns["image"].Visible = false;

            dgvListeCmdLivres.Columns["montant"].HeaderText = "Montant (€)";
            dgvListeCmdLivres.Columns["nbexemplaire"].HeaderText = "Nombre d'exemplaire";
            dgvListeCmdLivres.Columns["libelle"].HeaderText = "Etat du Suivi";
            dgvListeCmdLivres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Affichage des informations du livre
        /// </summary>
        /// <param name="document"></param>
        private void AfficheLivresInfosCmd(CommandeDocumentLivre document)
        {
            txbLivresAuteurCmd.Text = document.Auteur;
            txbLivresCollectionCmd.Text = document.Collection;
            txbLivresImageCmd.Text = document.Image;
            txbLivresIsbnCmd.Text = document.Isbn;
            txbLivresNumeroCmd.Text = document.IdLivredvd;
            txbLivresGenreCmd.Text = document.Genre;
            txbLivresPublicCmd.Text = document.Typepublic;
            txbLivresRayonCmd.Text = document.Rayon;
            txbLivresTitreCmd.Text = document.Titre;
            string image = document.Image;
            try
            {
                pcbLivresImageCmd.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImageCmd.Image = null;
            }
        }

        /// <summary>
        /// Permet de disable tout les groupes de saisies
        /// </summary>
        public void DisableAddEditCmdLivres()
        {
            grpLivresCmdAjout.Enabled = false;
            grpLivresCmdModif.Enabled = false;
            AddCmdLivre = false;
            EditCmdLivre = false;
        }

        /// <summary>
        /// Recherche et affichage des commandes correspondant au livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresNumRechercheCmd_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRechercheCmd.Text.Equals(""))
            {
                List<CommandeDocumentLivre> livres = lesCommandesLivres.FindAll(x => x.IdLivredvd.Equals(txbLivresNumRechercheCmd.Text));
                txbLivresNumRechercheCmd.Text = "";
                if (livres.Any())
                {             
                    InitDataGridViewLivreRecherche(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    InitDataGridViewLivre();
                }
            }
            else
            {
                InitDataGridViewLivre();
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeCmdLivres_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvListeCmdLivres.CurrentCell != null)
            {
                try
                {
                    CommandeDocumentLivre document = (CommandeDocumentLivre)bdgLivresListeCmd.List[bdgLivresListeCmd.Position];
                    AfficheLivresInfosCmd(document);
                }
                catch
                {
                    VideLivresZonesCmd();
                }
            }
            else
            {
                VideLivresZonesCmd();
            }
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZonesCmd()
        {
            txbLivresNumRechercheCmd.Text = "";
        }

        /// <summary>
        /// vide les zones de saisie d'une commande de livres
        /// </summary>
        private void VideLivreAjoutCmd()
        {
            txbIdCmdAdd.Text = "";
            numMontantCmdAdd.Value = 0;
            numNbExemplaireCmdAdd.Value = 0;
            dtpDateCmdAdd.Value = DateTime.Today;
        }

        /// <summary>
        /// vide les zones de modification d'une commande de livres
        /// </summary>
        private void ViderEditCmdLivre()
        {
            txbIdCmdEdit.Text = "";
            cboSuivis.SelectedIndex = 0;
        }

        /// <summary>
        /// Affiche les livres dans les combobox d'ajout d'une commande de livres
        /// </summary>
        public void RemplirCbLivre()
        {
            List<Livre> livres = controle.GetAllLivres();
            bdgLivresListe.DataSource = livres;
            cboLivres.DataSource = bdgLivresListe;
            if (cboLivres.Items.Count > 0)
            {
                cboLivres.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Affiche les status dans les combobox de modification d'une commande de livres
        /// </summary>
        public void RemplirCbSuivi()
        {
            List<Suivi> suivi = controle.GetAllSuivis();
            bdgStatutListe.DataSource = suivi;
            cboSuivis.DataSource = bdgStatutListe;
            if (cboSuivis.Items.Count > 0)
            {
                cboSuivis.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Affiche les valeurs dans les combobox de modification d'une commande de livres
        /// </summary>
        /// <param name="document"></param>
        private void RemplirEditCmdLivre(CommandeDocumentLivre document)
        {
            txbIdCmdEdit.Text = document.Id;
            string libelle = document.Libelle;
            cboSuivis.SelectedIndex = cboSuivis.FindStringExact(libelle);

        }

        /// <summary>
        /// Ouvre et referme l'interface d'ajout d'une commande de livres
        /// </summary>
        private void btnLivresAjouterCmd_Click(object sender, EventArgs e)
        {
            grpLivresCmdAjout.Enabled = true;
            AddCmdLivre = true;

            if (EditCmdLivre)
            {
                grpLivresCmdModif.Enabled = false;
                EditCmdLivre = false;
                ViderEditCmdLivre();
            }
        }

        /// <summary>
        /// Ouvre et referme l'interface de modification du statut d'une commande de livres
        /// </summary>
        private void btnLivresModifCmd_Click(object sender, EventArgs e)
        {
            CommandeDocumentLivre laCommande = (CommandeDocumentLivre)bdgLivresListeCmd.List[bdgLivresListeCmd.Position];

            if (laCommande.Libelle == "Réglée.")
            {
                MessageBox.Show("Cette commande est déja réglée, impossible de retourner en arrière", "Erreur");
                return;
            }

            grpLivresCmdModif.Enabled = true;
            EditCmdLivre = true;

            if (AddCmdLivre)
            {
                grpLivresCmdAjout.Enabled = false;
                AddCmdLivre = false;
                VideLivreAjoutCmd();
            }            
            RemplirEditCmdLivre(laCommande);
        }

        /// <summary>
        /// Demande d'ajout d'une commande de livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCmd_Click(object sender, EventArgs e)
        {
            // On regarde si l'id de la commande existe déjà si oui une erreur se produit
            string idCmd = txbIdCmdAdd.Text;
            int LivreExiste = bdgLivresListeCmd.IndexOf(bdgLivresListeCmd.List.OfType<CommandeDocumentLivre>().ToList().Find(f => f.Id == idCmd));
            int DVDExiste = bdgDvdListeCmd.IndexOf(bdgDvdListeCmd.List.OfType<CommandeDocumentDvd>().ToList().Find(f => f.Id == idCmd));
            int RevueExiste = bdgRevueListeCmd.IndexOf(bdgRevueListeCmd.List.OfType<CommandeRevue>().ToList().Find(f => f.Id == idCmd));
            if (LivreExiste != -1 || DVDExiste != -1 || RevueExiste != -1)
            {
                MessageBox.Show("L'Id de la commande existe déjà. Veuillez saisir une Id non existante.", "Erreur");
                return;
            }
            // Demande de l'ajout en fonction des informations entrées 
            if (!txbIdCmdAdd.Text.Equals("") && numMontantCmdAdd.Value > 0 && numNbExemplaireCmdAdd.Value >= 1 &&  MessageBox.Show("Etes vous sûr?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                int Montant = (int)numMontantCmdAdd.Value;
                int NbExemplaire = (int)numNbExemplaireCmdAdd.Value;

                Commande commande = new Commande(txbIdCmdAdd.Text, dtpDateCmdAdd.Value, Montant);
                CommandeDocument commandedocument = new CommandeDocument(txbIdCmdAdd.Text, NbExemplaire, ((Livre)bdgLivresListe.List[bdgLivresListe.Position]).Id);

                controle.AddCommande(commande);
                controle.AddCommandeDocument(commandedocument);

                InitDataGridViewLivre();
                VideLivreAjoutCmd();
                grpLivresCmdAjout.Enabled = false;
            }
            else
            {
                MessageBox.Show("Tous les champs doivent être remplis correctement.", "Information");
            }
        }

        /// <summary>
        /// Permet l'annulation de l'ajout d'une commande de livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCmdAnnuler_Click(object sender, EventArgs e)
        {
            VideLivreAjoutCmd();
            grpLivresCmdAjout.Enabled = false;
        }

        /// <summary>
        /// Demande de modification du suivi d'une commande de livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCmd_Click(object sender, EventArgs e)
        {
            Suivi suivi = (Suivi)bdgStatutListe.List[bdgStatutListe.Position];
            string suiviId = suivi.IdSuivi;
            string suiviLibelle = suivi.Libelle;
            CommandeDocumentLivre laCommande = (CommandeDocumentLivre)bdgLivresListeCmd.List[bdgLivresListeCmd.Position];
            if ((laCommande.Libelle == "Réglée." || laCommande.Libelle == "Livrée.") && ((suiviLibelle == "En cours.") || (suiviLibelle == "Relancée.")))
            {
                MessageBox.Show("La commande est dans un stade trop avancé pour revenir a cet état", "Erreur");
                RemplirEditCmdLivre(laCommande);
                return;
            }
            if (suiviLibelle == "Réglée." && laCommande.Libelle != "Livrée.")
            {
                MessageBox.Show("La commande ne peut etre réglée sans être livrée avant.", "Erreur");
                RemplirEditCmdLivre(laCommande);
                return;
            }
            if(MessageBox.Show("Etes vous sûr?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                controle.EditCommande(laCommande.Id, suiviId);
                InitDataGridViewLivre();
                ViderEditCmdLivre();
                grpLivresCmdModif.Enabled = false;
            }            
        }

        /// <summary>
        /// Permet l'annulation de la modification d'une commande de livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCmdAnnuler_Click(object sender, EventArgs e)
        {
            ViderEditCmdLivre();
            grpLivresCmdModif.Enabled = false;
        }

        /// <summary>
        /// Demande de suppression d'une commande de livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresSupprCmd_Click(object sender, EventArgs e)
        {
            CommandeDocumentLivre laCommande = (CommandeDocumentLivre)bdgLivresListeCmd.List[bdgLivresListeCmd.Position];
            if (laCommande.Libelle == "Livrée." || laCommande.Libelle == "Réglée.")
            {
                MessageBox.Show("La commande est dans un stade trop avancé pour être supprimée");
                return;
            }

            if (MessageBox.Show("Etes vous sûr?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                controle.DeleteCmd(laCommande.Id);
                InitDataGridViewLivre();
            }
        }

        /// <summary>
        /// Tri sur les colonnes de commandes de livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeCmdLivres_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZonesCmd();
            string titreColonne = dgvListeCmdLivres.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocumentLivre> sortedList = new List<CommandeDocumentLivre>();
            lesCommandesLivres = controle.GetAllCommandesLivres();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandesLivres.OrderByDescending(o => o.Id).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandesLivres.OrderByDescending(o => o.DateCommande).ToList();
                    break;
                case "Montant (€)":
                    sortedList = lesCommandesLivres.OrderByDescending(o => o.Montant).ToList();
                    break;
                case "Nombre d'exemplaire":
                    sortedList = lesCommandesLivres.OrderByDescending(o => o.NbExemplaire).ToList();
                    break;
                case "Etat du Suivi":
                    sortedList = lesCommandesLivres.OrderByDescending(o => o.Libelle).ToList();
                    break;
            }
            InitDataGridViewLivreRecherche(sortedList);        
        }

        #endregion

        #region Commandes de dvd
        //-----------------------------------------------------------
        // ONGLET "COMMANDES DE DVD"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Commandes de DVD : 
        /// appel des méthodes pour remplir le datagrid des commandes de livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCmdDvd_Enter(object sender, EventArgs e)
        {
            lesCommandesDvd = controle.GetAllCommandesDvd();
            InitDataGridViewDvd();
            RemplirCbDvd();
            RemplirCbSuiviDvd();
            DisableAddEditCmdDvd();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void InitDataGridViewDvd()
        {
            List<CommandeDocumentDvd> Dvd = controle.GetAllCommandesDvd();
            bdgDvdListeCmd.DataSource = Dvd;
            dgvListeCmdDvd.DataSource = bdgDvdListeCmd;
            // dgvListeCmdDvd.Columns["id"].Visible = false;
            dgvListeCmdDvd.Columns["idlivredvd"].Visible = false;
            dgvListeCmdDvd.Columns["idsuivi"].Visible = false;
            dgvListeCmdDvd.Columns["realisateur"].Visible = false;
            dgvListeCmdDvd.Columns["synopsis"].Visible = false;
            dgvListeCmdDvd.Columns["duree"].Visible = false;
            dgvListeCmdDvd.Columns["genre"].Visible = false;
            dgvListeCmdDvd.Columns["rayon"].Visible = false;
            dgvListeCmdDvd.Columns["typepublic"].Visible = false;
            dgvListeCmdDvd.Columns["titre"].Visible = false;
            dgvListeCmdDvd.Columns["image"].Visible = false;

            dgvListeCmdDvd.Columns["montant"].HeaderText = "Montant (€)";
            dgvListeCmdDvd.Columns["nbexemplaire"].HeaderText = "Nombre d'exemplaire";
            dgvListeCmdDvd.Columns["libelle"].HeaderText = "Etat du Suivi";
            dgvListeCmdDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Remplit le dategrid avec l'id d'un dvd reçue en paramètre
        /// </summary>
        private void InitDataGridViewDvdRecherche(List<CommandeDocumentDvd> lesDvd)
        {
            bdgDvdListeCmd.DataSource = lesDvd;
            dgvListeCmdDvd.DataSource = bdgDvdListeCmd;
            // dgvListeCmdDvd.Columns["id"].Visible = false;
            dgvListeCmdDvd.Columns["idlivredvd"].Visible = false;
            dgvListeCmdDvd.Columns["idsuivi"].Visible = false;
            dgvListeCmdDvd.Columns["realisateur"].Visible = false;
            dgvListeCmdDvd.Columns["synopsis"].Visible = false;
            dgvListeCmdDvd.Columns["duree"].Visible = false;
            dgvListeCmdDvd.Columns["genre"].Visible = false;
            dgvListeCmdDvd.Columns["rayon"].Visible = false;
            dgvListeCmdDvd.Columns["typepublic"].Visible = false;
            dgvListeCmdDvd.Columns["titre"].Visible = false;
            dgvListeCmdDvd.Columns["image"].Visible = false;

            dgvListeCmdDvd.Columns["montant"].HeaderText = "Montant (€)";
            dgvListeCmdDvd.Columns["nbexemplaire"].HeaderText = "Nombre d'exemplaire";
            dgvListeCmdDvd.Columns["libelle"].HeaderText = "Etat du Suivi";
            dgvListeCmdDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Affichage des informations du livre qui correspond à la commande sélectionné
        /// </summary>
        /// <param name="lesDvd"></param>
        private void AfficheDvdInfosCmd(CommandeDocumentDvd lesDvd)
        {
            txbDvdNumeroCmd.Text = lesDvd.IdLivredvd;
            txbDvdDureeCmd.Text = lesDvd.Duree.ToString();
            txbDvdTitreCmd.Text = lesDvd.Titre;
            txbDvdRealisateurCmd.Text = lesDvd.Realisateur;
            txbDvdSynopsisCmd.Text = lesDvd.Synopsis;
            txbDvdGenreCmd.Text = lesDvd.Genre;
            txbDvdPublicCmd.Text = lesDvd.Typepublic;
            txbDvdRayonCmd.Text = lesDvd.Rayon;
            txbDvdImageCmd.Text = lesDvd.Image;
            string image = lesDvd.Image;
            try
            {
                pcbLivresImageCmd.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImageCmd.Image = null;
            }
        }

        /// <summary>
        /// Recherche et affichage des commandes correspondant au livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRechercheCmd_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRechercheCmd.Text.Equals(""))
            {
                List<CommandeDocumentDvd> Dvd = lesCommandesDvd.FindAll(x => x.IdLivredvd.Equals(txbDvdNumRechercheCmd.Text));
                txbDvdNumRechercheCmd.Text = "";
                if (Dvd.Any())
                {
                    InitDataGridViewDvdRecherche(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    InitDataGridViewDvd();
                }
            }
            else
            {
                InitDataGridViewDvd();
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeCmdDvd_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvListeCmdDvd.CurrentCell != null)
            {
                try
                {
                    CommandeDocumentDvd Dvd = (CommandeDocumentDvd)bdgDvdListeCmd.List[bdgDvdListeCmd.Position];
                    AfficheDvdInfosCmd(Dvd);
                }
                catch
                {
                    VideDvdZonesCmd();
                }
            }
            else
            {
                VideDvdZonesCmd();
            }
        }

        /// <summary>
        /// Permet de disable tout les groupes de saisies
        /// </summary>
        public void DisableAddEditCmdDvd()
        {
            grpDvdCmdAjout.Enabled = false;
            grpDvdCmdModif.Enabled = false;
            AddCmdDvd = false;
            EditCmdDvd = false;
        }


        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZonesCmd()
        {
            txbDvdNumRechercheCmd.Text = "";
        }

        /// <summary>
        /// vide les zones de saisie d'une commande
        /// </summary>
        private void VideDvdAjoutCmd()
        {
            txbIdCmdAddDvd.Text = "";
            numMontantCmdAddDvd.Value = 0;
            numNbExemplaireCmdAddDvd.Value = 0;
            dtpDateCmdAddDvd.Value = DateTime.Today;
        }

        /// <summary>
        /// vide les zones de modification d'une commande
        /// </summary>
        private void ViderDvdCmdEdit()
        {
            txbIdCmdEditDvd.Text = "";
            cboSuivisDvd.SelectedIndex = 0;
        }

        /// <summary>
        /// Affiche les DVD dans les combobox d'ajout d'une commande
        /// </summary>
        public void RemplirCbDvd()
        {
            List<Dvd> dvd = controle.GetAllDvd();
            bdgDvdListe.DataSource = dvd;
            cboDvd.DataSource = bdgDvdListe;
            if (cboDvd.Items.Count > 0)
            {
                cboDvd.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Affiche les status dans les combobox de modification d'une commande
        /// </summary>
        public void RemplirCbSuiviDvd()
        {
            List<Suivi> suivi = controle.GetAllSuivis();
            bdgStatutListe.DataSource = suivi;
            cboSuivisDvd.DataSource = bdgStatutListe;
            if (cboSuivisDvd.Items.Count > 0)
            {
                cboSuivisDvd.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Affiche les valeurs dans les combobox de modification d'une commande
        /// </summary>
        /// <param name="dvd"></param>
        private void RemplirEditCmdDvd(CommandeDocumentDvd dvd)
        {
            txbIdCmdEditDvd.Text = dvd.Id;
            string libelle = dvd.Libelle;
            cboSuivisDvd.SelectedIndex = cboSuivisDvd.FindStringExact(libelle);

        }

        /// <summary>
        /// Ouvre et referme l'interface d'ajout d'une commande de Dvd
        /// </summary>
        private void btnDvdAjouterCmd_Click(object sender, EventArgs e)
        {
            grpDvdCmdAjout.Enabled = true;
            AddCmdDvd = true;

            if (EditCmdDvd)
            {
                grpDvdCmdModif.Enabled = false;
                EditCmdDvd = false;
                ViderDvdCmdEdit();
            }
        }

        /// <summary>
        /// Ouvre et referme l'interface de modification du statut d'une commande de Dvd
        /// </summary>
        private void btnDvdModifCmd_Click(object sender, EventArgs e)
        {
            CommandeDocumentDvd laCommande = (CommandeDocumentDvd)bdgDvdListeCmd.List[bdgDvdListeCmd.Position];

            if (laCommande.Libelle == "Réglée.")
            {
                MessageBox.Show("Cette commande est déja réglée, impossible de retourner en arrière", "Erreur");
                return;
            }

            grpDvdCmdModif.Enabled = true;
            EditCmdDvd = true;

            if (AddCmdDvd)
            {
                grpDvdCmdAjout.Enabled = false;
                AddCmdDvd = false;
                VideDvdAjoutCmd();
            }

            RemplirEditCmdDvd(laCommande);
        }

        /// <summary>
        /// Demande d'ajout d'une commande de Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCmdDvd_Click(object sender, EventArgs e)
        {
            // On regarde si l'id de la commande existe déjà si oui une erreur se produit
            string idCmdDvd = txbIdCmdAddDvd.Text;
            int LivreExiste = bdgLivresListeCmd.IndexOf(bdgLivresListeCmd.List.OfType<CommandeDocumentLivre>().ToList().Find(f => f.Id == idCmdDvd));
            int DVDExiste = bdgDvdListeCmd.IndexOf(bdgDvdListeCmd.List.OfType<CommandeDocumentDvd>().ToList().Find(f => f.Id == idCmdDvd));
            int RevueExiste = bdgRevueListeCmd.IndexOf(bdgRevueListeCmd.List.OfType<CommandeRevue>().ToList().Find(f => f.Id == idCmdDvd));
            if (LivreExiste != -1 || DVDExiste != -1 || RevueExiste != -1)
            {
                MessageBox.Show("L'Id de la commande existe déjà. Veuillez saisir une Id non existante.", "Erreur");
                return;
            }
            // Demande de l'ajout en fonction des informations entrées 
            if (!txbIdCmdAddDvd.Text.Equals("") && numMontantCmdAddDvd.Value > 0 && numNbExemplaireCmdAddDvd.Value >= 1 && MessageBox.Show("Etes vous sûr?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int Montant = (int)numMontantCmdAddDvd.Value;
                int NbExemplaire = (int)numNbExemplaireCmdAddDvd.Value;

                Commande commande = new Commande(txbIdCmdAddDvd.Text, dtpDateCmdAddDvd.Value, Montant);
                CommandeDocument commandedocument = new CommandeDocument(txbIdCmdAddDvd.Text, NbExemplaire, ((Dvd)bdgDvdListe.List[bdgDvdListe.Position]).Id);

                controle.AddCommande(commande);
                controle.AddCommandeDocument(commandedocument);

                InitDataGridViewDvd();
                VideDvdAjoutCmd();
                grpDvdCmdAjout.Enabled = false;
            }
            else
            {
                MessageBox.Show("Tous les champs doivent être remplis correctement.", "Information");
            }
        }

        /// <summary>
        /// Permet l'annulation de l'ajout d'une commande de dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCmdDvdAnnuler_Click(object sender, EventArgs e)
        {
            VideDvdAjoutCmd();
            grpDvdCmdAjout.Enabled = false;
        }

        /// <summary>
        /// Demande de modification du suivi d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCmdDvd_Click(object sender, EventArgs e)
        {
            Suivi suivi = (Suivi)bdgStatutListe.List[bdgStatutListe.Position];
            string suiviId = suivi.IdSuivi;
            string suiviLibelle = suivi.Libelle;
            CommandeDocumentDvd laCommande = (CommandeDocumentDvd)bdgDvdListeCmd.List[bdgDvdListeCmd.Position];
            if ((laCommande.Libelle == "Réglée." || laCommande.Libelle == "Livrée.") && ((suiviLibelle == "En cours.") || (suiviLibelle == "Relancée.")))
            {
                MessageBox.Show("La commande est dans un stade trop avancé pour revenir a cet état", "Erreur");
                RemplirEditCmdDvd(laCommande);
                return;
            }
            if (suiviLibelle == "Réglée." && laCommande.Libelle != "Livrée.")
            {
                MessageBox.Show("La commande ne peut etre réglée sans être livrée avant.", "Erreur");
                RemplirEditCmdDvd(laCommande);
                return;
            }
            if (MessageBox.Show("Etes vous sûr?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                controle.EditCommande(laCommande.Id, suiviId);
                InitDataGridViewDvd();
                ViderDvdCmdEdit();
                grpDvdCmdModif.Enabled = false;
            }
        }

        /// <summary>
        /// Permet l'annulation de la modification d'une commande de livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCmdDvdAnnuler_Click(object sender, EventArgs e)
        {
            ViderDvdCmdEdit();
            grpDvdCmdModif.Enabled = false;
        }

        /// <summary>
        /// Demande de suppression d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdSupprCmd_Click(object sender, EventArgs e)
        {
            CommandeDocumentDvd laCommande = (CommandeDocumentDvd)bdgDvdListeCmd.List[bdgDvdListeCmd.Position];
            if (laCommande.Libelle == "Livrée." || laCommande.Libelle == "Réglée.")
            {
                MessageBox.Show("La commande est dans un stade trop avancé pour être supprimée");
                return;
            }

            if (MessageBox.Show("Etes vous sûr?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                controle.DeleteCmd(laCommande.Id);
                InitDataGridViewDvd();
            }
        }

        /// <summary>
        /// Tri sur les colonnes de commandes de dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeCmdDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZonesCmd();
            string titreColonne = dgvListeCmdDvd.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocumentDvd> sortedList = new List<CommandeDocumentDvd>();
            lesCommandesDvd = controle.GetAllCommandesDvd();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandesDvd.OrderByDescending(o => o.Id).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandesDvd.OrderByDescending(o => o.DateCommande).ToList();
                    break;
                case "Montant (€)":
                    sortedList = lesCommandesDvd.OrderByDescending(o => o.Montant).ToList();
                    break;
                case "Nombre d'exemplaire":
                    sortedList = lesCommandesDvd.OrderByDescending(o => o.NbExemplaire).ToList();
                    break;
                case "Etat du Suivi":
                    sortedList = lesCommandesDvd.OrderByDescending(o => o.Libelle).ToList();
                    break;
            }
            InitDataGridViewDvdRecherche(sortedList);
        }
        #endregion

        #region Commandes de revues
        //-----------------------------------------------------------
        // ONGLET "COMMANDES DE REVUES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Commandes de Revues : 
        /// appel des méthodes pour remplir le datagrid des commandes de revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCmdRevues_Enter(object sender, EventArgs e)
        {
            lesCommandesRevues = controle.GetAllCommandesRevues();
            InitDataGridViewRevue();
            RemplirCbRevue();
            DisableAddCmdRevue();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void InitDataGridViewRevue()
        {
            List<CommandeRevue> revues = controle.GetAllCommandesRevues();
            bdgRevueListeCmd.DataSource = revues;
            dgvListeCmdRevue.DataSource = bdgRevueListeCmd;
            // dgvListeCmdRevue.Columns["id"].Visible = false;
            dgvListeCmdRevue.Columns["idrevue"].Visible = false;
            dgvListeCmdRevue.Columns["delaiMiseADispo"].Visible = false;
            dgvListeCmdRevue.Columns["periodicite"].Visible = false;
            dgvListeCmdRevue.Columns["genre"].Visible = false;
            dgvListeCmdRevue.Columns["rayon"].Visible = false;
            dgvListeCmdRevue.Columns["typepublic"].Visible = false;
            dgvListeCmdRevue.Columns["titre"].Visible = false;
            dgvListeCmdRevue.Columns["image"].Visible = false;
            dgvListeCmdRevue.Columns["empruntable"].Visible = false;

            dgvListeCmdRevue.Columns["montant"].HeaderText = "Montant (€)";
            dgvListeCmdRevue.Columns["datefinabo"].HeaderText = "DateFinAbonnement";
            dgvListeCmdRevue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Remplit le dategrid avec l'id d'une revue reçue en paramètre
        /// </summary>
        private void InitDataGridViewRevueRecherche(List<CommandeRevue> revues)
        {
            bdgRevueListeCmd.DataSource = revues;
            dgvListeCmdRevue.DataSource = bdgRevueListeCmd;
            // dgvListeCmdRevue.Columns["id"].Visible = false;
            dgvListeCmdRevue.Columns["idrevue"].Visible = false;
            dgvListeCmdRevue.Columns["delaiMiseADispo"].Visible = false;
            dgvListeCmdRevue.Columns["periodicite"].Visible = false;
            dgvListeCmdRevue.Columns["genre"].Visible = false;
            dgvListeCmdRevue.Columns["rayon"].Visible = false;
            dgvListeCmdRevue.Columns["typepublic"].Visible = false;
            dgvListeCmdRevue.Columns["titre"].Visible = false;
            dgvListeCmdRevue.Columns["image"].Visible = false;
            dgvListeCmdRevue.Columns["empruntable"].Visible = false;
            dgvListeCmdRevue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Affichage des informations de la revue qui correspond à la commande sélectionné
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheRevuesInfosCmd(CommandeRevue revue)
        {
            txbRevueNumeroCmd.Text = revue.IdRevue;
            chBxEmprunt.Checked = revue.Empruntable;
            txbRevueTitreCmd.Text = revue.Titre;
            txbRevuePeriodCmd.Text = revue.Periodicite;
            txbRevueDelaiCmd.Text = revue.DelaiMiseADispo.ToString();
            txbRevueGenreCmd.Text = revue.Genre;
            txbRevuePublicCmd.Text = revue.Typepublic;
            txbRevueRayonCmd.Text = revue.Rayon;
            txbRevueImageCmd.Text = revue.Image;
            string image = revue.Image;
            try
            {
                pcbRevueImageCmd.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevueImageCmd.Image = null;
            }
        }

        /// <summary>
        /// Recherche et affichage des commandes correspondant à la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevueNumRechercheCmd_Click(object sender, EventArgs e)
        {
            if (!txbRevueNumRechercheCmd.Text.Equals(""))
            {
                List<CommandeRevue> Revues = lesCommandesRevues.FindAll(x => x.IdRevue.Equals(txbRevueNumRechercheCmd.Text));
                txbRevueNumRechercheCmd.Text = "";
                if (Revues.Any())
                {
                    InitDataGridViewRevueRecherche(Revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    InitDataGridViewRevue();
                }
            }
            else
            {
                InitDataGridViewRevue();
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeCmdRevue_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvListeCmdRevue.CurrentCell != null)
            {
                try
                {
                    CommandeRevue Revues = (CommandeRevue)bdgRevueListeCmd.List[bdgRevueListeCmd.Position];
                    AfficheRevuesInfosCmd(Revues);
                }
                catch
                {
                    VideRevueZonesCmd();
                }
            }
            else
            {
                VideRevueZonesCmd();
            }
        }

        /// <summary>
        /// Permet de disable tout les groupes de saisies
        /// </summary>
        public void DisableAddCmdRevue()
        {
            grpRevueCmdAjout.Enabled = false;
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevueZonesCmd()
        {
            txbRevueNumRechercheCmd.Text = "";
        }

        /// <summary>
        /// vide les zones de saisie d'une commande
        /// </summary>
        private void VideRevueAjoutCmd()
        {
            txbIdCmdAddRevue.Text = "";
            numMontantCmdAddRevue.Value = 0;
            dtpDateCmdAddRevue.Value = DateTime.Today;
            dtpDateFinCmdAddRevue.Value = DateTime.Today;
        }

        /// <summary>
        /// Affiche les revues dans les combobox d'ajout d'une commande
        /// </summary>
        public void RemplirCbRevue()
        {
            List<Revue> revue = controle.GetAllRevues();
            bdgRevuesListe.DataSource = revue;
            cboRevue.DataSource = bdgRevuesListe;
            if (cboRevue.Items.Count > 0)
            {
                cboRevue.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Ouvre et referme l'interface d'ajout d'une commande de revue
        /// </summary>
        private void btnRevueAjouterCmd_Click(object sender, EventArgs e)
        {
            grpRevueCmdAjout.Enabled = true;
        }

        /// <summary>
        /// Demande d'ajout d'une commande de revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCmdRevue_Click(object sender, EventArgs e)
        {
            // Si la date de début est supérieur ou égale à la date de fin une erreur se produit
            if (dtpDateCmdAddRevue.Value >= dtpDateFinCmdAddRevue.Value)
            {
                MessageBox.Show("La date de début de début d'un abonnement ne peut pas être supérieur à la date de fin de celui-ci.", "Erreur");
                return;
            }
            // On regarde si l'id de la commande existe déjà si oui une erreur se produit
            string idCmdRevue = txbIdCmdAddRevue.Text;
            int LivreExiste = bdgLivresListeCmd.IndexOf(bdgLivresListeCmd.List.OfType<CommandeDocumentLivre>().ToList().Find(f => f.Id == idCmdRevue));
            int DVDExiste = bdgDvdListeCmd.IndexOf(bdgDvdListeCmd.List.OfType<CommandeDocumentDvd>().ToList().Find(f => f.Id == idCmdRevue));
            int RevueExiste = bdgRevueListeCmd.IndexOf(bdgRevueListeCmd.List.OfType<CommandeRevue>().ToList().Find(f => f.Id == idCmdRevue));
            if (LivreExiste != -1 || DVDExiste != -1 || RevueExiste != -1)
            {
                MessageBox.Show("L'Id de la commande existe déjà. Veuillez saisir une Id non existante.", "Erreur");
                return;
            }
            // Demande de l'ajout en fonction des informations entrées 
            if (!txbIdCmdAddRevue.Text.Equals("") && numMontantCmdAddRevue.Value > 0 && MessageBox.Show("Etes vous sûr?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int Montant = (int)numMontantCmdAddRevue.Value;

                Commande commande = new Commande(txbIdCmdAddRevue.Text, dtpDateCmdAddRevue.Value, Montant);
                Abonnement abonnement = new Abonnement(txbIdCmdAddRevue.Text, dtpDateFinCmdAddRevue.Value, ((Revue)bdgRevuesListe.List[bdgRevuesListe.Position]).Id);

                controle.AddCommande(commande);
                controle.AddAbonnementRevue(abonnement);

                InitDataGridViewRevue();
                VideRevueAjoutCmd();
                grpRevueCmdAjout.Enabled = false;
            }
            else
            {
                MessageBox.Show("Tous les champs doivent être remplis correctement.", "Information");
            }
        }

        /// <summary>
        /// Permet l'annulation de l'ajout d'une commande de revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCmdRevueAnnuler_Click(object sender, EventArgs e)
        {
            VideRevueAjoutCmd();
            grpRevueCmdAjout.Enabled = false;
        }

        /// <summary>
        /// Demande de suppression d'une commande de revue si il n'y a pas de date d'achat d'un exemplaire qui se situe entre les dates d'achats et de fin d'abonnements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevueSupprCmd_Click(object sender, EventArgs e)
        {
            CommandeRevue abonnement = (CommandeRevue)bdgRevueListeCmd.List[bdgRevueListeCmd.Position];
            List<Exemplaire> exemplaires = controle.GetExemplairesRevue(abonnement.IdRevue);
            bool ExemplaireExiste = false;
            // Contrôle les dates (début,fin) d'abonnement et de l'exemplaire (achat) pour voir si une ou plusieurs coincident
            foreach (var exemplaire in exemplaires.Where(x => ParutionDansAbonnement(abonnement.DateCommande, abonnement.DateFinAbo, x.DateAchat)))
            {
                   ExemplaireExiste = true;          
            }
            // Si date de l'exemplaire présent dans les dates de l'abonnement
            if (ExemplaireExiste)
            {
                MessageBox.Show("Au moins un exemplaire existe pendant la durée de cet abonnement, la suppression est donc impossible.", "Erreur");
                return;
            }
            // Demande de suppression si validation
            if (MessageBox.Show("Etes vous sûr?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                controle.DeleteAbonnement(abonnement.Id);
                InitDataGridViewRevue();
            }
        }

        /// <summary>
        /// retourne vrai si la date de parution est entre les 2 autres dates
        /// </summary>
        /// <param name="dateCommande"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="dateParution"></param>
        /// <returns></returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return (dateParution > dateCommande && dateParution < dateFinAbonnement);
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvListeCmdRevue_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevueZonesCmd();
            string titreColonne = dgvListeCmdRevue.Columns[e.ColumnIndex].HeaderText;
            List<CommandeRevue> sortedList = new List<CommandeRevue>();
            lesCommandesRevues = controle.GetAllCommandesRevues();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandesRevues.OrderByDescending(o => o.Id).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandesRevues.OrderByDescending(o => o.DateCommande).ToList();
                    break;
                case "Montant (€)":
                    sortedList = lesCommandesRevues.OrderByDescending(o => o.Montant).ToList();
                    break;
                case "DateFinAbonnement":
                    sortedList = lesCommandesRevues.OrderByDescending(o => o.DateFinAbo).ToList();
                    break;
            }
            InitDataGridViewRevueRecherche(sortedList);
        }
        #endregion
    }
}