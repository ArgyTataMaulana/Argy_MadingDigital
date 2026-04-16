using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; 


namespace MadingDigital
{
    public partial class Form1 : Form
    {
        public void TampilkanData()
        {
            Koneksi kon = new Koneksi();
            MySqlConnection conn = kon.GetConn();

            try
            {
                conn.Open();
                string query = "SELECT id_pengumuman, judul, isi_pengumuman, status, tanggal_upload FROM pengumuman";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader dr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(dr);

                
                dataGridView1.DataSource = dt;

                
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Judul";
                dataGridView1.Columns[2].HeaderText = "Isi Pengumuman";
                dataGridView1.Columns[3].HeaderText = "Status";
                dataGridView1.Columns[4].HeaderText = "Tanggal";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void HitungTotal()
        {
            Koneksi kon = new Koneksi();
            MySqlConnection conn = kon.GetConn();

            try
            {
                conn.Open();
                // Query Count
                string query = "SELECT COUNT(*) FROM pengumuman";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                int total = Convert.ToInt32(cmd.ExecuteScalar());

                lblTotal.Text = "Total Pengumuman: " + total.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghitung data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void BersihkanForm()
        {
            textBox1.Clear();
            textBox3.Clear();
            richTextBox1.Clear();
            comboBox1.SelectedIndex = -1; // Mengosongkan pilihan ComboBox
            dtpTanggal.Value = DateTime.Now; // Reset tanggal ke hari ini
        }

        // Panggil di event Klik tombol Bersihkan
        private void btnBersihkan_Click(object sender, EventArgs e)
        {
            BersihkanForm();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TampilkanData();
            HitungTotal();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void labelIsi_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox3.Text = row.Cells[1].Value.ToString();
                richTextBox1.Text = row.Cells[2].Value.ToString();
                comboBox1.Text = row.Cells[3].Value.ToString();
                dtpTanggal.Value = Convert.ToDateTime(row.Cells[4].Value);
            }
            catch (Exception)
            {
                
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            // 1. Konfirmasi sebelum ubah 
            DialogResult dialogResult = MessageBox.Show("Apakah Anda yakin ingin mengubah data ini?", "Konfirmasi Ubah", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                Koneksi kon = new Koneksi();
                MySqlConnection conn = kon.GetConn();
                try
                {
                    conn.Open();
                    // Query UPDATE berdasarkan ID
                    string query = "UPDATE pengumuman SET judul=@judul, isi_pengumuman=@isi, status=@status, tanggal_upload=@tgl WHERE id_pengumuman=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@judul", textBox1.Text);
                    cmd.Parameters.AddWithValue("@isi", richTextBox1.Text);
                    cmd.Parameters.AddWithValue("@status", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@tgl", dtpTanggal.Value);
                    cmd.Parameters.AddWithValue("@id", textBox1.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Berhasil Diperbarui!");

                    TampilkanData(); // Refresh tabel
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal Update: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            // 1. Validasi: Pastikan ada data yang dipilih (ID tidak kosong)
            if (textBox1.Text == "")
            {
                MessageBox.Show("Pilih data yang ingin dihapus terlebih dahulu dari tabel!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Konfirmasi Hapus 
            DialogResult dr = MessageBox.Show("Apakah Anda yakin ingin menghapus pengumuman ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                Koneksi kon = new Koneksi();
                MySqlConnection conn = kon.GetConn();
                try
                {
                    conn.Open();
                    // Query DELETE menggunakan parameter ID
                    string query = "DELETE FROM pengumuman WHERE id_pengumuman = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", textBox1.Text);

                    cmd.ExecuteNonQuery(); // Menjalankan perintah hapus
                    MessageBox.Show("Data Berhasil Dihapus", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 3. Refresh tampilan
                    TampilkanData();
                    HitungTotal();
                    BersihkanForm(); // Method untuk mengosongkan textbox
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal Hapus: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            BersihkanForm();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || richTextBox1.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Data belum lengkap! Harap isi Judul, Isi, dan Status.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Inisialisasi Koneksi
            Koneksi kon = new Koneksi();
            MySqlConnection conn = kon.GetConn();

            try
            {
                conn.Open();

              
                string query = "INSERT INTO pengumuman (judul, isi_pengumuman, tanggal_upload, status, id_admin) " +
                               "VALUES (@judul, @isi, @tgl, @status, @admin)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                // 4. Menggunakan Parameter agar aman dan rapi
                cmd.Parameters.AddWithValue("@judul", textBox3.Text);
                cmd.Parameters.AddWithValue("@isi", richTextBox1.Text);
                cmd.Parameters.AddWithValue("@tgl", dtpTanggal.Value);
                cmd.Parameters.AddWithValue("@status", comboBox1.Text);
                cmd.Parameters.AddWithValue("@admin", 1); // ID admin default

                // 5. Eksekusi Query 
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data Pengumuman Berhasil Disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 6. Refresh Tampilan & Hitung Total Otomatis
                TampilkanData();
                HitungTotal();

                // 7. Bersihkan Form 
                BersihkanForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Koneksi kon = new Koneksi();
            MySqlConnection conn = kon.GetConn();
            try
            {
                conn.Open();
                // Mencari judul yang mirip dengan isi txtCari
                string query = "SELECT * FROM pengumuman WHERE judul LIKE @cari";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cari", "%" + textBox2.Text + "%");

                MySqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            TampilkanData(); // Menampilkan semua data lagi
            HitungTotal();   // Mengupdate label total data
            textBox2.Clear(); // Mengosongkan kotak pencarian agar bersih

            MessageBox.Show("Daftar data telah di-reset ke semula.", "Informasi");

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog() { Filter = "CSV|*.csv", FileName = "Laporan_Mading.csv" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string csv = "";
                    foreach (DataGridViewColumn col in dataGridView1.Columns) csv += col.HeaderText + ",";
                    csv += "\n";
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells) csv += cell.Value?.ToString() + ",";
                        csv += "\n";
                    }
                    System.IO.File.WriteAllText(sfd.FileName, csv);
                    MessageBox.Show("Laporan Berhasil Diunduh!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Jalankan query ke tabel riwayat_upload
            Koneksi kon = new Koneksi();
            MySqlConnection conn = kon.GetConn();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM riwayat_upload", conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            dataGridView1.DataSource = dt; // Tabel berganti isi jadi riwayat
            conn.Close();
        }

        private void btnPilihGambar_Click(object sender, EventArgs e)
        {
            // Filter agar hanya file gambar yang bisa dipilih
            openFileDialog1.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Menampilkan preview gambar di PictureBox
                pbMading.Image = new Bitmap(openFileDialog1.FileName);

                // Simpan alamat file di properti 'Tag' agar mudah dipanggil saat upload
                pbMading.Tag = openFileDialog1.FileName;
            }
        }

        private void btnUploadGambar_Click(object sender, EventArgs e)
        {
            // Cek apakah sudah pilih gambar
            if (pbMading.Tag == null)
            {
                MessageBox.Show("Silakan pilih gambar terlebih dahulu!", "Peringatan");
                return;
            }

            Koneksi kon = new Koneksi();
            MySqlConnection conn = kon.GetConn();

            try
            {
                conn.Open();

                // Ambil path lengkap dari Tag
                string pathLengkap = pbMading.Tag.ToString();
                // Ambil nama filenya saja (misal: poster.jpg)
                string namaFile = System.IO.Path.GetFileName(pathLengkap);

                // Query INSERT ke tabel gambar_mading
                string query = "INSERT INTO gambar_mading (nama_file, path_file, tanggal_upload, id_admin) " +
                               "VALUES (@nama, @path, @tgl, @admin)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nama", namaFile);
                cmd.Parameters.AddWithValue("@path", pathLengkap);
                cmd.Parameters.AddWithValue("@tgl", DateTime.Now);
                cmd.Parameters.AddWithValue("@admin", 1); // Default admin ID

                cmd.ExecuteNonQuery();

                MessageBox.Show("Gambar Mading Berhasil Terdaftar di Sistem!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset preview setelah sukses
                pbMading.Image = null;
                pbMading.Tag = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Upload: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
