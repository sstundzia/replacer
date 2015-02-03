using System;
using System.Collections.Generic;
using NUnit.Framework;
using Replacer.Business.Engine;

namespace Replacer.Business.Tests
{
    [TestFixture]
    public class ReplacerTests
    {
        [Test]
        public void Test_ReplacerEngine_Ctor_Validates_Parameter_NULL()
        {
            Assert.That(() => new ReplacerEngine(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Test_ReplacerEngine_Ctor_Validates_Parameter_Empty()
        {
            Assert.That(() => new ReplacerEngine(new List<ReplacePattern>()),
                Throws.ArgumentException.With.Message.EqualTo("No replace patterns specified."));
        }

        [Test]
        public void Test_ReplacerEngine_ProcessString_Validates_NULL_Parameter()
        {
            ReplacePattern[] patterns =
            {
                new ReplacePattern
                {
                    Find = @"Test",
                    Replace = @"Test2",
                    ReplaceType = ReplaceType.Simple
                }
            };

            ReplacerEngine engine = new ReplacerEngine(patterns);
            Assert.That(() => engine.ProcessString(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Test_ReplacerEngine_ProcessString_Accepts_Empty_String()
        {
            ReplacePattern[] patterns =
            {
                new ReplacePattern
                {
                    Find = @"Test",
                    Replace = @"Test2",
                    ReplaceType = ReplaceType.Simple
                }
            };

            ReplacerEngine engine = new ReplacerEngine(patterns);
            Assert.That(() => engine.ProcessString(string.Empty), Throws.Nothing);
        }

        [Test]
        public void Test_ReplacerEngine_Processes_Input_With_Single_Pattern()
        {
            ReplacePattern[] patterns =
            {
                new ReplacePattern
                {
                    Find = @"Test",
                    Replace = @"Test2",
                    ReplaceType = ReplaceType.Simple
                }
            };

            ReplacerEngine engine = new ReplacerEngine(patterns);
            Assert.That(engine.ProcessString("Test_Test2"), Is.EqualTo("Test2_Test22"));
        }

        [Test]
        public void Test_ReplacerEngine_Processes_Input_With_Two_Patterns()
        {
            ReplacePattern[] patterns =
            {
                new ReplacePattern
                {
                    Find = @"Test",
                    Replace = @"Test2",
                    ReplaceType = ReplaceType.Simple
                },
                new ReplacePattern
                {
                    Find = @"Foo",
                    Replace = "Bar",
                    ReplaceType = ReplaceType.Simple
                }
            };

            ReplacerEngine engine = new ReplacerEngine(patterns);
            Assert.That(engine.ProcessString("TestFoo_TestFoo2Bar"), Is.EqualTo("Test2Bar_Test2Bar2Bar"));
        }
    }
}