namespace M3UToolkit {

    /// <summary>
    /// Main window of M3UToolkit, coordinating all UI groups and playlist generation logic.
    /// Provides a complete interface to manage files, configure renaming, set output options, and generate .m3u playlists.
    /// Key features include:
    /// - FilesGroup: view, add, remove, reorder, and drag & drop files.
    /// - RenameGroup: configure automatic renaming of files with affixes.
    /// - OutputGroup: select output folder, view progress, and generate the playlist.
    /// - AboutForm: displays application information.
    /// Supports both external file drag & drop and internal reordering via drag & drop.
    /// Handles validation, file copying, progress updates, and asynchronous playlist generation.
    /// </summary>
    internal partial class MainForm : Form {
        private int indexOfItemUnderMouseToDrag;

        private AboutForm about;

        public MainForm() {
            InitializeComponent();

            about  = new AboutForm();

            // ===== Setup Drag & Drop for files =====
            filesGroup.lstFiles.AllowDrop = true;
            filesGroup.lstFiles.DragEnter += LstFiles_DragEnter;
            filesGroup.lstFiles.DragDrop += LstFiles_DragDrop;

            filesGroup.lstFiles.MouseDown += LstFiles_MouseDown;
            filesGroup.lstFiles.DragOver += LstFiles_DragOver;
            filesGroup.lstFiles.DragDrop += LstFiles_DragDropInternal;

            // ===== Buttons =====
            filesGroup.btnAdd.Click += BtnAdd_Click;
            filesGroup.btnRemove.Click += BtnRemove_Click;
            filesGroup.btnClear.Click += BtnClear_Click;
            filesGroup.btnMoveUp.Click += BtnMoveUp_Click;
            filesGroup.btnMoveDown.Click += BtnMoveDown_Click;

            outputGroup.btnBrowse.Click += BtnBrowse_Click;
            outputGroup.btnGenerate.Click += BtnGenerate_Click;
        }

        #region About

            private void ButtonAbout_Click(object sender, EventArgs e) {
                about.ShowDialog();
            }

        #endregion

        #region Button Event Handlers

            private void BtnAdd_Click(object sender, EventArgs e) {
                using (OpenFileDialog ofd = new OpenFileDialog()) {
                    ofd.Multiselect = true;
                    ofd.Title = "Select files";

                    if (ofd.ShowDialog() == DialogResult.OK) {
                        foreach (var file in ofd.FileNames)
                            if (!filesGroup.lstFiles.Items.Contains(file))
                                filesGroup.lstFiles.Items.Add(file);
                    }
                }
            }

            private void BtnRemove_Click(object sender, EventArgs e) {
                if (filesGroup.lstFiles.SelectedItem != null)
                    filesGroup.lstFiles.Items.Remove(filesGroup.lstFiles.SelectedItem);
            }

            private void BtnClear_Click(object sender, EventArgs e) {
                filesGroup.lstFiles.Items.Clear();
            }

            private void BtnMoveUp_Click(object sender, EventArgs e) {
                int index = filesGroup.lstFiles.SelectedIndex;
                if (index > 0) {
                    var item = filesGroup.lstFiles.Items[index];
                    filesGroup.lstFiles.Items.RemoveAt(index);
                    filesGroup.lstFiles.Items.Insert(index - 1, item);
                    filesGroup.lstFiles.SelectedIndex = index - 1;
                }
            }

            private void BtnMoveDown_Click(object sender, EventArgs e) {
                int index = filesGroup.lstFiles.SelectedIndex;
                if (index >= 0 && index < filesGroup.lstFiles.Items.Count - 1) {
                    var item = filesGroup.lstFiles.Items[index];
                    filesGroup.lstFiles.Items.RemoveAt(index);
                    filesGroup.lstFiles.Items.Insert(index + 1, item);
                    filesGroup.lstFiles.SelectedIndex = index + 1;
                }
            }

            private void BtnBrowse_Click(object sender, EventArgs e) {
                using (var fbd = new FolderBrowserDialog()) {
                    if (fbd.ShowDialog() == DialogResult.OK)
                        outputGroup.txtOutputFolder.Text = fbd.SelectedPath;
                }
            }

