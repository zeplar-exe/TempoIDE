using System;

namespace TempoSourceGen
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependencyPropertyAttribute : Attribute
    {
        public readonly string Name;
        public readonly Type Type;
        public readonly Type OwnerType;

        public DependencyPropertyAttribute(string name, Type type, Type ownerType)
        {
            Name = name;
            Type = type;
            OwnerType = ownerType;
        }
    }
}