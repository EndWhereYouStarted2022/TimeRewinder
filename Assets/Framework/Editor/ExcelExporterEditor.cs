#if UNITY_EDITOR
using System;
using UnityEngine;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Config;
using System.Globalization;
using UnityEditor;

namespace DFramework
{
    public struct CellInfo
    {
        public string Type;
        public string Name;
        public string Description;
    }

    public class ExcelExporterEditor
    {
        private const string K_EXCEL_EXPORT_PROPERTY_PATH = "DFramework/Framework/ExcelTools/";
        private const string K_EXCEL_EXPORT_PROPERTY_KEY = "Excel Property";
        private const string K_EXCEL_DELETE_ALL_FILES_KEY = "Excel DeleteOldFiles";
        private static int _ExcelExportProperty = -1;
        private static int _ExcelDeleteAllOldFiles = -1;
        private static bool ExportProperty
        {
            get
            {
                if (_ExcelExportProperty == -1)
                {
                    _ExcelExportProperty = EditorPrefs.GetBool(K_EXCEL_EXPORT_PROPERTY_KEY, true) ? 1 : 0;
                }
                return _ExcelExportProperty != 0;
            }
            set
            {
                _ExcelExportProperty = value ? 1 : 0;
                EditorPrefs.SetBool(K_EXCEL_EXPORT_PROPERTY_KEY, value);
            }
        }
        private static bool DeleteAllOldFiles
        {
            get
            {
                if (_ExcelDeleteAllOldFiles == -1)
                {
                    _ExcelDeleteAllOldFiles = EditorPrefs.GetBool(K_EXCEL_DELETE_ALL_FILES_KEY, true) ? 1 : 0;
                }
                return _ExcelDeleteAllOldFiles != 0;
            }
            set
            {
                _ExcelDeleteAllOldFiles = value ? 1 : 0;
                EditorPrefs.SetBool(K_EXCEL_DELETE_ALL_FILES_KEY, value);
            }
        }


        [MenuItem(K_EXCEL_EXPORT_PROPERTY_PATH + "导出成属性类")]
        private static void ToggleExportAsProperty()
        {
            ExportProperty = !ExportProperty;
        }

        [MenuItem(K_EXCEL_EXPORT_PROPERTY_PATH + "导出成属性类", true, 101)]
        private static bool ToggleExportAsPropertyValidate()
        {
            Menu.SetChecked(K_EXCEL_EXPORT_PROPERTY_PATH + "导出成属性类", ExportProperty);
            return true;
        }

        [MenuItem(K_EXCEL_EXPORT_PROPERTY_PATH + "删除旧文件")]
        private static void ToggleDeleteAllFiles()
        {
            DeleteAllOldFiles = !DeleteAllOldFiles;
        }
        [MenuItem(K_EXCEL_EXPORT_PROPERTY_PATH + "删除旧文件", true, 102)]
        private static bool ToggleToggleDeleteAllFilesValidate()
        {
            Menu.SetChecked(K_EXCEL_EXPORT_PROPERTY_PATH + "删除旧文件", DeleteAllOldFiles);
            return true;
        }


        [MenuItem(K_EXCEL_EXPORT_PROPERTY_PATH + "ExportAll", false, 1)]
        private static void ExportConfigs()
        {
            try
            {
                Debug.Log("正在生成Json文件...");
                ExportConfigs(ExcelConfig.ClintPath);
                Debug.Log("正在构建结构类...");
                ExportAllClass(ExcelConfig.ClassPath, ExcelConfig.EXCEL_CLASS_NAMESPACE);
                Debug.Log("Excel导出完成...");
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError(e.ToString());
            }
        }

