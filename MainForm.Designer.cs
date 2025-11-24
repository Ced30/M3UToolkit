using System;
using System.Drawing;
using System.Windows.Forms;

namespace M3UToolkit
{
    /// <summary>
    /// Designer partial class for <see cref="MainForm"/>.
    /// Contains component declarations and UI initialization logic.
    /// </summary>
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private ListBox lstFiles;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnClear;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnBrowse;
        private Button btnGenerate;
        private TextBox txtGameName;
        private TextBox txtOutputFolder;
        private Label lblGameName;
        private Label lblOutputFolder;

        private GroupBox grpNaming;
        private CheckBox chkUseAutoNaming;
        private ComboBox cmbAffixType;

        private ProgressBarWithText progressBar;
        private ProgressBarWithText progressBarFile;

        private GroupBox grpOutput;
        private TableLayoutPanel panelBottom;
        private CheckBox chkCreateSubfolder;

        /// <summary>
        /// Handles the CheckedChanged event of the Rename Files checkbox.
        /// Enables or disables the affix dropdown accordingly.
        /// </summary>
        private void chkUseAutoNaming_CheckedChanged(object sender, EventArgs e)
        {
            cmbAffixType.Enabled = chkUseAutoNaming.Checked;
        }

        /// <summary>
        /// Releases all resources used by the form.
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Initializes all UI components of the form.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Component declarations
            this.lstFiles = new ListBox();
            this.btnAdd = new Button();
            this.btnRemove = new Button();
            this.btnClear = new Button();
            this.btnMoveUp = new Button();
            this.btnMoveDown = new Button();
            this.btnBrowse = new Button();
            this.btnGenerate = new Button();
            this.txtGameName = new TextBox();
            this.txtOutputFolder = new TextBox();
            this.lblOutputFolder = new Label();
            this.progressBar = new ProgressBarWithText();
            this.progressBarFile = new ProgressBarWithText();
            this.panelBottom = new TableLayoutPanel();
            this.chkUseAutoNaming = new CheckBox();
            this.chkCreateSubfolder = new CheckBox();
            this.grpNaming = new GroupBox();
            this.cmbAffixType = new ComboBox();
            this.grpOutput = new GroupBox();

            // ===== MenuStrip =====
            var menuStrip = new MenuStrip();
            var menuHelp = new ToolStripMenuItem("Help");
            var menuAbout = new ToolStripMenuItem("About");
            menuAbout.Click += new EventHandler(ButtonAbout_Click);
            menuHelp.DropDownItems.Add(menuAbout);
            menuStrip.Items.Add(menuHelp);
            menuStrip.Dock = DockStyle.Top;

            this.SuspendLayout();

