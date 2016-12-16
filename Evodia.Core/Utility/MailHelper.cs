using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Web;
using Umbraco.Web;
using System.Reflection;
using System.Text;
using Umbraco.Core.Models;

namespace Evodia.Core.Utility
{
    public class MailHelper
    {
        internal void CreateAndSendNotifications(object model, IPublishedContent formFolder)
        {
            var emailTemplate = LoadEmailTemplate("EmailTemplate");

            if (!string.IsNullOrEmpty(formFolder.GetPropertyValue<string>("internalNotificationAddress")))
            {
                var email = formFolder.GetPropertyValue<string>("internalNotificationAddress");
                var emailBody = GenerateMessageFromModel(model, formFolder);

                var isValidEmail = IsValidEmail(email);

                if (string.IsNullOrEmpty(emailBody) || !isValidEmail) return;

                var finishedEmail = emailTemplate.Replace("{{Content}}", emailBody);
                var mailMessage = CreateInternalMailMessage(finishedEmail, formFolder, email);

                SendMailMessage(mailMessage);
            }

            if (!formFolder.GetPropertyValue<bool>("sendNotification")) return;
            {
                var type = model.GetType();
                var email = (string)type.GetProperty("Email").GetValue(model, null);
                var emailBody = formFolder.GetPropertyValue<IHtmlString>("notificationMessage").ToString();
                var valuesToReplace = CreateValuesToReplace(model);

                emailBody = ReplacePlaceholders(emailBody, valuesToReplace);

                if (string.IsNullOrEmpty(emailBody)) return;

                var finishedEmail = emailTemplate.Replace("{{Content}}", emailBody);
                var mailMessage = CreateExternalMailMessage(finishedEmail, formFolder, email);

                SendMailMessage(mailMessage);
            }
        }

        private MailMessage CreateInternalMailMessage(string emailBody, IPublishedContent formFolder, string contactAddress)
        {
            var fromAddress = formFolder.HasValue("fromAddress")
                ? formFolder.GetPropertyValue<string>("fromAddress")
                : "noreply@" + HttpContext.Current.Request.Url.Host;
            var senderName = formFolder.GetPropertyValue<string>("senderName");
            var emailSubject = formFolder.Name + " has been submitted";
            var emailCcAddresses = formFolder.GetPropertyValue<string>("internalNotificationCc");

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress, senderName),
                Subject = emailSubject,
                Body = emailBody,
                IsBodyHtml = true,
                To =
                {
                    contactAddress
                }
            };

            if (string.IsNullOrEmpty(emailCcAddresses)) return mailMessage;

            foreach (var emailAddress in emailCcAddresses.Split(','))
            {
                if (IsValidEmail(emailAddress))
                {
                    mailMessage.CC.Add(emailAddress);
                }
            }

