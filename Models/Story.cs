using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MP3_Player.Models
{
    public class Story
    {
        public string StoryName { get; set; }
        public string FolderPath { get; set; }
        public string ImagePath { get; set; }
        public ObservableCollection<Episode> Episodes { get; set; }

        private Episode _currentEpisode;
        public Episode CurrentEpisode
        {
            get
            {
                return _currentEpisode;
            }
            set
            {
                _currentEpisode = value;
            }
        }

        public Episode GetEpisodeByPlayOrder(int playOrder)
        {
            foreach (Episode episode in Episodes)
            {
                if (episode.PlayOrder == playOrder)
                {
                    return episode;
                }
            }

            return null;
        }
    }
}
