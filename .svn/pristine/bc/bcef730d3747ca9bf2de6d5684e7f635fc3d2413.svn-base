using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JZ.IMS.Core.Utilities;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using Microsoft.Extensions.Localization;
using JZ.IMS.Core.Extensions;
using JZ.IMS.IRepository;
using System.Text;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// EPPlus助手类
    /// </summary>
    public class EPPlusHelper
    {
        /// <summary>
        /// 创建Excel模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList">数据</param>
        /// <param name="tpl_name">表名称</param>
        /// <param name="_repository">来源仓储层</param>
        /// <returns></returns>
        public static string CreateExcelTemplate(List<ImportDtl> dataList, string tpl_name, IImportDtlRepository _repository)
        {
            var webPath = string.Empty;
            try
            {
                string sWebRootFolder = Path.Combine(AppContext.BaseDirectory, "upload", "exceltpl");
                if (!Directory.Exists(sWebRootFolder))
                {
                    Directory.CreateDirectory(sWebRootFolder);
                }
                string upFileName = $@"{tpl_name}_up.xlsx"; //上传的模板
                var up_path = Path.Combine(sWebRootFolder, upFileName);
                webPath = $"/upload/exceltpl/{upFileName}";
                FileInfo up_file = new FileInfo(up_path);
                if (up_file.Exists)
                {
                    //直接返回上传的模板
                    return webPath;
                }

                string sFileName = $@"{tpl_name}_模板.xlsx";  //{DateTime.Now.ToString("yyyyMMddHHmm")} 
                var path = Path.Combine(sWebRootFolder, sFileName);
                webPath = $"/upload/exceltpl/{sFileName}";
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(path);
                }
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    //创建sheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("sheet1");
                    //worksheet.Cells.LoadFromCollection(dataList, true);
                    //表头字段
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataList[i].COLUMN_CAPTION;
                        worksheet.Column(i + 1).Width = 15;

                        worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //默认白色背景，黑色字体
                        Color backgroundColor = Color.White;
                        //字体颜色
                        Color fontColor = Color.Black;
                        //下载模板并且是必填项，将表格设置为黄色
                        if (dataList[i].IS_UNIQUE == 1)
                        {
                            backgroundColor = Color.Yellow;  //黄色唯一
                        }
                        else if (dataList[i].ISNULL_ABLE == 0)
                        {
                            backgroundColor = Color.LightGreen;  //浅绿色必填
                        }
                        else
                        {
                            backgroundColor = Color.White;
                        }
                        //下拉选项
                        if (!dataList[i].LISTVALIDATION_SQL.IsNullOrWhiteSpace())
                        {
                            string cell_address = string.Format("{0}:{0}", dataList[i].EXCEL_ITEM);
                            var listValidation = worksheet.DataValidations.AddListValidation(cell_address);
                            //加载下拉选项来源数据
                            var list = _repository.QueryEx<dynamic>(dataList[i].LISTVALIDATION_SQL);
                            if (list != null && list.Count > 0)
                            {
                                var sb = new StringBuilder();
                                foreach (var item in list)
                                {
                                    var val_caption = ((object[])((IDictionary<string, object>)item).Values)[0];
                                    string value = Convert.ToString(val_caption) ?? string.Empty;
                                    value = value.Trim();
                                    if (listValidation.Formula.Values.IndexOf(value) == -1)
                                    {
                                        //EXCEL下拉选项 总长度不能超过255个字符 
                                        if ((sb.ToString().Length + value.Length) < 254)
                                        {
                                            listValidation.Formula.Values.Add(value);
                                            sb.Append(value).Append(",");
                                        }
                                        else 
                                            break;
                                    }
                                }
                            }
                        }
                        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(backgroundColor);  //背景色
                        worksheet.Cells[1, i + 1].Style.Font.Color.SetColor(fontColor);//字体颜色

                        //设置边框的属性
                        var border = worksheet.Cells[1, i + 1].Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        border.Top.Color.SetColor(System.Drawing.Color.Black);
                        border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        border.Left.Color.SetColor(System.Drawing.Color.Black);
                        border.Right.Color.SetColor(System.Drawing.Color.Black);

                        //对齐方式
                        worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        //单元格自动适应大小
                        worksheet.Cells[1, i + 1].Style.ShrinkToFit = true;
                    }
                    //自动列宽
                    //worksheet.Cells.AutoFitColumns();
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                webPath = string.Empty;
                throw new Exception(ex.Message);
            }
            return webPath;
        }

        /// <summary>
        /// 从Excel写入数据列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tplList">模板列表</param>
        /// <param name="_localizer">本地化</param>
        /// <returns></returns>
        public static WebResponseContent WriteToDataList(string path, List<ImportDtl> tplList, IStringLocalizer<ImportExcelController> _localizer)
        {
            WebResponseContent responseContent = new WebResponseContent();
            string errmsg = string.Empty;

            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                //"未找到上传的文件,请重新上传。"
                return responseContent.Error(_localizer["nofind_file"]);
            }

            List<ImportExcelItem> entities = new List<ImportExcelItem>();
            using (ExcelPackage package = new ExcelPackage(file))
            {
                if (package.Workbook.Worksheets.Count == 0 ||
                    package.Workbook.Worksheets.FirstOrDefault().Dimension.End.Row <= 1)
                {
                    //未导入数据。
                    return responseContent.Error(_localizer["no_data_error"]);
                }

                ExcelWorksheet sheet = package.Workbook.Worksheets.FirstOrDefault();
                int columns = sheet.Dimension.Columns;
                if (columns > 60)
                {
                    //"导入文件有列数太多."
                    return responseContent.Error(_localizer["data_column_error"]);
                }

                for (int j = sheet.Dimension.Start.Column, k = sheet.Dimension.End.Column; j <= k; j++)
                {
                    string columnCNName = sheet.Cells[1, j].Value?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(columnCNName))
                    {
                        var options = tplList.Where(t => t.COLUMN_CAPTION.Trim() == columnCNName.Trim()).FirstOrDefault();
                        if (options == null)
                        {
                            //"导入文件列[{0}]不是模板中的列."  站点ID
                            errmsg = string.Format(_localizer["import_column_error"], columnCNName);
                            return responseContent.Error(errmsg);
                        }
                    }
                }

                PropertyInfo[] propertyInfos = typeof(ImportExcelItem).GetProperties().ToArray();
                for (int m = sheet.Dimension.Start.Row + 1, n = sheet.Dimension.End.Row; m <= n; m++)
                {
                    ImportExcelItem entity = Activator.CreateInstance<ImportExcelItem>();
                    for (int j = sheet.Dimension.Start.Column, k = sheet.Dimension.End.Column; j <= k; j++)
                    {
                        string value = sheet.Cells[m, j].Value?.ToString().Trim();
                        int col_idx = j - sheet.Dimension.Start.Column + 1;
                        string columnName = "Column" + Convert.ToString(col_idx);

                        PropertyInfo property = propertyInfos.Where(x => x.Name == columnName).FirstOrDefault();

                        if (tplList[col_idx - 1].ISNULL_ABLE == 0 && string.IsNullOrEmpty(value))
                        {
                            //$"第{0}行[{1}]验证未通过,不能为空。"
                            errmsg = string.Format(_localizer["column_check_error"], m, tplList[col_idx - 1].COLUMN_CAPTION);
                            return responseContent.Error(errmsg);
                        }
                        property.SetValue(entity, value);
                    }
                    entities.Add(entity);
                }
            }

            try
            {
                //删除文件
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch (Exception)
            {
            }

            return responseContent.OK(null, entities);
        }

        /// <summary>
        /// 加载CSV文件
        /// </summary>
        private void LoadCSVFile()
        {
            List<String> lines = new List<String>();
            using (StreamReader reader = new StreamReader("file.csv"))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            //现在你已经获得了CSV的所有行
            //用EPPLUS创建文件
            foreach (String line in lines)
            {
                var values = line.Split(';');
                foreach (String value in values)
                {
                    //use EPPLUS library to fill your file
                }
            }
        }

    }
}
