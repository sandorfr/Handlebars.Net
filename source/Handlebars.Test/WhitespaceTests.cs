using System;
#if mstest
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif

namespace HandlebarsDotNet.Test
{
#if !mstest
    [TestFixture]
#else
    [TestClass]
#endif
    public class WhitespaceTests
    {
#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void PreceedingWhitespace()
        {
            var source = "Hello, {{~name}} !";
            var template = Handlebars.Compile(source);
            var data = new {
                name = "Handlebars.Net"
            };
            var result = template(data);
            Assert.AreEqual("Hello,Handlebars.Net !", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void TrailingWhitespace()
        {
            var source = "Hello, {{name~}} !";
            var template = Handlebars.Compile(source);
            var data = new {
                name = "Handlebars.Net"
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void PrecedingAndTrailingWhitespace()
        {
            var source = "Hello, {{~name~}} !";
            var template = Handlebars.Compile(source);
            var data = new {
                name = "Handlebars.Net"
            };
            var result = template(data);
            Assert.AreEqual("Hello,Handlebars.Net!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void ComplexTest()
        {
            var source =
@"{{#each nav ~}}
  <a href=""{{url}}"">
    {{~#if test}}
      {{~title}}
    {{~else~}}
      Empty
    {{~/if~}}
  </a>
{{~/each}}";
            var template = Handlebars.Compile(source);
            var data = new {
                nav = new [] {
                    new {
                        url = "https://google.com",
                        test = true,
                        title = "Google"
                    },
                    new {
                        url = "https://bing.com",
                        test = false,
                        title = "Bing"
                    }
                }
            };
            var result = template(data);
            Assert.AreEqual(@"<a href=""https://google.com"">Google</a><a href=""https://bing.com"">Empty</a>", result);
        }
    }
}

