Screenshot hasil menjalankan sistem (File ReadMe) :
• Form koneksi
<img width="484" height="349" alt="image" src="https://github.com/user-attachments/assets/1f541b92-0256-4670-a647-dd0ebe00a36a" />
• Form input data
<img width="523" height="506" alt="image" src="https://github.com/user-attachments/assets/409c750d-5f43-436d-9366-bcd945b17dd2" />

• Form tampilan data
<img width="587" height="291" alt="image" src="https://github.com/user-attachments/assets/ccf237ad-896b-4fae-ab77-d40aa61f21a0" />

• Bukti insert, update, delete, dan search
<img width="1203" height="684" alt="image" src="https://github.com/user-attachments/assets/377605fe-2bce-4a53-8d93-817af79a3cdf" />
<img width="1202" height="681" alt="image" src="https://github.com/user-attachments/assets/fe3477fb-4be7-44db-ade7-e905542d320b" />


<img width="1209" height="671" alt="image" src="https://github.com/user-attachments/assets/0000c236-d2b1-40c5-8ead-22dda84e626a" />

### 🛡️ Skenario SQL Injection (UCP 2)

**1. Deskripsi Masalah**
Fitur pencarian pada tombol `btnCariBahaya` bersifat rentan karena menggunakan penggabungan string (concatenation) secara langsung:
`query = "SELECT * FROM v_tampil_pengumuman WHERE judul = '" + textBox2.Text + "'";`

**2. Langkah Serangan (Payload)**
Input yang digunakan: `' OR 1=1 -- `

**3. Dampak Serangan**
Logic pencarian berhasil di-bypass. Database mengevaluasi pernyataan `1=1` sebagai TRUE untuk setiap baris, sehingga sistem menampilkan seluruh data sensitif meskipun user tidak mengetahui kata kunci pencarian yang benar.

**4. Solusi Keamanan**
Serangan ini dicegah dengan menggunakan **Stored Procedure** (`sp_cari_pengumuman`). Dengan memisahkan logika query dan data input melalui Parameter, karakter berbahaya seperti `'` (kutip tunggal) dan `--` (comment) akan dibaca sebagai string literal, bukan perintah eksekusi SQL.

