using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.IO;
namespace MiniFramework
{
    public class ZipUtil
    {
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="DirectoryPath">文件夹路径</param>
        /// <param name="savePath">压缩包保存路径</param>
        /// <param name="zipName">压缩包名</param>
        public static void ZipDirectory(string DirectoryPath, string savePath, string zipName, string password = null)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                throw new FileNotFoundException("指定目录：" + DirectoryPath + "不存在！");
            }
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            if (!savePath.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
            {
                savePath += Path.AltDirectorySeparatorChar;
            }
            using (FileStream fileStream = File.Create(savePath + zipName))
            {
                using (ZipOutputStream outStream = new ZipOutputStream(fileStream))
                {
                    outStream.SetLevel(9);
                    outStream.Password = password;
                    ZipStep(DirectoryPath, outStream, "");
                }
            }
        }
        /// <summary>
        /// 递归目录
        /// </summary>
        private static void ZipStep(string targetDirectory, ZipOutputStream stream, string parentPath)
        {
            string[] fileNames = Directory.GetFileSystemEntries(targetDirectory);
            foreach (var file in fileNames)
            {
                if (Directory.Exists(file))
                {
                    string pPath = parentPath;
                    pPath += file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar.ToString()) + 1);
                    pPath += Path.DirectorySeparatorChar.ToString();
                    ZipStep(file, stream, pPath);
                }
                else
                {
                    using (FileStream fs = File.OpenRead(file))
                    {
                        if (fs.Length > 0)
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            string fileName = parentPath + file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar.ToString()) + 1);
                            ZipEntry entry = new ZipEntry(fileName);
                            entry.IsUnicodeText = true;
                            entry.DateTime = DateTime.Now;
                            entry.Size = fs.Length;
                            entry.CompressionMethod = CompressionMethod.Deflated;
                            fs.Close();
                            stream.PutNextEntry(entry);
                            stream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="zipFilePath">压缩包路径</param>
        /// <param name="saveDir">解压文件存放路径</param>
        /// <returns></returns>
        public static bool UpZipFile(string zipFilePath, string saveDir, string password = null)
        {
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("指定文件：" + zipFilePath + "不存在！");
            }
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }
            if (!saveDir.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
            {
                saveDir += Path.AltDirectorySeparatorChar;
            }
            using (ZipInputStream inputStream = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                inputStream.Password = password;
                ZipEntry theEntry;
                while ((theEntry = inputStream.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(saveDir + directoryName);
                    }
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        using (FileStream writer = File.Create(saveDir + theEntry.Name))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = inputStream.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    writer.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="zipFilePath">压缩包路径</param>
        /// <param name="saveDir">解压文件存放路径</param>
        /// <returns></returns>
        public static IEnumerator UpZipFile(string zipFilePath, string saveDir, Action callback, string password = null)
        {
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("指定文件：" + zipFilePath + "不存在！");
            }
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }
            if (!saveDir.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
            {
                saveDir += Path.AltDirectorySeparatorChar;
            }
            using (ZipInputStream inputStream = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                inputStream.Password = password;
                ZipEntry theEntry;
                while ((theEntry = inputStream.GetNextEntry()) != null)
                {
                    yield return null;
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(saveDir + directoryName);
                    }
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        using (FileStream writer = File.Create(saveDir + theEntry.Name))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                yield return null;
                                size = inputStream.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    writer.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (callback != null)
            {
                callback();
            }
        }
    }
}
