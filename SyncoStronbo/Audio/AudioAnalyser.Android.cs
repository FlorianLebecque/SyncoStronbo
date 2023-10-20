using Android.Media;
using Android.Media.Metrics;
using SyncoStronbo.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncoStronbo.Audio
{
    public class AudioAnalyser : IAudioAnalyser {

        private bool micOk = false;

        private const int SampleRate = 44100;
        private const ChannelIn ChannelConfig = ChannelIn.Mono;
        private const Android.Media.Encoding AudioFormat = Android.Media.Encoding.Pcm16bit;
        private int BufferSize = AudioRecord.GetMinBufferSize(SampleRate, ChannelConfig, AudioFormat);

        private AudioRecord audioRecord;
        private bool isRecording;
        public event EventHandler<double> AudioLevelChanged;


        private Thread th;

        private double level;

        public AudioAnalyser() {
            AudioLevelChanged += AudioChanged;

            th = new Thread(RecordThread);

            level = 0;
        }

        private void AudioChanged(object sender, double e) {
            level = e;
        }

        public async void Init() {
            PermissionStatus micPermision = await Permissions.CheckStatusAsync<Permissions.Microphone>();

            if (micPermision != PermissionStatus.Granted) {
                micPermision = await Permissions.RequestAsync<Permissions.Microphone>();
            }

            if (micPermision != PermissionStatus.Granted) {
                micOk = false;
            }

            audioRecord = new AudioRecord(AudioSource.Mic, SampleRate, ChannelConfig, AudioFormat, BufferSize);

            th.Start();
        }

        private void RecordThread() {
            if (audioRecord.State == State.Initialized) {
                isRecording = true;
                audioRecord.StartRecording();

                byte[] buffer = new byte[BufferSize];
                while (isRecording) {
                    audioRecord.Read(buffer, 0, BufferSize);
                    double audioLevel = CalculateAudioLevel(buffer);
                    AudioLevelChanged?.Invoke(this, audioLevel);
                }
            }
        }

        private double CalculateAudioLevel(byte[] audioData) {
            short[] audioSamples = new short[audioData.Length / 2];
            Buffer.BlockCopy(audioData, 0, audioSamples, 0, audioData.Length);

            double sum = 0.0;
            foreach (short sample in audioSamples) {
                sum += Math.Abs(sample);
            }

            double average = sum / audioSamples.Length;
            double normalizedLevel = average / short.MaxValue; // Normalize to [0, 1]

            return normalizedLevel;
        }
        public double GetHighLevel() {
            return 0;
        }

        public double GetLowLevel() {
            return 0;
        }

        public double GetMidLevel() {

            if (level < 0) level = 0;
            if (level > 1) level = 1;

            return level;

        }

       
    }
}
