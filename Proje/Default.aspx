<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Proje.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Anasayfa</title>
    <script>

        document.getElementById('<%=txtYonSif.ClientID%>').addEventListener("keyup", function (evente) {
            evente.preventDefault();
            if (evente.keyCode === 13) {
                document.getElementById('<%=btnPanelYonetici.ClientID%>').click();
            }
        });
        document.getElementById('<%=txtKulSif.ClientID%>').addEventListener("keyup", function (event) {
            event.preventDefault();
            if (event.keyCode === 13) {
                document.getElementById('<%=btnPanelKullanici.ClientID%>').click();
            }
        });
        function load() {
            document.getElementById('<%=txtKulAdi.ClientID%>').style.display = "none";
            document.getElementById('<%=txtYonAdi.ClientID%>').style.display = "none";
            document.getElementById('<%=txtKulSif.ClientID%>').style.display = "none";
            document.getElementById('<%=txtYonSif.ClientID%>').style.display = "none";
            document.getElementById('<%=btnPanelKullanici.ClientID%>').style.display = "none";
            document.getElementById('<%=btnPanelYonetici.ClientID%>').style.display = "none";
        }
        function Yonetici() {
            document.getElementById('<%=txtKulAdi.ClientID%>').style.display = "none";
            document.getElementById('<%=txtKulSif.ClientID%>').style.display = "none";
            document.getElementById('<%=txtYonSif.ClientID%>').style.display = "block";
            document.getElementById('<%=txtYonAdi.ClientID%>').style.display = "block";
            document.getElementById('<%=btnPanelKullanici.ClientID%>').style.display = "none";
            document.getElementById('<%=btnPanelYonetici.ClientID%>').style.display = "block";
            bt

        }
        function Kullanici() {
            document.getElementById('<%=txtKulAdi.ClientID%>').style.display = "block";
            document.getElementById('<%=txtKulSif.ClientID%>').style.display = "block";
            document.getElementById('<%=txtYonSif.ClientID%>').style.display = "none";
            document.getElementById('<%=txtYonAdi.ClientID%>').style.display = "none";
            document.getElementById('<%=btnPanelKullanici.ClientID%>').style.display = "block";
            document.getElementById('<%=btnPanelYonetici.ClientID%>').style.display = "none";
        }

    </script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>

</head>
<body onload="load()">
    <link rel="stylesheet" href="style/cssDefault.css" type="text/css" runat="server" />

    <form id="form1" runat="server">
        <div id="main">
            <div id="header">
                <p id="head">Abdulkadir VURGUN</p>
            </div>
            <table id="table" border="0">
                <tr>
                    <th colspan="2">Giriş Yöntemi Seçiniz</th>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPanelKullanici" runat="server" onclick="Kullanici()">Kullanıcı Panel</asp:Label></td>
                    <td>
                        <asp:Label ID="lblPanelYonetici" runat="server" onclick="Yonetici()">Yönetici Panel</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="txtKulAdi" CssClass="textbox" /></td>
                    <td>
                        <asp:TextBox runat="server" ID="txtYonAdi" CssClass="textbox" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="txtKulSif" CssClass="textbox" TextMode="Password" /></td>
                    <td>
                        <asp:TextBox runat="server" ID="txtYonSif" CssClass="textbox" TextMode="Password" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnPanelKullanici" runat="server" Text="Giriş" OnClick="btnPanelKullanici_Click" CssClass="button"></asp:Button>
                    </td>
                    <td>
                        <asp:Button ID="btnPanelYonetici" runat="server" Text="Giriş" OnClick="btnPanelYonetici_Click" CssClass="button"></asp:Button></td>
                </tr>
            </table>
        </div>



    </form>
</body>
</html>
