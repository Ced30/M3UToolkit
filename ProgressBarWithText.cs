namespace M3UToolkit
{
    /// <summary>
    /// A ProgressBar control that displays custom text over the progress bar.
    /// </summary>
    internal class ProgressBarWithText : ProgressBar
    {
        /// <summary>
        /// Gets or sets the text displayed on the progress bar.
        /// If empty, the bar displays the default "Value/Maximum".
        /// </summary>
        public string ProgressText { get; set; } = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarWithText"/> class
        /// with custom painting styles enabled.
        /// </summary>
        public ProgressBarWithText()
        {
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
        }

        /// <summary>
        /// Paints the progress bar and draws the custom text centered over it.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs"/> containing the graphics context.</param>
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
                e.Graphics.DrawString(
                    text,
                    f,
                    Brushes.White,
                    (rect.Width - sz.Width) / 2,
                    (rect.Height - sz.Height) / 2);
            }
        }
    }
}
