using System.Linq;
using Jammo.TextAnalysis.DotNet.CSharp.Helpers;
using TempoControls.Core.Types;

namespace TempoControls.Core.CompletionProviders
{
    public class CsCompletionProvider : ICompletionProvider
    {
        public AutoCompletion[] GetAutoCompletions(ColoredTextBox label)
        {
            var typingWord = label.GetTypingWord(true);
                 
            if (string.IsNullOrWhiteSpace(typingWord))
                return null;

            return CompletionHelper.MatchKeywordsByPartial(typingWord)
                .Where(kw => kw != typingWord)
                .Select(kw => new AutoCompletion(kw))
                .ToArray();
        }
    }
}