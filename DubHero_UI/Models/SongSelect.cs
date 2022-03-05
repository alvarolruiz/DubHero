using DubHero_UI.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DubHero_UI.Models
{
    public class SongSelect
    {

        private List<SongView> songList;

        public SongSelect()
        {
            SongList = new List<SongView>();
            generateSongsAsync();
        }

        public List<SongView> SongList { get => songList; set => songList = value; }

        private async void generateSongsAsync()
        {
            try
            {
                ListadosAssets listadosCarpetas = new ListadosAssets();
                //TODO -> Cuando no se debuguea la información no llega correctamente
                Task<List<SongView>> obtne = listadosCarpetas.getSongViewListAsync();
                songList = obtne.GetAwaiter().GetResult() ;
            }catch (Exception ex)
            {

            }

        }






    }
}
