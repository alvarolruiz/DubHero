using DubHero_UI.DataAccessLayer;
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
            ListadosCarpetas listadosCarpetas = new ListadosCarpetas(@"..\..\..\Assets\Songs\");
            songList = listadosCarpetas.getListadoCancionesDisponibles();

        }






    }
}
