using SyncoStronbo.Audio;
using SyncoStronbo.Light;
using System.ComponentModel;


namespace SyncoStronbo {
    public partial class MainPage : ContentPage {
        bool state = false;

        readonly IAudioAnalyser audioAnalyser;

        public MainPage() {
            
            audioAnalyser = new AudioAnalyser();
            audioAnalyser.Init();
            
            BackgroundWorker backgroundWorker = new();

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            
            backgroundWorker.RunWorkerAsync();
            backgroundWorker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            
            InitializeComponent();
        }

        private void OnsensitivitySliderChanged(object sender, EventArgs e){
            FourierAnalysis.sensitivity = (double)(sensitivitySlider.Value/100);
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {

            lowAudioSlider.Value = ((double[])e.Result)[0];
            midAudioSlider.Value = ((double[])e.Result)[1];
            HighAudioSlider.Value = ((double[])e.Result)[2];

            ((BackgroundWorker)sender).RunWorkerAsync();

        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {

            e.Result = new double[] { audioAnalyser.GetLowLevel() * 100 ,audioAnalyser.GetMidLevel() * 100 , audioAnalyser.GetHighLevel() * 100};

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