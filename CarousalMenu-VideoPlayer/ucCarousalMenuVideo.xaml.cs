using CarousalMenu.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CarousalMenu
{
    /// <summary>
    /// Interaction logic for ucCarousalMenuVideo.xaml
    /// </summary>
    public partial class ucCarousalMenuVideo : UserControl
    {
        private bool isPlay = false;
        private bool isFullScreen = false;
        private double volume = 100;
        private TimeSpan videoTime;
        private DispatcherTimer videoPositionSyncher = new DispatcherTimer();

        public ucCarousalMenuVideo()
        {
            InitializeComponent();

            grdMediaContainer.Visibility = Visibility.Hidden;

            videoPositionSyncher.Interval = TimeSpan.FromMilliseconds(300);
            videoPositionSyncher.Tick += VideoPositionSyncher_Tick;
        }

        private void VideoPositionSyncher_Tick(object sender, EventArgs e)
        {
            try
            {
                if (McMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds > 0)
                {
                    if (videoTime.TotalMilliseconds > 0)
                        seekbarSlider.Value = McMediaElement.Position.TotalMilliseconds * seekbarSlider.Maximum / videoTime.TotalMilliseconds;
                }
            }
            catch (Exception)
            {
            }
        }

        // Play & Pause & Close the media.
        private void PlayMedia()
        {
            isPlay = true;
            McMediaElement.Play();
            imgPlay.Visibility = Visibility.Collapsed;
            imgPause.Visibility = Visibility.Visible;
        }

        private void PauseMedia()
        {
            videoPositionSyncher.Stop();
            isPlay = false;
            McMediaElement.Pause();
            imgPlay.Visibility = Visibility.Visible;
            imgPause.Visibility = Visibility.Collapsed;
        }

        private void CloseMedia()
        {
            McMediaElement.Close();
            isPlay = false;
            imgPlay.Visibility = Visibility.Visible;
            imgPause.Visibility = Visibility.Collapsed;
            McMediaElement.Source = null;
            seekbarSlider.Value = 0;
            seekbarSlider.Maximum = 0;
            videoPositionSyncher.Stop();
            timeLineText.Text = "";
        }

        private void OnMouseDownPlayMedia(object sender, MouseButtonEventArgs args)
        {
            // The Play method will begin the media if it is not currently active or 
            // resume media if it is paused. This has no effect if the media is
            // already running.
            if (McMediaElement.Source == null)
                return;
            if (!isPlay)
                PlayMedia();
            else
                PauseMedia();
            // Initialize the MediaElement property values.
            InitializePropertyValues();
        }

        // Change the volume of the media.
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            McMediaElement.Volume = volumeSlider.Value / volumeSlider.Maximum;
            if (volumeSlider.Value > 0 && McMediaElement.IsMuted)
            {
                imgVolume.Source = new BitmapImage(new Uri("/Images/sound.png", UriKind.Relative));
                McMediaElement.IsMuted = true;
            }
        }

        void InitializePropertyValues()
        {
            McMediaElement.Volume = volumeSlider.Value;
        }

        private void imgVolume_MouseEnter(object sender, MouseEventArgs e)
        {
            volumeSlider.Visibility = Visibility.Visible;
        }

        private void imgVolume_MouseLeave(object sender, MouseEventArgs e)
        {
            volumeSlider.Visibility = Visibility.Collapsed;
        }

        private void Fullscreen()
        {
            TimeSpan pos = McMediaElement.Position;

            MainWindow wndMain = Application.Current.Windows[0] as MainWindow;
            if (!isFullScreen)
                grdMediaContainer.Children.Remove(grdMedia);
            wndMain.Fullscreen(grdMedia, !isFullScreen);
            if (isFullScreen)
                grdMediaContainer.Children.Add(grdMedia);
            isFullScreen = !isFullScreen;

            grdMediaGrid.Focus();

            //McMediaElement.Pause();
            McMediaElement.Position = pos;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Fullscreen();
        }

        private void McMediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                Fullscreen();
        }

        private void McMediaElement_KeyDown(object sender, KeyEventArgs e)
        {
            if (isFullScreen && e.Key == Key.Escape)
                Fullscreen();
        }

        private void ucCarousalMenu_LeafMenuClicked(object sender, RoutedEventArgs e)
        {
            LeafMenuClickedEventArgs arg = e as LeafMenuClickedEventArgs;
            if (arg != null)
            {
                CloseMedia();
                grdMediaContainer.Visibility = Visibility.Hidden;
                Dispatcher.Invoke(() =>
                {
                    grdMediaContainer.Visibility = Visibility.Visible;
                    Uri uri = new Uri(arg.Param.Url, UriKind.Relative);
                    McMediaElement.Source = uri;
                });
            }
        }
        private void timerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int hours = 0;
            int mins = 0;
            int secs = 0;
            int hoursCur = 0;
            int minsCur = 0;
            int secsCur = 0;
            double totalHours = 0;
            double totalMins = 0;
            double totalSecs = 0;
            try
            {
                totalHours = McMediaElement.NaturalDuration.TimeSpan.TotalHours;
                totalMins = McMediaElement.NaturalDuration.TimeSpan.TotalMinutes;
                totalSecs = McMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                hours = McMediaElement.NaturalDuration.TimeSpan.Hours;
                mins = McMediaElement.NaturalDuration.TimeSpan.Minutes;
                secs = McMediaElement.NaturalDuration.TimeSpan.Seconds + ((totalSecs - (int)totalSecs) > 0 ? 1 : 0);
                hoursCur = McMediaElement.Position.Hours;
                minsCur = McMediaElement.Position.Minutes;
                secsCur = McMediaElement.Position.Seconds;
            }
            catch (Exception)
            {
            }
            if (seekbarSlider.Value == seekbarSlider.Maximum)
                secsCur = secs;
            if ((int)totalMins == 0)
                timeLineText.Text = string.Format("{2:00} / {5:00}", hoursCur, minsCur, secsCur, hours, mins, secs);
            else if ((int)totalHours == 0)
                timeLineText.Text = string.Format("{1:00}:{2:00} / {4:00}:{5:00}", hoursCur, minsCur, secsCur, hours, mins, secs);
            else if (totalSecs == 0)
                timeLineText.Text = "";
            else
                timeLineText.Text = string.Format("{0:00}:{1:00}:{2:00} / {3:00}:{4:00}:{5:00}", hoursCur, minsCur, secsCur, hours, mins, secs);
        }

        private void imgVolume_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (McMediaElement.IsMuted)
            {
                imgVolume.Source = new BitmapImage(new Uri("/Images/sound.png", UriKind.Relative));
                volumeSlider.Value = volume;
            }
            else
            {
                volume = volumeSlider.Value;
                volumeSlider.Value = 0;
                imgVolume.Source = new BitmapImage(new Uri("/Images/mute.png", UriKind.Relative));
            }
            McMediaElement.IsMuted = !McMediaElement.IsMuted;
        }

        private void Element_MediaOpened(object sender, RoutedEventArgs e)
        {
            seekbarSlider.Maximum = 1000;
            videoTime = McMediaElement.NaturalDuration.TimeSpan;
            McMediaElement.Volume = volumeSlider.Value / volumeSlider.Maximum;
            videoPositionSyncher.Start();
        }

        private void Element_MediaEnded(object sender, RoutedEventArgs e)
        {
            seekbarSlider.Value = seekbarSlider.Maximum;
            videoPositionSyncher.Stop();
        }

        private void seekbarSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                videoPositionSyncher.Stop();
                videoPositionSyncher.Start();
                seekbarSlider_MouseLeftButtonUp(null, null);
            }
        }

        private void seekbarSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (videoTime.TotalMilliseconds > 0 && seekbarSlider.Maximum > 0)
            {
                videoPositionSyncher.Stop();
                videoPositionSyncher.Start();
                McMediaElement.Position = TimeSpan.FromMilliseconds(seekbarSlider.Value / seekbarSlider.Maximum * videoTime.TotalMilliseconds);
            }
        }
    }
}
