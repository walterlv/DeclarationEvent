using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Cvte.Windows.Input
{
    /// <summary>
    /// 指定通用事件的分发源，以将传统的 MouseDown/Move/Up, StylusDown/Move/Up 统一成 DeviceHover/Down/Move/Up。
    /// </summary>
    public class DeviceInputSource
    {
        public DeviceInputSource(UIElement target, bool preview = false)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            Target = target;
            _preview = preview;

            _previewMouseDownEventHandler = OnMouseDown;
            _previewMouseMoveEventHandler = OnMouseMove;
            _previewMouseUpEventHandler = OnMouseUp;
            _mouseDownEventHandler = OnMouseDown;
            _mouseMoveEventHandler = OnMouseMove;
            _mouseUpEventHandler = OnMouseUp;
            _lostMouseCaptureEventHandler = OnLostMouseCapture;

            _previewStylusDownEventHandler = OnStylusDown;
            _previewStylusMoveEventHandler = OnStylusMove;
            _previewStylusUpEventHandler = OnStylusUp;
            _stylusDownEventHandler = OnStylusDown;
            _stylusMoveEventHandler = OnStylusMove;
            _stylusUpEventHandler = OnStylusUp;
            _lostStylusCaptureEventHandler = OnLostStylusCapture;
        }

        /// <summary>
        /// 获取转换事件源的 <see cref="UIElement"/>。
        /// </summary>
        public UIElement Target { get; }

        /// <summary>
        /// 获取或设置事件的参考坐标系，如果未指定，则使用 <see cref="Target"/>。
        /// </summary>
        public UIElement Container { get; set; }

        /// <summary>
        /// 获取事件发生时的 Capture 对象，如果未指定，则使用 <see cref="Target"/>。
        /// </summary>
        public UIElement CaptureTarget { get; set; }
        
        private UIElement InternalContainer => Container ?? Target;

        /// <summary>
        /// 获取一个值，该值指示应该用隧道事件还是使用冒泡事件。
        /// </summary>
        private readonly bool _preview;

        private DeviceInputEventHandler _attachedInputDown;
        private DeviceInputEventHandler _attachedInputMove;
        private DeviceInputEventHandler _attachedInputUp;
        private UIElement _currentContainer;
        protected IInputElement CurrentCaptureTarget { get; private set; }

        private readonly MouseButtonEventHandler _previewMouseDownEventHandler;
        private readonly MouseEventHandler _previewMouseMoveEventHandler;
        private readonly MouseButtonEventHandler _previewMouseUpEventHandler;
        private readonly MouseButtonEventHandler _mouseDownEventHandler;
        private readonly MouseEventHandler _mouseMoveEventHandler;
        private readonly MouseButtonEventHandler _mouseUpEventHandler;
        private readonly MouseEventHandler _lostMouseCaptureEventHandler;

        private readonly StylusDownEventHandler _previewStylusDownEventHandler;
        private readonly StylusEventHandler _previewStylusMoveEventHandler;
        private readonly StylusEventHandler _previewStylusUpEventHandler;
        private readonly StylusDownEventHandler _stylusDownEventHandler;
        private readonly StylusEventHandler _stylusMoveEventHandler;
        private readonly StylusEventHandler _stylusUpEventHandler;
        private readonly StylusEventHandler _lostStylusCaptureEventHandler;

        public void Attach(DeviceInputEventHandler inputDown,
            DeviceInputEventHandler inputMove,
            DeviceInputEventHandler inputUp)
        {
            if (_attachedInputDown != null) InputDown -= _attachedInputDown;
            if (_attachedInputMove != null) InputMove -= _attachedInputMove;
            if (_attachedInputUp != null) InputUp -= _attachedInputUp;

            _attachedInputDown = inputDown;
            _attachedInputMove = inputMove;
            _attachedInputUp = inputUp;

            InputDown += _attachedInputDown;
            InputMove += _attachedInputMove;
            InputUp += _attachedInputUp;

            if (_preview)
            {
                Target.RemoveHandler(Mouse.PreviewMouseDownEvent, _previewMouseDownEventHandler);
                Target.RemoveHandler(Mouse.PreviewMouseMoveEvent, _previewMouseMoveEventHandler);
                Target.RemoveHandler(Mouse.PreviewMouseUpEvent, _previewMouseUpEventHandler);
                Target.RemoveHandler(Mouse.LostMouseCaptureEvent, _lostMouseCaptureEventHandler);
                Target.RemoveHandler(Stylus.PreviewStylusDownEvent, _previewStylusDownEventHandler);
                Target.RemoveHandler(Stylus.PreviewStylusMoveEvent, _previewStylusMoveEventHandler);
                Target.RemoveHandler(Stylus.PreviewStylusUpEvent, _previewStylusUpEventHandler);
                Target.RemoveHandler(Stylus.LostStylusCaptureEvent, _lostStylusCaptureEventHandler);

                Target.AddHandler(Mouse.PreviewMouseDownEvent, _previewMouseDownEventHandler);
                Target.AddHandler(Mouse.PreviewMouseMoveEvent, _previewMouseMoveEventHandler);
                Target.AddHandler(Mouse.PreviewMouseUpEvent, _previewMouseUpEventHandler);
                Target.AddHandler(Mouse.LostMouseCaptureEvent, _lostMouseCaptureEventHandler);
                Target.AddHandler(Stylus.PreviewStylusDownEvent, _previewStylusDownEventHandler);
                Target.AddHandler(Stylus.PreviewStylusMoveEvent, _previewStylusMoveEventHandler);
                Target.AddHandler(Stylus.PreviewStylusUpEvent, _previewStylusUpEventHandler);
                Target.AddHandler(Stylus.LostStylusCaptureEvent, _lostStylusCaptureEventHandler);
            }
            else
            {
                Target.RemoveHandler(Mouse.MouseDownEvent, _mouseDownEventHandler);
                Target.RemoveHandler(Mouse.MouseMoveEvent, _mouseMoveEventHandler);
                Target.RemoveHandler(Mouse.MouseUpEvent, _mouseUpEventHandler);
                Target.RemoveHandler(Mouse.LostMouseCaptureEvent, _lostMouseCaptureEventHandler);
                Target.RemoveHandler(Stylus.StylusDownEvent, _stylusDownEventHandler);
                Target.RemoveHandler(Stylus.StylusMoveEvent, _stylusMoveEventHandler);
                Target.RemoveHandler(Stylus.StylusUpEvent, _stylusUpEventHandler);
                Target.RemoveHandler(Stylus.LostStylusCaptureEvent, _lostStylusCaptureEventHandler);

                Target.AddHandler(Mouse.MouseDownEvent, _mouseDownEventHandler);
                Target.AddHandler(Mouse.MouseMoveEvent, _mouseMoveEventHandler);
                Target.AddHandler(Mouse.MouseUpEvent, _mouseUpEventHandler);
                Target.AddHandler(Mouse.LostMouseCaptureEvent, _lostMouseCaptureEventHandler);
                Target.AddHandler(Stylus.StylusDownEvent, _stylusDownEventHandler);
                Target.AddHandler(Stylus.StylusMoveEvent, _stylusMoveEventHandler);
                Target.AddHandler(Stylus.StylusUpEvent, _stylusUpEventHandler);
                Target.AddHandler(Stylus.LostStylusCaptureEvent, _lostStylusCaptureEventHandler);
            }
        }

        public virtual void Detach()
        {
            if (_preview)
            {
                Target.PreviewMouseDown -= OnMouseDown;
                Target.PreviewMouseMove -= OnMouseMove;
                Target.PreviewMouseUp -= OnMouseUp;
                Target.PreviewStylusDown -= OnStylusDown;
                Target.PreviewStylusMove -= OnStylusMove;
                Target.PreviewStylusUp -= OnStylusUp;
            }
            else
            {
                Target.MouseDown -= OnMouseDown;
                Target.MouseMove -= OnMouseMove;
                Target.MouseUp -= OnMouseUp;
                Target.StylusDown -= OnStylusDown;
                Target.StylusMove -= OnStylusMove;
                Target.StylusUp -= OnStylusUp;
            }

            if (_attachedInputDown != null) InputDown -= _attachedInputDown;
            if (_attachedInputMove != null) InputMove -= _attachedInputMove;
            if (_attachedInputUp != null) InputUp -= _attachedInputUp;
        }

        private DeviceType? _currentDeviceType;
        private DeviceButton _currentDeviceButton;
        private bool _isCapturing;

        private readonly Dictionary<int, DeviceInputEventArgs> _currentDevices =
            new Dictionary<int, DeviceInputEventArgs>();

        private void OnStylusDown(object sender, StylusDownEventArgs e)
        {
            RunOneByOne(() =>
            {
                if (_currentDeviceType != null && _currentDeviceType == DeviceType.Mouse) return;

                DeviceType? deviceType = GetDeviceType(e);
                if (deviceType == null) return;

                DeviceType type = deviceType.Value;
                DeviceButton button = GetDeviceButton(e);
                if (!_currentDevices.Any())
                {
                    RaiseStarting(new DeviceInputStartingEventArgs(e.StylusDevice.Id, type, button));
                    _currentContainer = InternalContainer;
                    CurrentCaptureTarget = CaptureTarget ?? (UIElement)e.OriginalSource;
                    _currentDeviceButton = button;
                    _currentDeviceType = type;
                    RaiseStarted(new DeviceInputStartedEventArgs(e.StylusDevice.Id, type, button));
                }

                DeviceInputEventArgs args = new DeviceInputEventArgs(e.StylusDevice.Id,
                    e.GetPosition(_currentContainer), type, _currentDeviceButton);
                if (_currentDeviceType == null || _currentDeviceType == type)
                {
                    _currentDevices[e.StylusDevice.Id] = args;
                    try
                    {
                        RaiseInputDown(args);
                    }
                    finally
                    {
                        _isCapturing = true;
                        CurrentCaptureTarget.CaptureStylus();
                        _isCapturing = false;

                        if (_preview) e.Handled = true;
                    }
                }
            });
        }

        private void OnStylusMove(object sender, StylusEventArgs e)
        {
            RunOneByOne(() =>
            {
                if (_currentDeviceType == DeviceType.Mouse || _isCapturing) return;

                DeviceType? deviceType = GetDeviceType(e);
                if (deviceType == null) return;
                DeviceType type = deviceType.Value;

                try
                {
                    if (_currentDeviceType == type && _currentDevices.ContainsKey(e.StylusDevice.Id))
                    {
                        try
                        {
                            //RaiseInputMove(new DeviceInputEventArgs(e.StylusDevice.Id,
                            //    e.GetPosition(_currentContainer), type, _currentDeviceButton));
                            RaiseInputMove(new DeviceInputEventArgs(e.StylusDevice.Id,
                                e.GetPosition(_currentContainer), type, _currentDeviceButton,
                                e.GetStylusPoints(_currentContainer).Select(x => x.ToPoint())));
                        }
                        catch (Exception)
                        {
                            ReleaseStylusCapture();
                            throw;
                        }
                    }
                    else
                    {
                        //RaiseInputHover(new DeviceInputEventArgs(e.StylusDevice.Id,
                        //    e.GetPosition(InternalContainer), type, DeviceButton.None));
                        RaiseInputHover(new DeviceInputEventArgs(e.StylusDevice.Id,
                            e.GetPosition(InternalContainer), type, DeviceButton.None,
                            e.GetStylusPoints(_currentContainer).Select(x => x.ToPoint())));
                    }
                }
                finally
                {
                    if (_preview) e.Handled = true;
                }
            });
        }

        private void OnStylusUp(object sender, StylusEventArgs e)
        {
            RunOneByOne(() =>
            {
                if (_currentDeviceType == DeviceType.Mouse) return;
                if (!_currentDevices.ContainsKey(e.StylusDevice.Id)) return;

                DeviceType? deviceType = GetDeviceType(e);
                if (deviceType == null) return;
                DeviceType type = deviceType.Value;

                if (_currentDeviceType == type)
                {
                    _currentDevices.Remove(e.StylusDevice.Id);
                    try
                    {
                        RaiseInputUp(new DeviceInputEventArgs(e.StylusDevice.Id,
                            e.GetPosition(_currentContainer), type, _currentDeviceButton));
                    }
                    finally
                    {
                        if (!_currentDevices.Any())
                        {
                            ReleaseStylusCapture();
                        }
                    }
                }

                // 禁止解除注释。详情请阅读 StylusEventArgs.Handled 事件可能造成的 WPF 触摸问题。
                // if (_preview) e.Handled = true;
            });
        }

        private void OnLostStylusCapture(object sender, StylusEventArgs e)
        {
            RunOneByOne(() =>
            {
                if (_currentDeviceType == null) return;
                DeviceType type = _currentDeviceType.Value;
                _currentDeviceType = null;

                try
                {
                    foreach (DeviceInputEventArgs args in _currentDevices.Values.ToArray())
                    {
                        args.VirtualType = VirtualDeviceType.LostCapture;
                        RaiseInputUp(args);
                    }
                }
                finally
                {
                    VirtualDeviceType virtualType = _currentDevices.Any()
                        ? VirtualDeviceType.LostCapture
                        : VirtualDeviceType.Device;
                    _currentDevices.Clear();
                    RaiseCompleted(new DeviceInputCompletedEventArgs(type, _currentDeviceButton, virtualType));
                }
            });
        }

        private void ReleaseStylusCapture()
        {
            _isCapturing = true;
            if (CurrentCaptureTarget.IsStylusCaptured)
            {
                CurrentCaptureTarget.ReleaseStylusCapture();
            }
            else
            {
                OnLostStylusCapture(null, null);
            }
            _isCapturing = false;
        }

        private DeviceType? GetDeviceType(StylusEventArgs e)
        {
            switch (e.StylusDevice.TabletDevice.Type)
            {
                case TabletDeviceType.Touch:
                    return DeviceType.Touch;
                case TabletDeviceType.Stylus:
                    return DeviceType.Stylus;
                default:
                    return null;
            }
        }

        private DeviceButton GetDeviceButton(StylusEventArgs e)
        {
            if (e.StylusDevice.Name == "Stylus")
            {
                StylusButton button = e.StylusDevice.StylusButtons.FirstOrDefault(x => x.Name == "Barrel Switch");
                if (button != null && button.StylusButtonState == StylusButtonState.Down)
                {
                    return DeviceButton.StylusBarrel;
                }
                return DeviceButton.Stylus;
            }
            if (e.StylusDevice.Name == "Eraser")
            {
                return DeviceButton.Eraser;
            }
            return DeviceButton.Stylus;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            RunOneByOne(() =>
            {
                if (e.StylusDevice != null) return;
                if (_currentDeviceType != null) return;

                DeviceButton button = GetDeviceButton(e);
                if (!_currentDevices.Any())
                {
                    RaiseStarting(new DeviceInputStartingEventArgs(-1, DeviceType.Mouse, button));
                    _currentContainer = InternalContainer;
                    CurrentCaptureTarget = CaptureTarget ?? (IInputElement)e.OriginalSource;
                    _currentDeviceButton = button;
                    _currentDeviceType = DeviceType.Mouse;
                    RaiseStarted(new DeviceInputStartedEventArgs(-1, DeviceType.Mouse, button));
                }

                DeviceInputEventArgs args = new DeviceInputEventArgs(-1,
                    e.GetPosition(_currentContainer), DeviceType.Mouse, _currentDeviceButton);
                _currentDevices[-1] = args;

                try
                {
                    RaiseInputDown(args);
                }
                finally
                {
                    _isCapturing = true;
                    CurrentCaptureTarget.CaptureMouse();
                    _isCapturing = false;

                    if (_preview) e.Handled = true;
                }
            });
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            RunOneByOne(() =>
            {
                if (e.StylusDevice != null || _isCapturing) return;

                try
                {
                    DeviceInputEventArgs args;
                    if (_currentDeviceType == DeviceType.Mouse)
                    {
                        args = new DeviceInputEventArgs(-1,
                            e.GetPosition(_currentContainer), DeviceType.Mouse, _currentDeviceButton);
                        _currentDevices[-1] = args;
                        try
                        {
                            RaiseInputMove(args);
                        }
                        catch
                        {
                            ReleaseMouseCapture();
                            throw;
                        }
                    }
                    else
                    {
                        args = new DeviceInputEventArgs(-1,
                            e.GetPosition(InternalContainer), DeviceType.Mouse, DeviceButton.None);
                        RaiseInputHover(args);
                    }
                }
                finally
                {
                    if (_preview) e.Handled = true;
                }
            });
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            RunOneByOne(() =>
            {
                if (e.StylusDevice != null) return;
                if (_currentDeviceType == DeviceType.Mouse)
                {
                    DeviceInputEventArgs args = new DeviceInputEventArgs(-1,
                        e.GetPosition(_currentContainer), DeviceType.Mouse, _currentDeviceButton);
                    _currentDevices.Remove(-1);
                    try
                    {
                        RaiseInputUp(args);
                    }
                    finally
                    {
                        ReleaseMouseCapture();
                    }
                }

                if (_preview) e.Handled = true;
            });
        }

        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            RunOneByOne(() =>
            {
                if (_currentDeviceType == null) return;
                DeviceType type = _currentDeviceType.Value;
                _currentDeviceType = null;

                try
                {
                    foreach (DeviceInputEventArgs args in _currentDevices.Values.ToArray())
                    {
                        args.VirtualType = VirtualDeviceType.LostCapture;
                        RaiseInputUp(args);
                    }
                }
                finally
                {
                    VirtualDeviceType virtualType = _currentDevices.Any()
                        ? VirtualDeviceType.LostCapture
                        : VirtualDeviceType.Device;
                    _currentDevices.Clear();
                    RaiseCompleted(new DeviceInputCompletedEventArgs(type, _currentDeviceButton, virtualType));
                }
            });
        }

        private void ReleaseMouseCapture()
        {
            _isCapturing = true;
            if (CurrentCaptureTarget.IsMouseCaptured)
            {
                CurrentCaptureTarget.ReleaseMouseCapture();
            }
            else
            {
                OnLostMouseCapture(null, null);
            }
            _isCapturing = false;
        }

        private DeviceButton GetDeviceButton(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                return DeviceButton.LeftButton;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                return DeviceButton.RightButton;
            }
            return DeviceButton.LeftButton;
        }

        private bool _isRunning;
        private readonly Queue<KeyValuePair<string, Action>> _actions = new Queue<KeyValuePair<string, Action>>();

        private void RunOneByOne(Action action, [CallerMemberName] string callerName = null)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            _actions.Enqueue(new KeyValuePair<string, Action>(callerName, action));
            if (_isRunning) return;
            _isRunning = true;
            try
            {
                Dictionary<Exception, string> exceptions = new Dictionary<Exception, string>();
                while (_actions.Any())
                {
                    KeyValuePair<string, Action> pair = _actions.Dequeue();
                    try
                    {
                        pair.Value.Invoke();
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex, pair.Key);
                    }
                }
                if (exceptions.Any())
                {
                    string s = exceptions.Count > 1 ? "s" : "";
                    string are = exceptions.Count > 1 ? "are" : "is";
                    throw new AggregateException(
                        $"DeviceInputSource: Error occurred during raising DeviceInput event. " +
                        $"The event{s} {are}: {string.Join(",", exceptions.Values)}.",
                        exceptions.Select(x => x.Key));
                }
            }
            finally
            {
                _isRunning = false;
            }
        }

        private void RaiseStarting(DeviceInputStartingEventArgs e)
        {
            DeviceInputStartingEventHandler handler = InputStarting;
            handler?.Invoke(this, e);
        }

        private void RaiseStarted(DeviceInputStartedEventArgs e)
        {
            DeviceInputStartedEventHandler handler = InputStarted;
            handler?.Invoke(this, e);
        }

        private void RaiseCompleted(DeviceInputCompletedEventArgs e)
        {
            DeviceInputCompletedEventHandler handler = InputCompleted;
            handler?.Invoke(this, e);
        }

        private void RaiseInputDown(DeviceInputEventArgs e)
        {
            DeviceInputEventHandler handler = InputDown;
            handler?.Invoke(this, e);
        }

        private void RaiseInputMove(DeviceInputEventArgs e)
        {
            DeviceInputEventHandler handler = InputMove;
            handler?.Invoke(this, e);
        }

        private void RaiseInputHover(DeviceInputEventArgs e)
        {
            DeviceInputEventHandler handler = InputHover;
            handler?.Invoke(this, e);
        }

        private void RaiseInputUp(DeviceInputEventArgs e)
        {
            DeviceInputEventHandler handler = InputUp;
            handler?.Invoke(this, e);
        }

        public event DeviceInputStartingEventHandler InputStarting;
        public event DeviceInputStartedEventHandler InputStarted;
        public event DeviceInputCompletedEventHandler InputCompleted;
        public event DeviceInputEventHandler InputDown;
        public event DeviceInputEventHandler InputMove;
        public event DeviceInputEventHandler InputHover;
        public event DeviceInputEventHandler InputUp;
    }

    public static class DeviceButtonExtensions
    {
        public static string GetName(this DeviceButton button, DeviceType type)
        {
            switch (type)
            {
                case DeviceType.Mouse:
                    return button == DeviceButton.LeftButton ? "LeftButton" : "RightButton";
                case DeviceType.Touch:
                    return "Touch";
                case DeviceType.Stylus:
                    if (button == DeviceButton.Stylus)
                    {
                        return "Stylus";
                    }
                    if (button == DeviceButton.StylusBarrel)
                    {
                        return "Barrel";
                    }
                    if (button == DeviceButton.Eraser)
                    {
                        return "Eraser";
                    }
                    break;
            }
            return "Unknown";
        }
    }
}
