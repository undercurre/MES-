using System;
using Aspose.Slides;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Words;
using Aspose.Words.Saving;
using System.IO;
using System.Reflection;
using JZ.IMS.Core.Helper;
using Spire.Xls;

namespace JZ.IMS.WebApi.Common
{
    /// <summary>
    /// office转PDF
    /// </summary>
    public class OfficeToPDF
    {

        public const string Key =
            "PExpY2Vuc2U+DQogIDxEYXRhPg0KICAgIDxMaWNlbnNlZFRvPkFzcG9zZSBTY290bGFuZCB" +
            "UZWFtPC9MaWNlbnNlZFRvPg0KICAgIDxFbWFpbFRvPmJpbGx5Lmx1bmRpZUBhc3Bvc2UuY2" +
            "9tPC9FbWFpbFRvPg0KICAgIDxMaWNlbnNlVHlwZT5EZXZlbG9wZXIgT0VNPC9MaWNlbnNlV" +
            "HlwZT4NCiAgICA8TGljZW5zZU5vdGU+TGltaXRlZCB0byAxIGRldmVsb3BlciwgdW5saW1p" +
            "dGVkIHBoeXNpY2FsIGxvY2F0aW9uczwvTGljZW5zZU5vdGU+DQogICAgPE9yZGVySUQ+MTQ" +
            "wNDA4MDUyMzI0PC9PcmRlcklEPg0KICAgIDxVc2VySUQ+OTQyMzY8L1VzZXJJRD4NCiAgIC" +
            "A8T0VNPlRoaXMgaXMgYSByZWRpc3RyaWJ1dGFibGUgbGljZW5zZTwvT0VNPg0KICAgIDxQc" +
            "m9kdWN0cz4NCiAgICAgIDxQcm9kdWN0PkFzcG9zZS5Ub3RhbCBmb3IgLk5FVDwvUHJvZHVj" +
            "dD4NCiAgICA8L1Byb2R1Y3RzPg0KICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl" +
            "0aW9uVHlwZT4NCiAgICA8U2VyaWFsTnVtYmVyPjlhNTk1NDdjLTQxZjAtNDI4Yi1iYTcyLT" +
            "djNDM2OGYxNTFkNzwvU2VyaWFsTnVtYmVyPg0KICAgIDxTdWJzY3JpcHRpb25FeHBpcnk+M" +
            "jAxNTEyMzE8L1N1YnNjcmlwdGlvbkV4cGlyeT4NCiAgICA8TGljZW5zZVZlcnNpb24+My4w" +
            "PC9MaWNlbnNlVmVyc2lvbj4NCiAgICA8TGljZW5zZUluc3RydWN0aW9ucz5odHRwOi8vd3d" +
            "3LmFzcG9zZS5jb20vY29ycG9yYXRlL3B1cmNoYXNlL2xpY2Vuc2UtaW5zdHJ1Y3Rpb25zLm" +
            "FzcHg8L0xpY2Vuc2VJbnN0cnVjdGlvbnM+DQogIDwvRGF0YT4NCiAgPFNpZ25hdHVyZT5GT" +
            "zNQSHNibGdEdDhGNTlzTVQxbDFhbXlpOXFrMlY2RThkUWtJUDdMZFRKU3hEaWJORUZ1MXpP" +
            "aW5RYnFGZkt2L3J1dHR2Y3hvUk9rYzF0VWUwRHRPNmNQMVpmNkowVmVtZ1NZOGkvTFpFQ1R" +
            "Hc3pScUpWUVJaME1vVm5CaHVQQUprNWVsaTdmaFZjRjhoV2QzRTRYUTNMemZtSkN1YWoyTk" +
            "V0ZVJpNUhyZmc9PC9TaWduYXR1cmU+DQo8L0xpY2Vuc2U+";


