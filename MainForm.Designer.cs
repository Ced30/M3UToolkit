using M3UToolkit;
using System.Windows.Forms;

namespace M3UToolkit
{
    partial class MainForm : Form
    {
        private FilesGroup filesGroup;
        private RenameGroup renameGroup;
        private OutputGroup outputGroup;

        public void InitializeComponent()
        {
            this.filesGroup = new FilesGroup();
            this.renameGroup = new RenameGroup();
            this.outputGroup = new OutputGroup();

            // ===== TableLayoutPanel principal =====
            var tableMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(10)
            };
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            tableMain.Controls.Add(filesGroup.Root, 0, 0);
            tableMain.SetColumnSpan(filesGroup.Root, 2);
            tableMain.Controls.Add(renameGroup.Root, 0, 1);
            tableMain.SetColumnSpan(renameGroup.Root, 2);
            tableMain.Controls.Add(outputGroup.Root, 0, 2);
            tableMain.SetColumnSpan(outputGroup.Root, 2);

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
            this.Controls.Add(menuStrip); // Ajout du MenuStrip
            this.MainMenuStrip = menuStrip; // Assignation officielle

            // ===== Paramètres du formulaire =====
            this.Text = "M3UToolkit v1.2.0";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(700, 580);
        }
    }
}