        private static void ExportConfigs(string exportDir)
        {
            if (!Directory.Exists(exportDir))
            {
                Directory.CreateDirectory(exportDir);
            }
            else if (DeleteAllOldFiles)
            {
                var dir = new DirectoryInfo(exportDir);
                var files = dir.GetFiles("*.txt", SearchOption.AllDirectories);
                Debug.Log($"{exportDir}路径下的文件夹中有{files.Length}个旧.txt文件");
                foreach (var file in files)
                {
                    var oldFilePath = exportDir + "/" + file.Name;
                    Debug.Log($"正在删除 {file.Name} 文件");
                    File.Delete(oldFilePath);
                }
            }
            var excelCount = Directory.GetFiles(ExcelConfig.ExcelPath).Length;
            var count = 0;
            EditorUtility.DisplayProgressBar("Export Json form Excel", "正在导出Json文件...", (float)count / excelCount);

            //遍历Excel表文件夹
            foreach (var path in Directory.GetFiles(ExcelConfig.ExcelPath))
            {
                count++;
                var filePath = path.Replace("\\", "/");
                //跳过不是表格文件的文件
                if (Path.GetExtension(filePath) != ".xlsx" && Path.GetExtension(filePath) != ".xls") continue;
                //跳过表格缓存文件
                if (Path.GetFileName(filePath).StartsWith("~")) continue;

                var fileName = Path.GetFileName(filePath);
                EditorUtility.DisplayProgressBar("Export Json form Excel", $"正在导出{fileName}的Json文件", (float)count / excelCount);

                ExportJson(filePath, exportDir);
            }
            EditorUtility.ClearProgressBar();
        }

        private static void ExportAllClass(string exportDir, string csNameSpace)
        {
            if (!Directory.Exists(exportDir))
            {
                Directory.CreateDirectory(exportDir);
            }
            else if (DeleteAllOldFiles)
            {
                var dir = new DirectoryInfo(exportDir);
                var files = dir.GetFiles("*.cs", SearchOption.AllDirectories);
                Debug.Log($"{exportDir}路径下的文件夹中有{files.Length}个旧.cs文件");
                foreach (var file in files)
                {
                    var oldFilePath = exportDir + "/" + file.Name;
                    Debug.Log($"正在删除 {file.Name} 文件");
                    File.Delete(oldFilePath);
                }
            }
            var excelCount = Directory.GetFiles(ExcelConfig.ExcelPath).Length;
            var count = 0;
            EditorUtility.DisplayProgressBar("Export CSharp Class form Excel", "正在导出结构类...", (float)count / excelCount);

            foreach (var path in Directory.GetFiles(ExcelConfig.ExcelPath))
            {
                count++;
                var filePath = path.Replace("\\", "/");
                //跳过不是表格文件的文件
                if (Path.GetExtension(filePath) != ".xlsx" && Path.GetExtension(filePath) != ".xls") continue;
                //跳过表格缓存文件
                if (Path.GetFileName(filePath).StartsWith("~")) continue;

                var fileName = Path.GetFileName(filePath);
                EditorUtility.DisplayProgressBar("Export CSharp Class form Excel", $"正在导出{fileName}的结构类文件...", (float)count / excelCount);
                ExportClass(filePath, exportDir, csNameSpace);
            }
            EditorUtility.ClearProgressBar();
        }

        private static void ExportClass(string filePath, string exportDir, string csNameSpace)
        {
            var xssfWorkbook = new XSSFWorkbook(filePath);
            //文件名（不带后缀）
            var protoName = Path.GetFileNameWithoutExtension(filePath);
            //CS文件路径
            var exportPath = Path.Combine(exportDir, $"{protoName}.cs");
            if (!DeleteAllOldFiles && File.Exists(exportPath))
            {
                Debug.Log($"删除旧的 {protoName}.cs 文件...");
                File.Delete(exportPath);
            }

            var txt = new FileStream(exportPath, FileMode.Create);
            var sw = new StreamWriter(txt);

            var sb = new StringBuilder();
            sb.Append("using System.Collections.Generic;\n");
            sb.Append($"namespace {csNameSpace}\n");
            sb.Append("{\n\t[System.Serializable]\n");
            sb.Append($"\tpublic class {protoName}\n");
            sb.Append("\t{");
            sw.WriteLine(sb);

            #region Excel 表格主类
            for (var i = 0; i < xssfWorkbook.NumberOfSheets; i++)
            {
                var sheet = xssfWorkbook.GetSheetAt(i);
                var sheetName = sheet.SheetName;
                sb.Clear();
                sb.Append($"\t\t///{sheet.GetRow(ExcelConfig.SHEET_EXPLAIN_LINE).GetCell(0)}\n");
                sb.Append($"\t\tpublic List<{sheetName}{ExcelConfig.SHEET_EXTENSION_NAME}> {sheetName} ");
                sb.Append(ExportProperty ? "{ get; set; }\n" : ";\n");
                sw.WriteLine(sb);
            }
            sb.Clear();
            sb.Append("\t}\n\n");
            sw.WriteLine(sb);
            #endregion

            #region 各工作表类
            for (var i = 0; i < xssfWorkbook.NumberOfSheets; i++)
            {
                var sheet = xssfWorkbook.GetSheetAt(i);
                var itemSb = new StringBuilder();
                // var cellCount = (int)sheet.GetRow(ExcelConfig.VARIABLE_NAME_LINE).LastCellNum;
                var cellCount = GetSheetColumns(sheet);
                itemSb.Append("\t[System.Serializable]\n");
                itemSb.Append($"\tpublic class {sheet.SheetName}{ExcelConfig.SHEET_EXTENSION_NAME}\n");
                itemSb.Append("\t{\n");
                for (var j = 0; j < cellCount; j++)
                {
                    itemSb.Append($"\t\t///{sheet.GetRow(ExcelConfig.COMMENTS_LINE).GetCell(j)}\n");
                    itemSb.Append($"\t\tpublic {sheet.GetRow(ExcelConfig.VARIABLE_TYPE_LINE).GetCell(j)} ");
                    itemSb.Append($"{sheet.GetRow(ExcelConfig.VARIABLE_NAME_LINE).GetCell(j)} ");
                    itemSb.Append(ExportProperty ? "{ get; set; }\n" : ";\n");
                }
                itemSb.Append("\t}\n");
                sw.WriteLine(itemSb.ToString());
            }
            #endregion

            sb.Clear();
            sb.Append("}");
            sw.WriteLine(sb);
            sw.Close();
            txt.Close();
        }


