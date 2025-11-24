namespace M3UToolkit {

    /// <summary>
    /// Represents the file management section of the M3UToolkit UI.
    /// Provides controls to view, add, remove, clear, and reorder files in a ListBox.
    /// Contains options for automatic renaming and creating subfolders.
    /// The UI is organized in a GroupBox with a main TableLayoutPanel:
    /// - The left column holds the ListBox displaying the files.
    /// - The right column contains buttons (Add, Remove, Clear, Move Up, Move Down) and CheckBoxes (Rename Files, Create Subfolder).
    /// Controls are properly anchored, padded, and optionally offset for a clean layout.
    /// </summary>
    internal class FilesGroup {
        public Control Root => grpFiles;

        public GroupBox grpFiles;
        public ListBox lstFiles;
        public Button btnAdd;
        public Button btnRemove;
        public Button btnClear;
        public Button btnMoveUp;
        public Button btnMoveDown;
        public CheckBox chkRename;
        public CheckBox chkSubfolder;

        private TableLayoutPanel layoutMain;

        public FilesGroup() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            // GroupBox principal
            grpFiles = new GroupBox {
                Text = "Files",
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                MinimumSize = new Size(0, 250)
            };

            // TableLayoutPanel principal à l'intérieur du GroupBox
            layoutMain = new TableLayoutPanel {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(5),
            };
            layoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // Liste
            layoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Boutons + checkboxes

            // Liste de fichiers
            lstFiles = new ListBox { Dock = DockStyle.Fill, HorizontalScrollbar = true };
            layoutMain.Controls.Add(lstFiles, 0, 0);

            // TableLayoutPanel pour les boutons + checkboxes
            var panelRight = new TableLayoutPanel {
                ColumnCount = 1,
                RowCount = 7, // 5 boutons + 2 checkboxes
                AutoSize = true,
                Dock = DockStyle.Fill
            };

            btnAdd = new Button { Text = "Add", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left };
            btnRemove = new Button { Text = "Remove", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left };
            btnClear = new Button { Text = "Clear", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left };
            btnMoveUp = new Button { Text = "↑", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left };
            btnMoveDown = new Button { Text = "↓", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left };

            chkRename = new CheckBox { Text = "Rename Files", AutoSize = true, Checked = true, Anchor = AnchorStyles.Top | AnchorStyles.Left };
            chkSubfolder = new CheckBox { Text = "Create subfolder", AutoSize = true, Checked = false, Anchor = AnchorStyles.Top | AnchorStyles.Left };

            panelRight.Controls.Add(btnAdd, 0, 0);
            panelRight.Controls.Add(btnRemove, 0, 1);
            panelRight.Controls.Add(btnClear, 0, 2);
            panelRight.Controls.Add(btnMoveUp, 0, 3);
            panelRight.Controls.Add(btnMoveDown, 0, 4);
            panelRight.Controls.Add(chkRename, 0, 5);
            panelRight.Controls.Add(chkSubfolder, 0, 6);

            // Décalage optionnel
            int offsetX = 15;
            foreach (var ctrl in new Control[] { btnAdd, btnRemove, btnClear, btnMoveUp, btnMoveDown, chkRename, chkSubfolder }) {
                ctrl.Margin = new Padding(offsetX, ctrl.Margin.Top, ctrl.Margin.Right, ctrl.Margin.Bottom);
            }

            layoutMain.Controls.Add(panelRight, 1, 0);

            grpFiles.Controls.Add(layoutMain);
        }
    }
}
