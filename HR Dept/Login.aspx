<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HR_Dept.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     
      <div class ="login-container">
  <section class="login-card">
      <h1>HR DEPARTMENT LOGIN</h1>
      <table>
          <tr>
              <td>
                  <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
              </td>
              <td>
                  <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox"></asp:TextBox>
              </td>
          </tr>
          <tr>
              <td>
                  <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
              </td>
              <td>
                 <asp:TextBox ID="txtPassword" runat="server" Textmode="Password" CssClass="textbox"></asp:TextBox>
                  
              </td>
          </tr>
      </table>
  
   <asp:Button ID="btnConfirm" runat="server" Text="LOGIN" CssClass="btnConfirm" OnClick="btnConfirm_Click" />
  <asp:Button ID="btnCancel" runat="server" Text="CLEAR" CssClass="btnCancel" OnClick="btnCancel_Click" />
  <asp:Label ID="lblFeedback" runat="server" Text =""></asp:Label>
   </section>
          </div>
</asp:Content>
