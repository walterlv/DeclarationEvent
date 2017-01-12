namespace Cvte.Windows.Input
{
    /// <summary>
    /// 表示输入的设备类型，此类型代表了事件最终在 WPF 中的表现形式。
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// 鼠标设备。
        /// </summary>
        Mouse,
        /// <summary>
        /// 触笔设备。
        /// </summary>
        Stylus,
        /// <summary>
        /// 触摸设备。
        /// </summary>
        Touch,
    }
    /// <summary>
    /// 设备的虚拟类型，部分包含确认和取消的事件中可能需要通过判断此类型以决定应用还是撤消操作。
    /// </summary>
    public enum VirtualDeviceType
    {
        /// <summary>
        /// 真实设备。
        /// </summary>
        Device,
        /// <summary>
        /// 因 <see cref="System.Windows.Input.Mouse.LostMouseCaptureEvent"/> 或者 <see cref="System.Windows.Input.Stylus.LostStylusCaptureEvent"/> 而产生的虚拟设备。
        /// </summary>
        LostCapture,
        /// <summary>
        /// 手动调用引发事件而产生的虚拟设备。
        /// </summary>
        Manual,
    }
    /// <summary>
    /// 设备的按钮，用于确定使用设备的哪一种操作，是普通操作、上下文操作还是特殊操作。
    /// </summary>
    public enum DeviceButton
    {
        None = 0,
        LeftButton = 1,
        Normal = 1,
        Stylus = 1,
        Context = 2,
        RightButton = 2,
        StylusBarrel = 2,
        Eraser = 3,
    }
}
