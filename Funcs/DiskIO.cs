using Big_Finish_Player.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Automation.Peers;

namespace Big_Finish_Player.Funcs
{
    public static class DiskIO
    {
        public static ObservableCollection<Series> GetCollection(string rootPath)
        {
            DirectoryInfo readCollection = new DirectoryInfo(rootPath);

            ObservableCollection<Series> seriesCollection = new ObservableCollection<Series>();

            foreach (var item in readCollection.GetDirectories())
            {
                seriesCollection.Add(new Series
                {
                    SeriesName = item.Name,
                    FolderPath = item.FullName
                });
            }

            return seriesCollection;
        }

        public static void GetStories(this Series series)
        {
            DirectoryInfo readStories = new DirectoryInfo(series.FolderPath);

            series.Stories = new ObservableCollection<Story>();

            foreach (DirectoryInfo fileInfo in readStories.GetDirectories())
            {
                series.Stories.Add(new Story
                {
                    StoryName = fileInfo.Name,
                    FolderPath = fileInfo.FullName
                });
            }
        }

        public static void GetEpisodes(this Story story)
        {
            if (story == null) return;
            DirectoryInfo readSeries = new DirectoryInfo(story.FolderPath);

            story.Episodes = new ObservableCollection<Episode>();

            int playOrder = 1;
            foreach (FileInfo fileInfo in readSeries.GetFiles("*.mp3"))
            {
                story.Episodes.Add(new Episode
                {
                    EpisodeName = fileInfo.Name,
                    FilePath = fileInfo.FullName,
                    PlayOrder = playOrder
                });
                playOrder++;
            }
        }
    }
}
