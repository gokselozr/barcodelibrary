using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace Proje
{
    public partial class Default : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=KITAP;Integrated Security=True;Pooling=False");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
        }
        protected void btnPanelYonetici_Click(object sender, EventArgs e)
        {
            try
            {

                SqlCommand command = new SqlCommand("SELECT YONETICI_ADI,YONETICI_SIFRE FROM YONETICI WHERE YONETICI_ADI=@adi and YONETICI_SIFRE=@sifre", connection);
                command.Parameters.AddWithValue("@adi", txtYonAdi.Text);
                command.Parameters.AddWithValue("@sifre", txtYonSif.Text);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                if (table.Rows.Count.ToString() == "1")
                {
                    Response.Redirect("/YoneticiPanel.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Hatalı Giriş.')</script>");
                }
            }
            catch (Exception ex)
            {

                Response.Write("<script>alert('Hata:" + ex.Message + "')</script>");
            }
        }

        protected void btnPanelKullanici_Click(object sender, EventArgs e)
        {
            try
            {
                Session["UserName"] = txtKulAdi.Text;
                SqlCommand command = new SqlCommand("SELECT KULLANICI_ADI,KULLANICI_SIFRE FROM KULLANICILAR WHERE KULLANICI_ADI=@adi and KULLANICI_SIFRE=@sifre", connection);
                command.Parameters.AddWithValue("@adi", txtKulAdi.Text);
                command.Parameters.AddWithValue("@sifre", txtKulSif.Text);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                if (table.Rows.Count.ToString() == "1")
                {
                    Response.Redirect("/KullaniciPanel.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Hatalı Giriş.')</script>");
                }
            }
            catch (Exception ex)
            {

                Response.Write("<script>alert('Hata:" + ex.Message + "')</script>");

            }

        }
    }
}