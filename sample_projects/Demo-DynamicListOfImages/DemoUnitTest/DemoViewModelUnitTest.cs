using DemoViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoUnitTest
{
    [TestClass]
    public class DemoViewModelUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestReceive()
        {
            // Please read the 'Arrange, Act, Assert' pattern described here:
            // https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/november/mvvm-writing-a-testable-presentation-layer-with-mvvm

            MessengerViewModel viewModel = new MessengerViewModel();

            // Trigger the callback by writing to the receive file, and
            // validate that our ViewModel received the event.
        }
    }
}
