using MadingDigital.DAL;
using MadingDigital.DAL.MadingDigital.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadingDigital.BLL
{
    public class PengumumanBLL
    {
        private readonly PengumumanDAL _dal = new PengumumanDAL();

        // ====================== BUSINESS LOGIC ======================
        private bool ValidasiInput(string judul, string isi)
        {
            if (string.IsNullOrWhiteSpace(judul))
            {
                throw new Exception("Judul pengumuman tidak boleh kosong!");
            }
            if (string.IsNullOrWhiteSpace(isi))
            {
                throw new Exception("Isi pengumuman tidak boleh kosong!");
            }
            return true;
        }

        private bool ValidasiTanggal(DateTime tanggal)
        {
            DateTime hariIni = DateTime.Now.Date;
            DateTime batasMax = hariIni.AddYears(10);

            if (tanggal.Date < hariIni)
            {
                throw new Exception("Tanggal tidak boleh di masa lalu!\nPilih hari ini atau tanggal mendatang.");
            }

            if (tanggal.Date > batasMax)
            {
                throw new Exception("Tanggal maksimal hanya boleh 10 tahun ke depan!");
            }

            return true;
        }

        // ====================== CRUD dengan Business Logic ======================
        public void TambahPengumuman(string judul, string isi, string status, DateTime tanggal, int idAdmin)
        {
            ValidasiInput(judul, isi);
            ValidasiTanggal(tanggal);

            _dal.TambahPengumuman(judul, isi, status, tanggal, idAdmin);
        }

        public void TambahBanyakPengumuman(DataTable dt, int defaultIdAdmin)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new Exception("Data Excel kosong!");
            }
            
            _dal.TambahBanyakPengumuman(dt, defaultIdAdmin);
        }

        public void UbahPengumuman(int id, string judul, string isi, string status, DateTime tanggal)
        {
            if (id <= 0) throw new Exception("ID tidak valid!");

            ValidasiInput(judul, isi);
            ValidasiTanggal(tanggal);

            _dal.UbahPengumuman(id, judul, isi, status, tanggal);
        }

        public void HapusPengumuman(int id)
        {
            if (id <= 0) throw new Exception("ID tidak valid!");
            _dal.HapusPengumuman(id);
        }

        public DataTable GetAllPengumuman()
        {
            return _dal.GetAllPengumuman();
        }

        public DataTable CariPengumuman(string keyword)
        {
            return _dal.CariPengumuman(keyword);
        }

        public int HitungTotalPengumuman()
        {
            return _dal.HitungTotalPengumuman();
        }

        public DataTable GetPengumumanAktifSekarang()
        {
            // Bisa tambah business logic nanti (misal: cache, sorting, dll)
            return _dal.GetPengumumanAktifSekarang();
        }

        public DataTable GetDataLaporan(string status, string tahun)
        {
            return _dal.GetDataLaporan(status, tahun);
        }
    }
}

