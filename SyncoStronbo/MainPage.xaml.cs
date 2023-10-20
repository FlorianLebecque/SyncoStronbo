using SyncoStronbo.Audio;
using SyncoStronbo.Light;
using System.ComponentModel;


namespace SyncoStronbo {
    public partial class MainPage : ContentPage {
        bool state = false;

        IAudioAnalyser audioAnalyser;

        public MainPage() {
            
            audioAnalyser = new AudioAnalyser();
            audioAnalyser.Init();
            
            BackgroundWorker backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            
            backgroundWorker.RunWorkerAsync();
            backgroundWorker.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            
            InitializeComponent();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            audioSlider.Value = (double)e.Result;

            ((BackgroundWorker)sender).RunWorkerAsync();

        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {

            e.Result = audioAnalyser.GetMidLevel() * 100;

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