            return mailMessage;
        }

        private static string GenerateMessageFromModel(object model, IPublishedContent formFolder)
        {
            var emailBody = new StringBuilder();

            var type = model.GetType();
            var properties = type.GetProperties();

            emailBody.Append("<h2>Summary of the form: </h2>");

            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.CanRead)
                {
                    if (propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(int))
                    {
                        if (propertyInfo.GetValue(model, null) != null)
                        {
                            emailBody.Append("<p><strong>" + DisplayNameHelper.GetDisplayName(propertyInfo) + ": </strong>" + propertyInfo.GetValue(model, null) + "</p>");
                        }
                    } else if (propertyInfo.PropertyType == typeof(HttpPostedFileBase))
                    {
                        if (propertyInfo.GetValue(model, null) != null)
                        {
                            var link = "ftp://176.74.18.115/Uploads/";
                            var linkPrefix = "";

                            switch (formFolder.Id)
                            {
                                case Constants.GenericCvFormFolderId:
                                    linkPrefix = "Generic_CV";
                                    break;
                                case Constants.JobCvFormFolderId:
                                    linkPrefix = "Job_CV";
                                    break;
                                case Constants.VacancyFormFolderId:
                                    linkPrefix = "Vacancies";
                                    break;
                            }

                            link = link + linkPrefix;

                            emailBody.Append("<p><strong>Attachment: </strong><a href='" + link + "'>" + link + "</a>" + "</p>");
                        }
                    }
                }
            }

            return emailBody.ToString();
        }

        private static string LoadEmailTemplate(string emailtemplateName)
        {
            string emailTemplate;
            using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Code/" + emailtemplateName + ".html")))
            {
                emailTemplate = reader.ReadToEnd();
            }

            return emailTemplate;
        }

        private static Dictionary<string, string> CreateValuesToReplace(object model)
        {
            var type = model.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var valuesToReplace = new Dictionary<string, string>();

            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.CanRead)
                {
                    if (propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(int))
                    {
                        if (propertyInfo.GetValue(model, null) != null)
                        {
                            valuesToReplace.Add("{{" + propertyInfo.Name + "}}", propertyInfo.GetValue(model, null).ToString());
                        }
                    }
                }
            }

            return valuesToReplace;
        }

        private static string ReplacePlaceholders(string emailBody, Dictionary<string, string> valuesToReplace)
        {
            foreach (var value in valuesToReplace)
            {
                emailBody = emailBody.Replace(value.Key, value.Value);
            }

            return emailBody;
        }

        private MailMessage CreateExternalMailMessage(string emailBody, IPublishedContent formFolder, string contactAddress)
        {
            var fromAddress = formFolder.HasValue("fromAddress")
                ? formFolder.GetPropertyValue<string>("fromAddress")
                : "noreply@" + HttpContext.Current.Request.Url.Host;
            var senderName = formFolder.GetPropertyValue<string>("senderName");
            var emailSubject = formFolder.HasValue("notificationTitle") 
                ? formFolder.GetPropertyValue<string>("notificationTitle") 
                : "Your form has been received";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress, senderName),
                Subject = emailSubject,
                Body = emailBody,
                IsBodyHtml = true,
                To =
                {
                    contactAddress
                }
            };

            return mailMessage;
        }

        private void SendMailMessage(MailMessage mailMessage)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.Send(mailMessage);
                mailMessage.Dispose();
            }
        }

        public bool IsValidEmail(string email)
        {
            bool isValid;

            try
            {
                var addr = new MailAddress(email);
                isValid = addr.Address == email;
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }

        public static class DisplayNameHelper
        {
            public static string GetDisplayName(object obj, string propertyName)
            {
                if (obj == null) return null;

                return GetDisplayName(obj.GetType(), propertyName);

            }

            public static string GetDisplayName(Type type, string propertyName)
            {
                var property = type.GetProperty(propertyName);
                if (property == null) return null;

                return GetDisplayName(property);
            }

            public static string GetDisplayName(PropertyInfo property)
            {
                var attrName = GetAttributeDisplayName(property);
                if (!string.IsNullOrEmpty(attrName))
                    return attrName;

                var metaName = GetMetaDisplayName(property);
                if (!string.IsNullOrEmpty(metaName))
                    return metaName;

                return property.Name.ToString(CultureInfo.InvariantCulture);
            }

            private static string GetAttributeDisplayName(PropertyInfo property)
            {
                var atts = property.GetCustomAttributes(
                    typeof(DisplayNameAttribute), true);
                if (atts.Length == 0)
                    return null;
                var displayNameAttribute = atts[0] as DisplayNameAttribute;

                return displayNameAttribute != null ? displayNameAttribute.DisplayName : null;
            }

            private static string GetMetaDisplayName(PropertyInfo property)
            {
                if (property.DeclaringType != null)
                {
                    var atts = property.DeclaringType.GetCustomAttributes(
                        typeof(MetadataTypeAttribute), true);
                    if (atts.Length == 0)
                        return null;

                    var metaAttr = atts[0] as MetadataTypeAttribute;
                    if (metaAttr != null)
                    {
                        var metaProperty =
                            metaAttr.MetadataClassType.GetProperty(property.Name);
                        return metaProperty == null ? null : GetAttributeDisplayName(metaProperty);
                    }
                }

                return null;
            }
        }
    }
}
