using UnityEngine;
namespace Config
{
    public static class ExcelConfig
    {
        ///Excel文件夹路径
        public static readonly string ExcelPath = Application.dataPath + "/Excel/";
        ///导出的Json文件存储路径
        public static readonly string ClintPath = Application.dataPath + "/Scripts/Config/Json/";
        ///Class结构类C#文件储存路径
        public static readonly string ClassPath = Application.dataPath + "/Scripts/Config/CS/";

        ///记录行的个数 不填或者-1为自动读取 
        public static readonly int[] RowCountLine = new int[2] { 0, 1 };
        ///记录列的个数 不填或者-1为自动读取 
        public static readonly int[] ColumnCountLine = new int[2] { 0, 3 };
        ///表格说明行
        public const int SHEET_EXPLAIN_LINE = 0;
        ///注释所在行
        public const int COMMENTS_LINE = 1;
        ///变量名所在行
        public const int VARIABLE_NAME_LINE = 2;
        ///变量类型所在行数
        public const int VARIABLE_TYPE_LINE = 3;
        ///数据开始行数
        public const int DATA_LINE = 4;

        ///结构类所在命名空间
        public const string EXCEL_CLASS_NAMESPACE = "Config";
        ///子工作表结构类扩展名
        public const string SHEET_EXTENSION_NAME = "Item";
        
    }
}
