
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using CalculatorApp.Helpers;

namespace CalculatorApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class CalculatorActivity : AppCompatActivity
    {
        private TextView calcText;
        CalculatorHelper helper;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_calculator);
            calcText = FindViewById<TextView>(Resource.Id.calculator_text_view);
            helper = new CalculatorHelper();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.main_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.currency)
            {
                StartActivity(typeof(CurrencyActivity));
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;
            calcText.Text = helper.Calculation(button.Text);
        }
    }
}