            private async void BtnGenerate_Click(object sender, EventArgs e) {
                if (!ValidateInputs()) return;
                if (!RemoveMissingFiles()) return;

                outputGroup.btnGenerate.Enabled = false;
                outputGroup.progressBar.Value = 0;

                try {
                    await GenerateAsync();
                    MessageBox.Show("Done! files copied and .m3u generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    filesGroup.lstFiles.Items.Clear();
                } catch (Exception ex) {
                    MessageBox.Show($"An error occurred:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } finally {
                    outputGroup.btnGenerate.Enabled = true;
                }
            }

            /// <summary>
            /// Validates user inputs before generating the playlist.
            /// </summary>
            /// <returns>True if all inputs are valid, false otherwise.</returns>
            private bool ValidateInputs() {
                if (filesGroup.lstFiles.Items.Count == 0) {
                    MessageBox.Show("Please add some files.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (renameGroup.chkUseAutoNaming.Checked && string.IsNullOrWhiteSpace(renameGroup.txtGameName.Text)) {
                    MessageBox.Show("Please enter a name for the renamed files.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(outputGroup.txtOutputFolder.Text) || !Directory.Exists(outputGroup.txtOutputFolder.Text)) {
                    MessageBox.Show("Please select a valid output folder.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true;
            }

        #endregion

        #region Generation Logic

            /// <summary>
            /// Generates the playlist asynchronously, optionally renaming files and creating subfolders.
            /// </summary>
            private async Task GenerateAsync() {
                string outputFolder = outputGroup.txtOutputFolder.Text.Trim();
                bool renameFiles = renameGroup.chkUseAutoNaming.Checked;
                bool createSubfolder = filesGroup.chkSubfolder.Checked;
                string gameName = renameGroup.txtGameName.Text.Trim();

                if (!renameFiles)
                    gameName = string.IsNullOrEmpty(gameName) ? "Playlist" : gameName;

                if (createSubfolder) {
                    outputFolder = Path.Combine(outputFolder, gameName);
                    if (!Directory.Exists(outputFolder))
                        Directory.CreateDirectory(outputFolder);
                }

                string affixType = (renameGroup.cmbAffixType.SelectedItem?.ToString() ?? "").Trim();
                if (affixType.Equals("None", StringComparison.OrdinalIgnoreCase))
                    affixType = "";

                int totalFiles = filesGroup.lstFiles.Items.Count;

                outputGroup.progressBar.Maximum = totalFiles;
                outputGroup.progressBar.Value = 0;
                outputGroup.progressBarFile.Maximum = 100;
                outputGroup.progressBarFile.Value = 0;

                List<string> m3uEntries = new List<string>();

                for (int i = 0; i < totalFiles; i++) {
                    string sourcePath = filesGroup.lstFiles.Items[i].ToString();
                    string extension = Path.GetExtension(sourcePath);
                    string destPath;

                    if (renameFiles) {
                        string innerText = string.IsNullOrEmpty(affixType)
                            ? $"{i + 1} of {totalFiles}"
                            : $"{affixType} {i + 1} of {totalFiles}";

                        string destName = $"{gameName} ({innerText}){extension}";
                        destPath = Path.Combine(outputFolder, destName);

                        using (FileStream source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                        using (FileStream target = new FileStream(destPath, FileMode.Create, FileAccess.Write)) {
                            byte[] buffer = new byte[65536];
                            long totalBytes = source.Length;
                            long readTotal = 0;

                            int read;
                            while ((read = await source.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                                await target.WriteAsync(buffer, 0, read);
                                readTotal += read;
                                outputGroup.progressBarFile.Value = (int)((readTotal * 100) / totalBytes);
                            }
                        }

                        m3uEntries.Add(Path.GetFileName(destPath));
                    } else {
                        m3uEntries.Add(Path.GetFileName(sourcePath));
                    }

                    outputGroup.progressBar.Value = i + 1;
                    outputGroup.progressBarFile.Value = 0;
                }

                string m3uFile = Path.Combine(outputFolder, $"{gameName}.m3u");
                File.WriteAllLines(m3uFile, m3uEntries);
            }

            private bool RemoveMissingFiles() {
                var missingFiles = filesGroup.lstFiles.Items.Cast<string>().Where(f => !File.Exists(f)).ToList();
                if (missingFiles.Count > 0) {
                    string msg = "The following files are missing and will be removed from the list:\n\n" +
                                 string.Join("\n", missingFiles);
                    MessageBox.Show(msg, "Missing Files", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    foreach (var file in missingFiles)
                        filesGroup.lstFiles.Items.Remove(file);

                    return false;
                }
                return true;
            }

        #endregion

        #region External Drag & Drop

            private void LstFiles_DragEnter(object sender, DragEventArgs e) {
                e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            }

            private void LstFiles_DragDrop(object sender, DragEventArgs e) {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files == null) return;

                foreach (var file in files)
                    if (!filesGroup.lstFiles.Items.Contains(file))
                        filesGroup.lstFiles.Items.Add(file);
            }

        #endregion

        #region Internal Drag & Drop (Reordering)

            private void LstFiles_MouseDown(object sender, MouseEventArgs e) {
                indexOfItemUnderMouseToDrag = filesGroup.lstFiles.IndexFromPoint(e.X, e.Y);
                if (indexOfItemUnderMouseToDrag != ListBox.NoMatches)
                    filesGroup.lstFiles.DoDragDrop(filesGroup.lstFiles.Items[indexOfItemUnderMouseToDrag], DragDropEffects.Move);
            }

            private void LstFiles_DragOver(object sender, DragEventArgs e) {
                e.Effect = DragDropEffects.Move;
            }

            private void LstFiles_DragDropInternal(object sender, DragEventArgs e) {
                object data = e.Data.GetData(typeof(string));
                if (data == null) return;

                Point point = filesGroup.lstFiles.PointToClient(new Point(e.X, e.Y));
                int index = filesGroup.lstFiles.IndexFromPoint(point);

                if (index == ListBox.NoMatches)
                    index = filesGroup.lstFiles.Items.Count;

                filesGroup.lstFiles.Items.Remove(data);
                filesGroup.lstFiles.Items.Insert(index, data);
                filesGroup.lstFiles.SelectedItem = data;
            }

        #endregion
    }
}
