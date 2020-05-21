using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Drawing;
using Newtonsoft.Json;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using IronBarCode;
using HtmlAgilityPack;

namespace Proje
{
    public partial class YoneticiPanel : System.Web.UI.Page
    {
        public static int i = 0;
        public static int k = 0;
        SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=KITAP;Integrated Security=True;Pooling=False");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            goruntule();
            lblUpload.Text = "";
        }
        public class kitapGetir
        {
            public string title { get; set; }
            public string subTitle { get; set; }
        }
        protected void goruntule()
        {
            try
            {
                #region Get Data from Database
                txtISBN.Text.Replace(" ", "");
                SqlCommand command = new SqlCommand("SELECT ISBN,KITAP_ADI,YAZAR,KITAP_FOTO FROM KITAPLAR WHERE KITAP_ADI LIKE '%'+@name+'%' AND ISBN LIKE '%'+@isbn+'%'", connection);
                command.Parameters.AddWithValue("@isbn", txtISBN.Text);
                command.Parameters.AddWithValue("@name", txtName.Text);
                command.ExecuteNonQuery();
                #endregion
                #region Write Data into Grid View
                System.Data.DataTable dataTable = new System.Data.DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataTable);
                GridView1.DataSourceID = null;
                GridView1.DataSource = dataTable;
                GridView1.DataBind();
                SqlCommand command2 = new SqlCommand("SELECT * FROM KITAPSURE", connection);
                command2.ExecuteNonQuery();
                System.Data.DataTable dataTable2 = new System.Data.DataTable();
                SqlDataAdapter dataAdapter2 = new SqlDataAdapter(command2);
                dataAdapter2.Fill(dataTable2);
                GridView2.DataSourceID = null;
                GridView2.DataSource = dataTable2;
                GridView2.DataBind();
                lblUpload.Style.Value = "Color:Green";
                lblUpload.Text = "Sorgu Durumu:Düzgün!";
                #endregion
            }
            catch (Exception ex)
            {
                lblUpload.Style.Value = "Color:Red";
                lblUpload.Text = "Sorgu Durumu:Bir hata oluştu. Bir nedenden dolayı görüntüleme işlemi gerçekleşmedi.<html></br></html> Hata Mesajı: " + ex.Message;
            }
        }
        protected void sil()
        {
            try
            {
                #region Delete Book from Database
                txtISBN.Text.Replace(" ", "");
                SqlCommand command = new SqlCommand("DELETE FROM KITAPLAR WHERE KITAP_ADI=''+@name+'' OR ISBN=''+@isbn+''", connection);
                command.Parameters.AddWithValue("@isbn", txtISBN.Text);
                command.Parameters.AddWithValue("@name", txtName.Text);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                command.ExecuteNonQuery();

                lblUpload.Style.Value = "Color:Green";
                lblUpload.Text = "Silme İşlemi:Başarılı!";
                #endregion
            }
            catch (Exception ex)
            {
                lblUpload.Style.Value = "Color:Red";
                lblUpload.Text = "Silme İşlemi:Bir hata oluştu. Bir nedenden dolayı silme işlemi gerçekleşmedi.<html></br></html> Hata Mesajı: " + ex.Message;
            }
            return;
        }
        protected void ekle()
        {
            txtISBN.Text = "";
            txtName.Text = "";
            i++;
            if (fileUploader.HasFile)
            {
                try
                {
                    #region Upload File
                    string filename = Path.GetFileName(fileUploader.FileName);
                    fileUploader.SaveAs(Server.MapPath(@"~/images/") + "image" + i + Path.GetExtension(filename));
                    image.ImageUrl = @"~/images/" + "image" + i + Path.GetExtension(filename);
                    #endregion
                    #region Barcode Reader
                    Bitmap bitmap = new Bitmap(Server.MapPath(image.ImageUrl));
                    BarcodeResult Result = IronBarCode.BarcodeReader.QuicklyReadOneBarcode(bitmap);
                    txtISBN.Text = Result.Text;
                    #endregion
                    #region Get Data From Web Site
                    var url = "https://www.abebooks.com/servlet/SearchResults?sts=t&isbn=" + txtISBN.Text;
                    var web = new HtmlWeb();
                    var doc = web.Load(url);
                    String[] value = new String[3];
                    value[0] = doc.DocumentNode.SelectSingleNode("//*[@id='book-1']/div[2]/div[1]/h2/a/span").InnerText.ToString();
                    value[1] = doc.DocumentNode.SelectSingleNode("//*[@id='book-1']/div[2]/div[1]/p[1]/strong").InnerText.ToString();
                    value[2] = doc.DocumentNode.SelectSingleNode("//*[@id='listing_1']/div/img").Attributes["src"].Value;
                    value[1] = value[1].Replace("Author:", "");
                    txtName.Text = value[0].Replace("&#39;", "'");
                    #endregion
                    #region Insert Book into Database
                    SqlCommand command = new SqlCommand("declare @isbn varchar(30); declare @name varchar(100); declare @author varchar(100); declare @image varchar(100);set @name=@namePar;set @isbn =@isbnPar; set @author=@authorPar; set @image=@imagePar;INSERT INTO KITAPLAR VALUES(@isbn, @name, DEFAULT, @author, @image,NULL)", connection);
                    command.Parameters.AddWithValue("@isbnPar", txtISBN.Text);
                    command.Parameters.AddWithValue("@namePar", txtName.Text);
                    command.Parameters.AddWithValue("@authorPar", value[1]);
                    command.Parameters.AddWithValue("@imagePar", value[2]);
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    command.ExecuteNonQuery();
                    lblUpload.Style.Value = "Color:Green";
                    lblUpload.Text = "Yükleme İşlemi:Başarılı!";
                    #endregion
                }
                catch (Exception ex)
                {
                    lblUpload.Style.Value = "Color:Red";
                    lblUpload.Text = "Yükleme İşlemi:Bir hata oluştu.Bir nedenden dolayı dosyayı yükleyemedik.<html></br></html> Hata Mesajı: " + ex.Message;
                }
            }
        }

        protected void btnGoster_Click(object sender, EventArgs e)
        {
            goruntule();
            txtISBN.Text = "";
            txtName.Text = "";
            image.ImageUrl = "";
        }


        protected void btnSil_Click(object sender, EventArgs e)
        {
            sil();

        }
        protected void imageEkle_Click(object sender, EventArgs e)
        {
            ekle();

        }
        
        protected void barcode_Click(object sender, EventArgs e)
        {
            k++;
                if (fileUploader.HasFile)
                {
                    try
                    {
                        #region Upload File
                        string filename = Path.GetFileName(fileUploader.FileName);
                        fileUploader.SaveAs(Server.MapPath(@"~/images/") + "barcodeImg" + k + Path.GetExtension(filename));
                        image.ImageUrl = @"~/images/" + "barcodeImg" + k + Path.GetExtension(filename);
                        #endregion
                        #region Barcode Reader
                        Bitmap bitmap = new Bitmap(Server.MapPath(image.ImageUrl));
                        BarcodeResult Result = IronBarCode.BarcodeReader.QuicklyReadOneBarcode(bitmap);
                        txtISBN.Text = Result.Text;
                        #endregion
                        lblUpload.Style.Value = "Color:Gray";
                        lblUpload.Text = "ISBN textbox kısmı dolduruldu silme işlemi yapmak için sil butonuna basınız.";
                    }
                    catch (Exception ex)
                    {
                        lblUpload.Style.Value = "Color:Red";
                        lblUpload.Text = "Bir hata oluştu.<html></br></html> Hata Mesajı: " + ex.Message;

                    }
                }
            else if (!fileUploader.HasFile)
            {
                lblUpload.Style.Value = "Color:Red";
                lblUpload.Text = "Dosya Seçmediniz.";
            }
           


        }

        protected void back_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx");
        }

        protected void btnTime_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand command = new SqlCommand("declare @time int;set @time = @timePar;UPDATE KITAPSURE SET KITAP_SURE = datediff(DAY, @time, KITAP_SURE)", connection);
                command.Parameters.AddWithValue("@timePar", txtTime.Text);
                command.ExecuteNonQuery();
                lblUpload.Style.Value = "Color:Green";
                lblUpload.Text = "Zamanı İleri Alma İşlemi Başarılı";
                goruntule();
            }
            catch (Exception ex)
            {
                lblUpload.Style.Value = "Color:Red";
                lblUpload.Text = "Bir Hata Oluştu.<html></br></html> Hata Mesajı:"+ex.Message;
            }
        }
    }
}