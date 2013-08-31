using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain;
using Xunit;

namespace FrameMobile.UnitTests
{
    public class TestBase : IUseFixture<TestBase>
    {
        public void SetFixture(TestBase data)
        {
            Bootstrapper.Start();

            #region For Request
            //var requestRepository = new Mock<IRequestRepository>();

            //ObjectFactory.Inject<IRequestRepository>(requestRepository.Object);

            //requestRepository.Setup<NameValueCollection>(s => s.Header).Returns(new NameValueCollection());
            #endregion
        }
    }
}
