using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoUnitTest
{
    class DemoModelUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestReceive()
        {
            // - Develop a test class the implements IMessageListener.
            // - Create a MessengerModel object and pass that listener object in
            //   to subscribe for notifications.
            // - Write to the 'receive' file and validate that you listener gets called back.
        }

        [TestMethod]
        public void TestSend()
        {
            // - Develop a test class the implements IMessageListener.
            // - Create a MessengerModel object and pass that listener object in
            //   to subscribe for notifications.
            // - Call send, and validate that the data is written to the 'send' file.
        }
    }
}
