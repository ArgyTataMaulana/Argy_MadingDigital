using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadingDigital.DAL
{
    namespace MadingDigital.DAL
    {
        public class PengumumanDAL
        {
            private Koneksi kon = new Koneksi();

            // ====================== GET DATA ======================
            public DataTable GetAllPengumuman()
            {
                DataTable dt = new DataTable();
                using (MySqlConnection conn = kon.GetConn())
                {
                    try
                    {
                        conn.Open();
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM v_tampil_pengumuman", conn);
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Gagal mengambil data: " + ex.Message);
                    }
                }
                return dt;
            }

            public DataTable CariPengumuman(string keyword)
            {
                DataTable dt = new DataTable();
                using (MySqlConnection conn = kon.GetConn())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("sp_cari_pengumuman", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_keyword", keyword);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Gagal mencari data: " + ex.Message);
                    }
                }
                return dt;
            }

            // ====================== CRUD ======================
            public void TambahPengumuman(string judul, string isi, string status, DateTime tanggal, int idAdmin)
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("sp_tambah_pengumuman", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_judul", judul.Trim());
                        cmd.Parameters.AddWithValue("p_isi", isi.Trim());
                        cmd.Parameters.AddWithValue("p_status", status);
                        cmd.Parameters.AddWithValue("p_id_admin", idAdmin);
                        cmd.Parameters.AddWithValue("p_tgl", tanggal.Date);
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException ex) when (ex.Number == 1644)
                    {
                        throw new Exception("Judul pengumuman sudah ada!");
                    }
                }
            }

            public void TambahBanyakPengumuman(DataTable dt, int defaultIdAdmin)
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    conn.Open();
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string query = @"INSERT INTO pengumuman (judul, isi_pengumuman, status, tanggal_upload, id_admin) 
                                             VALUES (@judul, @isi, @status, @tgl, @admin)";

                            MySqlCommand cmd = new MySqlCommand(query, conn, transaction);

                            cmd.Parameters.Add("@judul", MySqlDbType.VarChar);
                            cmd.Parameters.Add("@isi", MySqlDbType.Text);
                            cmd.Parameters.Add("@status", MySqlDbType.Enum);
                            cmd.Parameters.Add("@tgl", MySqlDbType.Date);
                            cmd.Parameters.Add("@admin", MySqlDbType.Int32);

                            foreach (DataRow row in dt.Rows)
                            {
                                string judul = row[0] != DBNull.Value ? row[0].ToString() : "";
                                string isi = row[1] != DBNull.Value ? row[1].ToString() : "";
                                string status = row[2] != DBNull.Value ? row[2].ToString() : "aktif";
                                
                                DateTime tanggal = DateTime.Now.Date;
                                if (dt.Columns.Count > 3 && row[3] != DBNull.Value)
                                {
                                    DateTime.TryParse(row[3].ToString(), out tanggal);
                                }

                                int adminId = defaultIdAdmin;
                                if (dt.Columns.Count > 4 && row[4] != DBNull.Value)
                                {
                                    int.TryParse(row[4].ToString(), out adminId);
                                }

                                if (string.IsNullOrWhiteSpace(judul) || string.IsNullOrWhiteSpace(isi))
                                    continue; // Skip empty rows

                                cmd.Parameters["@judul"].Value = judul.Trim();
                                cmd.Parameters["@isi"].Value = isi.Trim();
                                cmd.Parameters["@status"].Value = status.ToLower() == "nonaktif" ? "nonaktif" : "aktif";
                                cmd.Parameters["@tgl"].Value = tanggal;
                                cmd.Parameters["@admin"].Value = adminId;

                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Gagal import data: " + ex.Message);
                        }
                    }
                }
            }

            public void UbahPengumuman(int id, string judul, string isi, string status, DateTime tanggal)
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("sp_ubah_pengumuman", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_id", id);
                        cmd.Parameters.AddWithValue("p_judul", judul);
                        cmd.Parameters.AddWithValue("p_isi", isi);
                        cmd.Parameters.AddWithValue("p_status", status);
                        cmd.Parameters.AddWithValue("p_tgl", tanggal);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Gagal mengubah data: " + ex.Message);
                    }
                }
            }

            public void HapusPengumuman(int id)
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("sp_hapus_pengumuman", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_id", id);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Gagal menghapus data: " + ex.Message);
                    }
                }
            }

            public int HitungTotalPengumuman()
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM pengumuman", conn);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }

            public DataTable GetPengumumanAktifSekarang()
            {
                DataTable dt = new DataTable();
                using (MySqlConnection conn = kon.GetConn())
                {
                    try
                    {
                        conn.Open();
                        string query = @"
                SELECT 
                    p.id_pengumuman,
                    p.judul,
                    p.isi_pengumuman,
                    p.status,
                    p.tanggal_upload,
                    p.id_admin,
                    gm.path_file,
                    jt.tanggal_mulai,
                    jt.tanggal_selesai
                FROM pengumuman p
                INNER JOIN jadwal_tampil jt ON p.id_pengumuman = jt.id_pengumuman
                LEFT JOIN gambar_mading gm ON p.id_pengumuman = gm.id_pengumuman  
                WHERE p.status = 'aktif'
                  AND CURDATE() >= jt.tanggal_mulai 
                  AND CURDATE() <= jt.tanggal_selesai
                ORDER BY jt.tanggal_mulai DESC, p.id_pengumuman ASC";

                        MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Gagal mengambil pengumuman aktif: " + ex.Message);
                    }
                }
                return dt;
            }

            public DataTable GetDataLaporan(string status, string tahun)
            {
                DataTable dt = new DataTable();
                using (MySqlConnection conn = kon.GetConn())
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT id_pengumuman, judul, isi_pengumuman, status, tanggal_upload, id_admin FROM pengumuman WHERE 1=1";
                        
                        if (!string.IsNullOrEmpty(status) && status != "Semua")
                        {
                            query += " AND status = @status";
                        }
                        
                        if (!string.IsNullOrEmpty(tahun) && tahun != "Semua")
                        {
                            query += " AND YEAR(tanggal_upload) = @tahun";
                        }

                        query += " ORDER BY tanggal_upload DESC";

                        MySqlCommand cmd = new MySqlCommand(query, conn);

                        if (!string.IsNullOrEmpty(status) && status != "Semua")
                        {
                            cmd.Parameters.AddWithValue("@status", status);
                        }
                        if (!string.IsNullOrEmpty(tahun) && tahun != "Semua")
                        {
                            cmd.Parameters.AddWithValue("@tahun", tahun);
                        }

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Gagal mengambil data laporan: " + ex.Message);
                    }
                }
                return dt;
            }
        }
    }
}
