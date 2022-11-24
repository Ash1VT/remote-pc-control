using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using Server;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Resource = SharpDX.DXGI.Resource;

namespace ScreenCapturerNS {

    public static class ScreenCapturer {

        private enum Status : Int32 {
            Starts = 1,
            Active = 2,
            Stops = 3,
            Inactive = 4,
        }

        private static Exception globalException { get; set; }
        private static AutoResetEvent waitHandle { get; set; }
        private static ConcurrentQueue<Bitmap> bitmapQueue { get; set; }
        private static Thread captureThread { get; set; }
        private static Thread callbackThread { get; set; }

        private static volatile Status status;

        public static Boolean SkipFirstFrame { get; set; }
        public static Boolean SkipFrames { get; set; }
        public static Boolean PreserveBitmap { get; set; }

        public static event EventHandler<OnScreenUpdatedEventArgs> OnScreenUpdated;
        public static event EventHandler<OnCaptureStopEventArgs> OnCaptureStop;

        public static Boolean IsActive => status != Status.Inactive;
        public static Boolean IsNotActive => status == Status.Inactive;

        static ScreenCapturer() {
            globalException = null;
            waitHandle = null;
            bitmapQueue = null;
            captureThread = null;
            callbackThread = null;
            status = Status.Inactive;
            SkipFirstFrame = true;
            SkipFrames = true;
            PreserveBitmap = false;
        }

        public static void StartCapture(Int32 displayIndex = 0, Int32 adapterIndex = 0) {
            StartCapture(null, displayIndex, adapterIndex);
        }

        public static void StartCapture(Action<Bitmap> onScreenUpdated, Int32 displayIndex = 0, Int32 adapterIndex = 0) {
            if (status == Status.Inactive) {
                waitHandle = new AutoResetEvent(false);
                bitmapQueue = new ConcurrentQueue<Bitmap>();
                captureThread = new Thread(() => CaptureMain(adapterIndex, displayIndex));
                callbackThread = new Thread(() => CallbackMain(onScreenUpdated));
                status = Status.Starts;
                captureThread.Priority = ThreadPriority.Highest;
                captureThread.Start();
                callbackThread.Start();
            }
        }

        public static void StopCapture() {
            if (status == Status.Active) {
                status = Status.Stops;
            }
        }

