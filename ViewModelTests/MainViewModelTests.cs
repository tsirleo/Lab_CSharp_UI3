using System;
using System.ComponentModel;
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

        [Fact]
        public void ButtonWorkTest()
        {
            var mvm = new MainViewModel(null);
            Assert.Null(mvm.md);
            Assert.Null(mvm.sp);
            Assert.Null(mvm.sd);
            mvm.ApplyDataCommand.Execute(null);
            mvm.SplineCommand.Execute(null);
            Assert.NotNull(mvm.md);
            Assert.NotNull(mvm.sp);
            Assert.NotNull(mvm.sd);
        }

        [Fact]
        public void UpdateTriggerTest1()
        {
            var ter = new TestErrorReporter();
            var pcr = new PropertyChangedReporter();
            var mvm = new MainViewModel(ter);
            mvm.PropertyChanged += pcr.ChangeReport;
            Assert.False(pcr.ChangeHappend);
            mvm.ApplyDataCommand.Execute(null);
            Assert.False(ter.WasError);
            Assert.True(pcr.ChangeHappend);
        }

        [Fact]
        public void UpdateTriggerTest2()
        {
            var ter = new TestErrorReporter();
            var pcr = new PropertyChangedReporter();
            var mvm = new MainViewModel(ter);
            mvm.ApplyDataCommand.Execute(null);
            mvm.PropertyChanged += pcr.ChangeReport;
            Assert.False(pcr.ChangeHappend);
            mvm.SplineCommand.Execute(null);
            Assert.False(ter.WasError);
            Assert.True(pcr.ChangeHappend);
        }

        public class TestErrorReporter : IErrorReporter
        {
            public bool WasError { get; private set; }
            public void ReportError(string message) => WasError = true;
        }

        public class PropertyChangedReporter
        {
            public bool ChangeHappend { get; private set; }

            public void ChangeReport(object sender, PropertyChangedEventArgs e) => ChangeHappend = true;
        }
    }
}
