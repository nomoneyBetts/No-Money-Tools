// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

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
