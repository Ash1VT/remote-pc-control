using ScreenCapturerNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Server
{
    public class ScreenManager
    {
        private bool _isRunning;

        public delegate void ScreenHandler(Image changedPart, Point startPoint);

        public event ScreenHandler? ScreenChanged;
        

        public ScreenManager() 
        {
            _isRunning = false;
        }

        public void Start()
        {
            _isRunning = true;
            //int frames = 0;
            //System.Timers.Timer timer = new System.Timers.Timer(1000);
            //timer.Elapsed += (Object source, ElapsedEventArgs e) => { Console.WriteLine(frames); frames = 0; };
            //timer.AutoReset = true;
            //timer.Enabled = true;
            //timer.Start();


            var oldScreen = new Bitmap(1920, 1080);

            ScreenCapturer.StartCapture((Bitmap newScreen) =>
            {
                
                
                        ScreenChanged?.Invoke(newScreen, new Point(0,0));
                       
               
                //}
                //Rectangle bounds = new Rectangle(0, 0, oldScreen.Width, newScreen.Height);
                //var bmpDataA = oldScreen.LockBits(bounds, ImageLockMode.ReadWrite, oldScreen.PixelFormat);
                //var bmpDataB = newScreen.LockBits(bounds, ImageLockMode.ReadWrite, newScreen.PixelFormat);

                //const int height = 720;
                //int npixels = height * bmpDataA.Stride / 4;
                //unsafe
                //{
                //    int* pPixelsA = (int*)bmpDataA.Scan0.ToPointer();
                //    int* pPixelsB = (int*)bmpDataB.Scan0.ToPointer();

                //    for (int i = 0; i < npixels; ++i)
                //    {
                //        if (pPixelsA[i] != pPixelsB[i])
                //        {
                //            pPixelsB[i] = Color.Black.ToArgb();
                //        }
                //    }
                //}
                //oldScreen.UnlockBits(bmpDataA);
                //newScreen.UnlockBits(bmpDataB);


                //Point? point1 = null, point2 = null, point3 = null, point4 = null;

                //Task findFirstPixel = Task.Run(() =>
                //{
                //    var enumerator = oldScreen.GetDirectEnumerator();
                //    while (enumerator.MoveNext())
                //    {
                //        Point point = enumerator.Current;
                //        if (oldScreen[point] != newScreen[point])
                //        {
                //            point1 = point;
                //            break;
                //        }
                //    }
                //});



                //Task findSecondPixel = Task.Run(() =>
                //{
                //    var reverseEnumerator = oldScreen.GetReverseEnumerator();
                //    while (reverseEnumerator.MoveNext())
                //    {
                //        Point point = reverseEnumerator.Current;
                //        if (oldScreen[point] != newScreen[point])
                //        {
                //            point2 = point;
                //            break;
                //        }
                //    }
                //});

                //Task findFirstRotatedPixel = Task.Run(() =>
                //{
                //    var revers2eEnumerator = oldScreen.GetRotatedDirectEnumerator();
                //    while (revers2eEnumerator.MoveNext())
                //    {
                //        Point point = revers2eEnumerator.Current;
                //        if (oldScreen[point] != newScreen[point])
                //        {
                //            point3 = point;
                //            break;
                //        }
                //    }
                //});

                //Task findSecondRotatedPixel = Task.Run(() =>
                //{
                //    var r3everseEnumerator = oldScreen.GetRotatedReverseEnumerator();
                //    while (r3everseEnumerator.MoveNext())
                //    {
                //        Point point = r3everseEnumerator.Current;
                //        if (oldScreen[point] != newScreen[point])
                //        {
                //            point4 = point;
                //            break;
                //        }
                //    }
                //});


                //Task.WaitAll(findFirstPixel, findSecondPixel, findFirstRotatedPixel, findSecondRotatedPixel);

                //Task.Run(() =>
                //{
                //    if (point1 != null && point2 != null && point3 != null && point4 != null)
                //    {
                //        Point point11 = new Point(point1.Value.X, point1.Value.Y);
                //        Point point12 = new Point(point2.Value.X, point2.Value.Y);
                //        Point point13 = new Point(point3.Value.X, point3.Value.Y);
                //        Point point14 = new Point(point4.Value.X, point4.Value.Y);


                //        Point startPoint = new Point(Math.Min(Math.Min(point11.X, point12.X), Math.Min(point13.X, point14.X)),
                //                                Math.Min(Math.Min(point11.Y, point12.Y), Math.Min(point13.Y, point14.Y)));
                //        Point endPoint = new Point(Math.Max(Math.Max(point11.X, point12.X), Math.Max(point13.X, point14.X)),
                //                                Math.Max(Math.Max(point11.Y, point12.Y), Math.Max(point13.Y, point14.Y)));
                //        ScreenChanged?.Invoke(newScreen.Clone(startPoint, endPoint), startPoint);
                //    }
                //});

                //oldScreen.Dispoe();
            });


            //var screenStateLogger = new ScreenStateLogger();
            //screenStateLogger.ScreenRefreshed += (sender, data) =>
            //{
            //    frames++;
            //};
            //screenStateLogger.Start();

            //new Task(() =>
            //{
            //    while (_isRunning)
            //    {

            //        var newScreen = new ImageWrapper(ScreenMaker.Capture(0, 0, 1920, 1080));
            //        Point? point1 = null, point2 = null, point3 = null, point4 = null;

            //        Task findFirstPixel = Task.Run(() =>
            //        {
            //            var enumerator = oldScreen.GetDirectEnumerator();
            //            while (enumerator.MoveNext())
            //            {
            //                Point point = enumerator.Current;
            //                if (oldScreen[point] != newScreen[point])
            //                {
            //                    point1 = point;
            //                    break;
            //                }
            //            }
            //        });



            //        Task findSecondPixel = Task.Run(() =>
            //        {
            //            var reverseEnumerator = oldScreen.GetReverseEnumerator();
            //            while (reverseEnumerator.MoveNext())
            //            {
            //                Point point = reverseEnumerator.Current;
            //                if (oldScreen[point] != newScreen[point])
            //                {
            //                    point2 = point;
            //                    break;
            //                }
            //            }
            //        });

            //        Task findFirstRotatedPixel = Task.Run(() =>
            //        {
            //            var revers2eEnumerator = oldScreen.GetRotatedDirectEnumerator();
            //            while (revers2eEnumerator.MoveNext())
            //            {
            //                Point point = revers2eEnumerator.Current;
            //                if (oldScreen[point] != newScreen[point])
            //                {
            //                    point3 = point;
            //                    break;
            //                }
            //            }
            //        });

            //        Task findSecondRotatedPixel = Task.Run(() =>
            //        {
            //            var r3everseEnumerator = oldScreen.GetRotatedReverseEnumerator();
            //            while (r3everseEnumerator.MoveNext())
            //            {
            //                Point point = r3everseEnumerator.Current;
            //                if (oldScreen[point] != newScreen[point])
            //                {
            //                    point4 = point;
            //                    break;
            //                }
            //            }
            //        });


            //        Task.WaitAll(findFirstPixel, findSecondPixel, findFirstRotatedPixel, findSecondRotatedPixel);

            //        Task.Run(() =>
            //        {
            //            if (point1 != null && point2 != null && point3 != null && point4 != null)
            //            {
            //                Point point11 = new Point(point1.Value.X, point1.Value.Y);
            //                Point point12 = new Point(point2.Value.X, point2.Value.Y);
            //                Point point13 = new Point(point3.Value.X, point3.Value.Y);
            //                Point point14 = new Point(point4.Value.X, point4.Value.Y);


            //                Point startPoint = new Point(Math.Min(Math.Min(point11.X, point12.X), Math.Min(point13.X, point14.X)),
            //                                        Math.Min(Math.Min(point11.Y, point12.Y), Math.Min(point13.Y, point14.Y)));
            //                Point endPoint = new Point(Math.Max(Math.Max(point11.X, point12.X), Math.Max(point13.X, point14.X)),
            //                                        Math.Max(Math.Max(point11.Y, point12.Y), Math.Max(point13.Y, point14.Y)));
            //                ScreenChanged?.Invoke(newScreen.Clone(startPoint, endPoint), startPoint);
            //            }
            //        });

            //        frames++;
            //        oldScreen.Dispose();
            //        oldScreen = newScreen;
            //    }
            //}).Start();
        }
        private unsafe Tuple<Bitmap, Point> GetDiffBitmap(Bitmap bmp, Bitmap bmp2)
        {
            if (bmp.Width != bmp2.Width || bmp.Height != bmp2.Height)
                throw new Exception("Sizes must be equal.");

            Bitmap bmpRes = null;
            Point startPoint = new Point(bmp.Width - 1, bmp.Height - 1);
            Point endPoint = new Point(0, 0);


            System.Drawing.Imaging.BitmapData bmData = null;
            System.Drawing.Imaging.BitmapData bmData2 = null;
            System.Drawing.Imaging.BitmapData bmDataRes = null;

            try
            {

                //bmpRes = new Bitmap(bmp.Width, bmp.Height);
                


                bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bmData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bmDataRes = bmpRes.LockBits(new Rectangle(0, 0, bmpRes.Width, bmpRes.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                IntPtr scan0 = bmData.Scan0;
                IntPtr scan02 = bmData2.Scan0;
                IntPtr scan0Res = bmDataRes.Scan0;

                int stride = bmData.Stride;
                int stride2 = bmData2.Stride;
                int strideRes = bmDataRes.Stride;

                int nWidth = bmp.Width;
                int nHeight = bmp.Height;

                //for(int y = 0; y < nHeight; y++)
                System.Threading.Tasks.Parallel.For(0, nHeight, y =>
                {
                    //define the pointers inside the first loop for parallelizing
                    byte* p = (byte*)scan0.ToPointer();
                    p += y * stride;
                    byte* p2 = (byte*)scan02.ToPointer();
                    p2 += y * stride2;
                    byte* pRes = (byte*)scan0Res.ToPointer();
                    pRes += y * strideRes;

                    if (y < startPoint.Y)
                        startPoint.Y = y;
                    if (y > endPoint.Y)
                        endPoint.Y = y;

                    for (int x = 0; x < nWidth; x++)
                    {
                        //always get the complete pixel when differences are found
                        if (p[0] != p2[0] || p[1] != p2[1] || p[2] != p2[2])
                        {
                            if (x < startPoint.X)
                                startPoint.X = x;
                            if (x > endPoint.X)
                                endPoint.X = x;
                            

                            //pRes[0] = p2[0];
                            //pRes[1] = p2[1];
                            //pRes[2] = p2[2];

                            ////alpha (opacity)
                            //pRes[3] = p2[3];
                        }

                        p += 4;
                        p2 += 4;
                        //pRes += 4;
                    }
                });

                bmp.UnlockBits(bmData);
                bmp2.UnlockBits(bmData2);
                //bmpRes.UnlockBits(bmDataRes);
            }
            catch
            {
                if (bmData != null)
                {
                    try
                    {
                        bmp.UnlockBits(bmData);
                    }
                    catch
                    {

                    }
                }

                if (bmData2 != null)
                {
                    try
                    {
                        bmp2.UnlockBits(bmData2);
                    }
                    catch
                    {

                    }
                }

                //if (bmDataRes != null)
                //{
                //    try
                //    {
                //        bmpRes.UnlockBits(bmDataRes);
                //    }
                //    catch
                //    {

                //    }
                //}

                //if (bmpRes != null)
                //{
                //    bmpRes.Dispose();
                //    bmpRes = null;
                //}
            }

            int offset = 100;

            if (startPoint.X - offset < 0)
                startPoint.X = 0;
            else
            {
                startPoint.X -= offset;
            }

            if (startPoint.Y - offset < 0)
                startPoint.Y = 0;
            else
            {
                startPoint.Y -= offset;
            }

            if (endPoint.X + offset >= bmp2.Width)
                endPoint.X = bmp2.Width - 1;
            else
            {
                endPoint.X += offset;
            }

            if (endPoint.Y + offset >= bmp2.Height)
                endPoint.Y = bmp2.Height - 1;
            else
            {
                endPoint.Y += offset;
            }



            int width = endPoint.X - startPoint.X;
            int height = endPoint.Y - startPoint.Y;
            //width = width == 0 ? 1 : width;
            //height = height == 0 ? 1 : height;

            if (width <= 0 || height <= 0)
                return null;

            return new Tuple<Bitmap, Point>(bmp2.Clone(new Rectangle(startPoint, new Size(width, height)), bmp2.PixelFormat), startPoint);
        }
        public void Stop()
        {
            _isRunning = false;
        }
    }
}
