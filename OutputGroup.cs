using System;
using System.Drawing;
using System.Windows.Forms;

namespace M3UToolkit
{
    internal class OutputGroup
    {
        public Control Root => grpOutput;

        public GroupBox grpOutput;
        public TextBox txtOutputFolder;
        public Button btnBrowse;
        public ProgressBarWithText progressBar;
        public ProgressBarWithText progressBarFile;
        public Button btnGenerate;

        private TableLayoutPanel panelBottom;

        public OutputGroup()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            grpOutput = new GroupBox
            {
                Text = "Output & Progress",
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                MinimumSize = new Size(0, 180)
            };

            var layout = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 5,
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var lblOutputFolder = new Label { Text = "Output Folder:", AutoSize = true };
            layout.Controls.Add(lblOutputFolder, 0, 0);
            layout.SetColumnSpan(lblOutputFolder, 2);

            txtOutputFolder = new TextBox { ReadOnly = true, Height = 25, Dock = DockStyle.Fill };
            btnBrowse = new Button { Text = "...", AutoSize = true };

            panelBottom = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            panelBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panelBottom.Controls.Add(txtOutputFolder, 0, 0);
            panelBottom.Controls.Add(btnBrowse, 1, 0);

            layout.Controls.Add(panelBottom, 0, 1);
            layout.SetColumnSpan(panelBottom, 2);

            progressBarFile = new ProgressBarWithText { Minimum = 0, Maximum = 100, Dock = DockStyle.Fill };
            layout.Controls.Add(progressBarFile, 0, 2);
            layout.SetColumnSpan(progressBarFile, 2);

            progressBar = new ProgressBarWithText { Minimum = 0, Maximum = 100, Dock = DockStyle.Fill };
            layout.Controls.Add(progressBar, 0, 3);
            layout.SetColumnSpan(progressBar, 2);

            btnGenerate = new Button { Text = "Generate", AutoSize = true, Dock = DockStyle.Fill };
            layout.Controls.Add(btnGenerate, 0, 4);
            layout.SetColumnSpan(btnGenerate, 2);

            grpOutput.Controls.Add(layout);
        }
    }
}
