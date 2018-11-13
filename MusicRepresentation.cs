using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusikApp
{
    [Serializable]
    class MusicLibrary
    {
        private List<Track> Tracks = new List<Track>();
        public MusicLibrary()
        {

        }

        public void addNewTrack(Track track)
        {
            if(Tracks.Count >= 1)
            {
                int lastID = Tracks.Last().GetID();
                track.SetID(lastID + 1);
            }
            else
            {
                track.SetID(0);
            }
            this.Tracks.Add(track);
        }

        public List<Track> GetAll()
        {
            return this.Tracks;
        }

        public Track GetTrackByID(int id)
        {
            foreach(Track track in Tracks)
            {
                if(track.GetID() == id)
                {
                    return track;
                }
            }
            Track noTrack = new Track();
            return noTrack;
        }

        public List<Track> GetTracksByArtist(string lookForArtist)
        {
            List<Track> TracksByArtist = new List<Track>();
            foreach (Track track in Tracks)
            {
                foreach(string artist in track.GetArtists())
                {
                    if(artist.ToLower().Contains(lookForArtist.ToLower()))
                    {
                        TracksByArtist.Add(track);
                    }
                }
            }
            return TracksByArtist;
        }

        public List<Track> GetTracksByAlbum(string lookForAlbum)
        {
            List<Track> TracksByAlbum = new List<Track>();
            foreach (Track track in Tracks)
            {
                if (track.GetAlbum().ToLower().Contains(lookForAlbum.ToLower()))
                {
                    TracksByAlbum.Add(track);
                }
            }
            return TracksByAlbum;
        }

        public List<Track> GetTracksByGenre(string lookForGenre)
        {
            List<Track> TracksByGenre = new List<Track>();
            foreach (Track track in Tracks)
            {
                foreach (string genre in track.GetGenres())
                {
                    if (genre.ToLower().Contains(lookForGenre.ToLower()))
                    {
                        TracksByGenre.Add(track);
                    }
                }
            }
            return TracksByGenre;
        }
    }
    [Serializable]
    class Track
    {
        private int ID = 0;
        private string Path = "";
        private string FileName = "";
        private string Title = "";
        private string Album = "";
        private List<string> AlbumArtists = new List<string>();
        private List<string> Artists = new List<string>();
        private List<string> Genres = new List<string>();
        private TimeSpan Length = new TimeSpan();
        private int Year = 0;
        private int Number = 0;
        private int NumTracks = 0;
        private int BPM = 0;
        private string CoverArtPath = "";
        private int SampleRate;

        public bool alreadyInPlaylist = false;

        public Track()
        {
            this.ID = -1;
        }
        public Track(string Path, string FileName, string Title, string Album, List<string> AlbumArtists, List<string> Artists, List<string> Genres, TimeSpan Length, int Year, int Number, int NumTracks, int BPM, int SampleRate)
        {
            this.Path = Path;
            this.FileName = FileName;
            this.Title = Title;
            this.Album = Album;
            this.AlbumArtists = AlbumArtists;
            this.Artists = Artists;
            this.Genres = Genres;
            this.Length = Length;
            this.Year = Year;
            this.Number = Number;
            this.NumTracks = NumTracks;
            this.BPM = BPM;
            this.SampleRate = SampleRate;
        }

        public Track(string Path, string FileName, string Title, string Album, string[] AlbumArtists, string[] Artists, string[] Genres, TimeSpan Length, int Year, int Number, int NumTracks, int BPM)
        {
            this.Path = Path;
            this.FileName = FileName;
            this.Title = Title;
            this.Album = Album;
            foreach (string str in AlbumArtists)
            {
                this.AlbumArtists.Add(str);
            }
            foreach (string str in Artists)
            {
                this.Artists.Add(str);
            }
            foreach (string str in Genres)
            {
                this.Genres.Add(str);
            }
            this.Length = Length;
            this.Year = Year;
            this.Number = Number;
            this.NumTracks = NumTracks;
            this.BPM = BPM;
        }

        public bool alreadyContained()
        {
            return this.alreadyInPlaylist;
        }

        public string GetStringRepresentation()
        {
            string rep = "";
            foreach(string artist in this.AlbumArtists)
            {
                rep = rep + " " + artist;
            }
            rep = rep + ": " + this.Album + " - " + this.Number + " " + this.Title + " - ID: " + this.ID;
            return rep;
        }

        public int GetID()
        {
            return this.ID;
        }
        public void SetID(int id)
        {
            this.ID = id;
        }

        public string GetTitle()
        {
            return this.Title;
        }
        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public List<string> GetArtists()
        {
            return this.Artists;
        }
        public void ClearArtists()
        {
            this.Artists.Clear();
        }

        public List<string> GetGenres()
        {
            return this.Genres;
        }
        public void ClearGenres()
        {
            this.Genres.Clear();
        }
        public void AddArtist(string artist)
        {
            this.Artists.Add(artist);
        }
        
        public string GetAlbum()
        {
            return this.Album;
        }
        public void SetAlbum(string album)
        {
            this.Album = album;
        }

        public int GetNumber()
        {
            return this.Number;
        }

        public string GetPath()
        {
            return this.Path;
        }

        public int GetLengthInTenthSec()
        {
            return Length.Hours * 60 * 60*10 + Length.Minutes * 60*10 + Length.Seconds*10 + Length.Milliseconds/100;
        }

        public int GetSampleRate()
        {
            return this.SampleRate;
        }
    }
}
