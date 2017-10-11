using BruteForceGui.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace BruteForceGui.Implentation
{
    public class SharpZibLibImp : IZipMaster
    {

        public SharpZibLibImp()
        {

        }

        public IEnumerable<IZipEntity> GetAllFilesUnorderd(string path, string password = null)
        {
            using (var fs = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var zip = new ZipFile(fs);
                if(!string.IsNullOrEmpty(password))
                    zip.Password = password;

                var entities = new List<IZipEntity>();
                foreach (ZipEntry item in zip)
                {
                    if(item.IsFile)
                    {
                        var inputStream = zip.GetInputStream(item);
                        var ms = new MemoryStream();
                        inputStream.CopyTo(ms);
                        var myEntity = new MyZipEntity(item.Name, true, ms);
                        entities.Add(myEntity);
                    }
                }

                return entities;
            }

            
        }
    }
}
