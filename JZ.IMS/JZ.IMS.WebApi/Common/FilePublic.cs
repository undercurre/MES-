using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using JZ.IMS.ViewModels;

namespace JZ.IMS.WebApi.Common
{
    public static class FilePublic
    {
        public const string XLSFileNameExt = ".xls";
        public const string CSVFileNameExt = ".csv";
        public const string HTMLFileNameExt = ".html";
        public const string MHTFileNameExt = ".mht";
        public const string PDFFileNameExt = ".pdf";
        public const string RTFFileNameExt = ".rtf";
        public const string TXTFileNameExt = ".txt";
        public const string BMPFileNameExt = ".bmp";
        public const string GIFFileNameExt = ".gif";
        public const string JPEGFileNameExt = ".jpg";
        public const string PNGFileNameExt = ".png";
        public const string TIFFFileNameExt = ".tif";
        public const string WMFFileNameExt = ".wmf";

        public const string XMLConfigFile = @"\MiSFCS.XML";

        /// <summary>
        /// 確定文件是否存在
        /// </summary>
        /// <param name="fileDirectory"></param>
        /// <param name="filePostfix"></param>
        /// <returns></returns>
        public static string ConfirmFileExist(string fileDirectory, string filePostfix, string fileName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(fileDirectory);
            FileInfo[] fileInfos = directoryInfo.GetFiles(filePostfix);
            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.ToUpper() == fileName.ToUpper())
                {
                    return fileName;
                }
            }
            return null;
        }

        /// <summary>
        /// 從某目錄下獲取所有文件信息
        /// </summary>
        /// <returns></returns>
        public static FileInfo[] GetFileInfoFromTheDirectory(string postfixName, string directory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] fileInfos = directoryInfo.GetFiles(postfixName);
            return fileInfos;
        }

        /// <summary>
        /// 獲取目錄下所有文件信息
        /// </summary>
        /// <param name="postfixName"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static void GetFilesFromTheDirectory(SortedList<string, object> fileList,
            string postfixName, string directory, bool searchSubdirectories)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] fileInfos = directoryInfo.GetFiles(postfixName);
            foreach (FileInfo fileInfo in fileInfos)
            {
                StreamReader streamReader = fileInfo.OpenText();
                ArrayList list = new ArrayList();
                string oneLine = null;
                while ((oneLine = streamReader.ReadLine()) != null)
                {
                    list.Add(oneLine);
                }
                fileList.Add(fileInfo.Name, list);
                streamReader.Close();
            }

            // 搜索子目錄
            if (searchSubdirectories)
            {
                string[] subdirectories = System.IO.Directory.GetDirectories(directory);
                foreach (string subdirectory in subdirectories)
                {
                    GetFilesFromTheDirectory(fileList, postfixName, subdirectory, true);
                }
            }
        }

        /// <summary>
        /// 獲取單個文件內容
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public static ArrayList GetSimpleFileContent(string fileFullName)
        {
            if (!File.Exists(fileFullName))
            {
                return null;
            }
            StreamReader streamReader = File.OpenText(fileFullName);
            ArrayList list = new ArrayList();
            string oneLine = null;
            while ((oneLine = streamReader.ReadLine()) != null)
            {
                list.Add(oneLine);
            }
            streamReader.Close();
            return list;
        }

        /// <summary>
        /// 獲某目錄下所有文件夾(完整路徑)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetDirectories(string path)
        {
            return System.IO.Directory.GetDirectories(path).ToList<string>();
        }

        /// <summary>
        /// 取得指定路徑下的所有文件夾名稱
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static DirectoryInfo[] GetFolders(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            return directory.GetDirectories();
        }

        /// <summary>
        /// 刪除文件
        /// </summary>
        public static void DeleteFile(string fileFullName)
        {
            FileInfo fileInfo = new FileInfo(fileFullName);
            if (fileInfo.Exists)
            {
                fileInfo.Attributes = fileInfo.Attributes & ~FileAttributes.ReadOnly & ~FileAttributes.Hidden;
                fileInfo.Delete();
            }
        }

        /// <summary>
        /// 移動文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="newFilePath"></param>
        public static void MoveFile(string fileName, string oldFilePath, string newFilePath)
        {
            if (!Directory.Exists(newFilePath))
            {
                Directory.CreateDirectory(newFilePath);
            }
            string oldFullFileName = oldFilePath + fileName;
            string newFullFileName = newFilePath + fileName;

            FileInfo oldFileInfo = new FileInfo(oldFullFileName);
            if (!oldFileInfo.Exists)
            {
                return;
            }
            FileInfo newFileInfo = new FileInfo(newFullFileName);
            if (newFileInfo.Exists)
            {
                newFileInfo.Attributes = newFileInfo.Attributes & ~FileAttributes.ReadOnly & ~FileAttributes.Hidden;
                newFileInfo.Delete();
            }
            oldFileInfo.CopyTo(newFileInfo.FullName);
            if (oldFileInfo.Exists)
            {
                oldFileInfo.Attributes = oldFileInfo.Attributes & ~FileAttributes.ReadOnly & ~FileAttributes.Hidden;
                oldFileInfo.Delete();
            }
        }

        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        public static StreamWriter CreateFile(string fileName, string filePath)
        {
            DirectoryInfo directory = new DirectoryInfo(filePath);
            if (!directory.Exists)
            {
                directory.Create();
            }
            FileInfo fileInfo = new FileInfo(filePath + fileName);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            return new StreamWriter(filePath + fileName);
        }

        /// <summary>
        /// 根據指定編碼生成文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="encodingName">gb2312,big5,utf-8...</param>
        /// <returns>StreamWriter</returns>
        public static StreamWriter CreateFile(string fileName, string filePath, string encodingName)
        {
            DirectoryInfo directory = new DirectoryInfo(filePath);
            if (!directory.Exists)
            {
                directory.Create();
            }
            FileInfo fileInfo = new FileInfo(filePath + fileName);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            return new StreamWriter(filePath + fileName, false, System.Text.Encoding.GetEncoding(encodingName));
        }

        /// <summary>
        /// 添加文件內容
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static StreamWriter AppendFile(string fileName, string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath + fileName);
            if (!fileInfo.Exists)
            {
                return null;
            }
            return fileInfo.AppendText();
        }

        /// <summary>
        /// 獲取根目錄
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRoots()
        {
            List<string> rootList = new List<string>();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in drives)
            {
                rootList.Add(driveInfo.Name);
            }
            return rootList;
        }

        /// <summary>
        /// 重命名文件
        /// 需包含文件後綴
        /// </summary>
        /// <param name="resourceFileFullName"></param>
        /// <param name="targetFileFullName"></param>
        /// <param name="resourceFilePath"></param>
        /// <param name="targetFilePath"></param>
        public static void RenameFile(string resourceFileFullName, string targetFileFullName,
            string resourceFilePath, string targetFilePath)
        {
            if (!resourceFilePath.EndsWith(GlobalVariables.splitChar))
            {
                resourceFilePath += GlobalVariables.splitChar;
            }
            if (!targetFilePath.EndsWith(GlobalVariables.splitChar))
            {
                targetFilePath += GlobalVariables.splitChar;
            }
            string resourceFile = resourceFilePath + resourceFileFullName;
            string targetFile = targetFilePath + targetFileFullName;
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }
            File.Move(resourceFile, targetFile);
        }

    }
}
