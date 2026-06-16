using System;
using System.Collections.Generic;
using Gameplay.Estates.Generic;
using Gameplay.Investitions;
using UI.EstatePage.EstateViews.Renovation;

namespace Core.SaveSystem
{
    public interface IDataService
    {
        #region GeneralData
        public long Money { get; set; }
        #endregion
        #region Estates
        public List<Estate> Estates { get; }
        public void AddEstate(Estate estate);
        public void RemoveEstate(Estate estate);
        #endregion
        #region Investitions
        public List<Investition> Investitions { get; }
        #endregion
    }
}