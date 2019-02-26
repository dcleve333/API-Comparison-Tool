using System;
using Xunit;
using Xunit.Abstractions;
using XUnitCompareAPIs.Common;
using System.Threading.Tasks;

namespace XUnitCompareAPIs.Tests

{
    public class CompareTwoServices
    {
        private readonly ITestOutputHelper output;

        public CompareTwoServices(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task TestAsync()
        {
            var makeCall = new MakeCall();
            await makeCall.MakeCallsAsync();
        }


    }
}
