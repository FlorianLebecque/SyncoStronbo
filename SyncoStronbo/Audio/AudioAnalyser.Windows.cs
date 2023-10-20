using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncoStronbo.Audio {
    public class AudioAnalyser : IAudioAnalyser {
        private WaveInEvent waveIn;
        private double maxLevel;


        private bool micOk = false;

        public async void Init() {
            PermissionStatus micPermision = await Permissions.CheckStatusAsync<Permissions.Microphone>();

            if(micPermision != PermissionStatus.Granted) {
                micPermision = await Permissions.RequestAsync<Permissions.Microphone>();
            }

            if(micPermision != PermissionStatus.Granted) {
                micOk = false;
            }

            micOk = true;

            waveIn = new WaveInEvent();

            waveIn.DataAvailable += WaveIn_DataAvailable;

            waveIn.StartRecording();

            maxLevel = 0.0;
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e) {
            byte[] buffer = e.Buffer;
            int bytesRead = e.BytesRecorded;
            short max = 0;

            for (int index = 0; index < bytesRead; index += 2) {
                short sample = (short)((buffer[index + 1] << 8) | buffer[index]);
                if (sample > max) {
                    max = sample;
                }
            }

            double level = (max / 32768.0); // Normalize to the range [0, 1]
            maxLevel = level;
        }

        public double GetHighLevel() {
            throw new NotImplementedException();
        }

        public double GetMidLevel() {
            if (!micOk) {
                return 0;
            }

            if (maxLevel < 0.0) maxLevel = 0.0;
            if (maxLevel > 1.0) maxLevel = 1.0;

            return maxLevel;
        }

        public double GetLowLevel() {
            throw new NotImplementedException();
        }
    }
}
