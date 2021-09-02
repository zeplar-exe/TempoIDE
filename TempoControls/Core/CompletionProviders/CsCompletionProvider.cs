using System.Linq;
using Jammo.CsAnalysis.Helpers;
using TempoControls.Core.Static;
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