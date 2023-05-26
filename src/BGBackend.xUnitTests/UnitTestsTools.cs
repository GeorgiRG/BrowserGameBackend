using System.Text.Json;
using System.Net;

namespace BGBackend.xUnitTests
{
    public static class UnitTestsTools
    {
        private const string _jsonMediaType = "application/json";
        private const int _expectedMaxElapsedMilliseconds = 1500;
        public static void AssertResponseIsSuccessful(HttpResponseMessage httpResponse, string? desiredHeader)
        {
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }
        public static void AssertResponseHasHeaders(HttpResponseMessage httpResponse, string[] headers)
        {

        }
    }
}