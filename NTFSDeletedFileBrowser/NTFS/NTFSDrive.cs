using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;

namespace NTFSDeletedFileBrowser.NTFS
{
    public partial class NTFSDrive
    {
        public DriveInfo DriveInfo { get; set; }
        public List<DeletedFile> DeletedFiles;

        public NTFSDrive(DriveInfo driveInfo)
        {
            this.DriveInfo = driveInfo;
            this.DeletedFiles = new List<DeletedFile>();
        }

        public List<DeletedFile> getFiles()
        {
            NtfsReader ntfsReader = new NtfsReader(this.DriveInfo, RetrieveMode.All);
            IEnumerable<INode> nodes =
                ntfsReader.GetNodes(this.DriveInfo.Name);

            int directoryCount = 0, fileCount = 0;
            this.DeletedFiles.Clear();
            foreach (INode node in nodes)
            {
                if ((node.Attributes & Attributes.Directory) != 0)
                    directoryCount++;
                else
                {
                    this.DeletedFiles.Add(new DeletedFile(node.Name, node.FullName, node.CreationTime, node.LastChangeTime, node.LastAccessTime, node.Size));
                    fileCount++;
                } 
            }
            return this.DeletedFiles;
        }

        public override string ToString()
        {
            if (this.DriveInfo.VolumeLabel.Length == 0)
            {
                return this.DriveInfo.Name;
            }
            return this.DriveInfo.VolumeLabel + " (" + this.DriveInfo.Name + ")";
        }
        
    }

}
