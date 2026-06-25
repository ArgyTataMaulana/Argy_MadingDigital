using MadingDigital.DAL.MadingDigital.DAL;
using MadingDigital.BLL;
using MySql.Data.MySqlClient; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using ExcelDataReader;



namespace MadingDigital
{
    public partial class Form1 : Form
    {
        private readonly PengumumanBLL pengumumanBLL = new PengumumanBLL();
        private readonly GambarMadingBLL gambarBLL = new GambarMadingBLL();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TampilkanData();
            HitungTotal();
        }

       

        // ====================== DATA ======================
        public void TampilkanData()
        {
            try
            {
                DataTable dt = pengumumanBLL.GetAllPengumuman();
                bindingSource1.DataSource = dt;
                dataGridView1.DataSource = bindingSource1;

                // Binding Controls
                textBox1.DataBindings.Clear();
                textBox3.DataBindings.Clear();
                richTextBox1.DataBindings.Clear();
                comboBox1.DataBindings.Clear();

                textBox1.DataBindings.Add("Text", bindingSource1, "id_pengumuman");
                textBox3.DataBindings.Add("Text", bindingSource1, "judul");
                richTextBox1.DataBindings.Add("Text", bindingSource1, "isi_pengumuman");
                comboBox1.DataBindings.Add("Text", bindingSource1, "status");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void HitungTotal()
        {
            try
            {
                int total = pengumumanBLL.HitungTotalPengumuman();
                lblTotal.Text = "Total Pengumuman: " + total.ToString();
            }
            catch
            {
                lblTotal.Text = "Total Pengumuman: 0";
            }
        }

        private void BersihkanForm()
        {
            textBox1.Clear();
            textBox3.Clear();
            richTextBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            dtpTanggal.Value = DateTime.Now.Date;
            pbMading.Image = null;
            pbMading.Tag = null;
        }

        // ====================== CRUD PENGUMUMAN ======================
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                pengumumanBLL.TambahPengumuman(
                    textBox3.Text.Trim(),
                    richTextBox1.Text.Trim(),
                    comboBox1.Text,
                    dtpTanggal.Value,
                    1); // id_admin (nanti pakai session login)

                MessageBox.Show("Pengumuman berhasil disimpan!", "Sukses");
                BersihkanForm();
                TampilkanData();
                HitungTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Pilih data yang akan diubah terlebih dahulu!", "Peringatan");
                return;
            }

            if (MessageBox.Show("Simpan perubahan data ini?", "Konfirmasi Ubah",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    pengumumanBLL.UbahPengumuman(
                        Convert.ToInt32(textBox1.Text),
                        textBox3.Text.Trim(),
                        richTextBox1.Text.Trim(),
                        comboBox1.Text,
                        dtpTanggal.Value);

                    MessageBox.Show("Data berhasil diperbarui!", "Sukses");
                    TampilkanData();
                    BersihkanForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Pilih data yang ingin dihapus!", "Peringatan");
                return;
            }

            if (MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi Hapus",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    pengumumanBLL.HapusPengumuman(Convert.ToInt32(textBox1.Text));
                    MessageBox.Show("Data berhasil dihapus!", "Sukses");
                    TampilkanData();
                    BersihkanForm();
                    HitungTotal();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        // ====================== SEARCH & REFRESH ======================
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = pengumumanBLL.CariPengumuman(textBox2.Text.Trim());
                dataGridView1.DataSource = dt;
                lblTotal.Text = $"Ditemukan: {dt.Rows.Count} data";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mencari data: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            TampilkanData();
            HitungTotal();
            textBox2.Clear();
        }

        private void btnBersihkan_Click(object sender, EventArgs e)
        {
            BersihkanForm();
        }

        // ====================== FITUR GAMBAR MADINGS ======================
        private void btnPilihGambar_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pbMading.Image = Image.FromFile(openFileDialog1.FileName);
                    pbMading.Tag = openFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat preview gambar: " + ex.Message);
                }
            }
        }

        private void btnUploadGambar_Click(object sender, EventArgs e)
        {
            if (pbMading.Tag == null)
            {
                MessageBox.Show("Silakan pilih gambar terlebih dahulu!", "Peringatan");
                return;
            }

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Silakan klik pengumuman di tabel terlebih dahulu!", "Peringatan");
                return;
            }

            try
            {
                string pathLengkap = pbMading.Tag.ToString();
                string namaFile = System.IO.Path.GetFileName(pathLengkap);
                int idPengumuman = Convert.ToInt32(textBox1.Text);

                gambarBLL.UploadGambar(namaFile, pathLengkap, 1, idPengumuman);

                MessageBox.Show("Gambar berhasil diupload!", "Sukses");
                pbMading.Image = null;
                pbMading.Tag = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal upload gambar: " + ex.Message, "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e) // Lihat Riwayat Upload
        {
            try
            {
                DataTable dt = gambarBLL.GetRiwayatUpload();
                dataGridView1.DataSource = dt;
                lblTotal.Text = $"Total Riwayat: {dt.Rows.Count} data";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan riwayat: " + ex.Message);
            }
        }

        // ====================== CETAK LAPORAN CRYSTAL REPORTS ======================
        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            try
            {
                FormFilterLaporan formFilter = new FormFilterLaporan();
                formFilter.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuka laporan:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================== DOWNLOAD REPORT (CSV) ======================
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk diunduh!", "Info");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV File|*.csv",
                FileName = $"Laporan_Mading_{DateTime.Now:yyyyMMdd}"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string csv = "";
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                        csv += col.HeaderText + ",";

                    csv += "\n";

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                            csv += (cell.Value?.ToString() ?? "") + ",";
                        csv += "\n";
                    }

                    System.IO.File.WriteAllText(sfd.FileName, csv);
                    MessageBox.Show("Laporan berhasil diunduh!", "Sukses");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menyimpan file: " + ex.Message);
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            FormKoneksi login = new FormKoneksi();
            login.Show();
            this.Close();
        }

        // ====================== EVENT JADWAL TAMPIL  ======================
        private readonly JadwalTampilBLL jadwalBLL = new JadwalTampilBLL();

        private void btnAturJadwal_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Pilih pengumuman di tabel terlebih dahulu!", "Peringatan");
                return;
            }

            try
            {
                int idPengumuman = Convert.ToInt32(textBox1.Text);
                DateTime tanggalMulai = dtpJadwalMulai.Value;
                DateTime tanggalSelesai = dtpJadwalSelesai.Value;

                jadwalBLL.AturJadwal(idPengumuman, tanggalMulai, tanggalSelesai);

                // Cek apakah jadwal aktif sekarang
                bool aktif = jadwalBLL.CekJadwalAktif(idPengumuman);
                string statusJadwal = aktif ? "AKTIF sekarang" : "belum aktif";

                MessageBox.Show($"Jadwal berhasil diatur!\nStatus: {statusJadwal}", "Sukses");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLihatJadwal_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = jadwalBLL.GetAllJadwal();
                dataGridView1.DataSource = dt;
                lblTotal.Text = $"Total Jadwal: {dt.Rows.Count} data";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat jadwal: " + ex.Message);
            }

        }

        // ====================== EVENT HANDLER KOSONG ======================
        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void labelIsi_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void button2_Click_1(object sender, EventArgs e) { }
        private void button4_Click(object sender, EventArgs e) { }
        private void dtpTanggal_ValueChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) { }
        private void btnClear_Click(object sender, EventArgs e) { }
        private void btnCariBahaya_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnBillboard_Click(object sender, EventArgs e)
        {
            try
            {
                FormBillboard billboard = new FormBillboard();
                billboard.Show();           // atau billboard.ShowDialog(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuka Billboard:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ====================== EVENT BILLBOARD ======================
        private void btnBillboard_MouseEnter(object sender, EventArgs e)
        {
            btnBillboard.BackColor = Color.LimeGreen;     // Warna saat mouse di atas
            btnBillboard.ForeColor = Color.Black;
            btnBillboard.Font = new Font(btnBillboard.Font, FontStyle.Bold);
        }

        private void btnBillboard_MouseLeave(object sender, EventArgs e)
        {
            btnBillboard.BackColor = Color.DarkBlue;      // Kembali ke warna semula
            btnBillboard.ForeColor = Color.White;
            btnBillboard.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        // ====================== IMPORT EXCEL ======================
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel 97-2003 Workbook|*.xls", ValidateNames = true })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                });

                                DataTable dt = result.Tables[0];
                                if (dt != null)
                                {
                                    // 1 = ID Admin default (misal admin utama)
                                    pengumumanBLL.TambahBanyakPengumuman(dt, 1);
                                    MessageBox.Show("Data berhasil diimport dari Excel!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    TampilkanData(); // Refresh grid
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mengimport Excel:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}