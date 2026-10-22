using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace практика_7
{
    public partial class Game : INotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private string _genre;
        private decimal _price;
        private int _ageRating;
        private bool _isAvailable;

        public int Id
        {
            get => _id;
            set { 
                _id = value; 
                OnPropertyChanged(); 
            }
        }

        public string Title
        {
            get => _title;
            set { 
                _title = value; 
                OnPropertyChanged(); 
            }
        }

        public string Genre
        {
            get => _genre;
            set { 
                _genre = value;
                OnPropertyChanged(); 
            }
        }

        public decimal Price
        {
            get => _price;
            set { 
                _price = value; 
                OnPropertyChanged(); 
            }
        }

        public int AgeRating
        {
            get => _ageRating;
            set { 
                _ageRating = value; 
                OnPropertyChanged(); 
            }
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set { 
                _isAvailable = value; 
                OnPropertyChanged(); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

}
