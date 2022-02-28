using System;
using System.Collections.Generic;
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

        private void generateSongs() { 
        
            int i = 0;

            SongList.Add(new SongView("Cancion" + i++, "/Assets/Imagenes/icono.png", 1));
            SongList.Add(new SongView("Cancion" + i++, "/Assets/SongsImages/PERRO.jfif", 2));
            SongList.Add(new SongView("Cancion" + i++, "/Assets/SongsImages/PERRO.jfif", 3));
            SongList.Add(new SongView("Cancion" + i++, "/Assets/SongsImages/PERRO.jfif", 1));
            SongList.Add(new SongView("Cancion" + i++, "/Assets/SongsImages/PERRO.jfif", 2));
            SongList.Add(new SongView("Cancion" + i++, "/Assets/SongsImages/PERRO.jfif", 3));
        }



    }
}
