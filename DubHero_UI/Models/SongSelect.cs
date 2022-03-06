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
            Task.Run(() => this.generateSongsAsync()).Wait();
        }

        public List<SongView> SongList { get => songList; set => songList = value; }

        public async Task generateSongsAsync()
        {
            try
            {
                ListadosAssets listadosCarpetas = new ListadosAssets();
                songList = await listadosCarpetas.getSongViewListAsync();
            }catch (Exception ex)
            {

            }

        }






    }
}
