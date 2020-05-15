<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KullaniciPanel.aspx.cs" Inherits="Proje.KullaniciPanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="/style/cssKullanici.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td colspan="3">
                        <asp:Button runat="server" ID="back" CssClass="button back" Text="Geri" OnClick="back_Click"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <th colspan="3" class="th">Kitap Arama</th>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="txtISBNS" CssClass="textbox" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNameS" CssClass="textbox" />
                    </td>
                    <td>
                        <asp:Button runat="server" Text="Ara" ID="btnSearch" OnClick="btnSearch_Click" CssClass="button"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" CssClass="GridView">
                            <Columns>
                                <asp:BoundField DataField="ISBN" HeaderText="ISBN" SortExpression="ISBN" />
                                <asp:BoundField DataField="KITAP_ADI" HeaderText="Kitap Adı" SortExpression="KITAP_ADI" />
                                <asp:BoundField DataField="YAZAR" HeaderText="Yazarın Adı" SortExpression="YAZAR" />
                                <asp:ImageField DataImageUrlField="KITAP_FOTO" HeaderText="Kitap Kapak Fotoğrafı" SortExpression="KITAP_FOTO" ControlStyle-Height="60px">
                                </asp:ImageField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KITAPConnectionString %>" SelectCommand="SELECT [ISBN], [KITAP_ADI], [YAZAR], [KITAP_FOTO] FROM [KITAPLAR]"></asp:SqlDataSource>

                    </td>
                </tr>
                <tr>
                    <th class="th" colspan="3">Kitap Alma</th>
                </tr>
                <tr>
                    <td>
                        <asp:Image runat="server" ID="image1" Width="200px" /></td>
                    <td>
                        <asp:FileUpload runat="server" CssClass="button" ID="fileUploader1" />

                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnAl" Text="Kitap Al" OnClick="btnAl_Click" CssClass="button" />
                    </td>
                </tr>
                <tr>
                    <th class="th" colspan="3">Kitap Verme</th>
                </tr>
                <tr>
                    <td>
                        <asp:Image runat="server" ID="image2" Width="200px" /></td>
                    <td>
                        <asp:FileUpload runat="server" CssClass="button" ID="fileUploader2" />
                    </td>
                    <td>
                        <asp:Button Text="Kitap Ver" ID="btnVer" runat="server" OnClick="btnVer_Click" CssClass="button" /></td>
                </tr>
                <tr><th colspan="3" class="th">Üzerimdeki Kitaplar</th></tr>
                <tr>
                    <td colspan="3">

                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2" CssClass="GridView">
                            <Columns>
                                <asp:BoundField DataField="ISBN" HeaderText="ISBN" SortExpression="ISBN" />
                                <asp:BoundField DataField="KITAP_ADI" HeaderText="Kitap Adı" SortExpression="KITAP_ADI" />
                                <asp:BoundField DataField="YAZAR" HeaderText="Yazarın Adı" SortExpression="YAZAR" />
                                <asp:ImageField DataImageUrlField="KITAP_FOTO" SortExpression="KITAP_FOTO" HeaderText="Kitap Kapak Fotoğrafı" ControlStyle-Height="60px">
                                </asp:ImageField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:KITAPConnectionString %>" SelectCommand="SELECT [ISBN], [KITAP_ADI], [YAZAR], [KITAP_FOTO] FROM [KITAPLAR]"></asp:SqlDataSource>

                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
