using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoneWalletService.Controllers;
using StoneWalletService.Tests.MockRepositories;

namespace StoneWalletService.Tests
{
    [TestClass]
    public class WalletControllerTest
    {
        [TestMethod]
        public void ExecutePurchase_ShouldReturnCorrectWallet()
        {
            var controller = new WalletController(true, new MockWalletRepository(), new MockCardRepository(), new MockCardholderRepository(), new MockClock1());
            var result = controller.ExecutePurchase(2000);
            Assert.IsNotNull(result);
        }
    }
}
