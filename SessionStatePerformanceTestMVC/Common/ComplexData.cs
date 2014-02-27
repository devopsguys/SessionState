using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SessionStatePerformanceTestMVC.Common
{
    [Serializable]
    public class Album
    {
        public int AlbumId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string AlbumArtUrl { get; set; }
        public Genre Genre { get; set; }
        public Artist Artist { get; set; }
    }

    [Serializable]
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}