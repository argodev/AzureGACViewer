using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Net.Mail;

namespace GACViewerRole
{
    public class EmailHandler
    {
        public static void EmailError(HttpContext httpContext, Exception exception)
        {
            EmailError(httpContext, exception, null);
        }

        public static void EmailError(HttpContext httpContext, Exception exception, Attachment file)
        {
            StringBuilder errorMessage = new StringBuilder();

            if (httpContext != null)
            {
                if (!String.IsNullOrEmpty(httpContext.Request.UserHostAddress))
                {
                    errorMessage.Append("CLIENT IP ADDRESS: ");
                    errorMessage.Append(httpContext.Request.UserHostAddress);
                    errorMessage.Append("\r\n");
                }

                if (httpContext.Request.UrlReferrer != null)
                {
                    errorMessage.Append("REFERRER: ");
                    errorMessage.Append(httpContext.Request.UrlReferrer.ToString());
                    errorMessage.Append("\r\n");
                }

                errorMessage.Append("FORM: ");
                errorMessage.Append(httpContext.Request.Form.ToString());
                errorMessage.Append("\r\n");

                errorMessage.Append("QUERYSTRING: ");
                errorMessage.Append(httpContext.Request.QueryString.ToString());
                errorMessage.Append("\r\n");

                errorMessage.Append("HOST: ");
                errorMessage.Append(httpContext.Request.ServerVariables["HTTP_HOST"]);
                errorMessage.Append("\r\n");
            }


            errorMessage.Append("MESSAGE: ");
            errorMessage.Append(exception.Message);
            errorMessage.Append("\r\n");
            errorMessage.Append("SOURCE: ");
            errorMessage.Append(exception.Source);
            errorMessage.Append("\r\n");
            errorMessage.Append("TARGETSITE: ");
            errorMessage.Append(exception.TargetSite);
            errorMessage.Append("\r\n");
            errorMessage.Append("STACKTRACE:\r\n");
            errorMessage.Append(exception.StackTrace);
            errorMessage.Append("\r\n");

            #region Email


            MailAddress toMailAddress = new MailAddress("data@sciencecloud.us");
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(toMailAddress);
            mailMessage.From = new MailAddress("data@sciencecloud.us");
            mailMessage.Subject = "Azure GAC Viewer Exception";
            mailMessage.Body = errorMessage.ToString();
            mailMessage.IsBodyHtml = false;

            if (file != null)
                mailMessage.Attachments.Add(file);

            Exception lastException;
            foreach (String smtpServer in DnsMx.GetMXRecords(toMailAddress.Host))
            {
                SmtpClient smtpClient = new SmtpClient(smtpServer);
                try
                {
                    smtpClient.Send(mailMessage);
                    lastException = null;
                    break;
                }
                catch (Exception thrownException)
                {
                    lastException = thrownException;
                    continue;
                }

                if (lastException != null)
                    throw (lastException);
            }


            #endregion
        }

    }
}