using System;
using System.Collections;
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

        public static TType ToRealValue<TType>(this TType? nullable) where TType : struct
        {
            return (TType) nullable;
        }
    }
}