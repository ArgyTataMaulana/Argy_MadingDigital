using MadingDigital.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadingDigital.BLL
{
    public class GambarMadingBLL
    {
        private readonly GambarMadingDAL _dal = new GambarMadingDAL();

        public void UploadGambar(string namaFile, string pathFile, int idAdmin, int idPengumuman)
        {
            if (string.IsNullOrEmpty(namaFile) || string.IsNullOrEmpty(pathFile))
                throw new Exception("File gambar tidak valid!");

            _dal.UploadGambar(namaFile, pathFile, idAdmin, idPengumuman);
        }

        public DataTable GetAllGambarAktif()
        {
            try
            {
                return _dal.GetAllGambarAktif();
            }
            catch (Exception ex)
            {
                throw new Exception("Gagal mengambil data gambar aktif: " + ex.Message);
            }
        }

        public DataTable GetRiwayatUpload()
        {
            return _dal.GetRiwayatUpload();
        }




    }
}
