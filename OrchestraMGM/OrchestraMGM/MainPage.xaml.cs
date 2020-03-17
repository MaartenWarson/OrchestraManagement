using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using OrchestraMGM_Data;
using OrchestraMGM_Business;

namespace OrchestraMGM
{
    public sealed partial class MainPage : Page
    {
        private Musician selectedMusician;
        private int selectedColumn = 1;
        private long id = 0;
        private string voornaam, naam, adres, gemeente, instrument;
        private int postcode;

        public MainPage()
        {
            this.InitializeComponent();

            // Database wordt aangemaakt (als deze nog niet bestaat)
            DataAccess.InitializeDatabase();

            ShowDatabase();
        }

        private void AddData(object sender, RoutedEventArgs e)
        {
            voornaam = txtVoornaam.Text;
            naam = txtNaam.Text;
            adres = txtAdres.Text;
            if (txtPostcode.Text == "") // 'Indekken' als postcode niet wordt ingevuld
            {
                postcode = 0;   
            }
            else
            {
                postcode = Convert.ToInt32(txtPostcode.Text);
            }
            gemeente = txtGemeente.Text;
            instrument = txtInstrument.Text;

            DataAccess.AddData(voornaam, naam, adres, postcode, gemeente, instrument);

            ClearTextBoxes();
            ShowDatabase();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            voornaam = txtVoornaam.Text;
            naam = txtNaam.Text;
            adres = txtAdres.Text;
            postcode = Convert.ToInt32(txtPostcode.Text);
            gemeente = txtGemeente.Text;
            instrument = txtInstrument.Text;

            DataAccess.UpdateData(id, voornaam, naam, adres, postcode, gemeente, instrument);

            ClearTextBoxes();
            ShowDatabase();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.DeleteData(id);

            ClearTextBoxes();
            ShowDatabase();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBoxes();

            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;

            listviewOutput.SelectedItem = null;
            sortId.IsChecked = true;
        }

        private void ShowDatabase()
        {
            selectedColumn = GetColumnSortedBy();

            listviewOutput.SelectedItem = null;
            listviewOutput.ItemsSource = DataAccess.GetData(selectedColumn);
        }

        private void ListviewOutput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listviewOutput.SelectedItem != null) // Als IETS geselecteerd is
            {
                selectedMusician = (Musician)listviewOutput.SelectedItem;

                id = selectedMusician.Id;
                txtVoornaam.Text = selectedMusician.Voornaam;
                txtNaam.Text = selectedMusician.Naam;
                txtAdres.Text = selectedMusician.Adres;
                txtPostcode.Text = Convert.ToString(selectedMusician.Postcode);
                txtGemeente.Text = selectedMusician.Gemeente;
                txtInstrument.Text = selectedMusician.Instrument;

                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnAdd.IsEnabled = false;
            }
            else
            {
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void ClearTextBoxes()
        {
            txtVoornaam.Text = "";
            txtNaam.Text = "";
            txtAdres.Text = "";
            txtPostcode.Text = "";
            txtGemeente.Text = "";
            txtInstrument.Text = "";
        }

        private void TxtVoornaam_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTextBoxes();
        }

        private void TxtNaam_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTextBoxes();
        }

        private void TxtAdres_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTextBoxes();
        }

        private void TxtPostcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTextBoxes();
        }

        private void TxtGemeente_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTextBoxes();
        }

        private void TxtInstrument_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTextBoxes();
        }

        private void CheckTextBoxes()
        {
            if (txtVoornaam.Text.Trim() != "" && txtNaam.Text.Trim() != "" && listviewOutput.SelectedItem == null)
            {
                btnAdd.IsEnabled = true;
            }
            else
            {
                btnAdd.IsEnabled = false;
            }
        }

        private int GetColumnSortedBy()
        {
            if (sortId.IsChecked == true)
            {
                return 1;
            }

            if (sortVoornaam.IsChecked == true)
            {
                return 2;
            }

            if (sortNaam.IsChecked == true)
            {
               return 3;
            }

            if (sortAdres.IsChecked == true)
            {
                return 4;
            }

            if (sortPostcode.IsChecked == true)
            {
                return 5;
            }

            if (sortGemeente.IsChecked == true)
            {
                return 6;
            }

            if (sortInstrument.IsChecked == true)
            {
                return 7;
            }
            else
            {
                return 1;
            }
        }

        private void SortId_Checked(object sender, RoutedEventArgs e)
        {
            ShowDatabase();
        }

        private void SortVoornaam_Checked(object sender, RoutedEventArgs e)
        {
            ShowDatabase();
        }

        private void SortNaam_Click(object sender, RoutedEventArgs e)
        {
            ShowDatabase();
        }

        private void SortAdres_Click(object sender, RoutedEventArgs e)
        {
            ShowDatabase();
        }

        private void SortPostcode_Click(object sender, RoutedEventArgs e)
        {
            ShowDatabase();
        }

        private void SortGemeente_Click(object sender, RoutedEventArgs e)
        {
            ShowDatabase();
        }

        private void SortInstrument_Click(object sender, RoutedEventArgs e)
        {
            ShowDatabase();
        }
    }
}
