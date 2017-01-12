using System;
using System.Collections.Generic;
using System.Windows;

namespace Cvte.Windows.Input
{
    /// <summary>
    /// 为区分鼠标、触摸、触笔输入事件提供事件处理函数。
    /// </summary>
    public delegate void DeviceInputEventHandler(object sender, DeviceInputEventArgs e);

    /// <summary>
    /// 包含用于区分鼠标、触摸、触笔输入事件的事件参数。
    /// </summary>
    public class DeviceInputEventArgs : EventArgs
    {
        /// <summary>
        /// 获取设备类型的枚举值（鼠标、触摸、触笔）。
        /// </summary>
        public DeviceType Type { get; private set; }

        /// <summary>
        /// 获取或设置设备的虚拟类型（真实设备、虚拟设备）。
        /// </summary>
        public VirtualDeviceType VirtualType { get; set; }

        /// <summary>
        /// 获取事件操作中的设备操作按钮（鼠标左键、鼠标右键、触笔操作键、橡皮擦键）。
        /// </summary>
        public DeviceButton Button { get; set; }

        /// <summary>
        /// 获取事件的设备 ID。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 获取此事件相对于事件源的位置。
        /// </summary>
        public Point Position { get; private set; }

        /// <summary>
        /// 创建 <see cref="DeviceInputEventArgs"/> 的新实例。
        /// </summary>
        /// <param name="id">事件的设备 ID。</param>
        /// <param name="position">事件相对于事件源的位置。</param>
        /// <param name="type">设备类型的枚举值（鼠标、触摸、触笔）。</param>
        /// <param name="button">事件操作中的设备操作按钮（鼠标左键、鼠标右键、触笔操作键、橡皮擦键）。</param>
        /// <param name="touchPoints">触摸点集合。</param>
        public DeviceInputEventArgs(int id, Point position, DeviceType type, DeviceButton button, IEnumerable<Point> touchPoints = null)
        {
            Id = id;
            Position = position;
            Type = type;
            Button = button;
            _touchPoints = touchPoints;
        }

        /// <summary>
        /// 获取此事件相对于事件源的位置到上一次位置的向量。
        /// </summary>
        public IEnumerable<Point> GetTouchPoints()
        {
            if (_touchPoints == null)
            {
                _touchPoints = new[] { Position };
            }
            return _touchPoints;
        }

        private IEnumerable<Point> _touchPoints;
    }

    /// <summary>
    /// 为区分鼠标、触摸、触笔输入的拖拽事件提供事件处理函数。
    /// </summary>
    public delegate void DeviceInputDragEventHandler(object sender, DeviceInputDragEventArgs e);

    /// <summary>
    /// 为区分鼠标、触摸、触笔输入的拖拽事件提供事件参数。
    /// </summary>
    public class DeviceInputDragEventArgs : DeviceInputEventArgs
    {

        /// <summary>
        /// 创建 <see cref="DeviceInputDragEventArgs"/> 的新实例。
        /// </summary>
        /// <param name="id">事件的设备 ID。</param>
        /// <param name="type">设备类型的枚举值（鼠标、触摸、触笔）。</param>
        /// <param name="button">事件操作中的设备操作按钮（鼠标左键、鼠标右键、触笔操作键、橡皮擦键）。</param>
        /// <param name="originPosition">此事件相对于事件源的初始位置。</param>
        /// <param name="currentPosition">事件相对于事件源的位置。</param>
        /// <param name="offsetToOrigin">此事件相对于事件源的位置到初始位置的向量。</param>
        /// <param name="offsetToLast">此事件相对于事件源的位置到上一次位置的向量。</param>
        public DeviceInputDragEventArgs(int id, DeviceType type, DeviceButton button,
            Point originPosition, Point currentPosition, Vector offsetToOrigin, Vector offsetToLast)
            : base(id, currentPosition, type, button)
        {
            OriginPosition = originPosition;
            OffsetToOrigin = offsetToOrigin;
            OffsetToLast = offsetToLast;
        }

        /// <summary>
        /// 获取此事件相对于事件源的初始位置。
        /// </summary>
        public Point OriginPosition { get; set; }

        /// <summary>
        /// 获取此事件相对于事件源的位置到初始位置的向量。
        /// </summary>
        public Vector OffsetToOrigin { get; set; }

        /// <summary>
        /// 获取此事件相对于事件源的位置到上一次位置的向量。
        /// </summary>
        public Vector OffsetToLast { get; set; }
    }

