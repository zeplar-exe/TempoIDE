using System;
using System.Collections.Generic;
using System.IO;
using Jammo.ParserTools;
using TempoControls.Properties;
using TempoIDE.Properties;

namespace TempoIDE.Core.Helpers
{
    public static class ApplicationData
    {
        public static bool TryGet<T>(string path, out T result)
        {
            result = default;
            
            if (!TryGetDataDirectory(out var directory))
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_DIRECTORY, ErrorResources.NoDataDirectory);
                // TODO: Resources
                return false;
            }

            foreach (var file in path.Split('.'))
            {
                var newPath = Path.Join(directory.FullName, file);

                if (Directory.Exists(newPath))
                {
                    directory = new DirectoryInfo(newPath);
                }
                else if (File.Exists(newPath))
                {
                    
                }
                else
                {
                    
                }
            }

            return true;
        }

        private static IEnumerable<DataPair> Parse(string text)
        {
            var navigator = new Lexer(text).ToNavigator();

            
        }

        public static bool TryGetDataDirectory(out DirectoryInfo info)
        {
            info = new DirectoryInfo(Path.Join(Directory.GetCurrentDirectory(), "data"));

            return info.Exists;
        }
    }

    public class DataParser
    {
        private readonly EnumerableNavigator<LexerToken> navigator;

        public DataParser(string text)
        {
            navigator = new Lexer(text, new LexerOptions(t => t.Is(LexerTokenId.Whitespace) || t.Is(LexerTokenId.Newline)))
                .ToNavigator();
        }

        public IEnumerable<DataPair> Parse()
        {
            foreach (var token in navigator.EnumerateFromIndex())
            {
                switch (token.Id)
                {
                    case LexerTokenId.Alphabetic:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public class DataPair
    {
        public readonly string Key;
        public readonly DataValue Value;

        public DataPair(string key, DataValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public abstract class DataValue
    {
        public readonly string raw;

        protected DataValue(string raw)
        {
            this.raw = raw;
        }
    }

    public class TextData : DataValue
    {
        public TextData(string raw) : base(raw)
        {
        }
    }

    public class ExtendedData : DataValue
    {
        public ExtendedData(string raw) : base(raw)
        {
        }
    }
}