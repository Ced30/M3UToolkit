using M3UToolkit;
using System.Windows.Forms;

namespace M3UToolkit {

    /// <summary>
    /// Partial class for MainForm that sets up the UI layout and controls.
    /// Initializes and arranges the main UI groups (FilesGroup, RenameGroup, OutputGroup) within a single TableLayoutPanel.
    /// Adds a MenuStrip with a Help → About item linked to the AboutForm.
    /// Configures form properties such as fixed size, title, and menu integration.
    /// Ensures proper docking, auto-sizing, and scrolling behavior for all contained controls.
    /// </summary>
    partial class MainForm : Form {
        private FilesGroup filesGroup;
        private RenameGroup renameGroup;
        private OutputGroup outputGroup;

        public void InitializeComponent() {
            this.filesGroup = new FilesGroup();
            this.renameGroup = new RenameGroup();
            this.outputGroup = new OutputGroup();

            // ===== TableLayoutPanel =====
            var tableMain = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(10),
                AutoScroll = true
            };
            tableMain.RowStyles.Clear();

            // ===== Groups =====
            tableMain.Controls.Add(filesGroup.Root, 0, 0);
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            tableMain.Controls.Add(renameGroup.Root, 0, 1);
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            tableMain.Controls.Add(outputGroup.Root, 0, 2);
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // ===== MenuStrip =====
            var menuStrip = new MenuStrip();
            var menuHelp = new ToolStripMenuItem("Help");
            var menuAbout = new ToolStripMenuItem("About");
                menuAbout.Click += new EventHandler(ButtonAbout_Click);
                menuHelp.DropDownItems.Add(menuAbout);
                menuStrip.Items.Add(menuHelp);
                menuStrip.Dock = DockStyle.Top;

            // ===== Ajout aux Controls =====
            this.Controls.Add(tableMain);
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;

            // ===== Form parameters =====
            this.Text = "M3UToolkit v1.2.0";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(700, 580);
        }
    }
}
