using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Configuration;
using TrackerLibrary.Interfaces;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePriceForm : Form
    {
        private IPriceRequester callingForm;

        public CreatePriceForm(IPriceRequester caller)
        {
            InitializeComponent();

            callingForm = caller;

        }

        private void createPriceButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PriceModel model = new PriceModel(
                    placeNumberValue.Text,
                    placeNameValue.Text,                    
                    priceAmountValue.Text,
                    pricePercentageValue.Text);

                // Save into SQL and Text files
                GlobalConfig.connection.CreatePrice(model);

                callingForm.PriceComplete(model);

                this.Close();

                // Reset Form
                //placeNameValue.Text = "";
                //placeNumberValue.Text = "";
                //priceAmountValue.Text = "0";
                //pricePercentageValue.Text = "0";
            }
            else
            {
                MessageBox.Show("This form has invalid information. Try Again.");
            }
        }

        private bool ValidateForm()
        {
            bool output = true;

            int placeNumber = 0;
            decimal priceAmount = 0;
            int pricePercentage = 0;

            if (!int.TryParse(placeNumberValue.Text, out placeNumber))
            {
                output = false;
            }
            if (!decimal.TryParse(priceAmountValue.Text, out priceAmount))
            {
                output = false;
            }
            if (!int.TryParse(pricePercentageValue.Text, out pricePercentage))
            {
                output = false;
            }
            if (placeNameValue.Text.Length == 0)
            {
                output = false;
            }
            if (placeNumber < 1)
            {
                output = false;
            }
            if ((pricePercentage <= 0 || pricePercentage > 100) && priceAmount < 1)
            {
                output = false;
            }

            return output;
        }
    }
}
