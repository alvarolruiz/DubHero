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
        /// Obtiene la carpeta en la que se guarda todas la información de las canciones
        /// </summary>
        /// <returns></returns>
        private async Task<IReadOnlyList<StorageFolder>> getSongsFolder()
        {
            var assetsFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var songListFolder = await assetsFolder.GetFolderAsync("Songs");
            var listCarpetas = await songListFolder.GetFoldersAsync();
            return listCarpetas;

        }

        /// <summary>
        /// Obtiene un listado de todas las carpetas y busca dentro de ellas la información necesaria para crear un list de SongView
        ///
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
                    var infoFile = await l.GetFileAsync("info.txt");
                    dificultad = getDificulty(infoFile.Path);

                    var fotoFile = await l.GetFileAsync("foto.jpg");
                    fotoFilePath = fotoFile.Path;
                }catch (Exception ex)
                {

                }
                list.Add(new SongView(nombreCancion, fotoFilePath, dificultad));
            }
            return list;
        }

        /// <summary>
        /// Obtiene la dificultad de la canción de un archivo txt con la información.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private int getDificulty(String filePath)
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
