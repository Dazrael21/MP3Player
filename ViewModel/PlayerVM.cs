using Big_Finish_Player.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.ComponentModel;
using System.Collections.ObjectModel;

using static Big_Finish_Player.Funcs.ErrorLogging;
using static Big_Finish_Player.Funcs.DiskIO;
using System.Windows.Input;
using Microsoft.VisualBasic;
using System.Windows.Media;

namespace Big_Finish_Player.ViewModel
{
    public class PlayerVM : ViewModelBase
    {
        public Player Player { get; set; }

        private MediaPlayer mediaPlayer;

        private static int currentTrackNo = 1;

        public PlayerVM(IConfiguration configuration)
        {
            try
            {
                mediaPlayer = new MediaPlayer();

                Player = new Player
                {
                    Configuration = configuration
                };

                Paused = true;

                SeriesCollection = GetCollection(Player.RootPath);

                Play = new CustomCommand(r => true, CPlay);
                PlayCaption = "Play";
            }
            catch (Exception ex)
            {
                Log(ex);
                throw;
            }
        }

        public ObservableCollection<Series> SeriesCollection
        {
            get { return Player.SeriesCollection; }
            set
            {
                if (value != Player.SeriesCollection)
                    Player.SeriesCollection = value;
                this.SetPropertyChanged("SeriesCollection");
            }
        }

        public Series CurrentSeries
        {
            get { return Player.CurrentSeries; }
            set
            {
                if (value != Player.CurrentSeries)
                {
                    Player.CurrentSeries = value;
                }
                this.SetPropertyChanged("CurrentSeries");
            }
        }

        public Story CurrentStory
        {
            get
            {
                return Player.CurrentSeries?.CurrentStory;
            }
            set
            {
                if (value != Player.CurrentSeries.CurrentStory)
                {
                    Player.CurrentSeries.CurrentStory = value;
                    currentTrackNo = 1;
                }
                this.SetPropertyChanged("CurrentStory");
            }
        }

        public Episode CurrentEpisode
        {
            get
            {
                if (CurrentSeries == null) return null;
                if (CurrentStory == null) return null;
                return Player.CurrentSeries.CurrentStory.CurrentEpisode;
            }
            set
            {
                if (CurrentStory != null && value != CurrentEpisode)
                {
                    Player.CurrentSeries.CurrentStory.CurrentEpisode = value;
                    Paused = true;
                    LoadEpisode();
                }
                this.SetPropertyChanged("CurrentEpisode");
            }
        }

        private void LoadEpisode()
        {
            mediaPlayer.Open(new Uri(CurrentEpisode.FilePath));
            while(!mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                //Wait
            }
            CurrentEpisode.Duration = mediaPlayer.NaturalDuration.TimeSpan;
            MaxCurrentTrackSeconds = CurrentEpisode.Duration.TotalSeconds;
            CurrentTrackSeconds = 0;

            if (CurrentEpisode != null) currentTrackNo = CurrentEpisode.PlayOrder;
        }

        public ICommand Play { get; set; }
        private string playCaption;
        public string PlayCaption
        {
            get { return playCaption; }
            set { playCaption = value; this.SetPropertyChanged("PlayCaption"); }
        }

        private bool _paused;
        public bool Paused
        {
            get
            {
                return _paused;
            }
            set
            {
                _paused = value;
                if (_paused)
                {
                    mediaPlayer.Pause();
                    PlayCaption = "Play";
                }
                else
                {
                    mediaPlayer.Play();
                    PlayCaption = "Pause";
                }
            }
        }

        private double _maxCurrentTrackSeconds;
        public double MaxCurrentTrackSeconds
        {
            get
            {
                return _maxCurrentTrackSeconds;
            }
            set
            {
                _maxCurrentTrackSeconds = value;
                this.SetPropertyChanged("MaxCurrentTrackSeconds");
                this.SetPropertyChanged("MaxTrackDisplay");
            }
        }

        public string MaxTrackDisplay
        {
            get
            {
                return CurrentEpisode == null ? null : DateTime.Today.Add(CurrentEpisode.Duration).ToString("HH:mm:ss");
            }
        }

        public double CurrentTrackSeconds
        {
            get
            {
                if (mediaPlayer.Position == mediaPlayer.NaturalDuration && !Paused)
                {
                    int nextEpisode = currentTrackNo++;
                    CurrentEpisode = CurrentStory.GetEpisodeByPlayOrder(nextEpisode);
                    Paused = false;
                }
                return mediaPlayer.Position.TotalSeconds;
            }
            set
            {
                int newPos = Convert.ToInt32(value);
                mediaPlayer.Position = new TimeSpan(0, 0, 0, newPos, 0);
                this.SetPropertyChanged("CurrentTrackPositionDisplay");
            }
        }

        public string CurrentTrackPositionDisplay
        {
            get
            {
                return CurrentEpisode == null ? null : DateTime.Today.AddSeconds(CurrentTrackSeconds).ToString("HH:mm:ss");
            }
        }

        public void UpdateCurrentPosition()
        {
            this.SetPropertyChanged("CurrentTrackSeconds");
            this.SetPropertyChanged("CurrentTrackPositionDisplay");
        }

        private void CPlay(object obj)
        {
            Paused = !Paused;
        }
    }

    public class CustomCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public CustomCommand(Predicate<object> canExecute, Action<object> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public void Execute(object parameter)
        {
            if (this.canExecute != null)
            {
                execute(parameter);
            }
        }
    }
}