    /// <summary>
    /// 为区分鼠标、触摸、触笔输入的开始事件提供事件处理函数。
    /// </summary>
    public delegate void DeviceInputStartingEventHandler(object sender, DeviceInputStartingEventArgs e);

    /// <summary>
    /// 为区分鼠标、触摸、触笔输入的开始事件提供事件参数。
    /// </summary>
    public class DeviceInputStartingEventArgs : EventArgs
    {
        /// <summary>
        /// 获取设备类型的枚举值（鼠标、触摸、触笔）。
        /// </summary>
        public DeviceType Type { get; private set; }

        /// <summary>
        /// 获取事件操作中的设备操作按钮（鼠标左键、鼠标右键、触笔操作键、橡皮擦键）。
        /// </summary>
        public DeviceButton Button { get; set; }

        /// <summary>
        /// 获取事件的设备 ID。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 创建 <see cref="DeviceInputEventArgs"/> 的新实例。
        /// </summary>
        /// <param name="id">事件的设备 ID。</param>
        /// <param name="type">设备类型的枚举值（鼠标、触摸、触笔）。</param>
        /// <param name="button">事件操作中的设备操作按钮（鼠标左键、鼠标右键、触笔操作键、橡皮擦键）。</param>
        public DeviceInputStartingEventArgs(int id, DeviceType type, DeviceButton button)
        {
            Id = id;
            Type = type;
            Button = button;
        }
    }

    /// <summary>
    /// 为区分鼠标、触摸、触笔输入的开始事件提供事件处理函数。
    /// </summary>
    public delegate void DeviceInputStartedEventHandler(object sender, DeviceInputStartedEventArgs e);

    /// <summary>
    /// 为区分鼠标、触摸、触笔输入的开始事件提供事件参数。
    /// </summary>
    public class DeviceInputStartedEventArgs : EventArgs
    {
        /// <summary>
        /// 获取设备类型的枚举值（鼠标、触摸、触笔）。
        /// </summary>
        public DeviceType Type { get; private set; }

        /// <summary>
        /// 获取事件操作中的设备操作按钮（鼠标左键、鼠标右键、触笔操作键、橡皮擦键）。
        /// </summary>
        public DeviceButton Button { get; set; }

        /// <summary>
        /// 获取事件的设备 ID。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 创建 <see cref="DeviceInputEventArgs"/> 的新实例。
        /// </summary>
        /// <param name="id">事件的设备 ID。</param>
        /// <param name="type">设备类型的枚举值（鼠标、触摸、触笔）。</param>
        /// <param name="button">事件操作中的设备操作按钮（鼠标左键、鼠标右键、触笔操作键、橡皮擦键）。</param>
        public DeviceInputStartedEventArgs(int id, DeviceType type, DeviceButton button)
        {
            Id = id;
            Type = type;
            Button = button;
        }
    }

    /// <summary>
    /// 为区分鼠标、触摸、触笔输入的结束事件提供事件处理函数。
    /// </summary>
    public delegate void DeviceInputCompletedEventHandler(object sender, DeviceInputCompletedEventArgs e);

    /// <summary>
    /// 为区分鼠标、触摸、触笔输入的结束事件提供事件参数。
    /// </summary>
    public class DeviceInputCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// 获取设备类型的枚举值（鼠标、触摸、触笔）。
        /// </summary>
        public DeviceType Type { get; private set; }

        /// <summary>
        /// 获取事件操作中的设备操作按钮（鼠标左键、鼠标右键、触笔操作键、橡皮擦键）。
        /// </summary>
        public DeviceButton Button { get; set; }

        /// <summary>
        /// 获取或设置设备的虚拟类型（真实设备、虚拟设备）。
        /// </summary>
        public VirtualDeviceType VirtualType { get; private set; }

        /// <summary>
        /// 创建 <see cref="DeviceInputCompletedEventArgs"/> 的新实例。
        /// </summary>
        /// <param name="type">设备类型的枚举值（鼠标、触摸、触笔）。</param>
        /// <param name="button">事件操作中的设备操作按钮（鼠标左键、鼠标右键、触笔操作键、橡皮擦键）。</param>
        /// <param name="virtualType">结束事件的虚拟设备。</param>
        public DeviceInputCompletedEventArgs(DeviceType type, DeviceButton button, VirtualDeviceType virtualType)
        {
            Type = type;
            Button = button;
            VirtualType = virtualType;
        }
    }
}