        private static void ExportJson(string filePath, string exportDir)
        {
            //excel 2007之前的版本
            // using var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            // var xssfWorkbook = new XSSFWorkbook();
            var xssfWorkbook = new XSSFWorkbook(filePath);
            var protoName = Path.GetFileNameWithoutExtension(filePath);
            var exportPath = Path.Combine(exportDir, $"{protoName}.txt");
            if (!DeleteAllOldFiles && File.Exists(exportPath))
            {
                Debug.Log($"删除旧的 {protoName}.txt 文件...");
                File.Delete(exportPath);
            }

            var txt = new FileStream(exportPath, FileMode.Create);
            var sw = new StreamWriter(txt);

            sw.WriteLine("{\n");
            for (var i = 0; i < xssfWorkbook.NumberOfSheets; i++)
            {
                var sheet = xssfWorkbook.GetSheetAt(i);
                var sb = new StringBuilder();
                sb.Append($"\"{sheet.SheetName}\":[");
                sw.WriteLine(sb.ToString());
                ExportSheet(sheet, sw);
                sb.Clear();
                sb.Append("]\n");
                if (i < xssfWorkbook.NumberOfSheets - 1)
                {
                    sb.Append(",");
                }
                sw.WriteLine(sb.ToString());
            }
            sw.WriteLine("}");
            sw.Close();
            txt.Close();
        }

        private static bool CheckCellInvalid(ICell cell)
        {
            return cell.CellType == CellType.Blank || cell.CellType == CellType.Error || cell.CellType == CellType.Unknown;
        }
        
        private static int GetSheetRows(ISheet sheet)
        {
            // Debug.LogError("********* " + sheet.SheetName);
            var realCount = sheet.LastRowNum + 1;
            for (var i = 0; i < realCount; i++)
            {
                if (i < ExcelConfig.DATA_LINE) continue;
                var cell = sheet.GetRow(i).GetCell(0);
                if (cell == null)
                {
                    return i;
                }
                if (CheckCellInvalid(cell))
                    return i;
            }
            return realCount;
        }

        private static int GetSheetColumns(ISheet sheet)
        {

            var nameRow = sheet.GetRow(ExcelConfig.VARIABLE_NAME_LINE);
            var typeRow = sheet.GetRow(ExcelConfig.VARIABLE_TYPE_LINE);
            var realCount = nameRow.LastCellNum;
            for (var i = 0; i < realCount; i++)
            {
                if (CheckCellInvalid(nameRow.GetCell(i)) || CheckCellInvalid(typeRow.GetCell(i)))
                {
                    return i;
                }
            }
            return realCount;
        }

