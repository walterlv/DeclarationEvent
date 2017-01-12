// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration event chain node Extra info.
    /// </summary>
    public class DE
    {
        internal static readonly DE[] Empty = new DE[0];

        public static PointerCountInfo Single = new PointerCountInfo(PointerCountInfo.CountType.Single);

        public static DelayInfo Long = new DelayInfo(DelayInfo.DelayType.LongerThan);
        public static DelayInfo Short = new DelayInfo(DelayInfo.DelayType.ShorterThan);
    }

    public sealed class PointerCountInfo : DE
    {
        public enum CountType
        {
            /// <summary>
            /// 仅第一个指针设备作为操作设备进行计算。
            /// </summary>
            Single,

            /// <summary>
            /// 所有指针设备合并成一个指针设备计算。
            /// </summary>
            Merged,

            /// <summary>
            /// 全部指针设备均参与同等计算。
            /// </summary>
            All,

            /// <summary>
            /// 小于或等于指定数量的指针设备参与计算。
            /// </summary>
            LessThanOrEquals,

            /// <summary>
            /// 大于或等于指定数量的指针设备参与计算。
            /// </summary>
            MoreThanOrEquals,

            /// <summary>
            /// 要求确定数量的指针设备参与计算。
            /// </summary>
            Definite,

            /// <summary>
            /// 默认值，等同于 <see cref="Single"/>。仅第一个指针设备作为操作设备进行计算。
            /// </summary>
            Default = Single,
        }

        public CountType Type { get; private set; }

        public int Count { get; private set; }

        public PointerCountInfo(CountType type)
        {
            Type = type;
        }

        public PointerCountInfo(CountType type, int count)
        {
            Type = type;
            Count = count;
        }
    }

    public sealed class DelayInfo : DE
    {
        public enum DelayType
        {
            Unspecified,
            ShorterThan,
            LongerThan,
        }

        public static TimeSpan DefaultDelay => TimeSpan.FromSeconds(0.8);

        public DelayType Type { get; private set; }
        public TimeSpan Delay { get; private set; }

        public DelayInfo(DelayType type)
        {
            Type = type;
        }

        public DelayInfo(DelayType type, TimeSpan delay)
        {
            Type = type;
            Delay = delay;
        }
    }
}
