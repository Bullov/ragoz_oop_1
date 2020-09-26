using System.Collections.Generic;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using ragoz_oop_1.Components;
using ragoz_oop_1.Enums;

namespace ragoz_oop_1.ViewModels
{
    public class ApartmentViewModel : BaseViewModel
    {
        private int _number;
        private int _roomsNumber;
        private int _area;
        private int _livingArea;
        private int _livingPeopleNumber;
        private int _apartmentHeight;
        
        public FloorZone Zone { get; set; }

        public int[] Rooms => new int[_roomsNumber];
        
        public int ApartmentWidth => _area / _apartmentHeight;

        private RelayCommand _openApartmentInfo;

        public string Number
        {
            get => _number.ToString();
            set
            {
                var parse = int.TryParse(value, out var number);
                if (parse && number > 0)
                {
                    _number = number;
                    OnPropertyChanged(nameof(Number));
                }
            }
        }

        public string RoomsNumber
        {
            get => _roomsNumber.ToString();
            set
            {
                var parse = int.TryParse(value, out var number);
                if (parse && number > 0)
                {
                    _roomsNumber = number;
                    OnPropertyChanged(nameof(RoomsNumber));
                }
            }
        }

        public string Area
        {
            get => _area.ToString();
            set
            {
                var parse = int.TryParse(value, out var number);
                if (parse && number > 0)
                {
                    _area = number;
                    OnPropertyChanged(nameof(Area));
                }
            }
        }

        public string LivingArea
        {
            get => _livingArea.ToString();
            set
            {
                var parse = int.TryParse(value, out var number);
                if (parse && number > 0)
                {
                    _livingArea = number;
                    OnPropertyChanged(nameof(LivingArea));
                }
            }
        }

        public string LivingPeopleNumber
        {
            get => _livingPeopleNumber.ToString();
            set
            {
                var parse = int.TryParse(value, out var number);
                if (parse && number > 0)
                {
                    _livingPeopleNumber = number;
                    OnPropertyChanged(nameof(LivingPeopleNumber));
                }
            }
        }
        public int ApartmentHeight
        {
            get => _apartmentHeight;
            set
            {
                _apartmentHeight = value;
                OnPropertyChanged(nameof(ApartmentHeight));
            }
        }

        public RelayCommand OpenApartmentInfo => _openApartmentInfo ??= new RelayCommand(async _ =>
        {
            var dialog = new ApartmentModal
            {
                DataContext = new ApartmentModalViewModel
                {
                    IsEdit = false,
                    Area = Area,
                    LivingArea = LivingArea,
                    LivingPeopleNumber = LivingPeopleNumber,
                    Number = Number,
                    RoomsNumber = RoomsNumber
                }
            };
            await DialogHost.Show(dialog, "rootDialog");
        }, null);
    }
}