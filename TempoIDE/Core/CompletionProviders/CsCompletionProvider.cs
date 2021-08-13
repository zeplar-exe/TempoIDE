using System.Linq;
using TempoIDE.Core.Static;
using TempoIDE.Core.Types;
using TempoIDE.UserControls;

namespace TempoIDE.Core.CompletionProviders
{
    public class CsCompletionProvider : ICompletionProvider
    {
        public AutoCompletion[] GetAutoCompletions(SyntaxTextBox label)
        {
            var xmlData = ResourceCache.IntellisenseCs;
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