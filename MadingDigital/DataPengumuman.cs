using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadingDigital
{
    public class DataPengumuman
    {
        public int id_pengumuman { get; set; }
        public string judul { get; set; }
        public string isi_pengumuman { get; set; }
        public string status { get; set; }
        public DateTime tanggal_upload { get; set; }
        public int id_admin { get; set; }
    }
}
