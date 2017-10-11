using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceGui.Abstraction
{
    public interface IZipMaster
    {
        IEnumerable<IZipEntity> GetAllFilesUnorderd(string path,string password = default(string));
    }
}
