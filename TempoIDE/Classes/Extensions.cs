using System;
using System.Collections.Generic;

namespace TempoIDE.Classes
{
    public static class Extensions
    {
        public static TCollectionType LastIndex<TCollectionType>(this IList<TCollectionType> collection)
        {
            if (collection.Count == 0)
            {
                throw new IndexOutOfRangeException($"Collection {collection} is empty.");
            }
            
            return collection[^1];
        }

        public static T ToRealValue<T>(this T? nullable) where T : struct
        {
            return (T) nullable;
        }
    }
}