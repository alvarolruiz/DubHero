using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DubHero_UI.Models
{
    public class SongSelect
    {
        public SongSelect()
        {
            SongList = new List<String>();
            SongList.Add("pp");
            SongList.Add("pp");
            SongList.Add("pp");
            SongList.Add("pp");
            SongList.Add("pp");
            SongList.Add("pp");

        }

        public List<String> SongList { get; set; }


    }
}
