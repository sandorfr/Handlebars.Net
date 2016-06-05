﻿#if mstest
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

namespace HandlebarsDotNet.Test
{
#if !mstest
    [TestFixture]
#else
    [TestClass]
#endif
    public class HelperTests
    {
#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void HelperWithLiteralArguments()
        {
            Handlebars.RegisterHelper("myHelper", (writer, context, args) => {
                var count = 0;
                foreach(var arg in args)
                {
                    writer.Write("\nThing {0}: {1}", ++count, arg);
                }
            });

            var source = "Here are some things: {{myHelper 'foo' 'bar'}}";

            var template = Handlebars.Compile(source);

            var output = template(new { });

            var expected = "Here are some things: \nThing 1: foo\nThing 2: bar";

            Assert.AreEqual(expected, output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void HelperWithLiteralArgumentsWithQuotes()
        {
            var helperName = "helper-" + Guid.NewGuid().ToString(); //randomize helper name
            Handlebars.RegisterHelper(helperName, (writer, context, args) => {
                var count = 0;
                foreach(var arg in args)
                {
                    writer.WriteSafeString(
                        string.Format("\nThing {0}: {1}", ++count, arg));
                }
            });

            var source = "Here are some things: {{" + helperName + " 'My \"favorite\" movie' 'bar'}}";

            var template = Handlebars.Compile(source);

            var output = template(new { });

            var expected = "Here are some things: \nThing 1: My \"favorite\" movie\nThing 2: bar";

            Assert.AreEqual(expected, output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void InversionNoKey()
        {
            var source = "{{^key}}No key!{{/key}}";
            var template = Handlebars.Compile(source);
            var output = template(new { });
            var expected = "No key!";
            Assert.AreEqual(expected, output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void InversionFalsy()
        {
            var source = "{{^key}}Falsy value!{{/key}}";
            var template = Handlebars.Compile(source);
            var data = new
            {
                key = false
            };
            var output = template(data);
            var expected = "Falsy value!";
            Assert.AreEqual(expected, output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void InversionEmptySequence()
        {
            var source = "{{^key}}Empty sequence!{{/key}}";
            var template = Handlebars.Compile(source);
            var data = new
                {
                    key = new string[] { }
                };
            var output = template(data);
            var expected = "Empty sequence!";
            Assert.AreEqual(expected, output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void InversionNonEmptySequence()
        {
            var source = "{{^key}}Empty sequence!{{/key}}";
            var template = Handlebars.Compile(source);
            var data = new
                {
                    key = new string[] { "element" }
                };
            var output = template(data);
            var expected = "";
            Assert.AreEqual(expected, output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BlockHelperWithArbitraryInversion()
        {
            var source = "{{#ifCond arg1 arg2}}Args are same{{else}}Args are not same{{/ifCond}}";

            Handlebars.RegisterHelper("ifCond", (writer, options, context, arguments) => {
                if(arguments[0] == arguments[1])
                {
                    options.Template(writer, (object)context);
                }
                else
                {
                    options.Inverse(writer, (object)context);
                }
            });

            var dataWithSameValues = new
                {
                    arg1 = "a",
                    arg2 = "a"
                };
            var dataWithDifferentValues = new
                {
                    arg1 = "a",
                    arg2 = "b"
                };

            var template = Handlebars.Compile(source);

            var outputIsSame = template(dataWithSameValues);
            var expectedIsSame = "Args are same";
            var outputIsDifferent = template(dataWithDifferentValues);
            var expectedIsDifferent = "Args are not same";

            Assert.AreEqual(expectedIsSame, outputIsSame);
            Assert.AreEqual(expectedIsDifferent, outputIsDifferent);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void HelperWithNumericArguments()
        {
            Handlebars.RegisterHelper("myHelper", (writer, context, args) => {
                var count = 0;
                foreach(var arg in args)
                {
                    writer.Write("\nThing {0}: {1}", ++count, arg);
                }
            });

            var source = "Here are some things: {{myHelper 123 4567 -98.76}}";

            var template = Handlebars.Compile(source);

            var output = template(new { });

            var expected = "Here are some things: \nThing 1: 123\nThing 2: 4567\nThing 3: -98.76";

            Assert.AreEqual(expected, output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void HelperWithHashArgument()
        {
            Handlebars.RegisterHelper("myHelper", (writer, context, args) => {
                var hash = args[2] as Dictionary<string, object>;
                foreach(var item in hash)
                {
                    writer.Write(" {0}: {1}", item.Key, item.Value);
                }
            });

            var source = "Here are some things:{{myHelper 'foo' 'bar' item1='val1' item2='val2'}}";

            var template = Handlebars.Compile(source);

            var output = template(new { });

            var expected = "Here are some things: item1: val1 item2: val2";

            Assert.AreEqual(expected, output);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BlockHelperWithSubExpression()
        {
            Handlebars.RegisterHelper("isEqual", (writer, context, args) =>
            {
                writer.WriteSafeString(args[0].ToString() == args[1].ToString() ? "true" : null);
            });
        
            var source = "{{#if (isEqual arg1 arg2)}}True{{/if}}";
        
            var template = Handlebars.Compile(source);
        
            var expectedIsTrue = "True";
            var outputIsTrue = template(new { arg1 = 1, arg2 = 1 });
            Assert.AreEqual(expectedIsTrue, outputIsTrue);
        
            var expectedIsFalse = "";
            var outputIsFalse = template(new { arg1 = 1, arg2 = 2 });
            Assert.AreEqual(expectedIsFalse, outputIsFalse);
        }
    }
}

