namespace WebApiEmailService
{
    /// <summary>
    /// Class encapsulates configuration data is taken from the appsettings.json and put to service for injecting.
    /// </summary>
    public class EmailConfiguration
    {
            /// <summary>
            /// Field From for email.
            /// </summary>
            public string From { get; set; }
            /// <summary>
            /// SMTP server of your email provider.
            /// </summary>
            public string SmtpServer { get; set; }
            /// <summary>
            /// Port of SMTP server.
            /// </summary>    
            public int Port { get; set; }
            /// <summary>
            /// Your login.
            /// </summary>   
            public string UserName { get; set; }
            /// <summary>
            /// Your password.
            /// </summary>   
            public string Password { get; set; }

    }
}
