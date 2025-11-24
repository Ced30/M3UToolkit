namespace M3UToolkit {

    /// <summary>
    /// Represents the renaming section of the M3UToolkit UI.
    /// Provides controls to configure playlist and file naming options.
    /// Contains a GroupBox with a TableLayoutPanel that organizes:
    /// - A TextBox for entering the playlist name.
    /// - A ComboBox to select a renaming affix (None, Disc, Track, or File).
    /// - A CheckBox to enable or disable automatic file renaming, which also toggles the ComboBox.
    /// The layout is fully docked and properly padded for a clean UI.
    /// </summary>
    internal class RenameGroup {
        public Control Root => grpNaming;

        public GroupBox grpNaming;
        public TextBox txtGameName;
        public ComboBox cmbAffixType;
        public CheckBox chkUseAutoNaming;

        public RenameGroup() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            grpNaming = new GroupBox {
                //Text = "Naming Options",
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            var layout = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(5)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var lblName = new Label { Text = "Playlist Name:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            txtGameName = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(3, 3, 3, 5) };

            var lblDropdown = new Label { Text = "Renaming Affix:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            cmbAffixType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbAffixType.Items.AddRange(new object[] { "None", "Disc", "Track", "File" });
            cmbAffixType.SelectedIndex = 0;
            cmbAffixType.Dock = DockStyle.Left;

            chkUseAutoNaming = new CheckBox { Text = "Rename Files", Checked = true, AutoSize = true };
            chkUseAutoNaming.CheckedChanged += (s, e) => {
                cmbAffixType.Enabled = chkUseAutoNaming.Checked;
            };

            layout.Controls.Add(lblName, 0, 0);
            layout.Controls.Add(txtGameName, 1, 0);
            layout.Controls.Add(lblDropdown, 0, 1);
            layout.Controls.Add(cmbAffixType, 1, 1);

            grpNaming.Controls.Add(layout);
        }
    }
}
