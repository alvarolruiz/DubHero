using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DubHero_UI.Models
{
    public class SongSelect
    {

        private List<SongView> songList;

        public SongSelect()
        {
            SongList = new List<SongView>();
            generateSongs();
        }

        public List<SongView> SongList { get => songList; set => songList = value; }

        private void generateSongs()
        {

            int i = 0;

            SongList.Add(new SongView("Cancion" + i++, "/Assets/Imagenes/icono.png", 1));
            SongList.Add(new SongView("Cancion" + i++, "/Assets/SongsImages/PERRO.jfif", 2));
            SongList.Add(new SongView("Cancion" + i++, "/Assets/SongsImages/PERRO.jfif", 3));
            SongList.Add(new SongView("Cancion" + i++, "/Assets/SongsImages/PERRO.jfif", 1));
            SongList.Add(new SongView("Cancion" + i++, "/Assets/SongsImages/PERRO.jfif", 2));
            SongList.Add(new SongView("Cancion" + i++, "/Assets/SongsImages/PERRO.jfif", 3));


            //string docPath = @"D:\source\repos\Sistemas de gestion\Prueba directorios\PruebaComponente\Assets";
            //string docPath2 = @"C:\Users\sadac\source\repos\alvarolruiz\DubHero\DubHero_UI\Assets\" ;
            //string cancion;

            //List<string> dirs = new List<string>(Directory.EnumerateDirectories(docPath2));

            //foreach (var dir in dirs)
            //{
            //    cancion = $"{dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1)}";

            //    SongList.Add(new SongView(cancion, "/Assets/Imagenes/icono.png",1)); 
            //}
           

        }






    }
}
