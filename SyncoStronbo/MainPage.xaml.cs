using SyncoStronbo.Light;

namespace SyncoStronbo {
    public partial class MainPage : ContentPage {
        bool state = false;

        public MainPage() {
            InitializeComponent();
        }

        private void OnSliderChanged(object sender, EventArgs e) {
            LightController ls = LightController.GetInstance();

            Slider sld = (Slider)sender;

            ls.SetDelay((long)sld.Value);
        }

        private void OnCounterClicked(object sender, EventArgs e) {
            
            state = !state;

            LightController ls = LightController.GetInstance();

            if(state) {

                stateLabel.Text = "Light On!";
                //Flashlight.Default.TurnOnAsync();
                ls.Start();
            } else {

                stateLabel.Text = "Light Off :'(";

                //Flashlight.Default.TurnOffAsync();

                ls.Stop();
            }

        }
    }
}