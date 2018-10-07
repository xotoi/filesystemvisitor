using System.IO;

namespace FileSystemVisitor.EventArgs
{
    public class ItemFindedArgs<T> : System.EventArgs where T : FileSystemInfo
    {
        public T FindedItem { get; set; }
        public FilteringSteps StepType { get; set; }
    }
}