        /// <summary>
        /// office转pdf
        /// </summary>
        /// <param name="from">开始位置</param>
        /// <param name="to">结束位置</param>
        /// <param name="type">文件类型</param>
        /// <returns></returns>
        public static bool ProcessOfficeToPDF(String from, String to, string type)
        {
            bool result = false;
            type = type.ToUpper().Trim('.');

            try
            {
                //版本:19.10
                if (type.Equals("DOC") || type.Equals("DOCX"))//文件后缀名.docx  
                {
                    result = WordToPDF(from, to);
                }
                else if (type.Equals("XLS") || type.Equals("XLSX") || type.Equals("CSV"))//文件后缀名.xls 
                {
                    //result = ExeclToPDF(from, to);
                    result = PDFHelper.ExeclToPDFEx(from, to);
                }
                else if (type.Equals("PPT") || type.Equals("PPTX"))//文件后缀名.ppt
                {
                    result = PPTToPdfex(from, to);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool ExeclToPDF(String from, String to)
        {
            bool result = false;
            try
            {
                Aspose.Cells.Workbook xls = new Aspose.Cells.Workbook(from);
                Aspose.Cells.PdfSaveOptions xlsSaveOption = new Aspose.Cells.PdfSaveOptions();
                xlsSaveOption.SecurityOptions = new Aspose.Cells.Rendering.PdfSecurity.PdfSecurityOptions();
                #region pdf 加密
                //Set the user password
                //PDF加密功能
                //xlsSaveOption.SecurityOptions.UserPassword = "pdfKey";
                //Set the owner password
                //xlsSaveOption.SecurityOptions.OwnerPassword = "sxbztxmgzxt";
                #endregion
                //Disable extracting content permission
                xlsSaveOption.SecurityOptions.ExtractContentPermission = false;
                //Disable print permission
                xlsSaveOption.SecurityOptions.PrintPermission = false;
                xlsSaveOption.AllColumnsInOnePagePerSheet = true;

                //权限这块的设置成不可复制
                PdfSaveOptions saveOptions = new PdfSaveOptions();
                // Create encryption details and set owner password.
                PdfEncryptionDetails encryptionDetails = new PdfEncryptionDetails(string.Empty, "password", PdfEncryptionAlgorithm.RC4_128);
                // Start by disallowing all permissions.
                encryptionDetails.Permissions = PdfPermissions.DisallowAll;
                // Extend permissions to allow editing or modifying annotations.
                encryptionDetails.Permissions = PdfPermissions.ModifyAnnotations | PdfPermissions.DocumentAssembly;
                saveOptions.EncryptionDetails = encryptionDetails;
                // Render the document to PDF format with the specified permissions.
                //doc.Save(to, saveOptions);

                xls.Save(to, xlsSaveOption);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

       
        public static bool PPTToPdf(string from, string to)
        {
            bool result = false;
            try
            {
                to = Path.GetFileName(to);
                to = System.IO.Path.Combine(@"bin\Debug\netcoreapp2.2\upload\sopfile", to);
                Aspose.Slides.Presentation ppt = new Aspose.Slides.Presentation(from);
                ppt.Save(to, Aspose.Slides.Export.SaveFormat.Pdf);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;

        }

        /// <summary>
        /// ppt转pdf
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="newFilePath">转换后的文件地址</param>
        /// <returns></returns>
        public static bool PPTToPdfex(string path, string newFilePath)
        {
            bool result = false;
            try
            {
                //new Aspose.Slides.License().SetLicense(new MemoryStream(Convert.FromBase64String(Key)));
                //Crack();//生成之前先调用破解方法，去掉水印。
                Aspose.Slides.Presentation ppt = new Aspose.Slides.Presentation(path);
                ppt.Save(newFilePath, Aspose.Slides.Export.SaveFormat.Pdf);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }

        public static bool WordToPDF(String from, String to)
        {
            bool result = false;
            try
            {


                Document doc = new Document(from);
                ////保存为PDF文件，此处的SaveFormat支持很多种格式，如图片，epub,rtf 等等

                ////权限这块的设置成不可复制
                //PdfSaveOptions saveOptions = new PdfSaveOptions();
                //// Create encryption details and set owner password.
                //PdfEncryptionDetails encryptionDetails = new PdfEncryptionDetails(string.Empty, "password", PdfEncryptionAlgorithm.RC4_128);
                //// Start by disallowing all permissions.
                //encryptionDetails.Permissions = PdfPermissions.DisallowAll;
                //// Extend permissions to allow editing or modifying annotations.
                //encryptionDetails.Permissions = PdfPermissions.ModifyAnnotations | PdfPermissions.DocumentAssembly;
                //saveOptions.EncryptionDetails = encryptionDetails;
                //// Render the document to PDF format with the specified permissions.
                //doc.Save(to, saveOptions);

                doc.Save(to, SaveFormat.Pdf);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;

        }

        public static bool ExcelToPdf(Worksheet sheet, String to)
        {
            try
            {
                sheet.SaveToPdf(to);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //去掉水印
        //public static void Crack()//使用前调用一次即可
        //{
        //    try
        //    {
        //        ////调用Crack方法实现软破解
        //        //HOTPathchAsposeSlides解密
        //        string[] stModule = new string[8]
        //        {
        //            "\u0003\u2003\u2009\u2004",
        //            "\u0005\u2003\u2009\u2004",
        //            "\u000F\u2003\u2001\u2003",
        //            "\u0003\u2000",
        //            "\u000F",
        //            "\u0002\u2000",
        //            "\u0003",
        //            "\u0002"
        //        };
        //        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Aspose.Slides.License));
        //        Type typeLic = null, typeIsTrial = null, typeHelper = null;
        //        foreach (Type type in assembly.GetTypes())
        //        {
        //            if ((typeLic == null) && (type.Name == stModule[0]))
        //            {
        //                typeLic = type;
        //            }
        //            else if ((typeIsTrial == null) && (type.Name == stModule[1]))
        //            {
        //                typeIsTrial = type;
        //            }
        //            else if ((typeHelper == null) && (type.Name == stModule[2]))
        //            {
        //                typeHelper = type;
        //            }
        //        }
        //        if (typeLic == null || typeIsTrial == null || typeHelper == null)
        //        {
        //            throw new Exception();
        //        }
        //        object lic = Activator.CreateInstance(typeLic);
        //        int findCount = 0;
        //        foreach (System.Reflection.FieldInfo field in typeLic.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
        //        {
        //            if (field.FieldType == typeLic && field.Name == stModule[3])
        //            {
        //                field.SetValue(null, lic);
        //                ++findCount;
        //            }
        //            else if (field.FieldType == typeof(DateTime) && field.Name == stModule[4])
        //            {
        //                field.SetValue(lic, DateTime.MaxValue);
        //                ++findCount;
        //            }
        //            else if (field.FieldType == typeIsTrial && field.Name == stModule[5])
        //            {
        //                field.SetValue(lic, 1);
        //                ++findCount;
        //            }

        //        }
        //        foreach (System.Reflection.FieldInfo field in typeHelper.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
        //        {
        //            if (field.FieldType == typeof(bool) && field.Name == stModule[6])
        //            {
        //                field.SetValue(null, false);
        //                ++findCount;
        //            }
        //            if (field.FieldType == typeof(int) && field.Name == stModule[7])
        //            {
        //                field.SetValue(null, 128);
        //                ++findCount;
        //            }
        //        }
        //        if (findCount < 5)
        //        {
        //            throw new NotSupportedException("无效的版本");
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

    }
}
