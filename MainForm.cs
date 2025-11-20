namespace M3UToolkit
{

    public partial class MainForm : Form
    {
        private int indexOfItemUnderMouseToDrag;

        public MainForm()
        {
            InitializeComponent();

            // Drag & Drop externe
            lstFiles.AllowDrop = true;
            lstFiles.DragEnter += LstFiles_DragEnter;
            lstFiles.DragDrop += LstFiles_DragDrop;

            // Drag & Drop interne (r�organisation)
            lstFiles.MouseDown += LstFiles_MouseDown;
            lstFiles.DragOver += LstFiles_DragOver;
            lstFiles.DragDrop += LstFiles_DragDropInternal;

            // Boutons
            btnAdd.Click += BtnAdd_Click;
            btnRemove.Click += BtnRemove_Click;
            btnClear.Click += BtnClear_Click;
            btnMoveUp.Click += BtnMoveUp_Click;
            btnMoveDown.Click += BtnMoveDown_Click;
            btnBrowse.Click += BtnBrowse_Click;
            btnGenerate.Click += BtnGenerate_Click;
        }

        // ====================== Validation ======================
        private bool ValidateInputs()
        {
            if (lstFiles.Items.Count == 0)
            {
                MessageBox.Show("Please add some files.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Valider le nom uniquement si le renommage est activé
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

        // ====================== Buttons ======================
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Title = "Select files";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in ofd.FileNames)
                    {
                        if (!lstFiles.Items.Contains(file))
                            lstFiles.Items.Add(file);
                    }
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
            if (!ValidateInputs())
                return;

            if (!RemoveMissingFiles())
                return; // arr�ter la g�n�ration si des fichiers manquants ont �t� d�tect�s

            btnGenerate.Enabled = false;
            progressBar.Value = 0;

            try
            {
                await GenerateAsync();
                MessageBox.Show("Done! files copied and .m3u generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // D�lister tous les fichiers apr�s g�n�ration
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

        // ====================== Generation ======================
        private async Task GenerateAsync()
        {
            string outputFolder = txtOutputFolder.Text.Trim();
            bool renameFiles = chkUseAutoNaming.Checked;
            string gameName = txtGameName.Text.Trim();

            // Si pas de renommage, on met un nom par défaut pour le M3U
            if (!renameFiles)
                gameName = string.IsNullOrEmpty(gameName) ? "Playlist" : gameName;

            string affixType = (cmbAffixType.SelectedItem?.ToString() ?? "").Trim();
            if (affixType.Equals("None", StringComparison.OrdinalIgnoreCase))
                affixType = ""; // Aucun affixe

            int totalFiles = lstFiles.Items.Count;

            // Reset progress bars
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

                    // Copie du fichier avec mise à jour de la barre de progression
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
                    // Pas de renommage → on utilise juste le nom du fichier original
                    m3uEntries.Add(Path.GetFileName(sourcePath));
                }

                progressBar.Value = i + 1;
                progressBarFile.Value = 0; // Reset pour le fichier suivant
            }

            // Écriture du fichier M3U
            string m3uFile = Path.Combine(outputFolder, $"{gameName}.m3u");
            File.WriteAllLines(m3uFile, m3uEntries);
        }

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

                return false; // indique que des fichiers ont �t� retir�s
            }
            return true; // tous les fichiers existent
        }

        // ====================== Drag & Drop externe ======================
        private void LstFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void LstFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null) return;

            foreach (var file in files)
            {
                if (!lstFiles.Items.Contains(file))
                    lstFiles.Items.Add(file);
            }
        }

        // ====================== Drag & Drop interne ======================
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

            // si rel�ch� en dehors d�un item, mettre � la fin
            if (index == ListBox.NoMatches)
                index = lstFiles.Items.Count;

            lstFiles.Items.Remove(data);
            lstFiles.Items.Insert(index, data);
            lstFiles.SelectedItem = data;
        }
    }

}
