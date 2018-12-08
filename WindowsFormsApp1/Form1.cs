using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void OpenFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Get list of files
            lstBxScannedItems.Items.Clear();
            lstBxNewItems.Items.Clear();
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = @"D:\Media\", IsFolderPicker = true })
            {
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    lstBxScannedItems.Items.AddRange(Directory.GetFiles(dialog.FileName, $"*.{txtBxExtension.Text}",SearchOption.AllDirectories));
                }
            }
            //Looks at previous file names and creates a new
            var startItems = lstBxScannedItems.Items;
            var items = new string[lstBxScannedItems.Items.Count];
            foreach (string item in startItems)
            {
                var folder = item.Substring(0, item.LastIndexOf('\\') + 1);
                var file = item.Substring(item.LastIndexOf('\\') + 1);
                var episode = Regex.Match(file, "S\\d\\dE\\d\\d+").Value;
                var newItem = $"{folder}{txtBxName.Text} - {episode}.mp4";
                lstBxNewItems.Items.Add(newItem);
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < lstBxScannedItems.Items.Count; i++)
                {
                    File.Move(lstBxScannedItems.Items[i].ToString(), lstBxNewItems.Items[i].ToString());
                }
            }
            catch
            {
                MessageBox.Show("Failed to rename files","FAILED!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("All Files renamed successfully", "Complete",MessageBoxButtons.OK);
        }
    }
}
