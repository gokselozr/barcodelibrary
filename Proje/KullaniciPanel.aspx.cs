using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using IronBarCode;
using HtmlAgilityPack;
using System.Drawing;

namespace Proje
{
    public partial class KullaniciPanel : System.Web.UI.Page
    {
        public static int i = 0;
        public static string userName = "";
        SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=KITAP;Integrated Security=True;Pooling=False");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                userName = Session["UserName"].ToString();
            }
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            kitaplarım();
        }
        protected void goruntule()
        {
            txtISBNS.Text.Replace(" ", "");
            SqlCommand command = new SqlCommand("SELECT ISBN,KITAP_ADI,YAZAR,KITAP_FOTO FROM KITAPLAR WHERE KITAP_ADI LIKE '%'+@name+'%' AND ISBN LIKE '%'+@isbn+'%'", connection);
            command.Parameters.AddWithValue("@isbn", txtISBNS.Text);
            command.Parameters.AddWithValue("@name", txtNameS.Text);
            command.ExecuteNonQuery();
            System.Data.DataTable dataTable = new System.Data.DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataTable);
            GridView1.DataSourceID = null;
            GridView1.DataSource = dataTable;
            GridView1.DataBind();

            SqlCommand command2 = new SqlCommand("SELECT ISBN,KITAP_ADI,YAZAR,KITAP_FOTO FROM KITAPLAR WHERE KITAP_SAHIP='"+userName.ToString()+"'", connection);
            command2.ExecuteNonQuery();
            System.Data.DataTable dataTable2 = new System.Data.DataTable();
            SqlDataAdapter dataAdapter2 = new SqlDataAdapter(command2);
            dataAdapter2.Fill(dataTable2);
            GridView2.DataSourceID = null;
            GridView2.DataSource = dataTable2;
            GridView2.DataBind();
        }
        protected void al()
        {
            try
            {
                i++;
                string filename = Path.GetFileName(fileUploader1.FileName);
                fileUploader1.SaveAs(Server.MapPath(@"~/images/") + "image" + i + Path.GetExtension(filename));
                image1.ImageUrl = @"~/images/image" + i + Path.GetExtension(filename);
                Bitmap bitmap = new Bitmap(Server.MapPath(image1.ImageUrl));
                BarcodeResult Result = IronBarCode.BarcodeReader.QuicklyReadOneBarcode(bitmap);
                SqlCommand date = new SqlCommand("SELECT KITAP_SURE FROM KITAPSURE WHERE KULLANICI_ID='" + userName + "'", connection);
                date.ExecuteNonQuery();
                System.Data.DataTable dateTable = new System.Data.DataTable();
                SqlDataAdapter dateAdapter = new SqlDataAdapter(date);
                dateAdapter.Fill(dateTable);
                string onay = "";
                string yeni = "";
                DateTime[] dateRow = new DateTime[3];
                if (dateTable.Rows.Count==0)
                {
                    yeni = "yeni";
                }
                for (int t = 0; t < dateTable.Rows.Count; t++)
                {
                    dateRow[t] = Convert.ToDateTime(dateTable.Rows[t].ItemArray[0]);
                    int a = (DateTime.Now.Subtract(dateRow[t])).Days;
                    if (7 > a)
                    {
                        onay = "verildi";
                    }
                    else
                    {
                        onay = "verilmedi";
                        break;
                    }


                }

                if (onay == "verildi" || yeni=="yeni")
                {

                    SqlCommand kullanim = new SqlCommand("SELECT KULLANIMDA FROM KITAPLAR WHERE ISBN='" + Result.Text + "'", connection);
                    kullanim.ExecuteNonQuery();
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(kullanim);
                    dataAdapter.Fill(dataTable);
                    string kullanimda = dataTable.Rows[0].ItemArray[0].ToString();
                    kullanimda = kullanimda.Replace(" ", "");
                    if (kullanimda == "0")
                    {
                        SqlCommand kota = new SqlCommand("SELECT KITAP_KOTA FROM KULLANICILAR WHERE KULLANICI_ADI='" + userName + "'", connection);
                        kota.ExecuteNonQuery();
                        System.Data.DataTable kotaTable = new System.Data.DataTable();
                        SqlDataAdapter kotaAdapter = new SqlDataAdapter(kota);
                        kotaAdapter.Fill(kotaTable);
                        string kotaSTR = kotaTable.Rows[0].ItemArray[0].ToString();
                        kotaSTR = kotaSTR.Replace(" ", "");
                        int kotaN = Convert.ToInt32(kotaSTR.ToString());
                        if (kotaN < 3)
                        {
                            SqlCommand command = new SqlCommand("INSERT INTO KITAPSURE VALUES ('" + userName + "','" + Result.Text + "',GETDATE())", connection);
                            if (connection.State == System.Data.ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            command.ExecuteNonQuery();
                            SqlCommand kullanimc = new SqlCommand("UPDATE KITAPLAR SET KULLANIMDA='1',KITAP_SAHIP='"+userName+"' WHERE ISBN='" + Result.Text + "'", connection);
                            kullanimc.ExecuteNonQuery();
                            SqlCommand kotaK = new SqlCommand("UPDATE KULLANICILAR SET KITAP_KOTA+=1 WHERE KULLANICI_ADI='" + userName + "'", connection);
                            kotaK.ExecuteNonQuery();
                            Response.Write("<script>alert('Kitap Başarıyla Alınmıştır.')</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('3 Kitaptan Fazlasını Alamazsınız!')</script>");
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Kitap Kullanımda')</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('Tarihi Geçmiş Kitaplarınız Bulunmaktadır.')</script>");
                }

                goruntule();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Hata" + ex.Message + "')</script>");
            }

        }
        protected void ver()
        {
            try
            {
                i++;
                string filename = Path.GetFileName(fileUploader2.FileName);
                fileUploader2.SaveAs(Server.MapPath(@"~/images/") + "image" + i + Path.GetExtension(filename));
                image2.ImageUrl = @"~/images/image" + i + Path.GetExtension(filename);
                Bitmap bitmap = new Bitmap(Server.MapPath(image2.ImageUrl));
                BarcodeResult Result = IronBarCode.BarcodeReader.QuicklyReadOneBarcode(bitmap);
                SqlCommand kullanim = new SqlCommand("SELECT KULLANIMDA FROM KITAPLAR WHERE ISBN='" + Result.Text + "'", connection);
                kullanim.ExecuteNonQuery();
                System.Data.DataTable dataTable = new System.Data.DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(kullanim);
                dataAdapter.Fill(dataTable);
                string kullanimda = dataTable.Rows[0].ItemArray[0].ToString();
                kullanimda = kullanimda.Replace(" ", "");
                if (kullanimda == "1")
                {
                    SqlCommand kota = new SqlCommand("SELECT KITAP_KOTA FROM KULLANICILAR WHERE KULLANICI_ADI='" + userName + "'", connection);
                    kota.ExecuteNonQuery();
                    System.Data.DataTable kotaTable = new System.Data.DataTable();
                    SqlDataAdapter kotaAdapter = new SqlDataAdapter(kota);
                    kotaAdapter.Fill(kotaTable);
                    string kotaSTR = kotaTable.Rows[0].ItemArray[0].ToString();
                    kotaSTR = kotaSTR.Replace(" ", "");
                    int kotaN = Convert.ToInt32(kotaSTR.ToString());
                    if (kotaN > 0)
                    {
                        SqlCommand command = new SqlCommand("DELETE FROM KITAPSURE WHERE KULLANICI_ID='" + userName + "' AND KITAP_ID='" + Result.Text + "'", connection);
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        command.ExecuteNonQuery();
                        SqlCommand kullanimc = new SqlCommand("UPDATE KITAPLAR SET KULLANIMDA='0',KITAP_SAHIP=NULL WHERE ISBN='" + Result.Text + "'", connection);
                        kullanimc.ExecuteNonQuery();
                        SqlCommand kotaK = new SqlCommand("UPDATE KULLANICILAR SET KITAP_KOTA-=1 WHERE KULLANICI_ADI='" + userName + "'", connection);
                        kotaK.ExecuteNonQuery();
                        Response.Write("<script>alert('Kitap Başarıyla Verilmiştir.')</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Elinizde Hiç Kitap Yok. Olmayan Kitapları Veremezsiniz.')</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('Kitap Zaten Verilmiş.')</script>");
                }
                goruntule();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Hata" + ex.Message + "')</script>");
            }
        }
        protected void kitaplarım()
        {
            SqlCommand command = new SqlCommand("SELECT ISBN,KITAP_ADI,YAZAR,KITAP_FOTO FROM KITAPLAR WHERE KITAP_SAHIP='"+userName+"'", connection);
            command.Parameters.AddWithValue("@isbn", txtISBNS.Text);
            command.Parameters.AddWithValue("@name", txtNameS.Text);
            command.ExecuteNonQuery();
            System.Data.DataTable dataTable = new System.Data.DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataTable);
            GridView2.DataSourceID = null;
            GridView2.DataSource = dataTable;
            GridView2.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            goruntule();
            image1.ImageUrl = null;
            image2.ImageUrl = null;
        }

        protected void back_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnAl_Click(object sender, EventArgs e)
        {
            al();
            goruntule();
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            ver();
            goruntule();
        }
    }
}