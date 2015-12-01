using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace AWSS3Upload
{
    public class AmazonUploader
    {
        public TransferUtility utility;
        public TransferUtilityUploadRequest request;

        public void SetFileToS3(string _local_file_path, string _bucket_name, string _sub_directory, string _file_name_S3)
        {
            // Gelen Değerler :
            // _local_file_path     : Lokal dosya yolu örn. "d:\filename.zip"
            // _bucket_name         : S3 teki bucket adı ,Bucket önceden oluşturulmuş olmalıdır.
            // _sub_directory       : Boş değilse S3 içinde klasör oluşturulur yada varsa içine ekler dosyayı.
            // _file_name_S3        : Dosyanın S3 içindeki adı

            // IAmazonS3 class'ı oluşturuyoruz ,Benim lokasyonum RegionEndpoint.EUCentral1 onun için onu seçiyorum
            // Sizde lokasyonunuza göre değiştirmelisiniz.
            IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.EUCentral1);

            // Bir TransferUtility oluşturuyoruz(Türkçesi : Aktarım Programı).
            utility = new TransferUtility(client);
            // TransferUtilityUploadRequest oluşturuyoruz
            request = new TransferUtilityUploadRequest();

            if (_sub_directory == "" || _sub_directory == null)
            {
                request.BucketName = _bucket_name; //Alt klasör girmediysek direk bucket'ın içine atıyor.
            }
            else
            {   // Alt Klasör ve Bucket adı
                request.BucketName = _bucket_name + @"/" + _sub_directory;
            }
            request.Key = _file_name_S3; //Dosyanın S3 teki adı
            request.FilePath = _local_file_path; //Lokal Dosya Yolu
        }
        public bool Upload()
        {
            utility.Upload(request); //Transferi Başlatıyoruz
            return true; //Dosyanın gönderildiğini gösterir.
        }
    }
}
