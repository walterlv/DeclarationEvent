// ReSharper disable InconsistentNaming

using System;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration event chain node Metadata.
    /// </summary>
    public class DM
    {
        [Flags]
        private enum Flags
        {
            
        }

        internal static DM Parse(string text)
        {
            return new DM();
        }
    }
}
