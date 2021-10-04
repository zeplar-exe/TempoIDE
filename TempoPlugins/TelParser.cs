using Jammo.ParserTools;

namespace TempoPlugins
{
    public static partial class TelParser
    {
        public static TelStream Parse(string text)
        {
            var stream = new TelStream();
            var state = new StateMachine<ParserState>();
            var lexer = new Lexer(text);

            foreach (var token in lexer)
            {
                switch (state.Current)
                {
                    
                }
            }
            
            return stream;
        }

        private enum ParserState
        {
            
        }
    }
}