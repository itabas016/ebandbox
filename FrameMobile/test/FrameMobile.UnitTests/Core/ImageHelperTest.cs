using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Xunit;

namespace FrameMobile.UnitTests.Core
{
    public class ImageHelperTest
    {
        [Fact]
        public void ResizedTest()
        {
            var destFilePath = @"D:\NewsResources\Image\\";
            var oriFileName = @"D:\NewsResources\Original\_origin_274_664164582.jpg";
            var oriFileName2 = @"D:\NewsResources\Original\_origin_274_923586793.jpg";

            var outputbig1 = ImageHelper.ResizedToBig(oriFileName,destFilePath);
            var outputbig2 = ImageHelper.ResizedToBig(oriFileName2, destFilePath);

            Console.WriteLine(outputbig1);
            Console.WriteLine(outputbig2);
        }

        [Fact]
        public void ResizedHDTest()
        {
            long newsId = 43243123;
            var destFilePath = @"D:\NewsResources\Image\\";
            var oriFileName = @"D:\NewsResources\Original\_origin_274_664164582.jpg";
            var oriFileName2 = @"D:\NewsResources\Original\_origin_274_923586793.jpg";

            var outputHD = ImageHelper.ResizedHD(oriFileName, string.Format("{0}720\\",destFilePath), newsId);
            var outputHD2 = ImageHelper.ResizedHD(oriFileName2, string.Format("{0}720\\", destFilePath), newsId);

            var outputnormal = ImageHelper.ResizedNormal(oriFileName, string.Format("{0}480\\", destFilePath), newsId);
            var outputnormal2 = ImageHelper.ResizedNormal(oriFileName2, string.Format("{0}480\\", destFilePath), newsId);

            Console.WriteLine(outputHD);
            Console.WriteLine(outputHD2);

            Console.WriteLine(outputnormal);
            Console.WriteLine(outputnormal2);

        }
    }
}
