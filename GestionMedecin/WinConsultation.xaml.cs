using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestionMedecin
{
    /// <summary>
    /// Logique d'interaction pour WinConsultation.xaml
    /// </summary>
    public partial class WinConsultation : Window
    {
        private ObservableCollection<PersonneInfo> listePersonnes = new ObservableCollection<PersonneInfo>();
        public WinConsultation()
        {
            InitializeComponent();
            // Ajoutez des gestionnaires d'événements pour les boutons
            BtnAccueil.Click += BtnAccueil_Click;
            BtnSuivant.Click += BtnSuivant_Click;
            BtnPre.Click += BtnPre_Click;
            BtnSuivant2.Click += BtnSuivant2_Click;
            BtnPre2.Click += BtnPre2_Click;
            BtnTerminer.Click += BtnTerminer_Click;
        }

        private void BtnAccueil_Click(object sender, RoutedEventArgs e)
        {
            // Instancier la fenêtre MainWindow
            MainWindow mainWindow = new MainWindow();

            // Afficher la fenêtre MainWindow
            mainWindow.Show();

            // Fermer la fenêtre actuelle (WinConsultation)
            this.Close();
        }

        private void BtnSuivant_Click(object sender, RoutedEventArgs e)
        {
            // Parcourir les TabItems du MainTabControl
            foreach (TabItem tabItem in MainTabControl.Items)
            {
                // Vérifier si le Header du TabItem est "Coordonne"
                if (tabItem.Header.ToString() == "Coordonne")
                {
                    // Sélectionner le TabItem "Coordonne"
                    tabItem.IsSelected = true;
                    break;
                }
            }
        }



        private void BtnPre_Click(object sender, RoutedEventArgs e)
        {
            // Parcourir les TabItems du TabControl
            foreach (TabItem tabItem in MainTabControl.Items)
            {
                // Vérifier si le Header du TabItem est "information personnel"
                if (tabItem.Header.ToString() == "information personnel")
                {
                    // Sélectionner le TabItem "information personnel"
                    tabItem.IsSelected = true;
                    break;
                }
            }
        }

        private void BtnSuivant2_Click(object sender, RoutedEventArgs e)
        {
            // Parcourir les TabItems du TabControl
            foreach (TabItem tabItem in MainTabControl.Items)
            {
                // Vérifier si le Header du TabItem est "confirmation"
                if (tabItem.Header.ToString() == "confirmation")
                {
                    // Sélectionner le TabItem "confirmation"
                    tabItem.IsSelected = true;
                    break;
                }
            }
        }

        private void BtnPre2_Click(object sender, RoutedEventArgs e)
        {
            // Parcourir les TabItems du TabControl
            foreach (TabItem tabItem in MainTabControl.Items)
            {
                // Vérifier si le Header du TabItem est "Coordonne"
                if (tabItem.Header.ToString() == "Coordonne")
                {
                    // Sélectionner le TabItem "Coordonne"
                    tabItem.IsSelected = true;
                    break;
                }
            }
        }

        private List<PersonneInfo> personneInfos = new List<PersonneInfo>();

        private void BtnTerminer_Click(object sender, RoutedEventArgs e)
        {
            // Créer une instance de PersonneInfo à partir des données des TextBox
            PersonneInfo personne = new PersonneInfo
            {
                Nom = tbNomP.Text,
                Prenom = tbPrenomP.Text,
                LieuNaissance = tbLieuP.Text,
                DateNaissance = DpDateP.SelectedDate,
                Adresse = TbAdresse.Text,
                Email = tbMail.Text,
                Telephone = tbPhone.Text,
                Specialite = Cbspe2.Text,
                Medecin = TbMedecin2.Text
            };

            // Ajouter l'instance à la liste
            listePersonnes.Add(personne);

            // Lier le DataGrid à la liste
            dgSimple3.ItemsSource = listePersonnes;

            // Vider tous les TextBox et réinitialiser les contrôles
            ResetControls();
        }

        private void ResetControls()
        {
            tbNomP.Clear();
            tbPrenomP.Clear();
            tbLieuP.Clear();
            DpDateP.SelectedDate = null;
            TbAdresse.Clear();
            tbMail.Clear();
            tbPhone.Clear();
            Cbspe2.Text = "";
            TbMedecin2.Clear();

            // Sélectionner le premier onglet
            MainTabControl.SelectedItem = MainTabControl.Items[0];
        }
        public class PersonneInfo
        {
            public string? Nom { get; set; }
            public string? Prenom { get; set; }
            public string? LieuNaissance { get; set; }
            public DateTime? DateNaissance { get; set; }
            public string? Adresse { get; set; }
            public string? Email { get; set; }
            public string? Telephone { get; set; }
            public string? Specialite { get; set; }
            public string? Medecin { get; set; }
        }
    }
}