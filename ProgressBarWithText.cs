using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3UToolkit
{
    internal class ProgressBarWithText : ProgressBar
    {
        public string ProgressText { get; set; } = "";

        public ProgressBarWithText()
        {
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rect);
            rect.Inflate(-1, -1);

            double ratio = Maximum > 0 ? (double)Value / Maximum : 0;
            e.Graphics.FillRectangle(Brushes.Blue, 0, 0, (int)(rect.Width * ratio), rect.Height);

            string text = string.IsNullOrEmpty(ProgressText) ? $"{Value}/{Maximum}" : ProgressText;

            using (Font f = new Font("Segoe UI", 9, FontStyle.Bold))
            {
                SizeF sz = e.Graphics.MeasureString(text, f);
                e.Graphics.DrawString(text, f, Brushes.White,
                    (rect.Width - sz.Width) / 2,
                    (rect.Height - sz.Height) / 2);
            }
        }
    }
}
