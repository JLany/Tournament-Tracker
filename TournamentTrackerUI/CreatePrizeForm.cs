using TournamentTrackerLibrary;
using TournamentTrackerLibrary.Models;
using TournamentTrackerLibrary.InterCommunication;
using TournamentTrackerLibrary.Utility;

namespace TournamentTrackerUI;

public partial class CreatePrizeForm : Form
{
    private readonly IPrizeRequester clientProcess;

    public CreatePrizeForm(IPrizeRequester clientProcess)
    {
        InitializeComponent();

        // Set up
        this.clientProcess = clientProcess;

        // Event handlers
        this.createPrizeButton.Click += CreatePrizeButton_Click;
    }

    private void CreatePrizeButton_Click(object? sender, EventArgs e)
    {
        if (!ValidateForm())
        {
            MessageBox.Show("Form has invalid fields. Try again.");
            return;
        }

        var prize = new PrizeModel(
            placeNumberTextBox.Text,
            placeNameTextBox.Text,
            prizeAmountTextBox.Text,
            prizePercentageTextBox.Text
        );

        GlobalConfig.Connector.CreatePrize(prize);

        clientProcess.ReceivePrize(prize);

        // reshow default 
        ResetForm();
    }


    private bool ValidateForm()
    {
        bool output = true;

        // TODO - Show appropriate error messages on labels for each case.

        // check place number field
        if (int.TryParse(placeNumberTextBox.Text, out int placeNumberValue))
        {
            if (placeNumberValue < 1)
            {
                output = false;
            }
        }
        else
        {
            output = false;
        }

        // check place name field
        if (placeNameTextBox.Text.Length < 1 || placeNameTextBox.Text.Length > 100)
        {
            output = false;
        }

        if (!TournamentTrackerValidations.IsValidName(placeNameTextBox.Text))
        {
            output = false;
        }

        // check prize amount & prize percentage
        if (decimal.TryParse(prizeAmountTextBox.Text, out decimal prizeAmountValue))
        {
            if (prizeAmountValue < 0) // negative prize amount 
            {
                output = false;
            }
        }
        else
        {
            output = false;
        }

        if (int.TryParse(prizePercentageTextBox.Text, out int prizePercentageValue))
        {
            if (prizePercentageValue < 0 || prizePercentageValue > 100)
            {
                output = false;
            }
        }
        else
        {
            output = false;
        }

        if ((prizePercentageValue > 0 && prizeAmountValue > 0) || (prizeAmountValue == 0 && prizePercentageValue == 0))
        {
            // please specify either an amount or a percentage
            output = false;
        }

        // TODO - (OPTIONAL) Further validations are possible here
        // like: check for exceeding the total income of the tournament

        return output;
    }

    private void ResetForm()
    {
        placeNumberTextBox.Text = "";
        placeNameTextBox.Text = "";
        prizeAmountTextBox.Text = "0";
        prizePercentageTextBox.Text = "0";
    }
}
