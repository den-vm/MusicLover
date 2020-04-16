using System.IO;
using File = TagLib.File;

namespace MuloApi.Classes
{
    public class DataAudioFile
    {
        public DataAudioFile(string name, Stream stream)
        {
            Name = name;
            Stream = stream;
        }

        public string Name { get; set; }
        public Stream Stream { get; set; }
    }

    public class AudioFile : File.IFileAbstraction
    {
        private readonly DataAudioFile _file;

        public AudioFile(DataAudioFile dataAudioFile)
        {
            _file = dataAudioFile;
        }

        public string Name => _file.Name;

        public Stream ReadStream => _file.Stream;

        public Stream WriteStream => _file.Stream;

        public void CloseStream(Stream stream)
        {
            stream.Position = 0;
        }
    }
}