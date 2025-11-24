using System;
using System.Drawing;
using System.Windows.Forms;

namespace M3UToolkit
{
    internal class FilesGroup
    {
        public Control Root => panelMain;

        public ListBox lstFiles;
        public Button btnAdd;
        public Button btnRemove;
        public Button btnClear;
        public Button btnMoveUp;
        public Button btnMoveDown;
        public CheckBox chkRename;
        public CheckBox chkSubfolder;

        private Panel panelMain;

        public FilesGroup()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            panelMain = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            lstFiles = new ListBox { Dock = DockStyle.Fill, HorizontalScrollbar = true };

            var panelButtons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Right,
                AutoSize = true
            };

            btnAdd = new Button { Text = "Add", AutoSize = true };
            btnRemove = new Button { Text = "Remove", AutoSize = true };
            btnClear = new Button { Text = "Clear", AutoSize = true };
            btnMoveUp = new Button { Text = "↑", AutoSize = true };
            btnMoveDown = new Button { Text = "↓", AutoSize = true };

            panelButtons.Controls.AddRange(new Control[] { btnAdd, btnRemove, btnClear, btnMoveUp, btnMoveDown });

            chkRename = new CheckBox { Text = "Rename Files", AutoSize = true, Checked = true };
            chkSubfolder = new CheckBox { Text = "Create subfolder", AutoSize = true, Checked = false };

            panelButtons.Controls.Add(chkRename);
            panelButtons.Controls.Add(chkSubfolder);

            panelMain.Controls.Add(lstFiles);
            panelMain.Controls.Add(panelButtons);

            int offsetX = 15;

            foreach (var ctrl in new Control[] { btnAdd, btnRemove, btnClear, btnMoveUp, btnMoveDown, chkRename, chkSubfolder })
            {
                ctrl.Margin = new Padding(offsetX, ctrl.Margin.Top, ctrl.Margin.Right, ctrl.Margin.Bottom);
            }
        }
    }
}
