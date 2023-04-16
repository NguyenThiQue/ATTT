<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HamBamWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        html {
            background-image: url("https://ptithcm.edu.vn/wp-content/uploads/2021/08/1.jpg");
            background-repeat: no-repeat;
            background-attachment: fixed;
            background-size: 100% 100%;
            height: 100vh;
                 
               
        }
        body {
            
        }
        #form1 {
           display: flex;
           justify-content: center;
           align-items: center;
          
           margin: 146px 400px 146px 400px;
           background-color: #6da7c2;
           border-radius:6px;
           box-shadow: 10px 10px #96b1b7;
        }
        #txtUsername, #txtPassword{
            padding: 12px ;
            width: 100%;
            border-radius: 2px;
            outline: none;
            border: none;
            margin-bottom:12px;
        }
        div {
          padding:12px;
        }
        #lblUsername, #lblPassword {
            color: #fff;
        }
        #btnLogin, #btnRegister {
            border: none;
            border-radius: 2px;
            padding: 12px;
        }
        #btnRegister {
            float:right;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div >
            <asp:Label ID ="lblUsername" runat="server" Text ="Username" />
            <asp:TextBox ID ="txtUsername" runat="server" />
            <br />
            <asp:Label ID ="lblPassword" runat="server" Text ="Password" />
            <asp:TextBox ID ="txtPassword" runat="server" TextMode ="Password" />
            <br />
            <asp:Button ID ="btnLogin" runat="server" Text ="Login" OnClick="btnLogin_Click" /> &nbsp;
            <asp:Button ID ="btnRegister" runat="server" Text ="Register" OnClick="btnRegister_Click" /> 
            <br />
            <asp:Label ID ="lblResult" ForeColor="Red" runat="server"/>
        </div>
    </form>
</body>
</html>
