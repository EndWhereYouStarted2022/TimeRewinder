using System.Collections.Generic;
namespace DFramework
{
    public class AssetBundleData
    {
        public string Name;
        public List<AssetData> assetDataList = new List<AssetData>();
        public string[] dependencyNames;
    }
}
