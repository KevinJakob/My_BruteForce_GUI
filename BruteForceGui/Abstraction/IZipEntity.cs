using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceGui.Abstraction
{
    public interface IZipEntity
    {
        string Name { get; }
        bool IsFile { get; }
        Stream Stream { get; }
    }
}
