using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaterialDesignThemes.Wpf;
using ragoz_oop_1.Components;
using ragoz_oop_1.Enums;
using ragoz_oop_1.models;

namespace ragoz_oop_1.ViewModels
{
    public class FloorViewModel : BaseViewModel
    {
        private int _houseNumber;
        private string _street;
        private int _floorNum;
        private int _area;
        private int _apartmentsNum;
        public ObservableCollection<ApartmentViewModel> Apartments { get; set; } = new ObservableCollection<ApartmentViewModel>();
        public int FloorHeight => _area / 500;
        public int FloorWidth => 500;

        private float magicCoefficient => 0.35f;

        public ObservableCollection<ApartmentViewModel> ApartmentsZoneTop =>
            new ObservableCollection<ApartmentViewModel>(Apartments.Where(apartment => FloorZone.Top.Equals(apartment.Zone)));
        
        public ObservableCollection<ApartmentViewModel> ApartmentsZoneBottom =>
            new ObservableCollection<ApartmentViewModel>(Apartments.Where(apartment => FloorZone.Bottom.Equals(apartment.Zone)));
        
        private RelayCommand _addApartment;
        private RelayCommand _editApartment;
        private RelayCommand _deleteApartment;
        private RelayCommand _importData;
        private RelayCommand _exportData;
        public string HouseNumber
        {
            get => _houseNumber.ToString();
            set
            {
                var parse = int.TryParse(value, out var number);
                if (parse && number > 0)
                {
                    _houseNumber = number;
                    OnPropertyChanged(nameof(HouseNumber));
                }
            }
        }

        public string Street
        {
            get => _street;
            set
            {
                _street = value;
                OnPropertyChanged(nameof(Street));
            }
        }

        public string FloorNum
        {
            get => _floorNum.ToString();
            set
            {
                var parse = int.TryParse(value, out var number);
                if (parse && number > 0)
                {
                    _floorNum = number;
                    OnPropertyChanged(nameof(FloorNum));
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
                    OnPropertyChanged(nameof(FloorHeight));
                    OnPropertyChanged(nameof(FloorWidth));
                }
            }
        }

        public string ApartmentsNum
        {
            get => _apartmentsNum.ToString();
            set
            {
                var parse = int.TryParse(value, out var number);
                if (parse && number > 0)
                {
                    _apartmentsNum = number;
                    OnPropertyChanged(nameof(ApartmentsNum));
                }
            }
        }
        
