using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadingDigital.DAL
{
    public class JadwalTampilDAL
    {
        private readonly Koneksi kon = new Koneksi();

        public void AturJadwal(int idPengumuman, DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            using (var conn = kon.GetConn())
            {
                conn.Open();
                // Hapus jadwal lama kalau ada
                string queryHapus = "DELETE FROM jadwal_tampil WHERE id_pengumuman = @id";
                MySqlCommand cmdHapus = new MySqlCommand(queryHapus, conn);
                cmdHapus.Parameters.AddWithValue("@id", idPengumuman);
                cmdHapus.ExecuteNonQuery();

                // Insert jadwal baru
                string queryInsert = @"INSERT INTO jadwal_tampil 
                    (tanggal_mulai, tanggal_selesai, id_pengumuman) 
                    VALUES (@mulai, @selesai, @id)";
                MySqlCommand cmdInsert = new MySqlCommand(queryInsert, conn);
                cmdInsert.Parameters.AddWithValue("@mulai", tanggalMulai.Date);
                cmdInsert.Parameters.AddWithValue("@selesai", tanggalSelesai.Date);
                cmdInsert.Parameters.AddWithValue("@id", idPengumuman);
                cmdInsert.ExecuteNonQuery();
            }
        }

        public bool CekJadwalAktif(int idPengumuman)
        {
            using (var conn = kon.GetConn())
            {
                conn.Open();
                string query = @"SELECT COUNT(*) FROM jadwal_tampil 
                    WHERE id_pengumuman = @id
                    AND CURDATE() >= tanggal_mulai 
                    AND CURDATE() <= tanggal_selesai";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idPengumuman);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public DataTable GetAllJadwal()
        {
            DataTable dt = new DataTable();
            using (var conn = kon.GetConn())
            {
                conn.Open();
                string query = @"
            SELECT 
                jt.id_jadwal,
                p.judul,
                jt.tanggal_mulai,
                jt.tanggal_selesai,
                CASE 
                    WHEN CURDATE() >= jt.tanggal_mulai AND CURDATE() <= jt.tanggal_selesai 
                    THEN 'Aktif' 
                    WHEN CURDATE() < jt.tanggal_mulai THEN 'Belum Mulai'
                    ELSE 'Sudah Selesai'
                END AS status_jadwal
            FROM jadwal_tampil jt
            JOIN pengumuman p ON jt.id_pengumuman = p.id_pengumuman
            ORDER BY jt.tanggal_mulai DESC";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
