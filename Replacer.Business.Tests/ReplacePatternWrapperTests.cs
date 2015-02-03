using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Replacer.Business.Engine;

namespace Replacer.Business.Tests
{
    [TestFixture]
    public class ReplacePatternWrapperTests
    {
        [Test]
        [TestCase(@"${<NewGUID>}", SmartTagType.PerMatch)]
        [TestCase(@"${<NewFileGUID>}", SmartTagType.PerFile)]
        [TestCase(@"${<NewConstGUID>}", SmartTagType.PerBatch)]
        [TestCase(@"${<NewGUID()>}", SmartTagType.PerMatch)]
        [TestCase(@"${<NewFileGUID()>}", SmartTagType.PerFile)]
        [TestCase(@"${<NewConstGUID()>}", SmartTagType.PerBatch)]
        [TestCase(@"${<NewGUID(A)>}", SmartTagType.PerMatch)]
        [TestCase(@"${<NewFileGUID(A)>}", SmartTagType.PerFile)]
        [TestCase(@"${<NewConstGUID(A)>}", SmartTagType.PerBatch)]
        public void Test_GUID_SmartTag(string replacePattern, SmartTagType type)
        {
            ReplacePattern pattern = new ReplacePattern
            {
                Find = @"Test",
                Replace = replacePattern,
                ReplaceType = ReplaceType.Regex
            };
            ReplacePatternWrapper wrapper = new ReplacePatternWrapper(pattern);

            List<string> input = Enumerable.Repeat(@"Test", 3).ToList();

            // First Batch:
            AllAvailableSmartTags.Reset();

            List<string> part1 = input.Select(wrapper.ProcessPattern).ToList();

            AllAvailableSmartTags.ResetFileTags();

            List<string> part2 = input.Select(wrapper.ProcessPattern).ToList();

            // Second Batch:
            AllAvailableSmartTags.Reset();

            List<string> part3 = input.Select(wrapper.ProcessPattern).ToList();

            AllAvailableSmartTags.ResetFileTags();

            List<string> part4 = input.Select(wrapper.ProcessPattern).ToList();

            // Validate:
            IEnumerable<string> resultsForValidation;
            if (type == SmartTagType.PerMatch)
            {
                // All results have to be unique:
                resultsForValidation = part1.Union(part2).Union(part3).Union(part4);
            }
            else if (type == SmartTagType.PerFile)
            {
                // All results for single file have to match. Separate files must differ:
                Assert.That(part1.Distinct().Count(), Is.EqualTo(1));
                Assert.That(part2.Distinct().Count(), Is.EqualTo(1));
                Assert.That(part3.Distinct().Count(), Is.EqualTo(1));
                Assert.That(part4.Distinct().Count(), Is.EqualTo(1));
                resultsForValidation = new[]
                {
                    part1[0],
                    part2[0],
                    part3[0],
                    part4[0]
                };
            }
            else
            {
                Assert.That(part1.Union(part2).Distinct().Count(), Is.EqualTo(1));
                Assert.That(part3.Union(part4).Distinct().Count(), Is.EqualTo(1));
                resultsForValidation = new[]
                {
                    part1[0],
                    part3[0]
                };
            }

            Assert.That(resultsForValidation, Is.EquivalentTo(resultsForValidation.Distinct()));

            // Validate result as GUID:
            Guid guid;
            Assert.That(resultsForValidation.Select(g => Guid.TryParseExact(g, "D", out guid)).Distinct().Single(), Is.EqualTo(true));
        }
    }
}