using MadingDigital.BLL;
using System;
using System.Data;
using System.Windows.Forms;

namespace MadingDigital
{
    public partial class FormFilterLaporan : Form
    {
        private readonly PengumumanBLL pengumumanBLL = new PengumumanBLL();

        public FormFilterLaporan()
        {
            InitializeComponent();
        }

        private void FormFilterLaporan_Load(object sender, EventArgs e)
        {
            // Set default values for ComboBox
            cmbStatus.SelectedIndex = 0; // Semua
            
            // Populate Tahun
            cmbTahun.Items.Add("Semua");
            for (int i = DateTime.Now.Year; i >= 2020; i--)
            {
                cmbTahun.Items.Add(i.ToString());
            }
            cmbTahun.SelectedIndex = 0; // Semua
            
            btnCetak.Enabled = false;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string status = cmbStatus.SelectedItem.ToString();
                string tahun = cmbTahun.SelectedItem.ToString();

                DataTable dt = pengumumanBLL.GetDataLaporan(status, tahun);
                dataGridView1.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    btnCetak.Enabled = true;
                }
                else
                {
                    btnCetak.Enabled = false;
                    MessageBox.Show("Data tidak ditemukan untuk kriteria ini.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            string status = cmbStatus.SelectedItem.ToString();
            string tahun = cmbTahun.SelectedItem.ToString();

            // Buka form report
            report frmReport = new report(status, tahun);
            frmReport.Show();
        }
    }
}
