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

        /// <summary>
        /// Obtiene la carpeta donde se guardan toda la informción sobre las canciones disponibles
        /// </summary>
        /// <returns></returns>
        private async Task<IReadOnlyList<StorageFolder>> getSongsFolder()
        {
            var assetsFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var songListFolder = await assetsFolder.GetFolderAsync("Songs");
            var listCarpetas = songListFolder.GetFoldersAsync();
            return listCarpetas.GetResults();

        }

        /// <summary>
        /// Obtiene una lista de SongView por cada una de las subcarpetas que contiene la carpeta songs.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Obtiene la dificultad de un fichero (info.txt) donde se almacenará un entero con dicha informacion
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
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
