using System;

namespace SoundRecognition
{
    /// <summary>
    /// The MouseAxis class simplifies adjusting axis edges for click-and-drag pan and zoom events.
    /// After being instantiated with an initial axis and mouse position, it can return axis limits 
    /// for panning or zooming given a new mouse position later.
    /// </summary>
    class MouseAxis
    {
        public Axis xAxStart, yAxStart;
        public int xMouseStart, yMouseStart;
        public double x1, x2, y1, y2;

        public MouseAxis(Axis xAxis, Axis yAxis, int mouseX, int mouseY)
        {
            xAxStart = new Axis(xAxis.Min, xAxis.Max, xAxis.PXSize, xAxis.IsInverted);
            yAxStart = new Axis(yAxis.Min, yAxis.Max, yAxis.PXSize, yAxis.IsInverted);
            xMouseStart = mouseX;
            yMouseStart = mouseY;
            Pan(0, 0);
        }

        public void Pan(int xMouseNow, int yMouseNow)
        {
            int dX = xMouseStart - xMouseNow;
            int dY = yMouseNow - yMouseStart;
            x1 = xAxStart.Min + dX * xAxStart.UnitsPerPx;
            x2 = xAxStart.Max + dX * xAxStart.UnitsPerPx;
            y1 = yAxStart.Min + dY * yAxStart.UnitsPerPx;
            y2 = yAxStart.Max + dY * yAxStart.UnitsPerPx;
        }

        public void Zoom(int xMouseNow, int yMouseNow)
        {
            double dX = (xMouseNow - xMouseStart) * xAxStart.UnitsPerPx;
            double dY = (yMouseStart - yMouseNow) * yAxStart.UnitsPerPx;

            double dXFrac = dX / (Math.Abs(dX) + xAxStart.Span);
            double dYFrac = dY / (Math.Abs(dY) + yAxStart.Span);

            double xNewSpan = xAxStart.Span / Math.Pow(10, dXFrac);
            double yNewSpan = yAxStart.Span / Math.Pow(10, dYFrac);

            double xNewCenter = xAxStart.Center;
            double yNewCenter = yAxStart.Center;

            x1 = xNewCenter - xNewSpan / 2;
            x2 = xNewCenter + xNewSpan / 2;

            y1 = yNewCenter - yNewSpan / 2;
            y2 = yNewCenter + yNewSpan / 2;
        }
    }
}
