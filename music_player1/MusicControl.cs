using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroovyCodecs;
using NAudio.Wave;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

namespace music_player1
{
    public class MusicControl
    {
        
        public WaveOutEvent musicOut;
        public AudioFileReader musicFileReader;
        
        public void PLayMusic(string musicPath)
        {
            musicOut = new WaveOutEvent();
            musicFileReader = new AudioFileReader(musicPath);
            musicOut.Init(musicFileReader);
            musicOut.Play();
            

            musicOut.PlaybackStopped += MusicOut_PlaybackStopped;
        }
        public void RestartMusic(string musicPath)
        {
            musicOut.Stop();
            musicOut = new WaveOutEvent();
            musicFileReader = new AudioFileReader(musicPath);
            musicOut.Init(musicFileReader);
            musicOut.Play();
            musicOut.PlaybackStopped += MusicOut_PlaybackStopped;
        }

        public void MusicDuration(float slider)
        {
            musicFileReader.CurrentTime = new System.TimeSpan(0, 0, 0, 20);
        }

        private void MusicOut_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            musicOut.Play();
        }

        public void PlayMusicOnPause(string musicPath)
        {
            try
            {
                musicOut.Play();
            }
            catch
            {
                MessageBox.Show("Выбери музыку");
            }
        }
        public void ValueVolume(float value)
        {
            musicOut.Volume = (float)value;
        }
        public int Value_Duration(float slider)
        {
            var ts = TimeSpan.FromMinutes((double)slider);
            musicFileReader.CurrentTime = new TimeSpan((int)ts.TotalDays, (int)ts.TotalHours, (int)ts.TotalMinutes, (int)ts.TotalSeconds);
            return (int)ts.TotalSeconds;
        }
        public void StopMusic(string musicPath)
        {
            if (musicOut != null)
            {
                musicOut.Pause();
            }
            else
            {
                MessageBox.Show("hello");
            }
        }
    }
}
