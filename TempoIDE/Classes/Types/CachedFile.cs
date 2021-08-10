using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace TempoIDE.Classes.Types
{
    public class CachedFile
    {
        public readonly FileInfo FileInfo;
        public Encoding Encoding;
        public string Content { get; private set; }

        public CachedFile(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
            
            Update();
        }

        public async void Update()
        {
            FileInfo.Refresh();

            if (!FileInfo.Exists)
                return;

            using var file = FileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var buffer = new BufferedStream(file);
            using var reader = new BinaryReader(buffer);

            var bom = new byte[4];
            reader.Read(bom, 0, 4);

            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) Encoding = Encoding.UTF7;
            else if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) Encoding = Encoding.UTF8;
            else if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) Encoding = Encoding.UTF32;
            else if (bom[0] == 0xff && bom[1] == 0xfe) Encoding = Encoding.Unicode;
            else if (bom[0] == 0xfe && bom[1] == 0xff) Encoding = Encoding.BigEndianUnicode;
            else if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) Encoding = new UTF32Encoding(true, true);  //UTF-32BE
            else Encoding = Encoding.ASCII;

            await Task.Run(delegate
            {
                Content = Encoding.GetString(bom.Concat(reader.ReadAllBytes()).ToArray());
            });
        }
    }
}