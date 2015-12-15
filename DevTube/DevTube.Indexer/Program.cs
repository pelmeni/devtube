using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using DevTube.Api;
using DevTube.Business;

namespace DevTube.Indexer
{
    class Program
    {
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            if (string.IsNullOrEmpty(input) && string.IsNullOrEmpty(hash))
                return true;
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        static bool HasFiles(FSItemInfo fsii, List<FSItemInfo> list)
        {
            var fsiiFiles = list.Where(i => i.Level == fsii.Level + 1 && i.isFile);

            return fsiiFiles.Any();
        }
        static string GetCatFilesHash(MD5 md5Hash, FSItemInfo fsii, List<FSItemInfo> list)
        {
                var fsiiFiles = list.Where(i => i.Parent!=null && i.Parent.Path == fsii.Path && i.isFile);

            if (!fsiiFiles.Any())
                return string.Empty;

                var sb = new StringBuilder();

                foreach (var f in fsiiFiles)
                {
                    sb.Append(f.HashPath);
                }
                return GetMd5Hash(md5Hash, sb.ToString());
            
        }
        static void Main(string[] args)
        {

            var list = new List<FSItemInfo>();

            IndexPath("l:\\", list, null);

            //HashCataloguesPaths(list);

            ProcessList(list);

        }

        private static void ProcessList(List<FSItemInfo> list)
        {
            var ii = list.Select(i =>

                new Document()
                {
                    ContentType = i.isFile? i.Extension:"catalogue",
                    Path = i.Path,
                    Header = i.Path,
                    Size = i.Size,
                    Level = i.Level,
                    HashPath = i.HashPath
                }
                ).ToList();

            DocumentsHelper.InsertDocument(ii);

            DocumentsHelper.LinkParents(list);
        }

        private static void HashCataloguesPaths(List<FSItemInfo> list, bool validateOnly=false)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var catsEnum = list.Where(i => i.isFile == false).ToArray();

                foreach (var c in catsEnum)
                {
                    var cat = list.FirstOrDefault(i => i.Path == c.Path);
                    if (cat != null)
                    {
                        var filesHash = GetCatFilesHash(md5Hash, cat, list);
                        if (!validateOnly)
                            cat.HashPath = filesHash;
                        else
                        {
                            if (filesHash!=cat.HashPath)
                            {
                                Console.WriteLine(
                                    "Catalogue hash invalid! Catalogue path: {0}, hash: {1}. Real hash by files: {2}",
                                    cat.Path, cat.HashPath, filesHash);
                            }
                        }
                    }
                }
            }
        }

        private static void IndexPath(string path, List<FSItemInfo> list, FSItemInfo parent)
        {
            var di=new DirectoryInfo(path);

            var fsobjects = di.GetFileSystemInfos();
                    using (MD5 md5Hash = MD5.Create())
                    {
                        foreach (var o in fsobjects)
                        {

                            var attr = o.Attributes;
                            //directory
                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                var fsii = new FSItemInfo()
                                {
                                    Path = o.FullName.Replace("l:\\", ""),
                                    Parent = parent,
                                    Level = parent?.Level + 1 ?? 1,
                                    isFile = false,
                                    HashPath = GetMd5Hash(md5Hash, o.FullName.Replace("l:\\", ""))

                                };

                                list.Add(fsii);

                                IndexPath(o.FullName, list, fsii);
                            }
                            //file
                            else
                            {
                                var size = new FileInfo(o.FullName).Length;

                                var fsii = new FSItemInfo()
                                {
                                    Path = o.FullName.Replace("l:\\", ""),
                                    Extension = o.Extension.Replace(".", ""),
                                    Size = size,
                                    Parent = parent,
                                    Level = parent?.Level + 1 ?? 1,
                                    isFile = true,
                                    HashPath = GetMd5Hash(md5Hash, o.FullName.Replace("l:\\", ""))
                                };

                                list.Add(fsii);
                            }
                        }
                    }
        }


    }
}
