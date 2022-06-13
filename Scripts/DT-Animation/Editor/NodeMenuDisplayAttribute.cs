using System;

namespace NoMoney.DTAnimation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeMenuDisplayAttribute : Attribute
    {
        public readonly string Display;
        public NodeMenuDisplayAttribute(string display)
        {
            Display = display;
        }
    }
}
