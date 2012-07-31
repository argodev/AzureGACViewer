<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Upload.aspx.cs"
    Inherits="GACViewerRole.Upload" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div style="margin: 0px 0px 10px 0px">
        <p>Upload your Visual Studio Project File for the worker or web role and this page
        will compare it against the assemblies on Windows Azure looking for incorrect references.
        The C# version of this file has the extension <strong>.csproj</strong>. and the Visual Basic .NET
        file has the extension <strong>.vbproj</strong>.</p>
        <asp:FileUpload Width="600px" ID="FileUpload1" runat="server" /><br />
        <asp:Button ID="uploadButton" runat="server" OnClick="uploadButton_Click" Text="Upload" />
    </div>
    <hr />
    <asp:Panel Visible="false" ID="resultsPanel" runat="server">
        <div style="margin: 10px 0px 10px 0px">
            <div style="margin: 0px 0px 5px 0px">
                This is the list of assemblies reference in the uploaded project that are not present
                on Windows Azure:
            </div>
            <asp:GridView Width="100%" ShowHeaderWhenEmpty="true" ID="GridView1" runat="server"
                AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Name" ItemStyle-Width="100%" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("FullName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Copy Local" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# (Eval("Private") == null ? "Unknown" : Eval("Private").ToString()) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    All Assemblies In The Project Exist On Windows Azure.</EmptyDataTemplate>
            </asp:GridView>
            <div style="margin: 5px 0px 0px 0px">
                These assemblies must be marked as Copy Local = true, in Visual Studio. Double check
                all the ones marked as Unknown.
            </div>
        </div>
        <hr />
        <div style="margin: 10px 0px 10px 0px">
            <div style="margin: 0px 0px 5px 0px">
                This is the list of assemblies reference in the uploaded project:</div>
            <asp:GridView Width="100%" ID="GridView2" runat="server" AutoGenerateColumns="false"
                OnRowDataBound="GridView2_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Name" ItemStyle-Width="100%" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("FullName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Copy Local" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# (Eval("Private") == null ? "Unknown" : Eval("Private").ToString()) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Panel runat="server" ID="gridView2InstructionPanel" Visible="false">
                <div style="margin: 5px 0px 0px 0px">
                    These assemblies highlighted are being bundled in your Windows Azure package and
                    upload even though they are already on Windows Azure. You can reduce your package
                    size and your upload time if you set them Copy Local = false in Visual Studio.
                </div>
            </asp:Panel>
        </div>
    </asp:Panel>
    <asp:Panel ID="errorPanel" runat="server">
        <div style="color: Red">
            <asp:Literal runat="server" ID="errorMessage" />
        </div>
    </asp:Panel>
</asp:Content>
