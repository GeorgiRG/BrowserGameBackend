using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrowserGameBackend.Tools;

namespace BGBackend.xUnitTests.ToolsTests
{
    public class UserInputToolsTests
    {
        [InlineData("user")]
        [InlineData("User-asdf")]
        [InlineData("UserUSSERDFADF")]
        [InlineData("abv1234_-")]
        [Theory]
        public void AssertTrueOnUsernameGoodInput(string input)
        {
            Assert.True(UserInputTools.ValidUsername(input));
        }

        [InlineData("")]
        [InlineData(" ")]
        [InlineData("adsfa132!ASDFASDF!#¤&&%/%&/(&/GN")]
        [InlineData("zxcvzxcv121212zxcvzxcv121212zxcvzxcv121212zxcvzxcv121212zxcvzxcv121212zxcvzxcv121212zxcvzxcv121212zxcvzxcv121212")]
        [InlineData("____ZZZCdd11")]
        [InlineData("adfa.asdf@dfasdfasdf.asdfasdf")]
        [InlineData("1235113")]
        [Theory]
        public void AssertFalseOnUsernameBadInput(string input)
        {
            Assert.False(UserInputTools.ValidUsername(input));
        }

        [InlineData("good.mail@good.mail")]
        [InlineData("aa.bb@dd.cc")]
        [InlineData("bxvb1213@mail.com")]
        [Theory]
        public void AssertTrueOnEmailGoodInput(string input)
        {
            Assert.True(UserInputTools.ValidEmail(input));
        }

        [InlineData("")]
        [InlineData(" ")]
        [InlineData("adsfa132!ASDFASDF!#¤&&%/%&/(&/GN")]
        [InlineData("zxcvzxcv121212")]
        [InlineData("____ZZZCdd11")]
        [InlineData("adfa.asdf@dfasdfasdf.asdfasdf")]
        [InlineData("aaa.sdafsadfasdfasdf23423afasdfasdfasdfasdfasdfadsfasdfasfdasfasdfasdfa@asdfaszxcvzxvzxcvZasdfadszxcvzxvzxcvZasdfaddddddddddddddddddszxcvzxvzxcvZasdfaddddddddddddddddddszxcvzxvzxcvZasdfaddddddddddddddddddszxcvzxvzxcvZasdfaddddddddddddddddddszxcvzxvzxcvZasdfaddddddddddddddddddszxcvzxvzxcvZasdfaddddddddddddddddddszxcvzxvzxcvZasdfaddddddddddddddddddszxcvzxvzxcvZasdfadddddddddddddddddddddddddddddddddddddddddddddddddddddddfasdf")]
        [Theory]
        public void AssertFalseOnEmailBadInput(string input)
        {
            Assert.False(UserInputTools.ValidEmail(input));
        }

        [InlineData("123456789Aa")]
        [InlineData("ASDFa2ddfaff")]
        [InlineData("adsfa132aASDFASDF!#¤&&%/%&/(&/GN")]
        [Theory]
        public void AssertTrueOnPasswordGoodInput(string input)
        {
            Assert.True(UserInputTools.ValidPassword(input));
        }

        [InlineData("")]
        [InlineData(" ")]
        [InlineData("zxcvzxcv121212")]
        [InlineData("adfa.asdf@dfasdfasdf.asdfasdf")]
        [InlineData("aaa.sda")]
        [Theory]
        public void AssertFalseOnPasswordBadInput(string input)
        {
            Assert.False(UserInputTools.ValidPassword(input));
        }
    }
}
