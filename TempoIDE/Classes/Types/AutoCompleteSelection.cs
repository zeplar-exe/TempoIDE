using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TempoIDE.Classes.Types
{
    public class AutoCompletionSelection
    {
        private int index;
        public int Index
        {
            get => index;
            set => index = Math.Clamp(value, 0, Choices.Length - 1);
        }
        public AutoCompletion Selected => Choices.Length > 0 ? Choices[Index] : null;

        public readonly AutoCompletion[] Choices;

        public AutoCompletionSelection(AutoCompletion[] choices)
        {
            Choices = choices;
        }
    }
}