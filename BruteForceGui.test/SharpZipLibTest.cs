using BruteForceGui.Implentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceGui.test
{
    [TestClass]
    public class ZipTest
    {
        [TestMethod]
        public async Task SharpZipeExtractTest()
        {
            var zipimp = new SharpZibLibImp();
            string path = GetInputPath();
            string password = GetPassword();
            var streams = zipimp.GetAllFilesUnorderd(path, password);
            HandleResult(streams);
        }

        

        [TestMethod]
        public async Task DotNetZipExtractTest()
        {
            var dotnetZipimp = new DotNetZipImp();
            var result = dotnetZipimp.GetAllFilesUnorderd(GetInputPath(),GetPassword());
            HandleResult(result);
        }

        private static string GetPassword()
        {
            return "Test1";
        }

        private void HandleResult(IEnumerable<Abstraction.IZipEntity> streams)
        {
            foreach (var item in streams)
            {
                if (item.Stream is MemoryStream)
                {
                    CopyToDesktop((MemoryStream)item.Stream, item.Name);
                }
                else
                {
                    var ms = new MemoryStream();
                    item.Stream.CopyTo(ms);
                    CopyToDesktop(ms, item.Name);
                }
            }
        }

        private static string GetInputPath()
        {
            return Path.Combine(Environment.CurrentDirectory, "TestZip", "Geschafft.zip");
        }

        

        private void CopyToDesktop(MemoryStream memoryStream,string name)
        {
            var path = Path.Combine(@"C:\Users\MY\Desktop", name);
            File.WriteAllBytes(path, memoryStream.ToArray());
        }
        


    }
}
