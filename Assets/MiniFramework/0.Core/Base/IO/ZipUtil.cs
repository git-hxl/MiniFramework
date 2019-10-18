using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace MiniFramework
{
    /// <summary>
    /// 注:中文命名文件会出错！
    /// </summary>
    public class ZipUtil
    {
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="DirectoryPath">文件夹路径</param>
        /// <param name="savePath">压缩包保存路径</param>
        /// <param name="zipName">压缩包名</param>
        public static void ZipDirectory(string DirectoryPath, string savePath, string zipName)
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
                    ZipStep(DirectoryPath, outStream, "");
                }
            }
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="zipFilePath">压缩包路径</param>
        /// <param name="saveDir">解压文件存放路径</param>
        /// <returns></returns>
        public static bool UpZipFile(string zipFilePath, string saveDir)
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
                ZipEntry theEntry;
                while ((theEntry = inputStream.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(saveDir + directoryName);
                    }
                    if (!directoryName.EndsWith(Path.DirectorySeparatorChar.ToString()))
                        directoryName += Path.DirectorySeparatorChar.ToString();
                    if (fileName != string.Empty)
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
        /// 递归目录
        /// </summary>
        private static void ZipStep(string targetDirectory, ZipOutputStream stream, string parentPath)
        {
            Crc32 crc = new Crc32();
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
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        string fileName = parentPath + file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar.ToString()) + 1);
                        ZipEntry entry = new ZipEntry(fileName);
                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;
                        fs.Close();
                        crc.Reset();
                        crc.Update(buffer);
                        entry.Crc = crc.Value;
                        stream.PutNextEntry(entry);
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
    }
}
