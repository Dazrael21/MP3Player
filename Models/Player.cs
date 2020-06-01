using MP3_Player.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using static MP3_Player.Funcs.DiskIO;

namespace MP3_Player.Models
{
    public class Player
    {
        private string _rootPath = null;
        
        public IConfiguration Configuration { get; set; }

        public string RootPath {
            get
            {
                return _rootPath ?? Configuration["DefaultSourceRootPath"];
            }
            set
            {
                _rootPath = value;
            }
        }

        private ObservableCollection<Series> _seriesCollection = null;
        public ObservableCollection<Series> SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
            }
        }

        private Series _currentSeries = null;
        public Series CurrentSeries
        {
            get
            {
                return _currentSeries;
            }
            set
            {
                _currentSeries = value;
                _currentSeries.GetStories();
            }
        }
    }
}
