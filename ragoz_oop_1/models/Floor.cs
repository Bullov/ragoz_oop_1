using System.Collections.Generic;

namespace ragoz_oop_1.models
{
    public class Floor
    {
        public int HouseNumber { get; set; }
        public string Street { get; set; }
        public int FloorNum { get; set; }
        public int Area { get; set; }
        public int ApartmentsNum { get; set; }
        public List<Apartment> Apartments { get; set; }
    }
}