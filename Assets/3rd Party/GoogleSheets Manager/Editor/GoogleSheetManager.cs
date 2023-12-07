using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoogleSheetManager
{
    [CreateAssetMenu(fileName = "Data", menuName = "GoogleSheets/GoogleSheetManager", order = 1)]
    public class GoogleSheetManager : ScriptableObject
    {
        public string SpreadSheetID;
        public string DatabasePath = "Assets/GameData/";

        [SerializeField]
        public List<DataSheet> DataSheets = new List<DataSheet>();
    }

    [System.Serializable]
    public class DataSheet
    {
        public string DatabaseName;
        public string TabId;
        public string CSV;
        public bool IsUptodate = false;
        public SpreadsheetDatabase DatabaseObject;
    }
}