using ServicePoll.Models;
using System.Configuration;

namespace ServicePoll.Config
{
    public static class ServicePollConfig
    {
        private readonly static Section SpSection = (Section)ConfigurationManager.GetSection("servicePoll");

        public static string PollConnectionString
        {
            get
            {
                return SpSection.PollConnectionString;
            }
        }

        public static int CountTake
        {
            get
            {
                return SpSection.CountTake;
            }
        }

        public static string TempConnectionString
        {
            get
            {
                return SpSection.TempConnectionString;
            }
        }

        public static string TempCollectionName
        {
            get
            {
                return SpSection.TempCollectionName;
            }
        }

        public static FieldName TempFieldName
        {
            get
            {
                return SpSection.TempFieldName;
            }
        }
    }


    public class Section : ConfigurationSection
    {
        private const string _pollConnectionString = "pollConnectionString";
        private const string _countTake = "countTake";

        private const string _tempConnectionString = "tempConnectionString";
        private const string _tempCollectionName = "tempCollectionName";
        private const string _tempFieldName = "tempFieldName";


        /// <summary>
        /// Строка подключения к базе данных с опросами
        /// </summary>
        [ConfigurationProperty(_pollConnectionString)]
        public string PollConnectionString
        {
            get
            {
                return (string)this[_pollConnectionString];
            }
            set
            {
                this[_pollConnectionString] = value;
            }
        }

        /// <summary>
        /// Количество url`ов (с наибольшим количеством ответов), которые будут перемешаны
        /// </summary>
        [ConfigurationProperty(_countTake)]
        public int CountTake
        {
            get
            {
                return (int)this[_countTake];
            }
            set
            {
                this[_countTake] = value;
            }
        }
        #region Конфиг только для Maintenance
        /// <summary>
        /// Строка подключения к базе данных с коллекцией, из которой будем брать Url
        /// </summary>
        [ConfigurationProperty(_tempConnectionString)]
        public string TempConnectionString
        {
            get
            {
                return (string)this[_tempConnectionString];
            }
            set
            {
                this[_tempConnectionString] = value;
            }
        }

        /// <summary>
        /// Имя коллекции, из которой будем брать Url
        /// </summary>
        [ConfigurationProperty(_tempCollectionName)]
        public string TempCollectionName
        {
            get
            {
                return (string)this[_tempCollectionName];
            }
            set
            {
                this[_tempCollectionName] = value;
            }
        }

        /// <summary>
        /// Имя поля, из которого будем брать Url
        /// </summary>
        [ConfigurationProperty(_tempFieldName)]
        public FieldName TempFieldName
        {
            get
            {
                return (FieldName)this[_tempFieldName]; 
            }
            set
            {
                this[_tempFieldName] = value;
            }
        }
        #endregion
    }
}