        /// <summary>
        /// 导表Json
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sw"></param>
        private static void ExportSheet(ISheet sheet, TextWriter sw)
        {
            //变量名列数
            // var cellCount = (int)sheet.GetRow(ExcelConfig.VARIABLE_NAME_LINE).LastCellNum;
            // var countStr = GetCellString(sheet, ExcelConfig.ColumnCountLine[0], ExcelConfig.ColumnCountLine[1]);
            // int.TryParse(countStr, out var cellCount);
            // if (cellCount == 0 || cellCount == -1)
            // {
            //     cellCount = (int)sheet.GetRow(ExcelConfig.VARIABLE_NAME_LINE).LastCellNum;
            // }
            var columns = GetSheetColumns(sheet);

            var cellInfos = new CellInfo[columns];

            var commentsCells = sheet.GetRow(ExcelConfig.COMMENTS_LINE);
            var variableNameCells = sheet.GetRow(ExcelConfig.VARIABLE_NAME_LINE);
            var variableTypeCells = sheet.GetRow(ExcelConfig.VARIABLE_TYPE_LINE);
            for (var i = 0; i < columns; i++)
            {
                // var fieldDesc = GetCellString(sheet, CommentsLine, i);
                // var fieldName = GetCellString(sheet, VariableNameLine, i);
                // var fieldType = GetCellString(sheet, VariableTypeLine, i);
                var fieldDesc = commentsCells.GetCell(i).ToString();
                var fieldName = variableNameCells.GetCell(i).ToString();
                var fieldType = variableTypeCells.GetCell(i).ToString();
                cellInfos[i] = new CellInfo() { Name = fieldName, Type = fieldType, Description = fieldDesc };
            }
            // var lastRowStr = GetCellString(sheet, ExcelConfig.RowCountLine[0], ExcelConfig.RowCountLine[1]);
            // int.TryParse(lastRowStr, out var lastRowNum);
            // if (lastRowNum == 0 || lastRowNum == -1)
            // {
            //     lastRowNum = sheet.LastRowNum;
            // }
            var rows = GetSheetRows(sheet);

            for (var i = ExcelConfig.DATA_LINE; i < rows; i++)
            {
                var sb = new StringBuilder();
                sb.Append("{\n");
                var row = sheet.GetRow(i);
                for (var j = 0; j < columns; ++j)
                {
                    var desc = cellInfos[j].Description.ToLower();
                    if (desc.StartsWith("#")) continue;
                    var fieldValue = GetCellString(row, j);
                    var fieldType = cellInfos[j].Type;
                    if (fieldType.Equals(string.Empty))
                    {
                        Debug.Log("空类型");
                        continue;
                    }
                    if (fieldValue.Equals(string.Empty))
                    {
                        Debug.Log($"sheet:{sheet.SheetName} 中有空白字段[{i},{j}]");
                        // throw new Exception($"sheet:{sheet.SheetName} 中有空白字段[{i},{j}]");
                        // continue;
                    }
                    if (j > 0) sb.Append(",");
                    var fieldName = cellInfos[j].Name;
                    if (fieldName == "Id" || fieldName == "_id")
                    {
                        fieldName = "Id";
                    }
                    // 现在不支持默认参数，必须不能为空
                    // if (fieldType == "int" && fieldValue == "") fieldValue = "0";
                    sb.Append($"\"{fieldName}\":{Convert(fieldType, fieldValue)}");
                }
                sb.Append(i == sheet.LastRowNum ? "\n}" : "\n},");
                sw.WriteLine(sb.ToString());
            }
        }

        private static string Convert(string type, string value)
        {
            switch (type)
            {
                case "int[]":
                case "int32[]":
                case "long[]":
                    return $"[{value}]";
                case "string[]":
                    return $"[{value}]";
                case "int":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                    return value;
                case "string":
                    return $"\"{value}\"";
                case "int[,]":
                case "float[,]":
                case "double[,]":
                case "string[,]":
                    return $"[{value}]";
                default:
                    throw new Exception($"不支持此类型：{type}");
            }
        }

        private static string GetCellString(ISheet sheet, int i, int j)
        {
            return sheet.GetRow(i)?.ToString() ?? "";
        }

        private static string GetCellString(IRow row, int i)
        {
            return row?.GetCell(i)?.ToString() ?? "";
        }

        private static string GetCellString(ICell cell)
        {
            return cell?.ToString() ?? "";
        }

    }
}
#endif
