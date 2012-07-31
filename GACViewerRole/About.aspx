<%@ Page Title="About GAC Viewer" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="GACViewerRole.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        About
    </h2>
    <p>
        If your Windows Azure role relies on any assembly that is not part of the .NET Framework
        3.5 or the Windows Azure managed library (<a href="/">listed here</a>), you must
        explicitly include that assembly in the service package.
    </p>
    <p>
        Before you build and package your service, verify that: The Copy Local property
        is set to True for each referenced assembly in your project that is not <a href="/">
            listed here</a> as part of the Windows Azure SDK or the .NET Framework 3.5,
        if you are using Visual Studio.
    </p>
    <p>
        <img src="Content/CopyTrue.jpg" alt="Copy Local True" />
    </p>
    <p>
        If you are not using Visual Studio, you must specify the locations for referenced
        assemblies when you call CSPack. See CSPack Command-Line Tool for more information.
    </p>
</asp:Content>
