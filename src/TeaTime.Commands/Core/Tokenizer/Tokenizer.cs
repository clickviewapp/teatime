namespace TeaTime.Commands.Core.Tokenizer
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class Tokenizer
    {
        public static IEnumerable<string> Tokenize(string command)
        {
            command = command.Trim();
            var tokens = new List<string>();

            var index = 0;

            while (index < command.Length)
            {
                switch (command[index])
                {
                    case '\"':
                        ++index;
                        tokens.Add(GetStringInQuotation(command, ref index));
                        break;
                    default:
                        tokens.Add(GetNextWord(command, ref index));
                        break;
                }

                IgnoreWhiteSpaces(command, ref index);
            }

            return tokens;
        }

        private static string GetNextWord(string command, ref int index)
        {
            var regexPattern = $"( |\")";
            var match = Regex.Match(command.Substring(index), regexPattern);

            var delimitersIndex = match.Success ? match.Index + index : -1;

            string retVal;

            if (delimitersIndex == -1)
            {
                retVal = command.Substring(index, command.Length - index);
                index = command.Length;
                return retVal;
            }

            retVal = command.Substring(index, delimitersIndex - index);

            index = delimitersIndex;

            return retVal;
        }

        private static string GetStringInQuotation(string command, ref int index)
        {
            var words = new List<string>();
            IgnoreWhiteSpaces(command, ref index);
            
            while (index < command.Length)
            {
                words.Add(GetNextWord(command, ref index));
                IgnoreWhiteSpaces(command, ref index);

                if (index < command.Length && command[index] == '\"')
                    break;
            }

            ++index;
            return string.Join(" ", words);
        }

        private static void IgnoreWhiteSpaces(string command, ref int index)
        {
            while (index != command.Length)
            {
                if (command[index] != ' ')
                    break;

                ++index;
            } 
        }
    }
}
