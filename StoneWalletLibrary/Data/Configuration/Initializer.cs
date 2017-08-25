using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StoneWalletLibrary.Data.Configuration
{
    public static class Initializer
    {
        public static void Initialize()
        {
            Database.SetInitializer(new SetMigration());
        }
    }
}
