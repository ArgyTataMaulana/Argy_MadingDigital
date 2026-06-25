using MadingDigital.DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadingDigital.BLL
{
    public class JadwalTampilBLL
    {
        private readonly JadwalTampilDAL _dal = new JadwalTampilDAL();

        public void AturJadwal(int idPengumuman, DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            if (idPengumuman <= 0)
                throw new Exception("Pilih pengumuman terlebih dahulu!");

            if (tanggalMulai.Date > tanggalSelesai.Date)
                throw new Exception("Tanggal mulai tidak boleh lebih dari tanggal selesai!");

            if (tanggalSelesai.Date < DateTime.Now.Date)
                throw new Exception("Tanggal selesai tidak boleh di masa lalu!");

            _dal.AturJadwal(idPengumuman, tanggalMulai, tanggalSelesai);
        }

        public bool CekJadwalAktif(int idPengumuman)
        {
            if (idPengumuman <= 0) return false;
            return _dal.CekJadwalAktif(idPengumuman);
        }

        public DataTable GetAllJadwal()
        {
            return _dal.GetAllJadwal();
        }
    }
}
