using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M3UToolkit
{
    /// <summary>
    /// Main form for managing M3U playlists.
    /// Provides UI for adding, removing, reordering files, setting output folder,
    /// and generating M3U playlists with optional file renaming.
    /// </summary>
    public partial class MainForm : Form
    {
        private int indexOfItemUnderMouseToDrag;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// Sets up event handlers for buttons and drag-and-drop functionality.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            lstFiles.AllowDrop = true;
            lstFiles.DragEnter += LstFiles_DragEnter;
            lstFiles.DragDrop += LstFiles_DragDrop;

            lstFiles.MouseDown += LstFiles_MouseDown;
            lstFiles.DragOver += LstFiles_DragOver;
            lstFiles.DragDrop += LstFiles_DragDropInternal;

            btnAdd.Click += BtnAdd_Click;
            btnRemove.Click += BtnRemove_Click;
            btnClear.Click += BtnClear_Click;
            btnMoveUp.Click += BtnMoveUp_Click;
            btnMoveDown.Click += BtnMoveDown_Click;
            btnBrowse.Click += BtnBrowse_Click;
            btnGenerate.Click += BtnGenerate_Click;
        }

        /// <summary>
        /// Validates user inputs: file list, output folder, and optional naming.
        /// </summary>
        /// <returns>True if all inputs are valid; otherwise, false.</returns>
        private bool ValidateInputs()
        {
            if (lstFiles.Items.Count == 0)
            {
                MessageBox.Show("Please add some files.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (chkUseAutoNaming.Checked && string.IsNullOrWhiteSpace(txtGameName.Text))
            {
                MessageBox.Show("Please enter a name for the renamed files.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtOutputFolder.Text) || !Directory.Exists(txtOutputFolder.Text))
            {
                MessageBox.Show("Please select a valid output folder.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        #region Button Event Handlers

            private void BtnAdd_Click(object sender, EventArgs e)
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Multiselect = true;
                    ofd.Title = "Select files";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var file in ofd.FileNames)
                            if (!lstFiles.Items.Contains(file))
                                lstFiles.Items.Add(file);
                    }
                }
            }

            private void BtnRemove_Click(object sender, EventArgs e)
            {
                if (lstFiles.SelectedItem != null)
                    lstFiles.Items.Remove(lstFiles.SelectedItem);
            }

            private void BtnClear_Click(object sender, EventArgs e)
            {
                lstFiles.Items.Clear();
            }

            private void BtnMoveUp_Click(object sender, EventArgs e)
            {
                int index = lstFiles.SelectedIndex;
                if (index > 0)
                {
                    var item = lstFiles.Items[index];
                    lstFiles.Items.RemoveAt(index);
                    lstFiles.Items.Insert(index - 1, item);
                    lstFiles.SelectedIndex = index - 1;
                }
            }

            private void BtnMoveDown_Click(object sender, EventArgs e)
            {
                int index = lstFiles.SelectedIndex;
                if (index >= 0 && index < lstFiles.Items.Count - 1)
                {
                    var item = lstFiles.Items[index];
                    lstFiles.Items.RemoveAt(index);
                    lstFiles.Items.Insert(index + 1, item);
                    lstFiles.SelectedIndex = index + 1;
                }
            }

            private void BtnBrowse_Click(object sender, EventArgs e)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                        txtOutputFolder.Text = fbd.SelectedPath;
                }
            }

            private async void BtnGenerate_Click(object sender, EventArgs e)
            {
                if (!ValidateInputs()) return;
                if (!RemoveMissingFiles()) return;

                btnGenerate.Enabled = false;
                progressBar.Value = 0;

                try
                {
                    await GenerateAsync();
                    MessageBox.Show("Done! files copied and .m3u generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lstFiles.Items.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnGenerate.Enabled = true;
                }
            }

        #endregion

        #region Generation Logic

            /// <summary>
            /// Generates the playlist files asynchronously, optionally renaming them and creating a subfolder.
            /// </summary>
            /// <returns>A task representing the asynchronous operation.</returns>
            private async Task GenerateAsync()
            {
                string outputFolder = txtOutputFolder.Text.Trim();
                bool renameFiles = chkUseAutoNaming.Checked;
                bool createSubfolder = chkCreateSubfolder.Checked;
                string gameName = txtGameName.Text.Trim();

                if (!renameFiles)
                    gameName = string.IsNullOrEmpty(gameName) ? "Playlist" : gameName;

                if (createSubfolder)
                {
                    outputFolder = Path.Combine(outputFolder, gameName);
                    if (!Directory.Exists(outputFolder))
                        Directory.CreateDirectory(outputFolder);
                }

                string affixType = (cmbAffixType.SelectedItem?.ToString() ?? "").Trim();
                if (affixType.Equals("None", StringComparison.OrdinalIgnoreCase))
                    affixType = "";

                int totalFiles = lstFiles.Items.Count;

                progressBar.Maximum = totalFiles;
                progressBar.Value = 0;
                progressBarFile.Maximum = 100;
                progressBarFile.Value = 0;

                List<string> m3uEntries = new List<string>();

                for (int i = 0; i < totalFiles; i++)
                {
                    string sourcePath = lstFiles.Items[i].ToString();
                    string extension = Path.GetExtension(sourcePath);
                    string destPath;

                    if (renameFiles)
                    {
                        string innerText = string.IsNullOrEmpty(affixType)
                            ? $"{i + 1} of {totalFiles}"
                            : $"{affixType} {i + 1} of {totalFiles}";

                        string destName = $"{gameName} ({innerText}){extension}";
                        destPath = Path.Combine(outputFolder, destName);

                        using (FileStream source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                        using (FileStream target = new FileStream(destPath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] buffer = new byte[65536];
                            long totalBytes = source.Length;
                            long readTotal = 0;

                            int read;
                            while ((read = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await target.WriteAsync(buffer, 0, read);
                                readTotal += read;
                                progressBarFile.Value = (int)((readTotal * 100) / totalBytes);
                            }
                        }

                        m3uEntries.Add(Path.GetFileName(destPath));
                    }
                    else
                    {
                        m3uEntries.Add(Path.GetFileName(sourcePath));
                    }

                    progressBar.Value = i + 1;
                    progressBarFile.Value = 0;
                }

                string m3uFile = Path.Combine(outputFolder, $"{gameName}.m3u");
                File.WriteAllLines(m3uFile, m3uEntries);
            }

            /// <summary>
            /// Removes missing files from the file list and notifies the user.
            /// </summary>
            /// <returns>True if all files exist; otherwise, false.</returns>
            private bool RemoveMissingFiles()
            {
                var missingFiles = lstFiles.Items.Cast<string>().Where(f => !File.Exists(f)).ToList();
                if (missingFiles.Count > 0)
                {
                    string msg = "The following files are missing and will be removed from the list:\n\n" +
                                 string.Join("\n", missingFiles);
                    MessageBox.Show(msg, "Missing Files", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    foreach (var file in missingFiles)
                        lstFiles.Items.Remove(file);

                    return false;
                }
                return true;
            }

        #endregion

        #region External Drag & Drop

            private void LstFiles_DragEnter(object sender, DragEventArgs e)
            {
                e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            }

            private void LstFiles_DragDrop(object sender, DragEventArgs e)
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files == null) return;

                foreach (var file in files)
                    if (!lstFiles.Items.Contains(file))
                        lstFiles.Items.Add(file);
            }

        #endregion

        #region Internal Drag & Drop (Reordering)

            private void LstFiles_MouseDown(object sender, MouseEventArgs e)
            {
                indexOfItemUnderMouseToDrag = lstFiles.IndexFromPoint(e.X, e.Y);
                if (indexOfItemUnderMouseToDrag != ListBox.NoMatches)
                    lstFiles.DoDragDrop(lstFiles.Items[indexOfItemUnderMouseToDrag], DragDropEffects.Move);
            }

            private void LstFiles_DragOver(object sender, DragEventArgs e)
            {
                e.Effect = DragDropEffects.Move;
            }

            private void LstFiles_DragDropInternal(object sender, DragEventArgs e)
            {
                object data = e.Data.GetData(typeof(string));
                if (data == null) return;

                Point point = lstFiles.PointToClient(new Point(e.X, e.Y));
                int index = lstFiles.IndexFromPoint(point);

                if (index == ListBox.NoMatches)
                    index = lstFiles.Items.Count;

                lstFiles.Items.Remove(data);
                lstFiles.Items.Insert(index, data);
                lstFiles.SelectedItem = data;
            }

        #endregion
    }
}
