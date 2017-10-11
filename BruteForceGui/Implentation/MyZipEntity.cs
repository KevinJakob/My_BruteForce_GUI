using BruteForceGui.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BruteForceGui.Implentation
{
    public class MyZipEntity : IZipEntity
    {
        public string Name { get; }

        public bool IsFile { get; }

        public Stream Stream { get; }

        public MyZipEntity(string name,bool isFile,Stream stream)
        {
            Name = name;
            IsFile = isFile;
            Stream = stream;
        }
    }
}
