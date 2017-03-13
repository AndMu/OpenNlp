using System.IO;
using NUnit.Framework;
using OpenNLP.Tools.Chunker;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.Tokenize;

namespace OpenNlp.Tests.Integration
{
    [TestFixture]
    public class IntegrationTest
    {
        private EnglishMaximumEntropySentenceDetector sentenceDetector;

        private EnglishMaximumEntropyPosTagger postTagger;

        private EnglishTreebankChunker chunker;

        private EnglishMaximumEntropyTokenizer tokenizer;

        [SetUp]
        public void Setup()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\Resources\Models\");
            sentenceDetector = new EnglishMaximumEntropySentenceDetector(Path.Combine(path, "EnglishSD.nbin"));
            postTagger = new EnglishMaximumEntropyPosTagger(
                        Path.Combine(path, @"EnglishPOS.nbin"),
                        Path.Combine(path, @"Parser\tagdict"));
            tokenizer = new EnglishMaximumEntropyTokenizer(Path.Combine(path, "EnglishTok.nbin"));
            chunker = new EnglishTreebankChunker(Path.Combine(path, @"EnglishChunk.nbin"));
        }

        [Test]
        public void Test()
        {
            var txt = "1980 was certainly a year for bad backwoods slasher movies. \"Friday The 13th\" and \"The Burning\" may have been the best ones but there were like always a couple of stinkers not far behind like \"Don't Go Into The Woods Alone\" and this one. But in all fairness \"The Prey\" is nowhere near as bad as \"Don't Go Into The Woods\" but it's still not great either.";
            var sentences = sentenceDetector.SentenceDetect(txt);
            Assert.AreEqual(3, sentences.Length);
            var tokens = tokenizer.Tokenize(sentences[0]);
            Assert.AreEqual(11, tokens.Length);
            var tags = postTagger.Tag(tokens);
            Assert.AreEqual(11, tags.Length);
            var chunks = chunker.GetChunks(tokens, tags);
            Assert.AreEqual(7, chunks.Length);
        }
    }
}
