<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UploadJob.aspx.cs" Inherits="HR_Dept.UploadJob" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section id="form">
    <div class="form-container">

        <h3>Post a New Job</h3>

        <table>
            <tr>
                <td><asp:Label ID="lblJobTitle" runat="server" Text="Job Title:" /></td>
                <td><asp:TextBox ID="txtJobTitle" runat="server" CssClass="textbox" /></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblDept" runat="server" Text="Department:" /></td>
                <td><asp:TextBox ID="txtDept" runat="server" CssClass="textbox" /></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblEducationLevel" runat="server" Text="Education Level:" /></td>
                <td>
                    <asp:DropDownList ID="ddlEducationLevel" runat="server" CssClass="textbox">
                        <asp:ListItem Value="" Text="-- Select Level --" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblEducationField" runat="server" Text="Field of Study:" /></td>
                <td><asp:TextBox ID="txtEducationField" runat="server" CssClass="textbox" Placeholder="e.g. Computer Science" /></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblExperienceKeywords" runat="server" Text="Experience keywords" /></td>
<td><asp:TextBox ID="txtExperienceKeywords" runat="server" CssClass="textbox" placeholder="e.g. accountant, bookkeeper" />
                </td>
            </tr>
           <tr>
    <td valign="top">
        <asp:Label ID="lblSkills" runat="server" Text="Required Skills:" />
    </td>
    <td>

        <asp:DropDownList ID="ddlSkills" runat="server" CssClass="textbox" Width="220px" />

        <asp:Button ID="btnAddSkill"
            runat="server"
            Text="Add"
            OnClick="btnAddSkill_Click" CssClass="btnConfirm" />

        <br /><br />

        <asp:ListBox ID="lstSelectedSkills"
            runat="server"
            Width="220px"
            Height="120px" />

        <br />

        <asp:Button ID="btnRemoveSkill"
            runat="server"
            Text="Remove Selected"
            OnClick="btnRemoveSkill_Click" CssClass="btnConfirm"/>

        <hr />

        <asp:TextBox ID="txtNewSkill"
            runat="server"
            Placeholder="New Skill" />

        <asp:Button ID="btnNewSkill"
            runat="server"
            Text="Add New Skill"
            OnClick="btnNewSkill_Click" CssClass="btnConfirm"/>

    </td>
</tr>
           
        </table>
        <div class="button-row">
                            <asp:Button ID="btnCreate" runat="server" Text="Post Job" CssClass="btnConfirm" OnClick="btnCreate_Click" />
<asp:Button ID="btnCancel" runat="server" Text="Back" CssClass="btnCancel" OnClick="btnCancel_Click" />

<asp:Label ID="lblFeedback" runat="server" Text="" />
     </div>
        </div>
    </section>
</asp:Content>
