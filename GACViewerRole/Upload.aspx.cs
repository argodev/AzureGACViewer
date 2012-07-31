using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.GAC;
using GACViewerRole.Model;
using System.Reflection;
using System.Net.Mail;
using System.IO;

namespace GACViewerRole
{
    public partial class Upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void uploadButton_Click(object sender, EventArgs e)
        {
            gridView2InstructionPanel.Visible = false;
            resultsPanel.Visible = false;
            errorPanel.Visible = false;
            errorMessage.Text = String.Empty;

            if (FileUpload1.HasFile)
            {
                var projectReferences = (from projectReference in ProjectReferences()
                                         where !projectReference.Available
                                         orderby projectReference.FullName
                                         select projectReference);

                GridView2.DataSource = ProjectReferences();
                GridView2.DataBind();

                GridView1.DataSource = projectReferences;
                GridView1.DataBind();

                if (ProjectReferences().Count() > 0)
                {
                    resultsPanel.Visible = true;
                }
                else
                {
                    errorMessage.Text += "<p>No assembly references where found in your upload project file.  Are you sure this is a valid Visual Studio Project file?";
                    errorPanel.Visible = true;

                    // WWB: Send Error Email With Project File
                    try
                    {
                        FileUpload1.PostedFile.InputStream.Position = 0;
                        Attachment attachment = new Attachment(FileUpload1.PostedFile.InputStream, FileUpload1.FileName);

                        EmailHandler.EmailError(Context, new Exception("Unable To Parse File?"), attachment);
                        errorMessage.Text += " An error message with your project file has been successfully sent to the web site administrator for debugging.";
                    }
                    catch
                    {
                    }

                    errorMessage.Text += "</p>";
                }
            }
            else
            {
                resultsPanel.Visible = false;
            }
        }

        private List<ProjectReference> _projectReferences;

        private IEnumerable<ProjectReference> ProjectReferences()
        {
            if (_projectReferences == null)
            {
                _projectReferences = new List<ProjectReference>();
                foreach (ProjectReference projectReference in InternalProjectReferences())
                    _projectReferences.Add(projectReference);
            }

            return (_projectReferences);
        }

        private IEnumerable<ProjectReference> InternalProjectReferences()
        {
            if (FileUpload1.HasFile)
            {
                using (XmlReader xmlReader = XmlReader.Create(FileUpload1.PostedFile.InputStream))
                {
                    XDocument xDocument;

                    // WWB: Try To Parse Uploaded File
                    try
                    {
                        xDocument = XDocument.Load(xmlReader);
                    }
                    catch (XmlException xmlException)
                    {
                        errorPanel.Visible = true;
                        errorMessage.Text += "<p>";
                        String fileName = String.Empty;

                        errorMessage.Text += String.Format(" Unable to Parse XML Document, Is This a Visual Studio Project File? We are expecting a project file with the extension .vbproj or .csproj.");

                        try
                        {
                            errorMessage.Text += String.Format(" The file name of the upload file was: {0}.", Path.GetFileName(FileUpload1.PostedFile.FileName));
                        }
                        catch
                        {
                        }

                        // WWB: Send Error Email With Project File
                        try
                        {
                            FileUpload1.PostedFile.InputStream.Position = 0;
                            Attachment attachment = new Attachment(FileUpload1.PostedFile.InputStream, FileUpload1.FileName);

                            EmailHandler.EmailError(Context, xmlException, attachment);
                            errorMessage.Text += " An error message with your project file has been successfully sent to the web site administrator for debugging.";
                        }
                        catch
                        {
                        }

                        errorMessage.Text += "</p>";

                        yield break;
                    }

                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlReader.NameTable);
                    xmlNamespaceManager.AddNamespace("msbuild", "http://schemas.microsoft.com/developer/msbuild/2003");

                    foreach (XElement xElement in xDocument.XPathSelectElements("/msbuild:Project/msbuild:ItemGroup/msbuild:Reference", xmlNamespaceManager))
                    {
                        XAttribute includeAttribute = (from xAttribute in xElement.Attributes() where xAttribute.Name == "Include" select xAttribute).SingleOrDefault();
                        XElement privateElement = xElement.XPathSelectElement("msbuild:Private", xmlNamespaceManager);
                        Boolean? privateFlag = (privateElement == null ? (Boolean?)null : (Boolean?)Boolean.Parse(privateElement.Value));

                        Boolean available = false;
                        try
                        {
                            Assembly assembly = Assembly.LoadWithPartialName(includeAttribute.Value);
                            if (assembly != null)
                                available = true;
                        }
                        catch
                        {
                        }

                        yield return (new ProjectReference(includeAttribute.Value, privateFlag, available));
                    }

                    foreach (XElement xElement in xDocument.XPathSelectElements("/msbuild:Project/msbuild:ItemGroup/msbuild:ProjectReference", xmlNamespaceManager))
                    {
                        XElement nameElement = xElement.XPathSelectElement("msbuild:Name", xmlNamespaceManager);
                        XElement privateElement = xElement.XPathSelectElement("msbuild:Private", xmlNamespaceManager);
                        Boolean? privateFlag = (privateElement == null ? (Boolean?)null : (Boolean?)Boolean.Parse(privateElement.Value));

                        Boolean available = false;
                        try
                        {
                            Assembly assembly = Assembly.LoadWithPartialName(nameElement.Value);
                            if (assembly != null)
                                available = true;
                        }
                        catch
                        {
                        }

                        yield return (new ProjectReference(nameElement.Value, privateFlag, available));
                    }
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    ProjectReference projectReference = (ProjectReference)e.Row.DataItem;
                    if (projectReference.Private == false)
                    {
                        e.Row.Style["background-color"] = "#E47297";
                        e.Row.Style["color"] = "black";
                    }
                    if (projectReference.Private == null)
                    {
                        e.Row.Style["background-color"] = "#FFFFE0";
                        e.Row.Style["color"] = "black";
                    }
                    break;
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    ProjectReference projectReference = (ProjectReference)e.Row.DataItem;
                    if ((projectReference.Private == true) && (projectReference.Available))
                    {
                        e.Row.Style["background-color"] = "#FFFFE0";
                        e.Row.Style["color"] = "black";
                        gridView2InstructionPanel.Visible = true;
                    }
                    break;
            }
        }
    }
}