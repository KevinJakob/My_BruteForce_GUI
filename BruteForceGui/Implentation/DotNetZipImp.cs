using BruteForceGui.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using System.IO;

namespace BruteForceGui.Implentation
{
    public class DotNetZipImp : IZipMaster
    {
        public IEnumerable<IZipEntity> GetAllFilesUnorderd(string path, string password = null)
        {
            using (ZipFile zip = ZipFile.Read(path))
            {
                var list = new List<IZipEntity>();
                foreach (ZipEntry e in zip)
                {
                    if(!e.IsDirectory)
                    {
                        var ms = new MemoryStream();
                        e.ExtractWithPassword(ms, password);
                        var entry = new MyZipEntity(e.FileName, true, ms);
                        list.Add(entry);
                    }   
                }
                return list;
            }
        }

    }
}
