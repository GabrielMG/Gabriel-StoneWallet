using StoneWalletLibrary.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletService.Tests
{
    public class MockClock1 : IClock
    {
        public DateTime Now { get { return new DateTime(2017, 2, 5); } }
    }

    public class MockClock2 : IClock
    {
        public DateTime Now { get { return new DateTime(2017, 5, 15); } }
    }

    public class MockClock3 : IClock
    {
        public DateTime Now { get { return new DateTime(2017, 10, 30); } }
    }
}
