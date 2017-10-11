using BruteForceGui.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceGui.test
{
    [TestClass]
    public class MainViewModelTest
    {
        [TestMethod]
        public void StartBrutforce_ShouldNotBeEditable()
        {
            var vm = new MainViewModel();
            Assert.IsTrue(vm.IsEditableStartButton);
            vm.StartBruteForceAsync();
            Assert.IsFalse(vm.IsEditableStartButton);
        }
    }
}