        private static void CaptureMain(Int32 adapterIndex, Int32 displayIndex) {
            Resource screenResource = null;
            try {
                using (Factory1 factory1 = new Factory1()) {
                    using (Adapter1 adapter1 = factory1.GetAdapter1(adapterIndex)) {
                        using (Device device = new Device(adapter1)) {
                            using (Output output = adapter1.GetOutput(displayIndex)) {
                                using (Output1 output1 = output.QueryInterface<Output1>()) {
                                    Int32 width = output1.Description.DesktopBounds.Right - output1.Description.DesktopBounds.Left;
                                    Int32 height = output1.Description.DesktopBounds.Bottom - output1.Description.DesktopBounds.Top;
                                    Rectangle bounds = new Rectangle(Point.Empty, new Size(width, height));
                                    Texture2DDescription texture2DDescription = new Texture2DDescription {
                                        CpuAccessFlags = CpuAccessFlags.Read,
                                        BindFlags = BindFlags.None,
                                        Format = Format.B8G8R8A8_UNorm,
                                        Width = width,
                                        Height = height,
                                        OptionFlags = ResourceOptionFlags.None,
                                        MipLevels = 1,
                                        ArraySize = 1,
                                        SampleDescription = { Count = 1, Quality = 0 },
                                        Usage = ResourceUsage.Staging
                                    };
                                    using (Texture2D texture2D = new Texture2D(device, texture2DDescription)) {
                                        using (OutputDuplication outputDuplication = output1.DuplicateOutput(device)) {
                                            status = Status.Active;
                                            Int32 frameNumber = 0;
                                            do {
                                                try {
                                                    Result result = outputDuplication.TryAcquireNextFrame(100, out _, out screenResource);
                                                    if (result.Success) {
                                                        frameNumber += 1;
                                                        using (Texture2D screenTexture2D = screenResource.QueryInterface<Texture2D>()) {
                                                            device.ImmediateContext.CopyResource(screenTexture2D, texture2D);
                                                            DataBox dataBox = device.ImmediateContext.MapSubresource(texture2D, 0, MapMode.Read, MapFlags.None);
                                                            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                                                            BitmapData bitmapData = bitmap.LockBits(bounds, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                                                            IntPtr dataBoxPointer = dataBox.DataPointer;
                                                            IntPtr bitmapDataPointer = bitmapData.Scan0;

                                                            

                                                           

                                                            for (Int32 y = 0; y < height; y++) {
                                                                Utilities.CopyMemory(bitmapDataPointer, dataBoxPointer, width * 4);
                                                                dataBoxPointer = IntPtr.Add(dataBoxPointer, dataBox.RowPitch);
                                                                bitmapDataPointer = IntPtr.Add(bitmapDataPointer, bitmapData.Stride);

                                                            }

                                                           


                                                            bitmap.UnlockBits(bitmapData);
                                                            Graphics g = Graphics.FromImage(bitmap);
                                                            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                                                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                                                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                                                            User32.CURSORINFO cursorInfo;
                                                            cursorInfo.cbSize = Marshal.SizeOf(typeof(User32.CURSORINFO));
                                                            Int32 iconX = 0, iconY = 0;
                                                            IntPtr iconPointer;
                                                            if (User32.GetCursorInfo(out cursorInfo))
                                                            {
                                                                // if the cursor is showing draw it on the screen shot
                                                                if (cursorInfo.flags == User32.CURSOR_SHOWING)
                                                                {
                                                                    iconPointer = User32.CopyIcon(cursorInfo.hCursor);
                                                                    User32.ICONINFO iconInfo;

                                                                    if (User32.GetIconInfo(iconPointer, out iconInfo))
                                                                    {
                                                                        iconX = (int)((cursorInfo.ptScreenPos.x - ((int)iconInfo.xHotspot)));
                                                                        iconY = (int)((cursorInfo.ptScreenPos.y - ((int)iconInfo.yHotspot)));


                                                                        User32.DrawIcon(g.GetHdc(), iconX, iconY, cursorInfo.hCursor);
                                                                        g.ReleaseHdc();
                                                                    }
                                                                }
                                                            }


                                                            device.ImmediateContext.UnmapSubresource(texture2D, 0);
                                                            while (SkipFrames && bitmapQueue.Count > 1) {
                                                                bitmapQueue.TryDequeue(out Bitmap dequeuedBitmap);
                                                                dequeuedBitmap.Dispose();
                                                            }
                                                            if (frameNumber > 1 || SkipFirstFrame == false) {
                                                                bitmapQueue.Enqueue(bitmap);
                                                                waitHandle.Set();
                                                            }
                                                        }
                                                    } else {
                                                        if (ResultDescriptor.Find(result).ApiCode != Result.WaitTimeout.ApiCode) {
                                                            result.CheckError();
                                                        }
                                                    }
                                                } finally {
                                                    screenResource?.Dispose();
                                                    try {
                                                        outputDuplication.ReleaseFrame();
                                                    } catch { }
                                                }
                                            } while (status == Status.Active);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (Exception exception) {
                globalException = exception;
                status = Status.Stops;
            } finally {
                callbackThread.Join();
                Exception exception = globalException;
                while (bitmapQueue.Count > 0) {
                    bitmapQueue.TryDequeue(out Bitmap dequeuedBitmap);
                    dequeuedBitmap.Dispose();
                }
                globalException = null;
                waitHandle = null;
                bitmapQueue = null;
                captureThread = null;
                callbackThread = null;
                status = Status.Inactive;
                if (OnCaptureStop != null) {
                    OnCaptureStop(null, new OnCaptureStopEventArgs(exception != null ? new Exception(exception.Message, exception) : null));
                } else {
                    if (exception != null) {
                        ExceptionDispatchInfo.Capture(exception).Throw();
                    }
                }
            }
        }

        private static void CallbackMain(Action<Bitmap> onScreenUpdated) {
            try {
                while (status <= Status.Active) {
                    while (waitHandle.WaitOne(10) && bitmapQueue.TryDequeue(out Bitmap bitmap)) {
                        try {
                            onScreenUpdated?.Invoke(bitmap);
                            OnScreenUpdated?.Invoke(null, new OnScreenUpdatedEventArgs(bitmap));
                        } finally {
                            if (!PreserveBitmap) {
                                bitmap.Dispose();
                            }
                        }
                    }
                }
            } catch (Exception exception) {
                globalException = exception;
                status = Status.Stops;
            }
        }

    }

}
