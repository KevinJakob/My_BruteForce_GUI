using Microsoft.VisualStudio.TestTools.UnitTesting;
using BruteForceGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceLogic.Test
{
    [TestClass]
    public class LogicBruteForceTest
    {
        [TestMethod]
        public void CharSelector_ShouldHaveAll()
        {
            var vm = new MainViewModel();
            vm.StartBruteForceAsync();
            Assert.IsTrue(vm.WithLowerCase);
            Assert.IsTrue(vm.WithUpperCase);
            Assert.IsTrue(vm.WithNumbers);
            Assert.IsTrue(vm.WithSpecialChars);
        }
    }
}
