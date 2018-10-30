using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SoundRecognition
{
     public partial class SoundVisualizationUserControl : UserControl
     {
          public Figure Figure = new Figure(123, 123);

          public SoundVisualizationUserControl()
          {
               InitializeComponent();

               // add a mousewheel scroll handler
               pictureBox1.MouseWheel += new MouseEventHandler(this.pictureBox1_MouseWheel);

               // style the plot area
               Figure.styleForm();
               Figure.Zoom(.8, .8);
               Figure.labelTitle = "FFT";
          }

          private class SignalData
          {
               public double[] Values;
               public double SampleRate;
               public double XSpacing;
               public double OffsetX;
               public double OffsetY;
               public float LineWidth;
               public Color LineColor;
               public string Label;

               public SignalData(double[] values, double sampleRate, double offsetX = 0, double offsetY = 0, Color? lineColor = null, float lineWidth = 1, string label = null)
               {
                    this.Values = values;
                    this.SampleRate = sampleRate;
                    this.XSpacing = 1.0 / sampleRate;
                    this.OffsetX = offsetX;
                    this.OffsetY = offsetY;
                    if (lineColor == null) lineColor = Color.Green;//testing red
                    this.LineColor = (Color)lineColor;
                    this.LineWidth = lineWidth;
                    this.Label = label;
               }
          }

          private class XYData
          {
               public double[] Xs;
               public double[] Ys;
               public float lineWidth;
               public Color lineColor;
               public float markerSize;
               public Color markerColor;
               public string label;

               public XYData(double[] Xs, double[] Ys, float lineWidth = 1, Color? lineColor = null, float markerSize = 3, Color? markerColor = null, string label = null)
               {
                    this.Xs = Xs;
                    this.Ys = Ys;
                    this.lineWidth = lineWidth;
                    this.markerSize = markerSize;
                    this.label = label;
                    if (lineColor == null) lineColor = Color.Red;
                    this.lineColor = (Color)lineColor;
                    if (markerColor == null) markerColor = Color.Red;
                    this.markerColor = (Color)markerColor;
               }
          }

          private class AxisLine
          {
               public double value;
               public float lineWidth;
               public Color lineColor;
               public AxisLine(double Ypos, float lineWidth, Color lineColor, string label = null)
               {
                    this.value = Ypos;
                    this.lineWidth = lineWidth;
                    this.lineColor = lineColor;
               }
          }

          private List<SignalData> signalDataList = new List<SignalData>();
          private List<XYData> xyDataList = new List<XYData>();
          private List<AxisLine> hLines = new List<AxisLine>();
          private List<AxisLine> vLines = new List<AxisLine>();

          public void Hline(double Ypos, float lineWidth, Color lineColor)
          {
               hLines.Add(new AxisLine(Ypos, lineWidth, lineColor));
               Render();
          }

          public void Vline(double Xpos, float lineWidth, Color lineColor)
          {
               vLines.Add(new AxisLine(Xpos, lineWidth, lineColor));
               Render();
          }

          public void PlotXY(double[] Xs, double[] Ys, Color? color = null)
          {
               xyDataList.Add(new XYData(Xs, Ys, lineColor: color, markerColor: color));
               Figure.GraphClear();
               Render();
          }

          public void PlotSignal(double[] values, double sampleRate, Color? color = null, double offsetX = 0, double offsetY = 0)
          {
               signalDataList.Add(new SignalData(values, sampleRate, lineColor: color, offsetX: offsetX, offsetY: offsetY));
               Figure.GraphClear();
               Render();
          }

          public void Clear(bool renderAfterClearing = false)
          {
               xyDataList.Clear();
               signalDataList.Clear();
               hLines.Clear();
               vLines.Clear();
               if (renderAfterClearing) Render();
          }

          public void SaveDialog(string filename = "output.png")
          {
               SaveFileDialog savefile = new SaveFileDialog();
               savefile.FileName = filename;
               savefile.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*";
               if (savefile.ShowDialog() == DialogResult.OK) filename = savefile.FileName;
               else return;

               string basename = System.IO.Path.GetFileNameWithoutExtension(filename);
               string extension = System.IO.Path.GetExtension(filename).ToLower();
               string fullPath = System.IO.Path.GetFullPath(filename);

               switch (extension)
               {
                    case ".png":
                         pictureBox1.Image.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                         break;
                    case ".jpg":
                         pictureBox1.Image.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                         break;
                    case ".bmp":
                         pictureBox1.Image.Save(filename);
                         break;
                    default:
                         //TODO: messagebox error
                         break;
               }
          }

          public void AxisAuto()
          {
               double x1 = 0, x2 = 0, y1 = 0, y2 = 0;

               try
               {
                    foreach (XYData xyData in xyDataList)
                    {
                         if (x1 == x2)
                         {
                              // this is the first data we are scaling to, so just copy its bounds
                              x1 = xyData.Xs.Min();
                              x2 = xyData.Xs.Max();
                              y1 = xyData.Ys.Min();
                              y2 = xyData.Ys.Max();
                         }
                         else
                         {
                              // we've seen some data before, so only take it if it expands the axes
                              x1 = Math.Min(x1, xyData.Xs.Min());
                              x2 = Math.Max(x2, xyData.Xs.Max());
                              y1 = Math.Min(y1, xyData.Ys.Min());
                              y2 = Math.Max(y2, xyData.Ys.Max());
                         }
                    }
                    foreach (SignalData signalData in signalDataList)
                    {
                         if (x1 == x2)
                         {
                              // this is the first data we are scaling to, so just copy its bounds
                              x1 = signalData.OffsetX;
                              x2 = signalData.OffsetX + signalData.Values.Length * signalData.XSpacing;
                              y1 = signalData.Values.Min() + signalData.OffsetY;
                              y2 = signalData.Values.Max() + signalData.OffsetY;
                         }
                         else
                         {
                              // we've seen some data before, so only take it if it expands the axes
                              x1 = Math.Min(x1, signalData.OffsetX);
                              x2 = Math.Max(x2, signalData.OffsetX + signalData.Values.Length * signalData.XSpacing);
                              y1 = Math.Min(y1, signalData.Values.Min() + signalData.OffsetY);
                              y2 = Math.Max(y2, signalData.Values.Max() + signalData.OffsetY);
                         }
                    }

                    Figure.AxisSet(x1, x2, y1, y2);
                    Figure.Zoom(null, .9);
                    Render(true);
               }
               catch (InvalidOperationException)
               {
               }
          }

          private void Render(bool redrawFrame = false)
          {
               try
               {
                    Figure.BenchmarkThis(showBenchmark);
                    if (redrawFrame) Figure.FrameRedraw();
                    else Figure.GraphClear();

                    // plot XY points
                    foreach (XYData xyData in xyDataList)
                    {
                         Figure.PlotLines(xyData.Xs, xyData.Ys, xyData.lineWidth, xyData.lineColor);
                         Figure.PlotScatter(xyData.Xs, xyData.Ys, xyData.markerSize, xyData.markerColor);
                    }

                    // plot signals
                    foreach (SignalData signalData in signalDataList)
                    {
                         Figure.PlotSignal(signalData.Values, signalData.XSpacing, signalData.OffsetX, signalData.OffsetY, signalData.LineWidth, signalData.LineColor);
                    }

                    // plot axis lines
                    foreach (AxisLine axisLine in hLines)
                    {
                         Figure.PlotLines(
                             new double[] { Figure.xAxis.Min, Figure.xAxis.Max },
                             new double[] { axisLine.value, axisLine.value },
                             axisLine.lineWidth,
                             axisLine.lineColor
                             );
                    }
                    foreach (AxisLine axisLine in vLines)
                    {
                         Figure.PlotLines(
                             new double[] { axisLine.value, axisLine.value },
                             new double[] { Figure.yAxis.Min, Figure.yAxis.Max },
                             axisLine.lineWidth,
                             axisLine.lineColor
                             );
                    }

                    pictureBox1.Image = Figure.Render();
               }
               catch (InvalidOperationException)
               {
               }
          }

          // Mouse interaction.

          private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
          {
               if (e.Button == MouseButtons.Left) Figure.MousePanStart(e.X, e.Y); // left-click-drag pans
               else if (e.Button == MouseButtons.Right) Figure.MouseZoomStart(e.X, e.Y); // right-click-drag zooms
          }

          private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
          {
               if (e.Button == MouseButtons.Left) Figure.MousePanEnd();
               else if (e.Button == MouseButtons.Right) Figure.MouseZoomEnd();
          }

          private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
          {
               if (e.Button == MouseButtons.Middle) AxisAuto(); // Middle click to reset view.
          }

          private void pictureBox1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
          {
               double mag = 1.2;
               if (e.Delta > 0) Figure.Zoom(mag, mag);
               else Figure.Zoom(1.0 / mag, 1.0 / mag);
               Render();
          }

          public bool showBenchmark = false;
          private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
          {
               this.showBenchmark = !this.showBenchmark; // double-click graph to display benchmark stats
               Render();
          }

          private bool busyDrawingPlot = false;
          private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
          {
               if (Figure.MouseIsDragging() && !busyDrawingPlot)
               {
                    Figure.MouseMove(e.X, e.Y);
                    busyDrawingPlot = true;
                    Render(true);
                    Application.DoEvents();
                    busyDrawingPlot = false;
               }
          }

          private void pictureBox1_SizeChanged(object sender, EventArgs e)
          {
               Figure.Resize(pictureBox1.Width, pictureBox1.Height);
               Render(true);
          }

          public void SizeUpdate()
          {
               pictureBox1_SizeChanged(null, null);
          }

          /// <summary>
          /// Force SoundVisualizationUserControl to redraw itself. This is helpful after changing axis limits or labels.
          /// </summary>
          public void Redraw()
          {
               pictureBox1_SizeChanged(null, null);
          }
     }
}
