using System;
using System.Collections.Generic;
using Configs;
using Gameplay.Businesses.BusinessControllers.RepairShop.House.Model;
using Gameplay.Businesses.Generic.Models;

namespace Gameplay.Businesses.BusinessModels
{
    public class RepairShopBusinessModel: BusinessModel
    {
        public List<HouseModel> HouseOffers;
        public List<HouseModel> PurchasedHouses;
        public DateTime TimeData;

        public RepairShopBusinessModel(BusinessConfig businessConfig, string businessName) : base(businessConfig, businessName)
        {
            HouseOffers = new List<HouseModel>();
            PurchasedHouses = new List<HouseModel>();
        }

        public RepairShopBusinessModel()
        {
        }
    }
}