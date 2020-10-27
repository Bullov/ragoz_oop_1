using System.Collections.Generic;
using System.Windows;
using MaterialDesignThemes.Wpf;
using ragoz_oop_1.Enums;
using ragoz_oop_1.models;
using ragoz_oop_1.ViewModels;

namespace ragoz_oop_1.Components
{
    public class ApartmentModalViewModel : BaseViewModel
    {
        private int _number;
        private int _roomsNumber;
        private int _area;
        private int _livingArea;
        private int _livingPeopleNumber;

        public bool IsEdit { get; set; }
        public int MinArea { get; set; }
        public int MaxArea => IsTopZone ? TopZoneFreeArea : BottomZoneFreeArea;
        
        public int TopZoneFreeArea { get; set; }
        public int BottomZoneFreeArea { get; set; }

        public Visibility EditControlsVisibility => IsEdit ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ViewControlsVisibility => IsEdit ? Visibility.Collapsed : Visibility.Visible;
        

        public bool IsTopZone
        {
            get => _isTopZone;
            set
            {
                _isTopZone = value;
                OnPropertyChanged(nameof(IsTopZone));
                OnPropertyChanged(nameof(MaxArea));
            }
        }

        public bool IsBottomZone
        {
            get => _isBottomZone;
            set
            {
                _isBottomZone = value;
                OnPropertyChanged(nameof(IsBottomZone));
                OnPropertyChanged(nameof(MaxArea));
            }
        }


        private RelayCommand _setIsTopZone;
        private RelayCommand _setIsBottomZone;
        private RelayCommand _save;
        private bool _isTopZone = true;
        private bool _isBottomZone;

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
                if (parse && number >= MinArea && number <= MaxArea || !IsEdit)
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
                if (parse && number > 0 && number < _area)
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

        public RelayCommand Save => _save ??= new RelayCommand(_ =>
        {
            var apartment = new ApartmentViewModel
            {
                Area = _area.ToString(),
                LivingArea = _livingArea.ToString(),
                LivingPeopleNumber = _livingPeopleNumber.ToString(),
                Number = _number.ToString(),
                RoomsNumber = _roomsNumber.ToString(),
                Zone = IsTopZone ? FloorZone.Top : FloorZone.Bottom
            };
            DialogHost.Close("rootDialog", apartment);
        }, null);

        public RelayCommand SetIsTopZone => _setIsTopZone ??= new RelayCommand(_ =>
        {
            IsBottomZone = false;
            IsTopZone = true;
        }, null);

        public RelayCommand SetIsBottomZone => _setIsBottomZone ??= new RelayCommand(_ =>
        {
            IsBottomZone = true;
            IsTopZone = false;
        }, null);
    }
}