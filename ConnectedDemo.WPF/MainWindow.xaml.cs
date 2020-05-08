using System;
using System.Collections.Generic;
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
using ConnectedDemo.LIB.Entities;
using ConnectedDemo.LIB.Services;

namespace ConnectedDemo.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        bool isNieuw;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VulAdresSoorten();
            VulAdressen();
            MaakLeeg();
            grpAdressen.IsEnabled = true;
            grpGegevens.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;

        }
        private void MaakLeeg()
        {
            txtNaam.Text = "";
            txtAdres.Text = "";
            txtPost.Text = "";
            txtGemeente.Text = "";
            txtLand.Text = "";
            cmbSoorten.SelectedIndex = -1;

        }
        private void VulAdresSoorten()
        {
            cmbSoorten.ItemsSource = null;
            cmbSoorten.Items.Clear();

            cmbSoorten.ItemsSource = DBAddressType.GetAdressTypes();
        }
        private void VulAdressen()
        {
            lstAdressen.ItemsSource = null;
            lstAdressen.Items.Clear();

            lstAdressen.ItemsSource = DBAddress.GetAdresses();
        }
        private void lstAdressen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MaakLeeg();
            if (lstAdressen.SelectedIndex == -1) return;

            Address address = (Address)lstAdressen.SelectedItem;
            txtNaam.Text = address.Naam;
            txtAdres.Text = address.Adres;
            txtPost.Text = address.Post;
            txtGemeente.Text = address.Gemeente;
            txtLand.Text = address.Land;
            int indeks = 0;
            foreach (AddressType addressType in cmbSoorten.Items)
            {
                if (addressType.ID == address.Soort_ID)
                {
                    cmbSoorten.SelectedIndex = indeks;
                    break;
                }
                indeks++;
            }
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            isNieuw = true;
            grpAdressen.IsEnabled = false;
            grpGegevens.IsEnabled = true;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
            MaakLeeg();
            txtNaam.Focus();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstAdressen.SelectedIndex == -1) return;
            isNieuw = false;
            grpAdressen.IsEnabled = false;
            grpGegevens.IsEnabled = true;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
            txtNaam.Focus();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstAdressen.SelectedIndex == -1) return;
            if(MessageBox.Show("Zeker?","Adres wissen",MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                DBAddress.DeleteAddress((Address)lstAdressen.SelectedItem);
                MaakLeeg();
                VulAdressen();

            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text.Trim() == "")
            {
                MessageBox.Show("Naam invoeren !", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNaam.Focus();
                return;
            }
            if (cmbSoorten.SelectedIndex == -1)
            {
                MessageBox.Show("Adressoort selecteren !", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbSoorten.Focus();
                return;

            }
            Address address;
            if (isNieuw)
            {
                address = new Address();
                address.ID = Guid.NewGuid().ToString();
            }
            else
            {
                address = (Address)lstAdressen.SelectedItem;
            }
            address.Naam = txtNaam.Text;
            address.Adres = txtAdres.Text;
            address.Post = txtPost.Text;
            address.Gemeente = txtGemeente.Text;
            address.Land = txtLand.Text;

            AddressType addressType = (AddressType)cmbSoorten.SelectedItem;
            address.Soort_ID = addressType.ID;

            bool gelukt;
            if (isNieuw)
                gelukt = DBAddress.SaveNewAddress(address);
            else
                gelukt = DBAddress.UpdateAddress(address);

            if (!gelukt)
            {
                MessageBox.Show("Oeps ... something went wrong ... !", "DB ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            grpAdressen.IsEnabled = true;
            grpGegevens.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;

            VulAdressen();

            int indeks = 0;
            foreach (Address zoekadres in lstAdressen.Items)
            {
                if (zoekadres == address)
                {
                    lstAdressen.SelectedIndex = indeks;
                    break;
                }
                indeks++;

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            grpAdressen.IsEnabled = true;
            grpGegevens.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            lstAdressen_SelectionChanged(null, null);
            lstAdressen.Focus();

        }

        private void btnSoorten_Click(object sender, RoutedEventArgs e)
        {
            winSoorten win = new winSoorten();
            win.ShowDialog();
            if(win.reload)
            {
                VulAdresSoorten();
            }
        }
    }
}
