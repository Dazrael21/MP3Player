using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using static MP3_Player.Funcs.DiskIO;

namespace MP3_Player.Models
{
    public class Series
    {
        public string SeriesName { get; set; }
        public string FolderPath { get; set; }
        public string ImagePath { get; set; }
        public ObservableCollection<Story> Stories { get; set; }

        private Story _currentStory = null;
        public Story CurrentStory
        {
            get
            {
                return _currentStory;
            }
            set
            {
                _currentStory = value;
                _currentStory.GetEpisodes();
            }
        }
    }
}
