// ReSharper disable InconsistentNaming

using System;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration event chain node Extra info.
    /// </summary>
    public class DE
    {
        [Flags]
        private enum Flags
        {
            
        }

        internal static DE Parse(string text)
        {
            return new DE();
        }
    }
}
