using Emoji.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OfficeOpenXml;

namespace GestionMedecin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void AddConsultation_Click(object sender, RoutedEventArgs e)
        {
            // Instanciez votre nouvelle fenêtre
            WinConsultation nouvelleFenetre = new WinConsultation();

            // Affichez la nouvelle fenêtre
            nouvelleFenetre.Show();
        }

            private void BtnAdd_Click(object sender, RoutedEventArgs e)
            {
                // Vérifier si tous les TextBox sont remplis
                if (string.IsNullOrWhiteSpace(tbIdSpe.Text) || string.IsNullOrWhiteSpace(tbNameSpe.Text) || string.IsNullOrWhiteSpace(tbDescriptionSpe.Text))
                {
                    MessageBox.Show("Veuillez remplir tous les champs avant d'ajouter une nouvelle spécialité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vérifier si l'ID respecte la norme d'écriture
                if (!System.Text.RegularExpressions.Regex.IsMatch(tbIdSpe.Text, @"^[a-zA-Z0-9]+$"))
                {
                    MessageBox.Show("L'ID doit contenir uniquement des lettres et des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vérifier si l'ID a 5 caractères
                if (tbIdSpe.Text.Length != 5)
                {
                    MessageBox.Show("L'ID doit être composé de 5 caractères.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vérifier si le nom respecte la norme d'écriture
                if (!System.Text.RegularExpressions.Regex.IsMatch(tbNameSpe.Text, @"^[a-zA-Z\s]+$"))
                {
                    MessageBox.Show("Le nom doit contenir uniquement des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vérifier si la description respecte la norme d'écriture
                if (tbDescriptionSpe.Text.Length > 200)
                {
                    MessageBox.Show("La description ne doit pas dépasser 200 caractères.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Créer un nouvel objet avec les informations de vos TextBox
                var nouvelElement = new { IdSpe = tbIdSpe.Text, NameSpe = tbNameSpe.Text, DescriptionSpe = tbDescriptionSpe.Text };

                // Vérifier si un élément avec le même ID existe déjà
                foreach (var item in dgSimple.Items)
                {
                    if (((dynamic)item).IdSpe == tbIdSpe.Text)
                    {
                        // Afficher une boîte de dialogue demandant à l'utilisateur s'il est sûr de vouloir remplacer l'élément existant
                        MessageBoxResult result = MessageBox.Show("Un élément avec cet ID existe déjà. Êtes-vous sûr de vouloir le remplacer ?", "Confirmation", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            // Supprimer l'élément existant
                            dgSimple.Items.Remove(item);

                            // Sortir de la boucle
                            break;
                        }
                        else
                        {
                            // Si l'utilisateur choisit "Non", sortir de la méthode sans ajouter ni supprimer aucun élément
                            return;
                        }
                    }
                }

                // Ajouter l'objet à votre DataGrid
                dgSimple.Items.Add(nouvelElement);

            // Ajouter le nom de la nouvelle spécialité au ComboBox
            CbSpe.Items.Add(tbNameSpe.Text);

                // Trier les éléments dans le ComboBox par ordre alphabétique
                CbSpe.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));

                // Effacer les TextBox pour la prochaine entrée
                tbIdSpe.Clear();
                tbNameSpe.Clear();
                tbDescriptionSpe.Clear();
            }
        



        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si une ligne est sélectionnée
            if (dgSimple.SelectedItem != null)
            {
                // Obtenir l'élément sélectionné et le convertir en objet dynamique
                dynamic item = dgSimple.SelectedItem;

                // Trouver l'index de l'ancien nom dans le ComboBox
                int index = CbSpe.Items.IndexOf(item.NameSpe);
                if (index != -1)
                {
                    
                    // Supprimer l'ancien nom du ComboBox
                    CbSpe.Items.RemoveAt(index);

                    // Ajouter le nouveau nom au ComboBox
                    CbSpe.Items.Add(tbNameSpe.Text);

                }

                // Remplir les TextBox avec les informations de l'élément sélectionné
                tbIdSpe.Text = item.IdSpe;
                tbNameSpe.Text = item.NameSpe;
                tbDescriptionSpe.Text = item.DescriptionSpe;

                // Rafraîchir la DataGrid
                dgSimple.Items.Refresh();
            }
            // Parcourir tous les éléments du ComboBox
            for (int i = CbSpe.Items.Count - 1; i >= 0; i--)
            {
                // Si l'élément est vide, le supprimer
                if (string.IsNullOrWhiteSpace(CbSpe.Items[i].ToString()))
                {
                    CbSpe.Items.RemoveAt(i);
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si une ligne est sélectionnée
            if (dgSimple.SelectedItem != null)
            {
                // Afficher une boîte de dialogue de confirmation avant de supprimer l'élément
                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer cette spécialité ?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Obtenir l'élément sélectionné et le convertir en objet dynamique
                    dynamic item = dgSimple.SelectedItem;

                    // Supprimer l'élément sélectionné de la DataGrid
                    dgSimple.Items.Remove(dgSimple.SelectedItem);

                    // Trouver l'index de l'ancien nom dans le ComboBox
                    int index = CbSpe.Items.IndexOf(item.NameSpe);
                    if (index != -1)
                    {
                        // Supprimer l'ancien nom du ComboBox
                        CbSpe.Items.RemoveAt(index);
                    }
                }
            }
            // Parcourir tous les éléments du ComboBox
            for (int i = CbSpe.Items.Count - 1; i >= 0; i--)
            {
                // Si l'élément est vide, le supprimer
                if (string.IsNullOrWhiteSpace(CbSpe.Items[i].ToString()))
                {
                    CbSpe.Items.RemoveAt(i);
                }
            }

        }

        private void BtnAdd2_Click(object sender, RoutedEventArgs e)
        {
            // Vérifiez si un élément est sélectionné dans le ComboBox
            if (CbSpe.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une spécialité avant d'ajouter un nouveau médecin.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Récupérez l'élément sélectionné
            var specialite = CbSpe.SelectedItem.ToString();

            // Vérifier si l'ID respecte la norme d'écriture
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbIdM.Text, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("L'ID doit contenir uniquement des lettres et des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérifier si l'ID a 5 caractères
            if (tbIdM.Text.Length != 5)
            {
                MessageBox.Show("L'ID doit être composé de 5 caractères.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérifiez si le nom respecte la norme d'écriture
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbNameM.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Le nom doit contenir uniquement des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérifiez si le prénom respecte la norme d'écriture
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbPrenom.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Le prénom doit contenir uniquement des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérifiez si le téléphone respecte la norme d'écriture
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbTel.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("Le numéro de téléphone doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérifier si le telephone a 10 caractères
            if (tbTel.Text.Length != 10)
            {
                MessageBox.Show("Le telephone doit être composé de 10 chiffrees.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } 

            // Vérifiez si le salaire respecte la norme d'écriture
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbSalaire.Text, @"^[0-9]+(\.[0-9]{1,2})?$"))
            {
                MessageBox.Show("Le salaire doit être un nombre positif avec au plus deux chiffres après la virgule.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Créez un nouvel objet avec les informations de vos TextBoxes et le ComboBox
            var nouvelElement = new { IdMed = tbIdM.Text, NameMed = tbNameM.Text, PrenomMed = tbPrenom.Text, SpeMed = specialite, TelMed = tbTel.Text, SalaireMed = tbSalaire.Text };

            // Vérifiez si un élément avec le même ID existe déjà
            foreach (var item in dgSimple2.Items)
            {
                if (((dynamic)item).IdMed == tbIdM.Text)
                {
                    // Affichez une boîte de dialogue demandant à l'utilisateur s'il est sûr de vouloir remplacer l'élément existant
                    MessageBoxResult result = MessageBox.Show("Un élément avec cet ID existe déjà. Êtes-vous sûr de vouloir le remplacer ?", "Confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        // Supprimez l'élément existant
                        dgSimple2.Items.Remove(item);

                        // Sortez de la boucle
                        break;
                    }
                    else
                    {
                        // Si l'utilisateur choisit "Non", sortez de la méthode sans ajouter ni supprimer aucun élément
                        return;
                    }
                }
            }

            // Ajoutez le nouvel élément à la DataGrid
            dgSimple2.Items.Add(nouvelElement);

            // Effacez les TextBox pour la prochaine entrée
            tbIdM.Clear();
            tbNameM.Clear();
            tbPrenom.Clear();
            CbSpe.SelectedIndex = -1; // Réinitialisez le ComboBox
            tbTel.Clear();
            tbSalaire.Clear();
        }

        private void BtnUpdate2_Click(object sender, RoutedEventArgs e)
        {
            // Vérifiez si une ligne est sélectionnée
            if (dgSimple2.SelectedItem != null)
            {
                // Obtenez l'élément sélectionné et convertissez-le en objet dynamique
                dynamic item = dgSimple2.SelectedItem;

                // Remplissez les TextBox avec les informations de l'élément sélectionné
                tbIdM.Text = item.IdM;
                tbNameM.Text = item.NameM;
                tbPrenom.Text = item.PrenomMed;
                tbTel.Text = item.TelMed;
                tbSalaire.Text = item.SalaireMed;

                // Trouvez l'index de l'ancienne spécialité dans le ComboBox
                int index = CbSpe.Items.IndexOf(item.SpeMed);
                if (index != -1)
                {
                    // Sélectionnez l'ancienne spécialité dans le ComboBox
                    CbSpe.SelectedIndex = index;
                }

                // Rafraîchissez la DataGrid
                dgSimple2.Items.Refresh();
            }
        }

        private void BtnDelete2_Click(object sender, RoutedEventArgs e)
        {
            // Vérifiez si une ligne est sélectionnée
            if (dgSimple2.SelectedItem != null)
            {
                // Affichez une boîte de dialogue de confirmation avant de supprimer l'élément
                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce médecin ?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Supprimez l'élément sélectionné de la DataGrid
                    dgSimple2.Items.Remove(dgSimple2.SelectedItem);
                }
            }
        }

    }
}
