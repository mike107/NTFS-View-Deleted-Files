using NTFSDeletedFileBrowser.NTFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace NTFSDeletedFileBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<DeletedFile> currentFiles = new List<DeletedFile>();
        public MainWindow()
        {
            InitializeComponent();
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            var driveList = new List<NTFSDrive>();
            
            foreach (DriveInfo d in driveInfos)
            {
                if (d.IsReady)
                {
                    if (d.DriveFormat.Equals("NTFS"))
                    {
                        driveList.Add(new NTFSDrive(d));
                    }
                }
            }
            this.Drives.ItemsSource = driveList;
            this.Drives.SelectedIndex = 0;
            this.StatusMessage.Text = "Ready to scan";
        }
        private void GetDeletedFiles(object sender, RoutedEventArgs e)
        {
            this.SearchTextBox.IsReadOnly = true;
            this.SearchTextBox.Text = "";
            this.ScanDriveButton.IsEnabled = false;
            this.Drives.IsEnabled = false;
            this.FileProgressBar.Value = 0;
            this.FileList.Items.Clear();
            this.FileProgressBar.IsIndeterminate = true;
            
            NTFSDrive selectedDrive = (NTFSDrive) this.Drives.SelectedValue;
            this.StatusMessage.Text = "Scanning " + selectedDrive.DriveInfo.ToString();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                List<DeletedFile> files = selectedDrive.getFiles();
                List<DeletedFile> orderedFiles = files.OrderBy(file => file.Name).ToList();
                currentFiles = orderedFiles;
                this.Dispatcher.Invoke(() =>
                {
                    this.FileProgressBar.IsIndeterminate = false;
                });
                for (int i = 0; i < orderedFiles.Count; i++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        
                        this.FileList.Items.Add(orderedFiles[i]);
                        this.FileProgressBar.Value = ((i + 1) * 100) / orderedFiles.Count;
                        this.StatusMessage.Text = "Scanning " + selectedDrive.DriveInfo.ToString() + ". " + (i + 1) + " files found";
                    });
                };
                this.Dispatcher.Invoke(() =>
                {
                    this.StatusMessage.Text = "Finished scanning " + selectedDrive.DriveInfo.ToString() + ". " + orderedFiles.Count + " files found";
                    this.SearchTextBox.IsReadOnly = false;
                    this.ScanDriveButton.IsEnabled = true;
                    this.Drives.IsEnabled = true;
                });
            }).Start();
        }
        private bool SearchFilter(object item)
        {
            if (String.IsNullOrEmpty(SearchTextBox.Text))
                return true;
            else 
                return ((item as DeletedFile).Name.IndexOf(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);

        }
        private void SearchTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FileList.Items);
            view.Filter = SearchFilter;
            CollectionViewSource.GetDefaultView(FileList.Items).Refresh();
        }

        // file name
        private void SortByFileName(object sender, RoutedEventArgs e)
        {
            currentFiles = currentFiles.OrderBy(file => file.Name).ToList();
            this.FileList.Items.Clear();
            foreach (DeletedFile file in currentFiles)
            {
                this.FileList.Items.Add(file);
            }
        }

        // file name
        private void SortByLongFileName(object sender, RoutedEventArgs e)
        {
            currentFiles = currentFiles.OrderBy(file => file.LongName).ToList();
            this.FileList.Items.Clear();
            foreach (DeletedFile file in currentFiles)
            {
                this.FileList.Items.Add(file);
            }
        }

        // created time
        private void SortByCreatedTime(object sender, RoutedEventArgs e)
        {
            currentFiles = currentFiles.OrderBy(file => file.CreationTime).ToList();
            this.FileList.Items.Clear();
            foreach(DeletedFile file in currentFiles)
            {
                this.FileList.Items.Add(file);
            }
        }

        // last modified
        private void SortByLastChangedTime(object sender, RoutedEventArgs e)
        {
            currentFiles = currentFiles.OrderBy(file => file.LastChangeTime).ToList();
            this.FileList.Items.Clear();
            foreach (DeletedFile file in currentFiles)
            {
                this.FileList.Items.Add(file);
            }
        }

        // last accessed
        private void SortByLastAccessedTime(object sender, RoutedEventArgs e)
        {
            currentFiles = currentFiles.OrderBy(file => file.LastAccessTime).ToList();
            this.FileList.Items.Clear();
            foreach (DeletedFile file in currentFiles)
            {
                this.FileList.Items.Add(file);
            }
        }

        // last accessed
        private void SortBySize(object sender, RoutedEventArgs e)
        {
            currentFiles = currentFiles.OrderBy(file => file.Size).ToList();
            this.FileList.Items.Clear();
            foreach (DeletedFile file in currentFiles)
            {
                this.FileList.Items.Add(file);
            }
        }

    }
}
