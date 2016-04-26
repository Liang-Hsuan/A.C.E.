namespace AshlinCustomerEnquiry.Properties {   
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source = kpjvpp867r.database.windows.net; Initial Catalog = Design_Database;" +
            " Integrated Security = False; User ID = database_admin; Password = AshlinData1; " +
            "Connect Timeout = 60; Encrypt = False; TrustServerCertificate = False;")]
        public string Designcs {
            get {
                return ((string)(this["Designcs"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=kpjvpp867r.database.windows.net;Initial Catalog=ChannelPartner_Databa" +
            "se;Integrated Security=False;User ID=database_admin;Password=AshlinData1;Connect" +
            " Timeout=60;Encrypt=False;TrustServerCertificate=False;")]
        public string ASCMcs {
            get {
                return ((string)(this["ASCMcs"]));
            }
        }
    }
}
