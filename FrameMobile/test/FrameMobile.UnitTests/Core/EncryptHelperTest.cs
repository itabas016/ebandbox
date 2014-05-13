using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Xunit;

namespace FrameMobile.UnitTests
{
    public class EncryptHelperTest
    {
        [Fact]
        public void EncryptUrlTest()
        {
            var str= "{\"result\":-2}";
            var ret = EncryptionHelper.EncryptUrl(str.Base64Encode());
            Console.WriteLine(ret);
        }

        [Fact]
        public void DecryptUrlTest()
        {
            var str = "rjnjKNpLzS+8o8Fj5+";
            var ret = EncryptionHelper.DecryptUrl(str).Base64Decode();
            Console.WriteLine(ret);
        }
    }
}
