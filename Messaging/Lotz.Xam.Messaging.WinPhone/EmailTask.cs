using System;
using System.Collections.Generic;
using Lotz.Xam.Messaging.Abstractions;
using Microsoft.Phone.Tasks;

namespace Lotz.Xam.Messaging
{
    internal class EmailTask : IEmailTask
    {
        public EmailTask()
        {
        }

        #region IEmailTask Members

        public bool CanSendEmail { get { return true; } }

        public void SendEmail(EmailMessageRequest email)
        {
            if (email == null)
                throw new ArgumentNullException("email");

            if (CanSendEmail)
            {
                if (email.IsHtml)
                    throw new PlatformNotSupportedException("Sending HTML email not supported for Windows Phone");

                EmailComposeTask emailComposeTask = new EmailComposeTask
                                                    {
                                                        Subject = email.Subject,
                                                        Body = email.Message,
                                                        To = ToDelimitedAddress(email.Recipients),
                                                        Cc = ToDelimitedAddress(email.RecipientsCc),
                                                        Bcc = ToDelimitedAddress(email.RecipientsBcc)
                                                    };
                emailComposeTask.Show();
            }
        }

        public void SendEmail(string to, string subject, string message)
        {
            SendEmail(new EmailMessageRequest(to, subject, message));
        }

        #endregion

        #region Methods

        private static string ToDelimitedAddress(ICollection<string> collection)
        {
            return collection.Count == 0 ? string.Empty : string.Join(";", collection);
        }

        #endregion
    }
}