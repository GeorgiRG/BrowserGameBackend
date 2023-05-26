using BrowserGameBackend.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGBackend.xUnitTests.ToolsTests
{
    public class PasswordInputTests
    {
        private readonly Stopwatch sw = new ();

        [Fact]
        public void AssertPasswordHashCreated()
        {
            sw.Start ();
            string? passwordHash = PasswordTools.Hash("ProperPass123");
            string[] segments = passwordHash.Split(':');
            sw.Stop ();
            Assert.NotNull(passwordHash);
            Assert.IsType<string>(passwordHash);
            Assert.True(passwordHash.Length > 10);
            Assert.NotNull(segments);
            //check format HASH:SALT:ITERATION:ALGORITHM
            Assert.True(segments.Length == 4);
            Assert.True(sw.ElapsedMilliseconds < 1500);
            sw.Reset();

        }

        [Fact]
        public void AssertPasswordHashChecked()
        {
            sw.Start();

            string? passwordHash = PasswordTools.Hash("ProperPass123");
            Assert.True(PasswordTools.Verify("ProperPass123", passwordHash));
            sw.Stop();
            Assert.True(sw.ElapsedMilliseconds < 1500);

        }
    }
}
