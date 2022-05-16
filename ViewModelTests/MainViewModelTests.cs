using System;
using ViewModel;
using Xunit;

namespace ViewModelTests
{
    public class MainViewModelTests
    {
        [Fact]
        public void ErrorScenario1()
        {
            var ter = new TestErrorReporter();
            var mvm = new MainViewModel(ter);
            mvm.ndNum = -1;
            mvm.ApplyDataCommand.Execute(null);
            Assert.True(ter.WasError);
        }

        [Fact]
        public void NonErrorScenario()
        {
            var ter = new TestErrorReporter();
            var mvm = new MainViewModel(ter);
            mvm.ndNum = 5;
            mvm.sgstart = 1.01;
            mvm.sgend = 2;
            mvm.ApplyDataCommand.Execute(null);
            Assert.False(ter.WasError);
        }

        [Fact]
        public void ErrorScenario2()
        {
            var ter = new TestErrorReporter();
            var mvm = new MainViewModel(ter);
            mvm.spndNum = 1;
            mvm.SplineCommand.Execute(null);
            Assert.True(ter.WasError);
        }

        [Fact]
        public void ErrorScenario3()
        {
            var ter = new TestErrorReporter();
            var mvm = new MainViewModel(ter);
            mvm.md = null;
            mvm.SplineCommand.Execute(null);
            Assert.True(ter.WasError);
        }


        public class TestErrorReporter : IErrorReporter
        {
            public bool WasError { get; private set; }
            public void ReportError(string message) => WasError = true;
        }
    }
}
