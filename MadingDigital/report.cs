using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MadingDigital
{
    public partial class report : Form
    {
        private string statusFilter;
        private string tahunFilter;
        private readonly BLL.PengumumanBLL pengumumanBLL = new BLL.PengumumanBLL();

        public report(string status, string tahun)
        {
            InitializeComponent();
            this.statusFilter = status;
            this.tahunFilter = tahun;

            try
            {
                DataTable dt = pengumumanBLL.GetDataLaporan(statusFilter, tahunFilter);

                LaporanPengumuman laporan = new LaporanPengumuman();
                laporan.SetDataSource(dt);
                crystalReportViewer1.ReportSource = laporan;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal meload data laporan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