        public RelayCommand AddApartment => _addApartment ??= new RelayCommand(async _ =>
        {
            var maxArea = (int) (_area * magicCoefficient);
            var sumTopArea = ApartmentsZoneTop.Sum(item => int.Parse(item.Area));
            var sumBottomArea = ApartmentsZoneBottom.Sum(item => int.Parse(item.Area));

            var dialog = new ApartmentModal
            {
                DataContext = new ApartmentModalViewModel
                {
                    IsEdit = true,
                    MinArea = (int) (FloorHeight * magicCoefficient),
                    TopZoneFreeArea = maxArea - sumTopArea,
                    BottomZoneFreeArea = maxArea - sumBottomArea,
                }
            };
            var result =  await DialogHost.Show(dialog, "rootDialog");
            if (result is ApartmentViewModel newApartment)
            {
                newApartment.ApartmentHeight = (int) (FloorHeight * magicCoefficient);
                Apartments.Add(newApartment);
                OnPropertyChanged(FloorZone.Top.Equals(newApartment.Zone)
                    ? nameof(ApartmentsZoneTop)
                    : nameof(ApartmentsZoneBottom));
            }
        }, null);
        public RelayCommand EditApartment => _editApartment ??= new RelayCommand(async param =>
        {
            if (param is ApartmentViewModel currentApartment)
            {
                var maxArea = (int) (_area * magicCoefficient);
                var sumTopArea = ApartmentsZoneTop.Sum(item => int.Parse(item.Area));
                var sumBottomArea = ApartmentsZoneBottom.Sum(item => int.Parse(item.Area));
                if (FloorZone.Top.Equals(currentApartment.Zone))
                {
                    sumTopArea -= int.Parse(currentApartment.Area);
                }
                else
                {
                    sumBottomArea -= int.Parse(currentApartment.Area);
                }

                var dialog = new ApartmentModal
                {
                    DataContext = new ApartmentModalViewModel
                    {
                        IsEdit = true,
                        MinArea = (int) (FloorHeight * magicCoefficient),
                        TopZoneFreeArea = maxArea - sumTopArea,
                        BottomZoneFreeArea = maxArea - sumBottomArea,
                        Area = currentApartment.Area,
                        LivingArea = currentApartment.LivingArea,
                        LivingPeopleNumber = currentApartment.LivingPeopleNumber,
                        Number = currentApartment.Number,
                        RoomsNumber = currentApartment.RoomsNumber,
                        IsBottomZone = FloorZone.Bottom.Equals(currentApartment.Zone),
                        IsTopZone = FloorZone.Top.Equals(currentApartment.Zone),
                    }
                };
                var result =  await DialogHost.Show(dialog, "rootDialog");
                if (result is ApartmentViewModel newApartment)
                {
                    newApartment.ApartmentHeight = (int) (FloorHeight * magicCoefficient);
                    Apartments.Insert(Apartments.IndexOf(currentApartment), newApartment);
                    Apartments.Remove(currentApartment);
                    OnPropertyChanged(nameof(ApartmentsZoneTop));
                    OnPropertyChanged(nameof(ApartmentsZoneBottom));
                }
            }
                    
        }, null);

        public RelayCommand DeleteApartment => _deleteApartment ??= new RelayCommand(param =>
        {
            if (param is ApartmentViewModel apartment)
            {
                Apartments.Remove(apartment);
                OnPropertyChanged(nameof(ApartmentsZoneTop));
                OnPropertyChanged(nameof(ApartmentsZoneBottom));
            }
        }, null);

        public RelayCommand ImportData => _importData ??= new RelayCommand(_ =>
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var pathToFile = Path.Combine(directory, "FloorApp", "floorApp.save");
            var isFileExists = File.Exists(pathToFile);
            if (isFileExists)
            {
                LoadData(pathToFile);
            }
        }, null);

        public RelayCommand ExportData => _exportData ??= new RelayCommand(_ =>
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var pathToFile = Path.Combine(directory, "FloorApp", "floorApp.save");
            var isFileExists = File.Exists(pathToFile);
            if (!isFileExists)
            {
                Directory.CreateDirectory(Path.Combine(directory, "FloorApp"));
                File.Create(pathToFile);
            }
            SaveData(pathToFile);
            
        }, null);

        private void LoadData(string pathToFile)
        {
            var jsonString = File.ReadAllText(pathToFile);
            var deserializedObject = JsonSerializer.Deserialize<FloorViewModel>(jsonString);
            _area = deserializedObject._area;
            _street = deserializedObject._street;
            _apartmentsNum = deserializedObject._apartmentsNum;
            _floorNum = deserializedObject._floorNum;
            _houseNumber = deserializedObject._houseNumber;
            Apartments = deserializedObject.Apartments;
            RefreshAllProperties();
        }

        private void SaveData(string pathToFile)
        {
            var jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(pathToFile, jsonString);
        }

        private void RefreshAllProperties()
        {
            OnPropertyChanged(nameof(HouseNumber));
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(Apartments));
            OnPropertyChanged(nameof(ApartmentsZoneBottom));
            OnPropertyChanged(nameof(ApartmentsZoneTop));
            OnPropertyChanged(nameof(ApartmentsNum));
            OnPropertyChanged(nameof(Street));
            OnPropertyChanged(nameof(FloorNum));
            OnPropertyChanged(nameof(FloorHeight));
            OnPropertyChanged(nameof(FloorWidth));
        }
    }
}