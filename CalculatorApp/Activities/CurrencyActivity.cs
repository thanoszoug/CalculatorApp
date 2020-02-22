using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using CalculatorApp.Helpers;
using CalculatorApp.Models;
using System;

namespace CalculatorApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class CurrencyActivity : AppCompatActivity
    {
        private ProgressBar loadingSpinner;
        private Spinner convertFrom;
        private Spinner convertTo;
        private ArrayAdapter<string> adapter;
        private EditText amount;
        private TextView convertedAmount;
        private FixerIOHelper fixer;
        private readonly string[] currencies = Symbols.ValidSymbols;
        private readonly string[] selectedCurrencies = new string[2];

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_currency);

            loadingSpinner = FindViewById<ProgressBar>(Resource.Id.loading_spinner);
            convertFrom = FindViewById<Spinner>(Resource.Id.currency_to);
            convertTo = FindViewById<Spinner>(Resource.Id.currency_from);
            amount = FindViewById<EditText>(Resource.Id.amount);
            convertedAmount = FindViewById<TextView>(Resource.Id.converted_amount);

            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, currencies);
            convertFrom.Adapter = adapter;
            convertTo.Adapter = adapter;

            fixer = new FixerIOHelper(Secrets.FixerIoApiKey);

            convertFrom.ItemSelected += ConvertFrom_ItemSelected;
            convertTo.ItemSelected += ConvertTo_ItemSelected;
        }

        [Java.Interop.Export("ConvertClick")]
        public async void ConvertClick(View v)
        {
            loadingSpinner.Visibility = ViewStates.Visible;
            var fixerAmount = await fixer.ConvertAsync(selectedCurrencies[0], selectedCurrencies[1], double.Parse(amount.Text));
            UpdateFixerAmount(Math.Round(fixerAmount, 2));
            loadingSpinner.Visibility = ViewStates.Invisible;
        }

        private void ConvertFrom_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            selectedCurrencies[0] = e.Parent.GetItemAtPosition(e.Position).ToString();
        }

        private void ConvertTo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            selectedCurrencies[1] = e.Parent.GetItemAtPosition(e.Position).ToString();
        }

        private void UpdateFixerAmount(double amount) => convertedAmount.Text = "Converted Amount: " + amount.ToString();
    }
}