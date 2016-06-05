using System;
#if mstest
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
#endif
using System.Collections.Generic;

namespace HandlebarsDotNet.Test
{
#if !mstest
    [TestFixture]
#else
    [TestClass]
#endif
    public class IteratorTests
    {
#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void BasicIterator()
        {
            var source = "Hello,{{#each people}}\n- {{name}}{{/each}}";
            var template = Handlebars.Compile(source);
            var data = new {
                people = new []{
                    new { 
                        name = "Erik"
                    },
                    new {
                        name = "Helen"
                    }
                }
            };
            var result = template(data);
            Assert.AreEqual("Hello,\n- Erik\n- Helen", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void WithIndex()
        {
            var source = "Hello,{{#each people}}\n{{@index}}. {{name}}{{/each}}";
            var template = Handlebars.Compile(source);
            var data = new
                {
                    people = new[]{
                        new { 
                            name = "Erik"
                        },
                        new {
                            name = "Helen"
                        }
                    }
                };
            var result = template(data);
            Assert.AreEqual("Hello,\n0. Erik\n1. Helen", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void WithFirst()
        {
            var source = "Hello,{{#each people}}\n{{@index}}. {{name}} ({{name}} is {{#if @first}}first{{else}}not first{{/if}}){{/each}}";
            var template = Handlebars.Compile(source);
            var data = new
                {
                    people = new[]{
                        new { 
                            name = "Erik"
                        },
                        new {
                            name = "Helen"
                        }
                    }
                };
            var result = template(data);
            Assert.AreEqual("Hello,\n0. Erik (Erik is first)\n1. Helen (Helen is not first)", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void WithLast()
        {
            var source = "Hello,{{#each people}}\n{{@index}}. {{name}} ({{name}} is {{#if @last}}last{{else}}not last{{/if}}){{/each}}";
            var template = Handlebars.Compile(source);
            var data = new
                {
                    people = new[]{
                        new { 
                            name = "Erik"
                        },
                        new {
                            name = "Helen"
                        }
                    }
                };
            var result = template(data);
            Assert.AreEqual("Hello,\n0. Erik (Erik is not last)\n1. Helen (Helen is last)", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void Empty()
        {
            var source = "Hello,{{#each people}}\n- {{name}}{{else}} (no one listed){{/each}}";
            var template = Handlebars.Compile(source);
            var data = new
                {
                    people = new object[] { }
                };
            var result = template(data);
            Assert.AreEqual("Hello, (no one listed)", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void NullObject()
        {
            var source = "Hello,{{#each people}}\n- {{name}}{{else}} (no one listed){{/each}}";
            var template = Handlebars.Compile(source);
            var data = new
                {
                    people = (object)null
                };
            var result = template(data);
            Assert.AreEqual("Hello, (no one listed)", result);
        }

#if mstest
        [TestMethod]
#else
        [Test]
#endif
        public void NullSequence()
        {
            var source = "Hello,{{#each people}}\n- {{name}}{{else}} (no one listed){{/each}}";
            var template = Handlebars.Compile(source);
            var data = new
                {
                    people = (object[])null
                };
            var result = template(data);
            Assert.AreEqual("Hello, (no one listed)", result);
        }
    }
}