            // ===== TableLayoutPanel main container =====
            var tableMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(10)
            };
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            // ===== File list panel =====
            var panelList = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            lstFiles.Dock = DockStyle.Fill;
            lstFiles.HorizontalScrollbar = true;
            panelList.Controls.Add(lstFiles);
            tableMain.Controls.Add(panelList, 0, 0);

            // ===== Buttons panel =====
            var panelButtons = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, Dock = DockStyle.Fill };
            foreach (var btn in new[] { btnAdd, btnRemove, btnClear, btnMoveUp, btnMoveDown })
            {
                btn.AutoSize = true;
                panelButtons.Controls.Add(btn);
            }

            btnAdd.Text = "Add";
            btnRemove.Text = "Remove";
            btnClear.Text = "Clear";
            btnMoveUp.Text = "↑";
            btnMoveDown.Text = "↓";

            chkUseAutoNaming.Text = "Rename Files";
            chkUseAutoNaming.AutoSize = true;
            chkUseAutoNaming.Checked = true;
            chkUseAutoNaming.Margin = new Padding(5, btnMoveDown.Height, 0, 0);
            chkUseAutoNaming.CheckedChanged += (s, e) =>
            {
                cmbAffixType.Enabled = chkUseAutoNaming.Checked;
            };

            chkCreateSubfolder.Text = "Create subfolder";
            chkCreateSubfolder.AutoSize = true;
            chkCreateSubfolder.Checked = false;
            chkCreateSubfolder.Margin = new Padding(5, 0, 0, 0);

            panelButtons.Controls.Add(chkUseAutoNaming);
            panelButtons.Controls.Add(chkCreateSubfolder);

            tableMain.Controls.Add(panelButtons, 1, 0);

            // ===== Naming options group =====
            grpNaming.Text = "Naming Options";
            grpNaming.Dock = DockStyle.Fill;
            grpNaming.Padding = new Padding(10);
            grpNaming.Margin = new Padding(0, 5, 0, 10);

            var namingLayout = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };
            namingLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            namingLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            namingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            namingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var lblName = new Label
            {
                Text = "Playlist Name:",
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            txtGameName.Dock = DockStyle.Fill;
            txtGameName.Margin = new Padding(3, 3, 3, 5);

            namingLayout.Controls.Add(lblName, 0, 0);
            namingLayout.Controls.Add(txtGameName, 1, 0);

            var lblDropdown = new Label
            {
                Text = "Renaming Affix:",
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            cmbAffixType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAffixType.Items.AddRange(new object[] { "None", "Disc", "Track", "File" });
            cmbAffixType.SelectedIndex = 0;
            cmbAffixType.Dock = DockStyle.Left;
            cmbAffixType.Margin = new Padding(3, 3, 3, 5);

            namingLayout.Controls.Add(lblDropdown, 0, 1);
            namingLayout.Controls.Add(cmbAffixType, 1, 1);

            grpNaming.Controls.Add(namingLayout);
            tableMain.Controls.Add(grpNaming, 0, 1);
            tableMain.SetColumnSpan(grpNaming, 2);

            // ===== Output & Progress group =====
            grpOutput.Text = "Output & Progress";
            grpOutput.Dock = DockStyle.Fill;
            grpOutput.Padding = new Padding(10);
            grpOutput.Margin = new Padding(0, 5, 0, 10);
            grpOutput.MinimumSize = new Size(0, 180);

            var outputLayout = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 5,
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };
            outputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            outputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            lblOutputFolder.Text = "Output Folder:";
            lblOutputFolder.AutoSize = true;
            outputLayout.Controls.Add(lblOutputFolder, 0, 0);
            outputLayout.SetColumnSpan(lblOutputFolder, 2);

            txtOutputFolder.ReadOnly = true;
            txtOutputFolder.Height = 25;
            txtOutputFolder.Dock = DockStyle.Fill;
            btnBrowse.Text = "...";
            btnBrowse.AutoSize = true;

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
            panelBottom.MinimumSize = new Size(0, 30);

            outputLayout.Controls.Add(panelBottom, 0, 1);
            outputLayout.SetColumnSpan(panelBottom, 2);

            progressBarFile.Minimum = 0;
            progressBarFile.Maximum = 100;
            progressBarFile.Dock = DockStyle.Fill;
            outputLayout.Controls.Add(progressBarFile, 0, 2);
            outputLayout.SetColumnSpan(progressBarFile, 2);

            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Dock = DockStyle.Fill;
            outputLayout.Controls.Add(progressBar, 0, 3);
            outputLayout.SetColumnSpan(progressBar, 2);

            btnGenerate.Text = "Generate";
            btnGenerate.AutoSize = true;
            btnGenerate.Dock = DockStyle.Fill;
            outputLayout.Controls.Add(btnGenerate, 0, 4);
            outputLayout.SetColumnSpan(btnGenerate, 2);

            grpOutput.Controls.Add(outputLayout);
            tableMain.Controls.Add(grpOutput, 0, 2);
            tableMain.SetColumnSpan(grpOutput, 2);

            // ===== MainForm settings =====
            this.ClientSize = new Size(700, 580);

            // Add menuStrip **before** tableMain
            this.Controls.Add(tableMain);
            this.Controls.Add(menuStrip);

            this.MainMenuStrip = menuStrip;
            this.Text = "M3UToolkit v1.1.0";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.ResumeLayout(false);
            this.PerformLayout();
        }

    }
}
