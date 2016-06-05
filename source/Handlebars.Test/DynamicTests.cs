#if mstest
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif
using System;
using System.Dynamic;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Test
{
#if !mstest
    [TestFixture]
#else
    [TestClass]
#endif
    public class DynamicTests
    {
#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void DynamicObjectBasicTest()
        {
            var model = new MyDynamicModel();

            var source = "Foo: {{foo}}\nBar: {{bar}}";

            var template = Handlebars.Compile(source);

            var output = template(model);

            Assert.AreEqual("Foo: 1\nBar: hello world", output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void JsonTestIfTruthy()
		{
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>("{\"myfield\":\"test1\",\"truthy\":\"test2\"}");

			var source = "{{myfield}}{{#if truthy}}{{truthy}}{{/if}}";

			var template = Handlebars.Compile(source);

			var output = template(model);

			Assert.AreEqual("test1test2", output);
		}

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void JsonTestIfFalsyMissingField()
		{
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>("{\"myfield\":\"test1\"}");

			var source = "{{myfield}}{{#if mymissingfield}}{{mymissingfield}}{{/if}}";

			var template = Handlebars.Compile(source);

			var output = template(model);

			Assert.AreEqual("test1", output);
		}

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void JsonTestIfFalsyValue()
		{
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>("{\"myfield\":\"test1\",\"falsy\":null}");

			var source = "{{myfield}}{{#if falsy}}{{falsy}}{{/if}}";

			var template = Handlebars.Compile(source);

			var output = template(model);

			Assert.AreEqual("test1", output);
		}

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void JsonTestArrays(){
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>("[{\"Key\": \"Key1\", \"Value\": \"Val1\"},{\"Key\": \"Key2\", \"Value\": \"Val2\"}]");

            var source = "{{#each this}}{{Key}}{{Value}}{{/each}}";

            var template = Handlebars.Compile(source);

            var output = template(model);

            Assert.AreEqual("Key1Val1Key2Val2", output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void JObjectTest() {
            object nullValue = null;
            var model = JObject.FromObject(new { Nested = new { Prop = "Prop" }, Nested2 = nullValue });

            var source = "{{NotExists.Prop}}";

            var template = Handlebars.Compile(source);

            var output = template(model);

            Assert.AreEqual("", output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void SystemJsonTestArrays()
        {
#if mstest
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject("[{\"Key\": \"Key1\", \"Value\": \"Val1\"},{\"Key\": \"Key2\", \"Value\": \"Val2\"}]");
#else
            var model = System.Web.Helpers.Json.Decode("[{\"Key\": \"Key1\", \"Value\": \"Val1\"},{\"Key\": \"Key2\", \"Value\": \"Val2\"}]");
#endif

            var source = "{{#each this}}{{Key}}{{Value}}{{/each}}";

            var template = Handlebars.Compile(source);

            var output = template(model);

            Assert.AreEqual("Key1Val1Key2Val2", output);
        }


        private class MyDynamicModel : DynamicObject
        {
            private Dictionary<string, object> properties = new Dictionary<string, object>
            {
                { "foo", 1 },
                { "bar", "hello world" }
            };

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if(properties.ContainsKey(binder.Name))
                {
                    result = properties[binder.Name];
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
        }
    }
}

