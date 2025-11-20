namespace M3UToolkit
{


    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox txtGameName;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Label lblGameName;
        private System.Windows.Forms.Label lblOutputFolder;

        private System.Windows.Forms.GroupBox grpNaming;
        private System.Windows.Forms.CheckBox chkUseAutoNaming;
        private System.Windows.Forms.ComboBox cmbAffixType;

        private ProgressBarWithText progressBar;
        private ProgressBarWithText progressBarFile;

        private System.Windows.Forms.GroupBox grpOutput;
        private System.Windows.Forms.TableLayoutPanel panelBottom;
        private System.Windows.Forms.Label lblProgressText;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ----- Déclarations -----
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.txtGameName = new System.Windows.Forms.TextBox();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.progressBar = new ProgressBarWithText();
            this.progressBarFile = new ProgressBarWithText();
            this.panelBottom = new System.Windows.Forms.TableLayoutPanel();
            this.chkUseAutoNaming = new System.Windows.Forms.CheckBox();
            this.grpNaming = new System.Windows.Forms.GroupBox();
            this.cmbAffixType = new System.Windows.Forms.ComboBox();
            this.grpOutput = new System.Windows.Forms.GroupBox();

            this.SuspendLayout();

            // ===== TableLayoutPanel principal =====
            var tableMain = new System.Windows.Forms.TableLayoutPanel();
            tableMain.Dock = DockStyle.Fill;
            tableMain.ColumnCount = 2;
            tableMain.RowCount = 5;
            tableMain.Padding = new Padding(10);
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Liste
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // Checkbox
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // Naming GroupBox
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // Output & Progress GroupBox
            tableMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // Padding finale
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            // ===== Liste et boutons =====
            var panelList = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            lstFiles.Dock = DockStyle.Fill;
            lstFiles.HorizontalScrollbar = true;
            panelList.Controls.Add(lstFiles);
            tableMain.Controls.Add(panelList, 0, 0);

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
            tableMain.Controls.Add(panelButtons, 1, 0);

            // ===== Checkbox =====
            chkUseAutoNaming.Text = "Rename Files";
            chkUseAutoNaming.AutoSize = true;
            chkUseAutoNaming.Checked = true;
            chkUseAutoNaming.CheckedChanged += (s, e) =>
            {
                txtGameName.Enabled = chkUseAutoNaming.Checked;
                cmbAffixType.Enabled = chkUseAutoNaming.Checked;
            };
            tableMain.Controls.Add(chkUseAutoNaming, 0, 1);
            tableMain.SetColumnSpan(chkUseAutoNaming, 2);

            // ===== GroupBox Naming Options =====
            grpNaming.Text = "Naming Options";
            grpNaming.Dock = DockStyle.Fill;
            grpNaming.Padding = new Padding(10);
            grpNaming.Margin = new Padding(0, 5, 0, 10);

            var namingLayout = new TableLayoutPanel();
            namingLayout.ColumnCount = 2;
            namingLayout.RowCount = 2;
            namingLayout.Dock = DockStyle.Fill;
            namingLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            namingLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            namingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            namingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            namingLayout.Padding = new Padding(5);

            var lblName = new Label { Text = "Game Name:", AutoSize = true };
            txtGameName.Dock = DockStyle.Fill;
            txtGameName.Margin = new Padding(3, 3, 3, 5);
            namingLayout.Controls.Add(lblName, 0, 0);
            namingLayout.Controls.Add(txtGameName, 1, 0);

            var lblAffix = new Label { Text = "Affix type:", AutoSize = true };
            cmbAffixType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAffixType.Items.AddRange(new object[] { "None", "Disc", "Track", "File" });
            cmbAffixType.SelectedIndex = 0;
            cmbAffixType.Dock = DockStyle.Fill;
            cmbAffixType.Margin = new Padding(3, 3, 3, 5);
            namingLayout.Controls.Add(lblAffix, 0, 1);
            namingLayout.Controls.Add(cmbAffixType, 1, 1);

            grpNaming.Controls.Add(namingLayout);
            tableMain.Controls.Add(grpNaming, 0, 2);
            tableMain.SetColumnSpan(grpNaming, 2);

            // ===== GroupBox Output & Progress =====
            grpOutput.Text = "Output & Progress";
            grpOutput.Dock = DockStyle.Fill;
            grpOutput.Padding = new Padding(10);
            grpOutput.Margin = new Padding(0, 5, 0, 10);
            grpOutput.MinimumSize = new Size(0, 180);

            var outputLayout = new TableLayoutPanel();
            outputLayout.ColumnCount = 2;
            outputLayout.RowCount = 5;
            outputLayout.Dock = DockStyle.Fill;
            outputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            outputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            outputLayout.Padding = new Padding(5);

            // Assign explicit heights pour éviter le problème de visibilité
            outputLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));     // lblOutputFolder
            outputLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F)); // txt + browse
            outputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); // progressBarFile
            outputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); // progressBar
            outputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // btnGenerate

            // Label Output
            lblOutputFolder.Text = "Output Folder:";
            lblOutputFolder.AutoSize = true;
            outputLayout.Controls.Add(lblOutputFolder, 0, 0);
            outputLayout.SetColumnSpan(lblOutputFolder, 2);

            // txtOutput + btnBrowse
            txtOutputFolder.ReadOnly = true;
            txtOutputFolder.Dock = DockStyle.Fill;
            txtOutputFolder.Height = 25;
            btnBrowse.Text = "...";
            btnBrowse.AutoSize = true;

            panelBottom = new TableLayoutPanel();
            panelBottom.ColumnCount = 2;
            panelBottom.Dock = DockStyle.Fill;
            panelBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panelBottom.Controls.Add(txtOutputFolder, 0, 0);
            panelBottom.Controls.Add(btnBrowse, 1, 0);

            outputLayout.Controls.Add(panelBottom, 0, 1);
            outputLayout.SetColumnSpan(panelBottom, 2);

            // ProgressBars
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

            // Generate Button
            btnGenerate.Text = "Generate";
            btnGenerate.AutoSize = true;
            btnGenerate.Dock = DockStyle.Fill;
            outputLayout.Controls.Add(btnGenerate, 0, 4);
            outputLayout.SetColumnSpan(btnGenerate, 2);

            grpOutput.Controls.Add(outputLayout);
            tableMain.Controls.Add(grpOutput, 0, 3);
            tableMain.SetColumnSpan(grpOutput, 2);

            // ===== MainForm =====
            this.ClientSize = new Size(700, 580);
            this.Controls.Add(tableMain);
            this.Text = "M3UToolkit";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.ResumeLayout(false);
        }

    }


}
