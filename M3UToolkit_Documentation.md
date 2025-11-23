# M3UToolkit – Project Documentation

## Namespace: `M3UToolkit`

### Class: ProgressBarWithText
```csharp
internal class ProgressBarWithText : ProgressBar
```
**Description:**  
A custom progress bar that displays text (either custom or default “Value/Maximum”) centered on top of the bar. Inherits from `ProgressBar`.

**Properties:**
- `string ProgressText` – Custom text displayed on the progress bar. If empty, shows default "Value/Maximum".

**Constructors:**
- `ProgressBarWithText()` – Initializes the control with optimized double buffering and custom painting enabled.

**Methods:**
- `protected override void OnPaint(PaintEventArgs e)` – Paints the bar with blue fill according to progress and draws the centered text.

### Class: MainForm
```csharp
public partial class MainForm : Form
```
**Description:**  
Main application form for managing file playlists and generating `.m3u` playlists. Supports file drag & drop, file renaming, subfolder creation, and progress feedback.

**Fields / Controls:**
- `ListBox lstFiles` – Displays added files.
- `Button btnAdd, btnRemove, btnClear, btnMoveUp, btnMoveDown, btnBrowse, btnGenerate` – Various action buttons.
- `TextBox txtGameName, txtOutputFolder` – Input for playlist name and output folder.
- `Label lblGameName, lblOutputFolder` – Labels for inputs.
- `GroupBox grpNaming, grpOutput` – Containers for naming options and output/progress.
- `CheckBox chkUseAutoNaming, chkCreateSubfolder` – Options for file renaming and subfolder creation.
- `ComboBox cmbAffixType` – Dropdown for renaming affix.
- `ProgressBarWithText progressBar, progressBarFile` – Progress indicators for total operation and per file.

**Constructor:**
- `MainForm()` – Initializes components, hooks event handlers for buttons, drag & drop, and checkboxes.

**Validation Methods:**
- `private bool ValidateInputs()` – Validates that files exist, output folder exists, and naming field is filled if renaming is enabled.
- `private bool RemoveMissingFiles()` – Removes files from the list that no longer exist; warns the user.

**Button Handlers:**
- `BtnAdd_Click` – Open file dialog and add files to the list.
- `BtnRemove_Click` – Removes selected file from the list.
- `BtnClear_Click` – Clears the file list.
- `BtnMoveUp_Click` / `BtnMoveDown_Click` – Moves selected item in the list.
- `BtnBrowse_Click` – Opens folder browser for output folder selection.
- `BtnGenerate_Click` – Validates inputs, removes missing files, then generates `.m3u` playlist and optionally copies/renames files asynchronously.

**Playlist Generation:**
- `private async Task GenerateAsync()` – Handles copying/renaming files, updating progress bars, and writing the `.m3u` file.

**Drag & Drop Support:**
- External drag & drop: `LstFiles_DragEnter` / `LstFiles_DragDrop`.
- Internal drag & drop (reordering): `LstFiles_MouseDown` / `LstFiles_DragOver` / `LstFiles_DragDropInternal`.

### Partial Class: MainForm (Designer)
```csharp
partial class MainForm
```
**Description:**  
Contains component declarations and `InitializeComponent()` method. Responsible for constructing the UI layout:

- Main `TableLayoutPanel` for organizing list, buttons, naming options, and output/progress.
- FlowLayoutPanel for vertical button arrangement.
- GroupBoxes for "Naming Options" and "Output & Progress".
- TableLayoutPanels for arranging labels, text boxes, combo boxes, progress bars, and buttons.
- All controls have proper docking, margins, and sizes for a consistent layout.

**Important Methods:**
- `private void InitializeComponent()` – Initializes and arranges all UI components. Hooks up the minimal logic for enabling/disabling controls (e.g., affix dropdown when renaming is checked).
- `protected override void Dispose(bool disposing)` – Releases resources used by components.

### Usage Summary
1. **Add files** via the Add button or drag & drop.
2. **Optionally rename files** and set an affix for numbering.
3. **Choose an output folder** (and optionally a subfolder).
4. **Generate playlist** – copies files if renaming is enabled, updates progress bars, and writes `.m3u` playlist.
5. **Internal drag & drop** – reorder playlist items directly in the list box.
