<%@ Page Title="Dynamic List Of Assemblies on Windows Azure" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="GACViewerRole._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div style="margin: 0px 0px 10px 0px">
        Dynamic List Of Assemblies on Windows Azure OS 2.*
    </div>
    <asp:GridView runat="server" OnInit="gridView_Init" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="Full Name">
                <ItemTemplate>
                    <%# ((System.Reflection.AssemblyName)Eval("Name")).FullName %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
