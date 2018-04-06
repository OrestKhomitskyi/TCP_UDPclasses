using NAudio.Wave;

namespace ZoomFake_TCP_.Media
{
    public class Audio
    {
        public void GetWave()
        {
            WaveIn waveIn=new WaveIn();
            waveIn.BufferMilliseconds = 50;
            waveIn.StartRecording();
            //waveIn
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
        }
    }
}
