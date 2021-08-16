using System.Linq;
using TempoControls;
using TempoControls.Core.Static;
using TempoControls.Core.Types;

namespace TempoIDE.Core.CompletionProviders
{
    public class CsCompletionProvider : ICompletionProvider
    {
        public AutoCompletion[] GetAutoCompletions(ColoredTextBox label)
        {
            var xmlData = IntellisenseCache.IntellisenseCs;
            var keywords = xmlData.Root.Element("keywords").Elements("kw");

            var typingWord = label.GetTypingWord(true);
                 
            if (string.IsNullOrWhiteSpace(typingWord))
                return null;

            return keywords
                .Where(kw => kw.Value.StartsWith(typingWord) && kw.Value != typingWord)
                .Select(kw => new AutoCompletion(kw.Value))
                .ToArray();
        }
    }
}