using Microsoft.VisualStudio.TestTools.UnitTesting;
using PressureControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Reflection;

namespace PressureControl.Tests
{
    [TestClass()]
    public class MessageTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MessageTest()
        {
            new Message(null);
        }

        //er valt niks te testen zonder echte hardware, of gevirtualiseerd...
    }
}