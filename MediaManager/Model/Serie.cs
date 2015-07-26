﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediaManager.Helpers;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    [Table("Series")]
    public class Serie : Video
    {
        private IList<string> _availableTranslations;
        private string _folderPath;
        private IList<string> _genresList;
        private int _id;
        private Ids _ids;
        private Images _images;
        private string _overview;
        private string _title;

        [JsonProperty("day")]
        public string AirDay { get; set; }

        [JsonProperty("aired_episodes", NullValueHandling = NullValueHandling.Ignore)]
        public int AiredEpisodes { get; set; }

        [JsonProperty("time")]
        public string AirTime { get; set; }

        [JsonProperty("timezone")]
        public string AirTimezone { get; set; }

        [JsonProperty("available_translations")]
        public IList<string> AvailableTranslations { get { return _availableTranslations; } set { _availableTranslations = value; Traducoes = Helpers.Helper.ListToString(value); } }

        [JsonProperty("certification")]
        public string Certification { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("first_aired", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? FirstAired { get; set; }

        public string FolderPath { get { return _folderPath; } set { _folderPath = value; OnPropertyChanged("FolderPath"); } }

        public string Generos { get; private set; }

        [JsonProperty("genres")]
        public IList<string> GenresList { get { return _genresList; } set { _genresList = value; Generos = Helpers.Helper.ListToString(value); } }

        [JsonProperty("homepage")]
        public object Homepage { get; set; }

        [Key, Column(Order = 0)]
        public int ID { get { return _id; } set { _id = value; } }

        [JsonProperty("ids"), NotMapped]
        public virtual Ids Ids { get { return _ids; } set { _ids = value; } }

        [JsonProperty("images"), NotMapped]
        public virtual Images Images { get { return _images; } set { _images = value; OnPropertyChanged("Images"); } }

        public bool IsAnime { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        public string MetadataFolder { get; set; }

        [JsonProperty("network")]
        public string Network { get; set; }

        [JsonProperty("overview")]
        public string Overview { get { return _overview; } set { _overview = value; OnPropertyChanged("Overview"); } }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public double Rating { get; set; }

        [JsonProperty("runtime", NullValueHandling = NullValueHandling.Ignore)]
        public int Runtime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [NotMapped]
        public Helper.Enums.TipoConteudo Tipo { get { return Helper.Enums.TipoConteudo.show; } set { throw new NotSupportedException(); } }

        [NotMapped]
        public string TipoString { get { return Helper.Enums.ToString(Tipo); } }

        [JsonProperty("title")]
        public string Title { get { return _title; } set { _title = value; OnPropertyChanged("Title"); } }

        public string Traducoes { get; private set; }

        [JsonProperty("trailer")]
        public object Trailer { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("votes", NullValueHandling = NullValueHandling.Ignore)]
        public int Votes { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int Year { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}