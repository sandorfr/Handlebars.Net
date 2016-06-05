﻿#if mstest
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace HandlebarsDotNet.Test
{
#if !mstest
    [TestFixture]
#else
    [TestClass]
#endif
    public class BasicIntegrationTests
    {

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPath()
        {
            var source = "Hello, {{name}}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
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
        public void BasicPathWhiteSpace()
        {
            var source = "Hello, {{ name }}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
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
        public void BasicCurlies()
        {
            var source = "Hello, {name}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                name = "Handlebars.Net"
            };
            var result = template(data);
            Assert.AreEqual("Hello, {name}!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicCurliesWithLeadingSlash()
        {
            var source = "Hello, \\{name\\}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                name = "Handlebars.Net"
            };
            var result = template(data);
            Assert.AreEqual("Hello, \\{name\\}!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPathArray()
        {
            var source = "Hello, {{ names.[1] }}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                names = new[] { "Foo", "Handlebars.Net" }
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPathArrayChildPath()
        {
            var source = "Hello, {{ names.[1].name }}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                names = new[] { new { name = "Foo" }, new { name = "Handlebars.Net" } }
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPathArrayNoSquareBracketsChildPath()
        {
            var source = "Hello, {{ names.1.name }}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                names = new[] { new { name = "Foo" }, new { name = "Handlebars.Net" } }
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPropertyOnArray()
        {
            var source = "Array is {{ names.Length }} item(s) long";
            var template = Handlebars.Compile(source);
            var data = new
            {
                names = new[] { new { name = "Foo" }, new { name = "Handlebars.Net" } }
            };
            var result = template(data);
            Assert.AreEqual("Array is 2 item(s) long", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicIfElse()
        {
            var source = "Hello, {{#if basic_bool}}Bob{{else}}Sam{{/if}}!";
            var template = Handlebars.Compile(source);
            var trueData = new
            {
                basic_bool = true
            };
            var falseData = new
            {
                basic_bool = false
            };
            var resultTrue = template(trueData);
            var resultFalse = template(falseData);
            Assert.AreEqual("Hello, Bob!", resultTrue);
            Assert.AreEqual("Hello, Sam!", resultFalse);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicIfElseIf()
        {
            var source = "{{#if isActive}}active{{else if isInactive}}inactive{{/if}}";
            var template = Handlebars.Compile(source);
            var activeData = new
            {
                isActive = true
            };
            var inactiveData = new
            {
                isInactive = true
            };
            var resultTrue = template(activeData);
            var resultFalse = template(inactiveData);
            Assert.AreEqual("active", resultTrue);
            Assert.AreEqual("inactive", resultFalse);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicIfElseIfElse()
        {
            var source = "{{#if isActive}}active{{else if isInactive}}inactive{{else}}nada{{/if}}";
            var template = Handlebars.Compile(source);
            var activeData = new
            {
                isActive = true
            };
            var inactiveData = new
            {
                isInactive = true
            };
            var elseData = new
            {
            };
            var resultActive = template(activeData);
            var resultInactive = template(inactiveData);
            var resultElse = template(elseData);
            Assert.AreEqual("active", resultActive);
            Assert.AreEqual("inactive", resultInactive);
            Assert.AreEqual("nada", resultElse);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicWith()
        {
            var source = "Hello,{{#with person}} my good friend {{name}}{{/with}}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                person = new
                {
                    name = "Erik"
                }
            };
            var result = template(data);
            Assert.AreEqual("Hello, my good friend Erik!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicWithInversion()
        {
            var source = "Hello, {{#with person}} my good friend{{else}}nevermind{{/with}}";
            var template = Handlebars.Compile(source);

            Assert.AreEqual("Hello, nevermind", template(new { }));
            Assert.AreEqual("Hello, nevermind", template(new { person = false }));
            Assert.AreEqual("Hello, nevermind", template(new { person = new string[] { } }));
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicEncoding()
        {
            var source = "Hello, {{name}}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                name = "<b>Bob</b>"
            };
            var result = template(data);
            Assert.AreEqual("Hello, &lt;b&gt;Bob&lt;/b&gt;!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicComment()
        {
            var source = "Hello, {{!don't render me!}}{{name}}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                name = "Carl"
            };
            var result = template(data);
            Assert.AreEqual("Hello, Carl!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicCommentEscaped()
        {
            var source = "Hello, {{!--don't {{render}} me!--}}{{name}}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                name = "Carl"
            };
            var result = template(data);
            Assert.AreEqual("Hello, Carl!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicObjectEnumerator()
        {
            var source = "{{#each enumerateMe}}{{this}} {{/each}}";
            var template = Handlebars.Compile(source);
            var data = new
            {
                enumerateMe = new
                {
                    foo = "hello",
                    bar = "world"
                }
            };
            var result = template(data);
            Assert.AreEqual("hello world ", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicObjectEnumeratorWithKey()
        {
            var source = "{{#each enumerateMe}}{{@key}}: {{this}} {{/each}}";
            var template = Handlebars.Compile(source);
            var data = new
            {
                enumerateMe = new
                {
                    foo = "hello",
                    bar = "world"
                }
            };
            var result = template(data);
            Assert.AreEqual("foo: hello bar: world ", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDictionaryEnumerator()
        {
            var source = "{{#each enumerateMe}}{{this}} {{/each}}";
            var template = Handlebars.Compile(source);
            var data = new
            {
                enumerateMe = new Dictionary<string, object>
                {
                    { "foo", "hello" },
                    { "bar", "world" }
                }
            };
            var result = template(data);
            Assert.AreEqual("hello world ", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDictionaryEnumeratorWithKey()
        {
            var source = "{{#each enumerateMe}}{{@key}}: {{this}} {{/each}}";
            var template = Handlebars.Compile(source);
            var data = new
            {
                enumerateMe = new Dictionary<string, object>
                {
                    { "foo", "hello" },
                    { "bar", "world" }
                }
            };
            var result = template(data);
            Assert.AreEqual("foo: hello bar: world ", result);
        }


#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPathDictionaryStringKeyNoSquareBrackets()
        {
            var source = "Hello, {{ names.Foo }}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                names = new Dictionary<string, string>
                {
                    { "Foo" , "Handlebars.Net" }
                }
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPathDictionaryStringKey()
        {
            var source = "Hello, {{ names.[Foo] }}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                names = new Dictionary<string, string>
                {
                    { "Foo" , "Handlebars.Net" }
                }
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPathDictionaryIntKeyNoSquareBrackets()
        {
            var source = "Hello, {{ names.42 }}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                names = new Dictionary<int, string>
                {
                    { 42 , "Handlebars.Net" }
                }
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPathDictionaryLongKeyNoSquareBrackets()
        {
            var source = "Hello, {{ names.42 }}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                names = new Dictionary<long, string>
                {
                    { 42 , "Handlebars.Net" }
                }
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPathDictionaryIntKey()
        {
            var source = "Hello, {{ names.[42] }}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                names = new Dictionary<int, string>
                {
                    { 42 , "Handlebars.Net" }
                }
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPathDictionaryLongKey()
        {
            var source = "Hello, {{ names.[42] }}!";
            var template = Handlebars.Compile(source);
            var data = new
            {
                names = new Dictionary<long, string>
                {
                    { 42 , "Handlebars.Net" }
                }
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }


#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void DynamicWithMetadataEnumerator()
        {
            var source = "{{#each enumerateMe}}{{this}} {{/each}}";
            var template = Handlebars.Compile(source);
            dynamic data = new ExpandoObject();
            data.enumerateMe = new ExpandoObject();
            data.enumerateMe.foo = "hello";
            data.enumerateMe.bar = "world";
            var result = template(data);
            Assert.AreEqual("hello world ", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void DynamicWithMetadataEnumeratorWithKey()
        {
            var source = "{{#each enumerateMe}}{{@key}}: {{this}} {{/each}}";
            var template = Handlebars.Compile(source);
            dynamic data = new ExpandoObject();
            data.enumerateMe = new ExpandoObject();
            data.enumerateMe.foo = "hello";
            data.enumerateMe.bar = "world";
            var result = template(data);
            Assert.AreEqual("foo: hello bar: world ", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicHelper()
        {
            Handlebars.RegisterHelper("link_to", (writer, context, parameters) =>
            {
                writer.WriteSafeString("<a href='" + parameters[0] + "'>" + parameters[1] + "</a>");
            });

            string source = @"Click here: {{link_to url text}}";

            var template = Handlebars.Compile(source);

            var data = new
            {
                url = "https://github.com/rexm/handlebars.net",
                text = "Handlebars.Net"
            };

            var result = template(data);
            Assert.AreEqual("Click here: <a href='https://github.com/rexm/handlebars.net'>Handlebars.Net</a>", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicHelperPostRegister()
        {
            string source = @"Click here: {{link_to_post_reg url text}}";

            var template = Handlebars.Compile(source);

            Handlebars.RegisterHelper("link_to_post_reg", (writer, context, parameters) =>
            {
                writer.WriteSafeString("<a href='" + parameters[0] + "'>" + parameters[1] + "</a>");
            });

            var data = new
            {
                url = "https://github.com/rexm/handlebars.net",
                text = "Handlebars.Net"
            };

            var result = template(data);


            Assert.AreEqual("Click here: <a href='https://github.com/rexm/handlebars.net'>Handlebars.Net</a>", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDeferredBlock()
        {
            string source = "Hello, {{#person}}{{name}}{{/person}}!";

            var template = Handlebars.Compile(source);

            var data = new
            {
                person = new
                {
                    name = "Bill"
                }
            };

            var result = template(data);
            Assert.AreEqual("Hello, Bill!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDeferredBlockString()
        {
            string source = "{{#person}} -{{this}}- {{/person}}";

            var template = Handlebars.Compile(source);

            var result = template(new { person = "Bill" });
            Assert.AreEqual(" -Bill- ", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDeferredBlockWithWhitespace()
        {
            string source = "Hello, {{ # person }}{{ name }}{{ / person }}!";

            var template = Handlebars.Compile(source);

            var data = new
            {
                person = new
                {
                    name = "Bill"
                }
            };

            var result = template(data);
            Assert.AreEqual("Hello, Bill!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDeferredBlockFalsy()
        {
            string source = "Hello, {{#person}}{{name}}{{/person}}!";

            var template = Handlebars.Compile(source);

            var data = new
            {
                person = false
            };

            var result = template(data);
            Assert.AreEqual("Hello, !", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDeferredBlockNull()
        {
            string source = "Hello, {{#person}}{{name}}{{/person}}!";

            var template = Handlebars.Compile(source);

            var data = new
            {
                person = (object)null
            };

            var result = template(data);
            Assert.AreEqual("Hello, !", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDeferredBlockEnumerable()
        {
            string source = "Hello, {{#people}}{{this}} {{/people}}!";

            var template = Handlebars.Compile(source);

            var data = new
            {
                people = new[] {
                    "Bill",
                    "Mary"
                }
            };

            var result = template(data);
            Assert.AreEqual("Hello, Bill Mary !", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDeferredBlockNegated()
        {
            string source = "Hello, {{^people}}nobody{{/people}}!";

            var template = Handlebars.Compile(source);

            var data = new
            {
                people = new string[] {
                }
            };

            var result = template(data);
            Assert.AreEqual("Hello, nobody!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDeferredBlockNegatedContext()
        {
            var template = Handlebars.Compile("Hello, {{^obj}}{{name}}{{/obj}}!");

            Assert.AreEqual("Hello, nobody!", template(new { name = "nobody" }));
            Assert.AreEqual("Hello, nobody!", template(new { name = "nobody", obj = new string[0] }));
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDeferredBlockInversion()
        {
            var template = Handlebars.Compile("Hello, {{#obj}}somebody{{else}}{{name}}{{/obj}}!");

            Assert.AreEqual("Hello, nobody!", template(new { name = "nobody" }));
            Assert.AreEqual("Hello, nobody!", template(new { name = "nobody", obj = false }));
            Assert.AreEqual("Hello, nobody!", template(new { name = "nobody", obj = new string[0] }));
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDeferredBlockNegatedInversion()
        {
            var template = Handlebars.Compile("Hello, {{^obj}}nobody{{else}}{{name}}{{/obj}}!");

            var array = new[]
            {
                new {name = "John"},
                new {name = " and "},
                new {name = "Sarah"}
            };

            Assert.AreEqual("Hello, John and Sarah!", template(new { obj = array }));
            Assert.AreEqual("Hello, somebody!", template(new { obj = true, name = "somebody" }));
            Assert.AreEqual("Hello, person!", template(new { obj = new { name = "person" } }));
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicPropertyMissing()
        {
            string source = "Hello, {{first}} {{last}}!";

            var template = Handlebars.Compile(source);

            var data = new
            {
                first = "Marc"
            };

            var result = template(data);
            Assert.AreEqual("Hello, Marc !", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicNullOrMissingSubProperty()
        {
            string source = "Hello, {{name.first}}!";

            var template = Handlebars.Compile(source);

            var data = new
            {
                name = (object)null
            };

            var result = template(data);
            Assert.AreEqual("Hello, !", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicNumericFalsy()
        {
            string source = "Hello, {{#if falsy}}Truthy!{{/if}}";

            var template = Handlebars.Compile(source);

            var data = new
            {
                falsy = 0
            };

            var result = template(data);
            Assert.AreEqual("Hello, ", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicNullFalsy()
        {
            string source = "Hello, {{#if falsy}}Truthy!{{/if}}";

            var template = Handlebars.Compile(source);

            var data = new
            {
                falsy = (object)null
            };

            var result = template(data);
            Assert.AreEqual("Hello, ", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicNumericTruthy()
        {
            string source = "Hello, {{#if truthy}}Truthy!{{/if}}";

            var template = Handlebars.Compile(source);

            var data = new
            {
                truthy = -0.1
            };

            var result = template(data);
            Assert.AreEqual("Hello, Truthy!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicStringFalsy()
        {
            string source = "Hello, {{#if falsy}}Truthy!{{/if}}";

            var template = Handlebars.Compile(source);

            var data = new
            {
                falsy = ""
            };

            var result = template(data);
            Assert.AreEqual("Hello, ", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicEmptyArrayFalsy()
        {
            var source = "{{#if Array}}stuff: {{#each Array}}{{this}}{{/each}}{{/if}}";

            var template = Handlebars.Compile(source);

            var data = new
            {
                Array = new object[] { }
            };

            var result = template(data);

            Assert.AreEqual("", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicTripleStash()
        {
            string source = "Hello, {{{dangerous_value}}}!";

            var template = Handlebars.Compile(source);

            var data = new
            {
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
        public void BasicEscape()
        {
            string source = @"Hello, \{{raw_value}}!";

            var template = Handlebars.Compile(source);

            var data = new
            {
                raw_value = "<div>I shouldn't display</div>"
            };

            var result = template(data);
            Assert.AreEqual(@"Hello, {{raw_value}}!", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicNumberLiteral()
        {
            string source = "{{eval 2  3}}";

            Handlebars.RegisterHelper("eval",
                (writer, context, args) => writer.Write("{0} {1}", args[0], args[1]));

            var template = Handlebars.Compile(source);

            var data = new { };

            var result = template(data);
            Assert.AreEqual("2 3", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicRoot()
        {
            string source = "{{#people}}- {{this}} is member of {{@root.group}}\n{{/people}}";

            var template = Handlebars.Compile(source);

            var data = new
            {
                group = "Engineering",
                people = new[]
                    {
                        "Rex",
                        "Todd"
                    }
            };

            var result = template(data);
            Assert.AreEqual("- Rex is member of Engineering\n- Todd is member of Engineering\n", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void ImplicitConditionalBlock()
        {
            var template =
                "{{#home}}Welcome Home{{/home}}{{^home}}Welcome to {{newCity}}{{/home}}";

            var data = new
            {
                newCity = "New York City",
                oldCity = "Los Angeles",
                home = false
            };

            var compiler = Handlebars.Compile(template);
            var result = compiler.Invoke(data);
            Assert.AreEqual("Welcome to New York City", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicDictionary()
        {
            var source =
                "<div id='userInfo'>UserName: {{userInfo.userName}} Language: {{userInfo.language}}</div>"
                + "<div id='main' style='width:{{clientSettings.width}}px; height:{{clientSettings.height}}px'>body</div>";

            var template = Handlebars.Compile(source);

            var embeded = new Dictionary<string, object>();
            embeded.Add("userInfo",
                new
                {
                    userName = "Ondrej",
                    language = "Slovak"
                });
            embeded.Add("clientSettings",
                new
                {
                    width = 120,
                    height = 80
                });

            var result = template(embeded);
            var expectedResult =
                "<div id='userInfo'>UserName: Ondrej Language: Slovak</div>"
                + "<div id='main' style='width:120px; height:80px'>body</div>";

            Assert.AreEqual(expectedResult, result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicHashtable()
        {
            var source = "{{dictionary.[key]}}";

            var template = Handlebars.Compile(source);

            var result = template(new
            {
                dictionary = new Hashtable
                {
                    { "key", "Hello world!" }
                }
            });
            var expectedResult = "Hello world!";

            Assert.AreEqual(expectedResult, result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicHashtableNoSquareBrackets()
        {
            var source = "{{dictionary.key}}";

            var template = Handlebars.Compile(source);

            var result = template(new
            {
                dictionary = new Hashtable
                {
                    { "key", "Hello world!" }
                }
            });
            var expectedResult = "Hello world!";

            Assert.AreEqual(expectedResult, result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicMockIDictionary()
        {
            var source = "{{dictionary.[key]}}";

            var template = Handlebars.Compile(source);

            var result = template(new
            {
                dictionary = new MockDictionary()
            });
            var expectedResult =
                "Hello world!";

            Assert.AreEqual(expectedResult, result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicMockIDictionaryNoSquareBrackets()
        {
            var source = "{{dictionary.key}}";

            var template = Handlebars.Compile(source);

            var result = template(new
            {
                dictionary = new MockDictionary()
            });
            var expectedResult =
                "Hello world!";

            Assert.AreEqual(expectedResult, result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicMockIDictionaryIntKey()
        {
            var source = "{{dictionary.[42]}}";

            var template = Handlebars.Compile(source);

            var result = template(new
            {
                dictionary = new MockDictionary()
            });
            var expectedResult =
                "Hello world!";

            Assert.AreEqual(expectedResult, result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicMockIDictionaryIntKeyNoSquareBrackets()
        {
            var source = "{{dictionary.42}}";

            var template = Handlebars.Compile(source);

            var result = template(new
            {
                dictionary = new MockDictionary()
            });
            var expectedResult =
                "Hello world!";

            Assert.AreEqual(expectedResult, result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void TestNoWhitespaceBetweenExpressions()
        {

            var source = @"{{#is ProgramID """"}}no program{{/is}}{{#is ProgramID ""1081""}}some program text{{/is}}";

            Handlebars.RegisterHelper("is", (output, options, context, args) =>
                {
                    if (args[0] == args[1])
                    {
                        options.Template(output, context);
                    }
                });


            var template = Handlebars.Compile(source);

            var result = template(new
            {
                ProgramID = "1081"
            });

            var expectedResult =
                "some program text";

            Assert.AreEqual(expectedResult, result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void DictionaryIteration()
        {
            string source = @"{{#ADictionary}}{{@key}},{{value}}{{/ADictionary}}";
            var template = Handlebars.Compile(source);
            var result = template(new
            {
                ADictionary = new Dictionary<string, int>
                        {
                            { "key5", 14 },
                            { "key6", 15 },
                            { "key7", 16 },
                            { "key8", 17 }
                        }
            });

            Assert.AreEqual("key5,14key6,15key7,16key8,17", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void ObjectEnumeration()
        {
            string source = @"{{#each myObject}}{{#if this.length}}<b>{{@key}}</b>{{#each this}}<li>{{this}}</li>{{/each}}<br>{{/if}}{{/each}}";
            var template = Handlebars.Compile(source);
            var result = template(new
            {
                myObject = new
                {
                    arr = new[] { "hello", "world" },
                    notArr = 1
                }
            });

            Assert.AreEqual("<b>arr</b><li>hello</li><li>world</li><br>", result);
        }



        private class MockDictionary : IDictionary<string, string>
        {
            public void Add(string key, string value)
            {
                throw new NotImplementedException();
            }
            public bool ContainsKey(string key)
            {
                return true;
            }
            public bool Remove(string key)
            {
                throw new NotImplementedException();
            }
            public bool TryGetValue(string key, out string value)
            {
                throw new NotImplementedException();
            }
            public string this[string index]
            {
                get
                {
                    return "Hello world!";
                }
                set
                {
                    throw new NotImplementedException();
                }
            }
            public ICollection<string> Keys
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            public ICollection<string> Values
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            public void Add(KeyValuePair<string, string> item)
            {
                throw new NotImplementedException();
            }
            public void Clear()
            {
                throw new NotImplementedException();
            }
            public bool Contains(KeyValuePair<string, string> item)
            {
                throw new NotImplementedException();
            }
            public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }
            public bool Remove(KeyValuePair<string, string> item)
            {
                throw new NotImplementedException();
            }
            public int Count
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            public bool IsReadOnly
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            {
                throw new NotImplementedException();
            }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}

