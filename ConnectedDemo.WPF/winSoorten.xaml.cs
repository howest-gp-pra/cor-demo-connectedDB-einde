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
using System.Windows.Shapes;
using ConnectedDemo.LIB.Entities;
using ConnectedDemo.LIB.Services;

namespace ConnectedDemo.WPF
{
    /// <summary>
    /// Interaction logic for winSoorten.xaml
    /// </summary>
    public partial class winSoorten : Window
    {
        public bool reload = false;
        public winSoorten()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VulLstSoorten();
        }
        private void VulLstSoorten()
        {
            txtEdit.Text = "";
            txtNew.Text = "";
            lstSoorten.ItemsSource = null;
            lstSoorten.Items.Clear();
            lstSoorten.ItemsSource = DBAddressType.GetAdressTypes();

        }
        private void lstSoorten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtEdit.Text = "";
            txtNew.Text = "";
            if (lstSoorten.SelectedIndex == -1) return;

            txtEdit.Text = ((AddressType)lstSoorten.SelectedItem).Soort;


        }

        private void btnSaveNew_Click(object sender, RoutedEventArgs e)
        {
            if(txtNew.Text.Trim() == "")
            {
                txtNew.Focus();
                return;
            }
            AddressType addressType = new AddressType();
            addressType.ID = Guid.NewGuid().ToString();
            addressType.Soort = txtNew.Text.Trim();
            DBAddressType.SaveNewAddressType(addressType);
            VulLstSoorten();
            reload = true;

        }

        private void btnSaveCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (lstSoorten.SelectedIndex == -1) return;
            if (txtEdit.Text.Trim() == "")
            {
                txtEdit.Focus();
                return;
            }
            AddressType addressType = ((AddressType)lstSoorten.SelectedItem);
            DBAddressType.UpdateAddressType(addressType);
            VulLstSoorten();
            reload = true;
        }

        private void btnDeleteCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (lstSoorten.SelectedIndex == -1) return;
            AddressType addressType = ((AddressType)lstSoorten.SelectedItem);

            if (DBAddressType.IsAddressTypeInUse(addressType))
                MessageBox.Show("Deze soort is in gebruik en kan niet verwijderd worden", "Fout");
            else
            {
                DBAddressType.DeleteAddressType(addressType);
                VulLstSoorten();
                reload = true;
            }
        }
    }
}
