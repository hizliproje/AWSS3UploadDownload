using System;
using System.Windows.Forms;
using Amazon.S3.Transfer;
using System.Threading;
using System.IO;

namespace AWSS3Upload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Thread AwsUploadThread;
        private void doldur()
        {
            // Dosya seçilme kontrolü yapıyoruz.
            if (txtFilePath.Text.Trim() == string.Empty)
            {
                // Dosya Seçilmediyse Kullanıcıya uyarı verip fonksiyondan çıkılmasını sağlıyoruz.
                MessageBox.Show("Lütfen Bir Dosya Seçiniz.");
                return;
            }
            // Dosya ve Klasör adlarını oluşturuyoruz.
            string file_name = txtFilePath.Text.Trim(); // Tam dosya yolu
            string bucket_name = "BucketName"; // S3 Bucket adınız
            string directory_name = "DirectoryName"; // S3 Bucket'ınızdaki klasör adınız (Ana Klasöre atmak için boş bırakabilirsiniz.)
            string s3_file_name = "s3FileName" + txtFilePath.Tag.ToString(); // Dosyanın S3teki adı + Dosya uzantısı

            AmazonUploader myUploader = new AmazonUploader(); // AmazonUploader'ı Başlatıyoruz.

            // S3 ile bağlantıyı gerçekleştiriyoruz
            myUploader.SetFileToS3(file_name, bucket_name, directory_name, s3_file_name);
            // Yükleme durumunu görmek için eventı oluşturuyoruz
            myUploader.request.UploadProgressEvent += Request_UploadProgressEvent1;
            // Upload'ı Başlatıyoruz 
            bool result = myUploader.Upload();

            // Dönen sonucu kontrol ediyoruz. Sonuca Göre kullanıcıya mesaj verdiriyoruz.
            if (result)
                // Result = true
                MessageBox.Show("Dosya Yükleme İşlemi Başarılı..");
            else
                // Result = false
                MessageBox.Show("Dosya Yükleme İşlemi Başarısız.");

            //ProgressBar Ve Labeli Sıfırlıyoruz.
            ProgressReset();
        }
        void ProgressReset()
        {
            prgUploadStatus.Value = 0;
            lblUploadStatus.Text = "";
            txtUploadFileNameStatus.Text = "Yükleme Tamamlandı";
        }

        private void Request_UploadProgressEvent1(object sender, UploadProgressArgs e)
        {
            // Değerlerin Açıklamaları :
            // e.TransferredBytes   : Transferi Gerçekleşen Dosya Boyutu (Byte)
            // e.TotalBytes         : Toplam Transfer Gerçekleşcek Dosya Boyutu (Byte)
            // e.PercentDone        : Transfer Gerçekleşme Yüzdesi (%)
            // e.FilePath           : Transfer Edilen Dosyanın Lokal Yolu

            // Dosya Yükleme gelişmelerini label'a yazdırıyoruz
            lblUploadStatus.Text = e.TransferredBytes + " / " + e.TotalBytes;
            txtUploadFileNameStatus.Text = e.FilePath + " Yükleniyor.";
            prgUploadStatus.Value = e.PercentDone;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Kullanıcıya Dosya Seçtirtmek için OpenFileDialog Oluşturuyoruz.
            OpenFileDialog ofd = new OpenFileDialog();

            // Eğer Dosya Seçildiyse
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Dosya işlemlerine başlıyoruz.

                txtFilePath.Text = ofd.FileName;// Dosya Yolunu textbox'a Kaydediyoruz.
                txtFilePath.Tag = Path.GetExtension(ofd.FileName.ToString()); // Dosya uzantısını ise textboxın tagine kaydediyoruz.
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Dosya Yükleme Yapılırken Formun Kitlenmemesi için Thread Kullanıyoruz
            AwsUploadThread = new Thread(new ThreadStart(doldur));
            AwsUploadThread.Start();
        }
    }
}
