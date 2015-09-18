using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MpcHcDotNet.Model {
    public class PlayInfo : INotifyPropertyChanged {
        #region INotifyPropertyChanged Member
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string pName) {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(pName));
        }
        #endregion

        private string title;
        public string Title {
            get { return title; }
            set {
                if (title == value) return;

                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string author;
        public string Author {
            get { return author; }
            set {
                if (author == value) return;

                author = value;
                RaisePropertyChanged("Author");
            }
        }

        private string description;
        public string Description {
            get { return description; }
            set {
                if (description == value) return;

                description = value;
                RaisePropertyChanged("Description");
            }
        }

        private string path;
        public string Path {
            get {
                return path;
            }
            set {
                path = value;
                RaisePropertyChanged("Path");
            }
        }

        private string duration;
        public string Duration {
            get { return duration; }
            set {
                if (duration == value) return;

                duration = value;
                RaisePropertyChanged("Duration");

                if (duration.Contains(".")) {
                    DurationSeconds = Convert.ToInt32(
                        duration.Substring(0, duration.IndexOf('.'))
                    );
                } else {
                    DurationSeconds = Convert.ToInt32(duration);
                }
            }
        }

        private int durationSeconds;
        public int DurationSeconds {
            get { return durationSeconds; }
            set {
                if (durationSeconds == value) return;

                durationSeconds = value;
                RaisePropertyChanged("DurationSeconds");

                calculatePercentagePlayed();   
            }
        }

        private int currentSecond;
        public int CurrentSecond {
            get { return currentSecond; }
            set {
                if (currentSecond == value) return;

                currentSecond = value;
                RaisePropertyChanged("CurrentSecond");

                calculatePercentagePlayed();   
            }
        }

        private int percentagePlayed;
        public int PercentagePlayed {
            get { return percentagePlayed; }
            set {
                if (percentagePlayed == value) return;

                percentagePlayed = value;
                RaisePropertyChanged("PercentagePlayed");
            }
        }

        private void calculatePercentagePlayed() {
            if (CurrentSecond == DurationSeconds)
                PercentagePlayed = 100;
            else if (DurationSeconds != 0) {
                double cache = (double) CurrentSecond / DurationSeconds;
                PercentagePlayed = Convert.ToInt32(cache * 100);
            } else {
                PercentagePlayed = 0;
            }
        }
    }
}
