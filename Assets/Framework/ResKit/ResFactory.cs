
namespace DFramework.Framework.ResKit
{
    public class ResFactory
    {
        /// <summary>
        /// 生成Res
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="ownerBundle"></param>
        /// <returns></returns>
        public static Res Create(string assetName,string ownerBundle)
        {
            Res res = null; 
            if (ownerBundle != null)
            {
                res = new AssetRes(assetName, ownerBundle);
            }
            else if (assetName.StartsWith("resources://"))
            {
                res = new ResourcesRes(assetName);
            }
            else
            {
                res = new AssetBundleRes(assetName);
            }
            return res;
        }
    }
}
