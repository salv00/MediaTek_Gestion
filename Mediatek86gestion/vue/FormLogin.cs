using System;
using System.Windows.Forms;
using Mediatek86.controleur;
using Mediatek86.metier;

namespace Mediatek86.vue
{
    /// <summary>
    /// Fenêtre de login
    /// </summary>
    public partial class frmLogin : Form
    {
        /// <summary>
        /// Instance du controleur
        /// </summary>
        private readonly Controle controle;

        /// <summary>
        /// Initialisation des composants graphiques
        /// </summary>
        /// <param name="controle"></param>
        internal frmLogin(Controle controle)
        {
            InitializeComponent();
            this.controle = controle;
        }

        /// <summary>
        /// Demande de connexion 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text.Equals("") || txtPwd.Text.Equals(""))
            {
                MessageBox.Show("Tous les champs doivent etre remplis", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Service service = controle.ControleAuthentification(txtLogin.Text, txtPwd.Text);
            if (service == null)
            {
                MessageBox.Show("Les identifiants saisis sont incorrects", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLogin.Text = "";
                txtPwd.Text = "";
                txtLogin.Focus();
                return;
            }
            if (service.ServiceInt == 3) //Si Service = Culture
            {
                DialogResult result = MessageBox.Show("Le service Culture n'as pas les autorisations suffisantes pour utiliser l'application", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
            this.Hide();
            (new FrmMediatek(controle, service)).ShowDialog();
        }
    }
}
