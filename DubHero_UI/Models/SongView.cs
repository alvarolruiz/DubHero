using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DubHero_UI.Models
{
    public class SongView
    {
        private string name;
        private string srcImage;
        private int dificulty;

        public SongView(string name, string srcImage, int dificulty)
        {
            this.Name = name;
            this.SrcImage = srcImage;
            this.Dificulty = dificulty;
        }

        public SongView(string name, int dificulty)
        {
            this.name=name;
            this.dificulty=dificulty;
        }

        public string Name { get => name; set => name = value; }
        public string SrcImage { get => srcImage; set => srcImage = value; }
        public int Dificulty { get => dificulty; set => dificulty = value; }
    }
}
