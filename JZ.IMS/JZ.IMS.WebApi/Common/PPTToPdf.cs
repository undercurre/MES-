//using Aspose.Slides;
//using Aspose.Words;
//using Aspose.Words.Saving;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Presentation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Common
{
    public class PPTToPdf
    {

        //     public void pptToPdf(string from, string to)
        //     {

        //         try
        //         {
        //             Crack();
        //             Presentation ppt = new Presentation(from);
        //             ppt.Save(to, Aspose.Slides.Export.SaveFormat.Pdf);
        //             Console.WriteLine("成功!");
        //         }
        //         catch (Exception ex)
        //         {

        //             throw ex;
        //         }
        //     }

        //     public static void Crack()//使用前调用一次即可
        //     {
        //         try
        //         {
        //             ////调用Crack方法实现软破解
        //             //HOTPathchAsposeSlides解密
        //             string[] stModule = new string[8]
        //             {
        //                 "\u0003\u2003\u2009\u2004",
        //                 "\u0005\u2003\u2009\u2004",
        //                 "\u000F\u2003\u2001\u2003",
        //                 "\u0003\u2000",
        //                 "\u000F",
        //                 "\u0002\u2000",
        //                 "\u0003",
        //                 "\u0002"
        //             };
        //             System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Aspose.Slides.License));
        //             Type typeLic = null, typeIsTrial = null, typeHelper = null;
        //             foreach (Type type in assembly.GetTypes())
        //             {
        //                 if ((typeLic == null) && (type.Name == stModule[0]))
        //                 {
        //                     typeLic = type;
        //                 }
        //                 else if ((typeIsTrial == null) && (type.Name == stModule[1]))
        //                 {
        //                     typeIsTrial = type;
        //                 }
        //                 else if ((typeHelper == null) && (type.Name == stModule[2]))
        //                 {
        //                     typeHelper = type;
        //                 }
        //             }
        //             if (typeLic == null || typeIsTrial == null || typeHelper == null)
        //             {
        //                 throw new Exception();
        //             }
        //             object lic = Activator.CreateInstance(typeLic);
        //             int findCount = 0;
        //             foreach (System.Reflection.FieldInfo field in typeLic.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
        //             {
        //                 if (field.FieldType == typeLic && field.Name == stModule[3])
        //                 {
        //                     field.SetValue(null, lic);
        //                     ++findCount;
        //                 }
        //                 else if (field.FieldType == typeof(DateTime) && field.Name == stModule[4])
        //                 {
        //                     field.SetValue(lic, DateTime.MaxValue);
        //                     ++findCount;
        //                 }
        //                 else if (field.FieldType == typeIsTrial && field.Name == stModule[5])
        //                 {
        //                     field.SetValue(lic, 1);
        //                     ++findCount;
        //                 }

        //             }
        //             foreach (System.Reflection.FieldInfo field in typeHelper.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
        //             {
        //                 if (field.FieldType == typeof(bool) && field.Name == stModule[6])
        //                 {
        //                     field.SetValue(null, false);
        //                     ++findCount;
        //                 }
        //                 if (field.FieldType == typeof(int) && field.Name == stModule[7])
        //                 {
        //                     field.SetValue(null, 128);
        //                     ++findCount;
        //                 }
        //             }
        //             if (findCount < 5)
        //             {
        //                 throw new NotSupportedException("无效的版本");
        //             }
        //         }
        //         catch (Exception e)
        //         {
        //             throw e;
        //         }
        //     }


        public static bool PPTToPDF(string from, string to)
        {
            bool result = false;


            try
            {
                //Presentation ppt = new Presentation(from, Spire.Presentation.FileFormat.Auto);
                //ppt.SaveToFile(to, Spire.Presentation.FileFormat.PDF);
                Presentation presentation = new Presentation();
                presentation.LoadFromFile(from);
                presentation.SaveToFile(to, Spire.Presentation.FileFormat.PDF);
                result = true;
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;






        }
    }
}
