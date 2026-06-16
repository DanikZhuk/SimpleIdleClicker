using System;
using System.Collections.Generic;
using Gameplay.Estates.Generic;
using UI.EstatePage.EstateViews.Renovation;

namespace Gameplay.Estates.Renovation
{
    public class RenovationEstate : Estate
    {
        public List<House> HouseOffers;
        public List<House> PurchasedHouses;
        public DateTime TimeData;

        public RenovationEstate(string name, EstateType type, long sellPrice) : base(name, type, sellPrice)
        {
        }

        public RenovationEstate()
        {
        }
    }
}