using System;
using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace SwissTransport.App.Helper
{
    public static class MailUtility
    {
        /// <summary>
        /// This extension-method saves the provided MailMessage in a file to the disk.
        /// The File can later be opened in the default mailprogram.
        /// </summary>
        /// <param name="message">The MailMessage which should be openend</param>
        /// <param name="filename">The filename which the written mail-file should have</param>
        /// <param name="addUnsentHeader">If the mail has the status "New Message"</param>
        /// <remarks>Source: https://stackoverflow.com/questions/20328598/open-default-mail-client-along-with-a-attachment</remarks>
        public static void Save(this MailMessage message, string filename, bool addUnsentHeader = true)
        {
            using (var filestream = File.Open(filename, FileMode.Create))
            {
                if (addUnsentHeader)
                {
                    var binaryWriter = new BinaryWriter(filestream);

                        //Write the Unsent header to the file so the mail client knows this mail must be presented in "New message" mode
                        binaryWriter.Write(System.Text.Encoding.UTF8.GetBytes("X-Unsent: 1" + Environment.NewLine));
                }

                var assembly = typeof(SmtpClient).Assembly;
                var mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");

                // Get reflection info for MailWriter contructor
                var mailWriterContructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                    null, new[] {typeof(Stream)}, null);

                // Construct MailWriter object with our FileStream
                var mailWriter = mailWriterContructor.Invoke(new object[] {filestream});

                // Get reflection info for Send() method on MailMessage
                var sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);

                sendMethod.Invoke(message, BindingFlags.Instance | BindingFlags.NonPublic, null,
                    new object[] {mailWriter, true, true}, null);

                // Finally get reflection info for Close() method on our MailWriter
                var closeMethod = mailWriter.GetType()
                    .GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);

                // Call close method
                closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { },
                    null);
            }
        }
    }
}