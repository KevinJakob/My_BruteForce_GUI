using BruteForceGui.Implentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task DotNetZipExtractTest()
        {
            var dotnetZipimp = new DotNetZipImp();
            var result = dotnetZipimp.GetAllFilesUnorderd(GetInputPath(),GetPassword());
            HandleResult(result);
        }

        [TestMethod]
        public async Task DotNetZipExtractInMemoryTest()
        {
            var dotnetZipimp = new DotNetZipImp();
            var ms = new MemoryStream(File.ReadAllBytes(GetInputPath()));
            var sw = new Stopwatch();
            sw.Start();
            var result = dotnetZipimp.GetAllFilesUnorderd(ms, GetPassword());
            var res1 = sw.ElapsedMilliseconds;
            sw.Reset();
            sw.Start();
            try
            {
                var xxx = dotnetZipimp.GetAllFilesUnorderd(ms, "xyz");
            }
            catch (Exception)
            {
                var res2 = sw.ElapsedMilliseconds;
                sw.Stop();
            }
            HandleResult(result);
        }

        private static string GetPassword()
        {
            return "cc";
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
