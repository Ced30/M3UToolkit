using System.Runtime.InteropServices;

namespace M3UToolkit {

    /// <summary>
    /// "About" window for M3UToolkit.
    /// Displays version, author, and license information in a read-only TextBox.
    /// The window is centered relative to its parent and includes a "Close" button to dismiss it.
    /// The TextBox caret is hidden even when it receives focus.
    /// </summary>
    internal class AboutForm : Form {
        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        public AboutForm() {
            this.Text = "About M3UToolkit";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            TextBox textBoxAbout = new TextBox {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                Text = @"
    M3UToolkit v1.2.0
    100 % safe multi-disc .m3u creator

    Created by Ced30
    https://github.com/Ced30/M3UToolkit

    Open-source under GPL-3.0

    Commercial integration in paid products:
    → contact ced30.dev@proton.me for private license",
                TabStop = false
            };

            // hide cursor when textbox gets focus
            textBoxAbout.GotFocus += (s, e) => HideCaret(textBoxAbout.Handle);

            Button btnClose = new Button {
                Text = "Close",
                Dock = DockStyle.Bottom,
                Height = 35
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(textBoxAbout);
            this.Controls.Add(btnClose);

            // focus on the close buton on startup
            this.Shown += (s, e) => {
                this.ActiveControl = btnClose;
                HideCaret(textBoxAbout.Handle);
            };
        }
    }
}
