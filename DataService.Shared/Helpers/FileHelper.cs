using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataService.Shared.Helpers
{
    /// <summary>
    /// 描述：文件操作类
    /// 作者：Leopisces
    /// 创建日期：2022/8/12 14:03:47
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        public static void CreateFile(string filePath, string text, Encoding encoding)
        {
            try
            {
                if (IsExistFile(filePath))
                {
                    DeleteFile(filePath);
                }
                if (!IsExistFile(filePath))
                {
                    string directoryPath = GetDirectoryFromFilePath(filePath);
                    CreateDirectory(directoryPath);

                    //Create File
                    FileInfo file = new FileInfo(filePath);
                    using (FileStream stream = file.Create())
                    {
                        using (StreamWriter writer = new StreamWriter(stream, encoding))
                        {
                            writer.Write(text);
                            writer.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void CreateDirectory(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 获取文件目录
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetDirectoryFromFilePath(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            DirectoryInfo directory = file.Directory;
            return directory.FullName;
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
