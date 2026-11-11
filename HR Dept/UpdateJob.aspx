<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UpdateJob.aspx.cs" Inherits="HR_Dept.UpdateJob" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section id="form">
         <div class="form-container">
        <h3>&#9999;&#65039; Update Job</h3>

        <table>
            <tr>
                <td><asp:Label runat="server" Text="Job Title:" /></td>
                <td><asp:TextBox ID="txtJobTitle" runat="server" CssClass="textbox" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" Text="Department:" /></td>
                <td><asp:TextBox ID="txtDepartment" runat="server" CssClass="textbox" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" Text="Education Level:" /></td>
                <td>
                    <asp:DropDownList ID="ddlEducationLevel" runat="server" CssClass="textbox">
                        <asp:ListItem Value="" Text="-- Select Level --" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td><asp:Label runat="server" Text="Field of Study:" /></td>
                <td><asp:TextBox ID="txtEducationField" runat="server" CssClass="textbox" Placeholder="e.g. Computer Science" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" Text="Experience keywords" /></td>
                    
<td><asp:TextBox ID="txtExperienceKeywords" runat="server" CssClass="textbox" placeholder="e.g. accountant, bookkeeper" />
                </td>
            </tr>
            <tr>
    <td valign="top">
        <asp:Label runat="server" Text="Required Skills:" />
    </td>
    <td>

        <asp:DropDownList ID="ddlSkills" runat="server" CssClass="textbox" Width="220px"/>

        <asp:Button ID="btnAddSkill"
            runat="server"
            Text="Add"
            OnClick="btnAddSkill_Click" cssClass="btnConfirm"/>

        <br /><br />

        <asp:ListBox
            ID="lstSelectedSkills"
            runat="server"
            Width="280px"
            Height="150px"
            SelectionMode="Multiple" />

        <br />

        <asp:Button
            ID="btnRemoveSkill"
            runat="server"
            Text="Remove Selected"
            OnClick="btnRemoveSkill_Click" CssClass="btnConfirm" />

        <hr />

        <asp:TextBox
            ID="txtNewSkill"
            runat="server"
            Placeholder="New Skill" />

        <asp:Button
            ID="btnAddNewSkill"
            runat="server"
            Text="Add New Skill"
            OnClick="btnAddNewSkill_Click" CssClass="btnConfirm" />

    </td>
</tr>
        </table>
             
         <div class="button-row">
        <asp:Button ID="btnSave" runat="server" Text="Save Changes" OnClick="btnSave_Click" CssClass="btnConfirm"/>
        <asp:Button ID="btDelete" runat="server" Text="Delete Job" OnClick="btnDelete_Click"
            OnClientClick="return confirm('Are you sure you want to delete this job?');" CssClass="btnCancel" />
        <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="btnCancel" />

        <asp:Label ID="lblMessage" runat="server" Text="" />
             </div>
             </div>
    </section>
</asp:Content>
       
