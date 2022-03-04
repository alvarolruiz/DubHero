using DubHero_UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DubHero_DAL
{
    public class ListadosCarpetas
    {
        private String pathCarpetas;

        public ListadosCarpetas(string pathCarpetas)
        {
            this.pathCarpetas=pathCarpetas;
        }

        public List<SongView> getListadoCancionesDisponibles()
        {
            List<string> dirs = new List<String>(Directory.EnumerateDirectories(pathCarpetas));
            String pathCarpetaCancion = pathCarpetas;
            String nombreCancion = "";
            int dificultad = 0;
            List<SongView> list = new List<SongView>();
            foreach (var dir in dirs)
            {
                nombreCancion=($"{dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1)}");
                dificultad = getDificultad(nombreCancion);
                list.Add(new SongView(nombreCancion, dificultad));
            }
            return list;

        }

        private int getDificultad(String nombreCancion)
        {
            String pathFichero = pathCarpetas+ nombreCancion+ "\info.txt";
            int dificultad = 0;
            foreach (string line in System.IO.File.ReadLines(pathFichero)) 
            {
                dificultad = int.Parse(line);
            }
            return dificultad;
        }

        private int getImagePath(String nombreCancion)
        {
            String pathFichero = pathCarpetas+ nombreCancion+ "\info.txt";
            int dificultad = 0;
            foreach (string line in System.IO.File.ReadLines(pathFichero))
            {
                dificultad = int.Parse(line);
            }
            return dificultad;
        }

    }
}
