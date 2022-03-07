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
        private async Task<IReadOnlyList<StorageFolder>> GetSongsFolder()
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
       public async Task<List<SongView>> GetSongViewListAsync()
        {
            List<SongView> list = new List<SongView>();
            var listaCarpetas =  await GetSongsFolder();
            var ll =  listaCarpetas.AsEnumerable();
            String folderName="";
            String fotoFilePath="";
            var info = new Dictionary<string, string>();
            foreach (var l in ll)
            {
                try
                {
                    folderName = l.Name;
                    var infoFile = await l.GetFileAsync("info.txt");
                    info = GetInfo(infoFile.Path);

                    var fotoFile = await l.GetFileAsync("foto.jpg");
                    fotoFilePath = fotoFile.Path;
                }catch (Exception ex)
                {

                }
                list.Add(new SongView(folderName,info["songName"], info["artist"],int.Parse(info["dificulty"]), fotoFilePath));
            }
            return list;
        }

        /// <summary>
        /// Obtiene la dificultad de la canción de un archivo txt con la información.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private Dictionary<String, String> GetInfo(String filePath)
        {
            var info = new Dictionary<string, string>();
            int contador = 0;
            foreach (string line in System.IO.File.ReadLines(filePath)) 
            {
                if(contador == 0)
                {
                    info["songName"] = line;
                }else if(contador == 1)
                {
                    info["artist"] = line;
                }else if(contador == 2)
                {
                    info["dificulty"] = line;
                }
                contador++;
            }
            return info;
        }

    }
}
