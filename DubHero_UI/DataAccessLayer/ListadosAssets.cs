using DubHero_UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace DubHero_UI.DataAccessLayer
{
    public class ListadosAssets
    {
        public ListadosAssets()
        {
        }

        private async Task<IReadOnlyList<StorageFolder>> getSongsFolder()
        {
            var assetsFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var songListFolder = await assetsFolder.GetFolderAsync("Songs");
            var listCarpetas = songListFolder.GetFoldersAsync();
            return listCarpetas.GetResults();

        }

       public async Task<List<SongView>> getSongViewListAsync()
        {
            List<SongView> list = new List<SongView>();
           var listaCarpetas =  await getSongsFolder();
           var ll =  listaCarpetas.AsEnumerable();
            String nombreCancion="";
            int dificultad=0;
            String fotoFilePath="";
            foreach (var l in ll)
            {
                try
                {
                    nombreCancion = l.Name;
                    var infoFile = l.GetFileAsync("info.txt");
                    dificultad = getDificultad(infoFile.GetResults().Path);

                    var fotoFile = l.GetFileAsync("foto.jpg");
                     fotoFilePath = fotoFile.GetResults().Path;
                }catch (Exception ex)
                {

                }
                list.Add(new SongView(nombreCancion, fotoFilePath, dificultad));

            }
            return list;
        }

        private int getDificultad(String filePath)
        {
            int dificultad = 0;
            foreach (string line in System.IO.File.ReadLines(filePath)) 
            {
                dificultad = int.Parse(line);
            }
            return dificultad;
        }

    }
}
