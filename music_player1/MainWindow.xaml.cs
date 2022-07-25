using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;
using System.Windows.Threading;
using Message = System.Windows.MessageBox;
using resultat = System.Windows.Forms.DialogResult;

namespace music_player1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MusicControl musicControl = new MusicControl();
        public MainWindow()
        {
            InitializeComponent();
        }

        public WaveOutEvent musicOut;
        public AudioFileReader musicFileReader;

        List<string> list_music = new List<string>();
        List<string> list_path = new List<string>();
        bool music_is = true;
        string path;
        private void Button_Click_PlayMusic(object sender, RoutedEventArgs e)
        {
            if (play_Button.Content == "Play")
            {
                if (music_is == true)
                {
                    play_Button.Content = "Stop";
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Mp3 files(*.mp3)|*.mp3|All files(*.*)|*.*";
                    var names = openFileDialog.FileName.Split("'\'");
                    if (openFileDialog.ShowDialog() == resultat.OK)
                    {
                        slider1.Value = 0;
                        musicControl.PLayMusic(openFileDialog.FileName);

                        list_music.Add(System.IO.Path.GetFileName(openFileDialog.FileName));
                        list_path.Add(openFileDialog.FileName);

                        title_music.Text = System.IO.Path.GetFileName(openFileDialog.FileName);

                        DispatcherTimer dispatcherTimer = new DispatcherTimer();
                        dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                        dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                        musicOut = new WaveOutEvent();
                        musicFileReader = new AudioFileReader(openFileDialog.FileName);
                        dispatcherTimer.Start();
                    }
                    path = openFileDialog.FileName;

                }
                else
                {
                    musicControl.PlayMusicOnPause(path);
                    play_Button.Content = "Stop";
                }
            }
            else
            {
                play_Button.Content = "Play";
                musicControl.StopMusic(path);
            }
            comboBox1.ItemsSource = new List<string>(list_music);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            musicControl.PlayMusicOnPause(path);
            musicControl.RestartMusic(path);
        }

        private void play_Button_Click(object sender, RoutedEventArgs e)
        {
            if (play_Button.Content == "Play")
            {
                if (music_is == true)
                {
                    play_Button.Content = "Stop";
                    musicControl.PlayMusicOnPause(path);
                }
                music_is = false;
            }
            else
            {
                musicControl.StopMusic(path);
                play_Button.Content = "Play";
                music_is = true;
                EventTrigger ellipse_animation2 = ellipse_animation;
                ellipse_animation1.Storyboard.Begin(this, true);
                ellipse_animation1.Storyboard.Stop(this);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            musicControl.ValueVolume((float)musicVolume.Value);
        }
        private void Slider1_value(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value1 = musicControl.Value_Duration((float)slider1.Value);
            slider1.Maximum = value1;

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            slider1.Value += 1;
            CommandManager.InvalidateRequerySuggested();
        }
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            slider1.Value = 0;
            title_music.Text = comboBox1.SelectedItem.ToString();
            musicControl.RestartMusic(list_path[comboBox1.SelectedIndex]);
        }
    }
}
