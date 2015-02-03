using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Replacer.Business.Engine;

namespace Replacer.Business.Tests
{
    [TestFixture]
    public class ReplacePatternWrapperTests
    {
        [Test]
        [TestCase(new []{@"${<NewGUID>}"}, SmartTagType.PerMatch)]
        [TestCase(new []{@"${<NewFileGUID>}"}, SmartTagType.PerFile)]
        [TestCase(new []{@"${<NewConstGUID>}"}, SmartTagType.PerBatch)]
        [TestCase(new []{@"${<NewGUID()>}"}, SmartTagType.PerMatch)]
        [TestCase(new []{@"${<NewFileGUID()>}"}, SmartTagType.PerFile)]
        [TestCase(new []{@"${<NewConstGUID()>}"}, SmartTagType.PerBatch)]
        [TestCase(new []{@"${<NewGUID(A)>}"}, SmartTagType.PerMatch)]
        [TestCase(new []{@"${<NewFileGUID(A)>}"}, SmartTagType.PerFile)]
        [TestCase(new []{@"${<NewConstGUID(A)>}"}, SmartTagType.PerBatch)]
        [TestCase(new[] { @"${<NewGUID(A)>}", @"${<NewGUID(B)>}" }, SmartTagType.PerMatch)]
        [TestCase(new[] { @"${<NewFileGUID(A)>}", @"${<NewFileGUID(B)>}" }, SmartTagType.PerFile)]
        [TestCase(new[] { @"${<NewConstGUID(A)>}", @"${<NewConstGUID(B)>}" }, SmartTagType.PerBatch)]
        public void Test_GUID_SmartTag(string[] replacePatterns, SmartTagType type)
        {
            List<ReplacePatternWrapper> wrappers =
                replacePatterns.Select((pattern, i) =>
                    new ReplacePatternWrapper(new ReplacePattern
                    {
                        Find = @"Test" + (i + 1).ToString(CultureInfo.InvariantCulture),
                        Replace = pattern,
                        ReplaceType = ReplaceType.Regex
                    }))
                    .ToList();


            List<string> input =
                replacePatterns.SelectMany((pattern, i) =>
                    Enumerable.Repeat(
                        @"Test" + (i + 1).ToString(CultureInfo.InvariantCulture),
                        3))
                    .ToList();

            // First Batch:
            AllAvailableSmartTags.Reset();

            List<Tuple<string, string>> part1 = input.Select(s => new Tuple<string, string>(s, wrappers.Aggregate(s, (sourceString, wrapper) => wrapper.ProcessPattern(sourceString)))).ToList();

            AllAvailableSmartTags.ResetFileTags();

            List<Tuple<string, string>> part2 = input.Select(s => new Tuple<string, string>(s, wrappers.Aggregate(s, (sourceString, wrapper) => wrapper.ProcessPattern(sourceString)))).ToList();

            // Second Batch:
            AllAvailableSmartTags.Reset();

            List<Tuple<string, string>> part3 = input.Select(s => new Tuple<string, string>(s, wrappers.Aggregate(s, (sourceString, wrapper) => wrapper.ProcessPattern(sourceString)))).ToList();

            AllAvailableSmartTags.ResetFileTags();

            List<Tuple<string, string>> part4 = input.Select(s => new Tuple<string, string>(s, wrappers.Aggregate(s, (sourceString, wrapper) => wrapper.ProcessPattern(sourceString)))).ToList();

            // Validate:
            IEnumerable<Tuple<string, string>> resultsForValidation;
            if (type == SmartTagType.PerMatch)
            {
                // All results have to be unique:
                resultsForValidation = part1.Union(part2).Union(part3).Union(part4);
            }
            else if (type == SmartTagType.PerFile)
            {
                // All results for single file have to match. Separate files must differ:
                AssertDistinctCountEqualsOne(part1);
                AssertDistinctCountEqualsOne(part2);
                AssertDistinctCountEqualsOne(part3);
                AssertDistinctCountEqualsOne(part4);

                resultsForValidation =
                    part1.Distinct()
                        .Union(part2.Distinct())
                        .Union(part3.Distinct())
                        .Union(part4.Distinct());
            }
            else
            {
                AssertDistinctCountEqualsOne(part1.Union(part2));
                AssertDistinctCountEqualsOne(part3.Union(part4));
                resultsForValidation =
                    part1.Union(part2).Distinct()
                        .Union(part3.Union(part4).Distinct());
            }

            Assert.That(resultsForValidation, Is.EquivalentTo(resultsForValidation.Distinct()));

            // Validate result as GUID:
            Guid guid;
            Assert.That(resultsForValidation.Select(g => Guid.TryParseExact(g.Item2, "D", out guid)).Distinct().Single(), Is.EqualTo(true));
        }

        private static void AssertDistinctCountEqualsOne(IEnumerable<Tuple<string, string>> input)
        {
            Assert.That(
                    input.GroupBy(r => r.Item1, r => r.Item2)
                    .Select(g => g.Distinct().Count())
                    .Distinct().Count(), Is.EqualTo(1));
        }
    }
}