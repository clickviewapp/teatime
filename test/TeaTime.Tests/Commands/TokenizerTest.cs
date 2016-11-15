namespace TeaTime.Tests.Commands
{
    using System.Linq;
    using TeaTime.Commands.Core.Tokenizer;
    using Xunit;

    public class TokenizerTest
    {
        [Fact]
        public void SimpleCommand()
        {
            var command = "coffee order cap";

            var tokens = Tokenizer.Tokenize(command);

            Assert.Equal(3, tokens.Count());

            Assert.Equal("coffee", tokens.ElementAt(0));
            Assert.Equal("order", tokens.ElementAt(1));
            Assert.Equal("cap", tokens.ElementAt(2));
        }

        [Fact]
        public void QuoteCommand()
        {
            var command = "coffee order \"flat white\"";

            var tokens = Tokenizer.Tokenize(command);

            Assert.Equal(3, tokens.Count());

            Assert.Equal("coffee", tokens.ElementAt(0));
            Assert.Equal("order", tokens.ElementAt(1));
            Assert.Equal("flat white", tokens.ElementAt(2));
        }

        [Fact]
        public void MixCommand()
        {
            var command = "coffee order \"flat white\" order \"cap\" \"long black\"";

            var tokens = Tokenizer.Tokenize(command);

            Assert.Equal(6, tokens.Count());

            Assert.Equal("coffee", tokens.ElementAt(0));
            Assert.Equal("order", tokens.ElementAt(1));
            Assert.Equal("flat white", tokens.ElementAt(2));
            Assert.Equal("order", tokens.ElementAt(3));
            Assert.Equal("cap", tokens.ElementAt(4));
            Assert.Equal("long black", tokens.ElementAt(5));
        }
    }
}
