using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadingDigital.DAL
{
    public class GambarMadingDAL
    {
        private readonly Koneksi kon = new Koneksi();

        public void UploadGambar(string namaFile, string pathFile, int idAdmin, int idPengumuman)
        {
            using (var conn = kon.GetConn())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string queryGambar = @"INSERT INTO gambar_mading 
                    (nama_file, path_file, tanggal_upload, id_admin, id_pengumuman) 
                    VALUES (@nama, @path, @tgl, @admin, @idPengumuman)";

                        MySqlCommand cmdGambar = new MySqlCommand(queryGambar, conn, transaction);
                        cmdGambar.Parameters.AddWithValue("@nama", namaFile);
                        cmdGambar.Parameters.AddWithValue("@path", pathFile);
                        cmdGambar.Parameters.AddWithValue("@tgl", DateTime.Now);
                        cmdGambar.Parameters.AddWithValue("@admin", idAdmin);
                        cmdGambar.Parameters.AddWithValue("@idPengumuman", idPengumuman);
                        cmdGambar.ExecuteNonQuery();

                        string queryRiwayat = @"INSERT INTO riwayat_upload 
                    (nama_file, tanggal_upload, id_admin) 
                    VALUES (@namaLog, @tglLog, @admin)";

                        MySqlCommand cmdRiwayat = new MySqlCommand(queryRiwayat, conn, transaction);
                        cmdRiwayat.Parameters.AddWithValue("@namaLog", namaFile);
                        cmdRiwayat.Parameters.AddWithValue("@tglLog", DateTime.Now);
                        cmdRiwayat.Parameters.AddWithValue("@admin", idAdmin);
                        cmdRiwayat.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public DataTable GetRiwayatUpload()
        {
            DataTable dt = new DataTable();
            using (var conn = kon.GetConn())
            {
                conn.Open();
                string query = "SELECT * FROM riwayat_upload ORDER BY tanggal_upload DESC";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable GetAllGambarAktif()
        {
            DataTable dt = new DataTable();
            using (var conn = kon.GetConn())
            {
                conn.Open();
                string query = @"
            SELECT 
                id_gambar,
                nama_file,
                path_file,
                tanggal_upload,
                id_admin,
                id_pengumuman,
                (SELECT nama_admin FROM admin WHERE id_admin = g.id_admin) AS nama_admin
            FROM gambar_mading g
            WHERE id_pengumuman IS NOT NULL
            ORDER BY tanggal_upload DESC;";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
