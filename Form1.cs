using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Pranav_UR2_Armbot_VS_Code_v2
{
    public partial class Form1 : Form
    {
        // webcam feed
        private VideoCapture _capture;

        // binary threshold value
        private int binaryThreshold;

        // intialize serial port
        SerialPort serialPort;

        // makes sure that the bot is ready to receieve new coordinates
        private int newCoordReady = 1; // 1 = READY, 0 = BUSY

      
        // hold the position and shape of the next target.
        private (PointF position, ShapeType shape)? pendingTarget = null;


        // for shape id, have square be 0 and triangle be 1
        enum ShapeType
        {
            Square = 0,
            Triangle = 1
        }


        public Form1()
        {
            // serial port properties
            serialPort = new SerialPort("COM8", 9600);

            // open the serial port
            try
            {
                serialPort.Open();
            }
            catch
            {
                // if it fails print an error message
                Console.WriteLine("Unable to open COM port - check if the port is already in use.");
            }

            // process data from arduino (via serial port) into this function
            serialPort.DataReceived += SerialPort_DataReceived;


            InitializeComponent();

            // write FORM STARTED to debug output window
            Debug.WriteLine("FORM STARTED");

            // start video capture
            _capture = new VideoCapture(1);
            //Application.Idle += ProcessFrame;

            // binary threshold parameters
            binaryThreshBar.Value = (int)binaryThreshold;
            binaryThreshBar.Minimum = 0;
            binaryThreshBar.Maximum = 255;
            binaryThreshBar.TickFrequency = 1;
            binaryThreshBar.Value = 30;
        }

        // start the frame processing after clicking the Start button
        private void startButton_Click(object sender, EventArgs e)
        {
            Application.Idle += ProcessFrame;
        }

        // create a default frame and display raw webcam image
        private void ProcessFrame(object sender, EventArgs e)
        {
            Mat frame = _capture.QueryFrame();
            // reseize frame
            int newHeight = (frame.Size.Height * rawWebcamImg.Size.Width)/frame.Size.Width;
            Size newSize = new Size(rawWebcamImg.Size.Width, newHeight);
            CvInvoke.Resize(frame, frame, newSize);
            if (frame != null)
            {
                rawWebcamImg.Image = frame.ToBitmap();
                ProcessImage(frame);
            }
        }

        // image processing operations
        private void ProcessImage(Mat frame)
        {
            // set the pending target to null at the start of each frame (changes if shape is detected)
            pendingTarget = null;

            //////////////// Image Warping /////////////
            // Make image look like it's being viewed top down

            PointF[] srcPoints = new PointF[]
            {
                    new PointF(0, 0),
                    new PointF(frame.Width, 0),
                    new PointF(0, frame.Height),
                    new PointF(frame.Width, frame.Height)
            };

            // new frame
            PointF[] dstPoints = new PointF[]
        {
                new PointF(frame.Width*-0.35f, frame.Height*-0.8f),
                new PointF(frame.Width*1.375f, frame.Height*-0.8f),
                new PointF(0, frame.Height*1.3f),
                new PointF(frame.Width, frame.Height*1.3f)
        };

            // apply the warp
            Mat perspectiveMatrix = CvInvoke.GetPerspectiveTransform(srcPoints, dstPoints);
            Mat warpedFrame = new Mat();
            CvInvoke.WarpPerspective(frame, warpedFrame, perspectiveMatrix, frame.Size);

            // display the warped image
            warpedImg.Image = warpedFrame.ToBitmap();


            //////////////// Canny Image ///////////////
            // create a binary copy of the original image
            Mat grayFrame = new Mat();
            CvInvoke.CvtColor(warpedFrame, grayFrame, ColorConversion.Bgr2Gray);
            Mat binaryFrame = new Mat();
            CvInvoke.Threshold(grayFrame, binaryFrame, binaryThreshBar.Value, binaryThreshBar.Maximum, ThresholdType.BinaryInv);

            // display trackbar value
            binThreshText.Text = "" + binaryThreshBar.Value;

            // display binary mask
            binaryImg.Image = binaryFrame.ToBitmap();

            Mat edges = new Mat();
            CvInvoke.Canny(grayFrame, edges, 100, 200);


            // Contour Data
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(binaryFrame, contours, null, RetrType.External,
                    ChainApproxMethod.ChainApproxSimple);

               // create a clone of warpedFrame (new frame applies effects onto warpedFrame clone)
                Mat contoursFrame = warpedFrame.Clone();


                // get frame center in pixels
                int centerX = warpedFrame.Width / 2;
                int centerY = warpedFrame.Height / 2;
                Point paperCenter = new Point(centerX, centerY);

                // known real-world position in inches (center of paper/frame is alinged with robot base and 9 inches away)
                float realWorldX = 0f;
                float realWorldY = 9.0f;

                // draw center dot
                CvInvoke.Circle(contoursFrame, paperCenter, 3, new MCvScalar(255, 0, 255), -1);

                

               // draw contours
                for (int i = 0; i < contours.Size; i++)
                {
                    VectorOfPoint approx = new VectorOfPoint();
                    CvInvoke.ApproxPolyDP(contours[i], approx, 0.04 * CvInvoke.ArcLength(contours[i], true),
                    true);

                    // id the shape
                    ShapeType shape;

                    // if 3 sides are seen, shapeID is triangle (defined as 1 above), if 4 shapeID is square (0)
                    if (approx.Size == 3)
                        shape = ShapeType.Triangle;
                    else if (approx.Size == 4)
                        shape = ShapeType.Square;
                    else
                        // ignore everything else
                        continue; 

                    // use moments to find centroids of shapes in inches (robot base is at bottom of screen, so y increases going up, x increases going left)
                    var moments = CvInvoke.Moments(approx);
                    if (moments.M00 != 0)
                    {
                        // use moments to find centroid
                        int cx = (int)(moments.M10 / moments.M00);
                        int cy = (int)(moments.M01 / moments.M00);
                        Point centroid = new Point(cx, cy);

                        // put cicle at centroid
                        CvInvoke.Circle(contoursFrame, centroid, 5, new MCvScalar(0, 0, 255), -1);

                        // Convert to inches
                        float inchPerPixelX = 8.5f / warpedFrame.Width;
                        float inchPerPixelY = 11.0f / warpedFrame.Height;
                        
                        // find distance centroid is from circle (for kinematics)
                        float xFromCenter = (cx - warpedFrame.Width / 2f) * inchPerPixelX;
                        float yFromCenter = (cy - warpedFrame.Height / 2f) * inchPerPixelY;

                        // Transform to robot's coordinate system (base is at top of pag)
                        float realX = -xFromCenter;      // left is positive
                        float realY = yFromCenter + 9f;  // 9 inches from center to base

                        // Display coordinates in inches
                        CvInvoke.PutText(contoursFrame, $"({realX:F2}, {realY:F2}) in",
                            new Point(cx + 10, cy), FontFace.HersheySimplex, 0.5, new MCvScalar(0, 255, 0), 1);
                                                                    

                        // if there is no chosen shape to target, choose the one seen in this specififc frame (must be valid shape)
                        // get it's coordinates and shape type (triangle or square)
                        if (pendingTarget == null)
                        {
                            pendingTarget = (new PointF(realX, realY), shape);
                        }                 
                    }

                    // if it has 3 sides, idenitfy shape as triangle
                    if (approx.Size == 3)
                    {
                        CvInvoke.PutText(contoursFrame, "Triangle", approx[0], FontFace.HersheySimplex, 0.5, new
                        MCvScalar(0, 255, 255));
                        CvInvoke.DrawContours(contoursFrame, contours, i,
                            new MCvScalar(0, 255, 255), 2);

                    }

                    // if it has 4 sides, idenitfy shape as square
                    else if (approx.Size == 4)
                    {
                        CvInvoke.PutText(contoursFrame, "Square", approx[0], FontFace.HersheySimplex, 0.5, new
                        MCvScalar(255, 255, 0));
                        CvInvoke.DrawContours(contoursFrame, contours, i,
                            new MCvScalar(255, 255, 0), 2);

                    }                       
                }

                // if the bot can recieve new coordinates and has a pendingTarget value
                if (Volatile.Read(ref newCoordReady) == 1 && pendingTarget.HasValue)
                {
                    // have the target be the coordinates and shape id
                    var target = pendingTarget.Value;

                    // and send these values to arduino
                    serialPort.WriteLine(
                        $"<{target.position.X:F2},{target.position.Y:F2},{(int)target.shape}>");

                    // write these values in the output window as well just to be sure the correct info is being sent
                    Debug.WriteLine(
                        $"<{target.position.X:F2},{target.position.Y:F2},{(int)target.shape}>");
                    
                    // reset newCoordReady to 0 (bot is busy) and have there be no pendingTarget as of now
                    Volatile.Write(ref newCoordReady, 0);
                    pendingTarget = null;
                }


                // display the shapes and their centroids
                shpeIDContorImg.Image = contoursFrame.ToBitmap();

            }

            // if pendingTarget has no value (is null)
            if (!pendingTarget.HasValue)
            {
                // print this to debug output
                Debug.WriteLine("No shape detected — waiting");
            }
        }

        // used to determine if we can send new coordinates to bot
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // read message from arduino (bot)
                string msg = serialPort.ReadLine().Trim();

                if (msg == "1")
                {
                    // bot is ready
                    Interlocked.Exchange(ref newCoordReady, 1);
                    Debug.WriteLine("RX READY (1)");
                }
                else if (msg == "0")
                {
                    // bot is busy
                    Interlocked.Exchange(ref newCoordReady, 0);
                    Debug.WriteLine("RX BUSY (0)");
                }
                else
                {
                    // Ignore all other Arduino debug text (just incase)
                    Debug.WriteLine("Trash (ignored): " + msg);
                }
            }
            catch { }
        }


        // when window is closed, close webcam feed
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_capture != null)
            {
                _capture.Dispose();
            }
        }

        // Did not write code for Stop button (cannot remove or program will give errors)
        private void stopButton_Click(object sender, EventArgs e)
        {
           
        }
    }
}
