// ReSharper disable InconsistentNaming

using System;
using Cvte.Windows.Input;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration event chain node Extra info.
    /// </summary>
    public abstract class DE
    {
        internal static readonly DE[] Empty = new DE[0];

        public static DE Confirmed = new SignalInfo(SignalInfo.SignalType.Confirmed);
        public static DE Optional = new SignalInfo(SignalInfo.SignalType.Optional);

        public static DE Single = new PointerCountInfo(PointerCountInfo.CountType.Single);
        public static DE Merge = new PointerCountInfo(PointerCountInfo.CountType.Merged);
        public static DE AllPointers = new PointerCountInfo(PointerCountInfo.CountType.All);

        public static DE Near = new MoveInfo(MoveInfo.DistanceType.NearerThan);
        public static DE Far = new MoveInfo(MoveInfo.DistanceType.FartherThan);

        public static DE Long = new DelayInfo(DelayInfo.DelayType.LongerThan);
        public static DE Short = new DelayInfo(DelayInfo.DelayType.ShorterThan);
    }

    internal sealed class SignalInfo : DE
    {
        internal enum SignalType
        {
            Unspecified,
            Confirmed,
            Optional,
        }

        public SignalType Signal { get; private set; }

        public SignalInfo(SignalType signal)
        {
            Signal = signal;
        }
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

    public sealed class MoveInfo : DE
    {
        public enum DistanceType
        {
            Unspecified,
            NearerThan,
            FartherThan,
        }

        public enum RangeType
        {
            Point,
            Visual,
        }

        public static double GetDefaultDistance(DeviceType deviceType)
        {
            return 1;
        }

        public DistanceType DType { get; private set; }
        public RangeType RType { get; private set; }
        public double Distance { get; private set; }

        public MoveInfo(DistanceType type)
        {
            DType = type;
        }

        public MoveInfo(DistanceType type, double distance)
        {
            DType = type;
            Distance = distance;
        }

        public MoveInfo(DistanceType dtype, RangeType rtype, double distance)
        {
            DType = dtype;
            RType = rtype;
            Distance = distance;
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
        
        public static TimeSpan GetDefaultDelay(DeviceType deviceType)
        {
            return TimeSpan.FromSeconds(0.8);
        }

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
