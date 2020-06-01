using System;
using System.Collections.Generic;
using System.Text;

namespace Big_Finish_Player.Models
{
    public class Episode
    {
        public string EpisodeName { get; set; }
        public string FilePath { get; set; }
        public int PlayOrder { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
