<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AdminBoard.aspx.cs" Inherits="HR_Dept.AdminBoard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section id="form1"> 
        <div class="jobs-header">
            <h3>&#128203; Available Job Listings</h3>
             <asp:Button ID="btnCreate" runat="server" Text="Post New Job" OnClick="btnUpload_Click" CssClass="btnConfirm"/>
        </div>

        <asp:Repeater ID="rptJobs" runat="server">
            <HeaderTemplate>
                <div class="cards-grid">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="card" onclick="location.href='UpdateJob.aspx?job_id=<%# Eval("job_id") %>'">
                    <div class="job-title"><%# Eval("job_title") %></div>
                    <div class="department"><%# Eval("department") %></div>
                    <div class="education-label">Education Required:</div>
                    <div class="education">
                        <%# Eval("level_name") %>
                        <%# !string.IsNullOrEmpty(Eval("education_keywords").ToString()) ? " in " + Eval("education_keywords") : "" %>

                    </div>
                    <div class="skills-label">Required Skills:</div>
                    <div class="skills"><%# Eval("skills_display") %></div>
                </div>
            </ItemTemplate>

            <FooterTemplate>
                
                </div>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Label ID="lblNoJobs" runat="server" CssClass="no-jobs" Visible="false" Text="No jobs available at the moment." />

       
    </section>
</asp:Content>
