using System;
#if mstest
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif

using System.IO;

namespace HandlebarsDotNet.Test
{
#if !mstest
    [TestFixture]
#else
    [TestClass]
#endif
    public class TripleStashTests
	{
#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void UnencodedPartial()
		{
			string source = "Hello, {{{>unenc_person}}}!";

			var template = Handlebars.Compile(source);

			var data = new {
				name = "Marc"
			};

			var partialSource = "<div>{{name}}</div>";
			using(var reader = new StringReader(partialSource))
			{
				var partialTemplate = Handlebars.Compile(reader);
				Handlebars.RegisterTemplate("unenc_person", partialTemplate);
			}

			var result = template(data);
			Assert.AreEqual("Hello, <div>Marc</div>!", result);
		}

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void EncodedPartialWithUnencodedContents()
		{
			string source = "Hello, {{>enc_person}}!";

			var template = Handlebars.Compile(source);

			var data = new {
				name = "<div>Marc</div>"
			};

			var partialSource = "<div>{{{name}}}</div>";
			using(var reader = new StringReader(partialSource))
			{
				var partialTemplate = Handlebars.Compile(reader);
				Handlebars.RegisterTemplate("enc_person", partialTemplate);
			}

			var result = template(data);
			Assert.AreEqual("Hello, <div><div>Marc</div></div>!", result);
		}

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void UnencodedObjectEnumeratorItems()
		{
			var source = "{{#each enumerateMe}}{{{this}}} {{/each}}";
			var template = Handlebars.Compile(source);
			var data = new
			{
				enumerateMe = new
				{
					foo = "<div>hello</div>",
					bar = "<div>world</div>"
				}
			};
			var result = template(data);
			Assert.AreEqual("<div>hello</div> <div>world</div> ", result);
		}

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void FailingBasicTripleStash()
        {
            string source = "{{#if a_bool}}{{{dangerous_value}}}{{/if}}Hello, {{{dangerous_value}}}!";

            var template = Handlebars.Compile(source);

            var data = new
                {
                    a_bool = false,
                    dangerous_value = "<div>There's HTML here</div>"
                };

            var result = template(data);
            Assert.AreEqual("Hello, <div>There's HTML here</div>!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void UnencodedEncodedUnencoded()
        {
            string source = "{{{dangerous_value}}}...{{dangerous_value}}...{{{dangerous_value}}}!";

            var template = Handlebars.Compile(source);

            var data = new
                {
                    a_bool = false,
                    dangerous_value = "<div>There's HTML here</div>"
                };

            var result = template(data);
            Assert.AreEqual("<div>There's HTML here</div>...&lt;div&gt;There's HTML here&lt;/div&gt;...<div>There's HTML here</div>!", result);
        }
	}
}

