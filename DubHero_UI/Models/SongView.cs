using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DubHero_UI.Models
{
    public class SongView
    {
        private string folderName;
        private string name;
        private string artist;
        private string srcImage;
        private int dificulty;

        public SongView(string folderName, string name, string artist, int dificulty, string srcImage)
        {
            this.FolderName = folderName;
            this.Name = name;
            this.Artist = artist;
            this.Dificulty = dificulty;
            this.SrcImage = srcImage;
        }


        public string FolderName { get => folderName; set => folderName=value; }
        public string Artist { get => artist; set => artist=value; }
        public string Name { get => name; set => name = value; }
        public string SrcImage { get => srcImage; set => srcImage = value; }
        public int Dificulty { get => dificulty; set => dificulty = value; }
    }
}
