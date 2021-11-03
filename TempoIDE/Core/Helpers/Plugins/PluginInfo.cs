using System;
using System.Collections.Generic;

namespace TempoIDE.Core.Helpers.Plugins
{
    public readonly struct PluginInfo
    {
        public readonly Uri Uri;
        public readonly string Name;
        public readonly string Author;

        public PluginInfo(Uri uri, string name, string author)
        {
            Uri = uri;
            Name = name;
            Author = author;
        }

        public static IEnumerable<PluginInfo> FromPluginSite()
        {
            yield break;
        }
    }
}