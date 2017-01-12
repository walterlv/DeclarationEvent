// ReSharper disable InconsistentNaming

using System;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration event chain node Extra info.
    /// </summary>
    public enum DE
    {
        ShortTime,
        LongTime,
    }

    public class DeclarationNodeExtra
    {
        internal static readonly DE[] Empty = new DE[0];
    }
}
