using System;

namespace NTFSDeletedFileBrowser
{
    public class DeletedFile
    {
        public String Name { get; }
        public String LongName { get; }
        public DateTime CreationTime { get; }
        public DateTime LastChangeTime { get; }
        public DateTime LastAccessTime { get; }
        public DeletedFileSize Size { get; }
        
        public DeletedFile(String name, String longName, DateTime creationTime, DateTime lastChangeTime, DateTime lastAccessTime, UInt64 size) 
        {
            this.Name = name;
            this.LongName = longName;
            this.CreationTime = creationTime;
            this.LastChangeTime = lastChangeTime;
            this.LastAccessTime = lastAccessTime;
            this.Size = new DeletedFileSize(size);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class DeletedFileSize : IComparable<DeletedFileSize>
    {
        public UInt64 Size { get; }

        public DeletedFileSize(UInt64 size)
        {
            this.Size = size;
        }

        public override bool Equals(object obj)
        {
            var item = obj as DeletedFileSize;

            if (item == null)
            {
                return false;
            }

            return this.Size == item.Size;
        }

        public override int GetHashCode()
        {
            return this.Size.GetHashCode();
        }

        public override string ToString()
        {
            if (Size > 1000000000)
                return (Size / 1000000000).ToString() + " GB";
            if (Size > 1000000)
                return (Size / 1000000).ToString() + " MB";
            if (Size > 1000)
                return (Size / 1000).ToString() + " KB";
            return Size.ToString() + " bytes";
        }

        public int CompareTo(DeletedFileSize that)
        {
            if (this.Size > that.Size) return -1;
            if (this.Size == that.Size) return 0;
            return 1;
        }
    }
}
