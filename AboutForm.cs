using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace M3UToolkit
{
    public class AboutForm : Form
    {
        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        public AboutForm()
        {
            this.Text = "About M3UToolkit";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            // TextBox pour afficher le texte About
            TextBox textBoxAbout = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                Text =
@"M3UToolkit v1.0
100 % safe multi-disc .m3u creator

Created by Ced30
https://github.com/Ced30/M3UToolkit

Open-source under GPL-3.0

Commercial integration in paid products:
→ contact ced30.dev@proton.me for private license",
                TabStop = false
            };

            // Cacher le caret dès que le TextBox reçoit le focus
            textBoxAbout.GotFocus += (s, e) => HideCaret(textBoxAbout.Handle);

            // Bouton Close
            Button btnClose = new Button
            {
                Text = "Close",
                Dock = DockStyle.Bottom,
                Height = 35
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(textBoxAbout);
            this.Controls.Add(btnClose);

            // Focus sur le bouton Close au lancement
            this.Shown += (s, e) =>
            {
                this.ActiveControl = btnClose;
                HideCaret(textBoxAbout.Handle);
            };
        }
    }
}
