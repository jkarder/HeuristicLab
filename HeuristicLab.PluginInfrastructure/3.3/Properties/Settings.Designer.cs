﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HeuristicLab.PluginInfrastructure.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("anonymous")]
        public string UpdateLocationUserName {
            get {
                return ((string)(this["UpdateLocationUserName"]));
            }
            set {
                this["UpdateLocationUserName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("GetTheLab!")]
        public string UpdateLocationPassword {
            get {
                return ((string)(this["UpdateLocationPassword"]));
            }
            set {
                this["UpdateLocationPassword"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowPluginUploadControls {
            get {
                return ((bool)(this["ShowPluginUploadControls"]));
            }
            set {
                this["ShowPluginUploadControls"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://services.heuristiclab.com/Deployment-3.3/UpdateService.svc")]
        public string UpdateLocation {
            get {
                return ((string)(this["UpdateLocation"]));
            }
            set {
                this["UpdateLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://services.heuristiclab.com/Deployment-3.3/AdminService.svc")]
        public string UpdateLocationAdministrationAddress {
            get {
                return ((string)(this["UpdateLocationAdministrationAddress"]));
            }
            set {
                this["UpdateLocationAdministrationAddress"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point StarterFormLocation {
            get {
                return ((global::System.Drawing.Point)(this["StarterFormLocation"]));
            }
            set {
                this["StarterFormLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Size StarterFormSize {
            get {
                return ((global::System.Drawing.Size)(this["StarterFormSize"]));
            }
            set {
                this["StarterFormSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Normal")]
        public global::System.Windows.Forms.FormWindowState StarterFormWindowState {
            get {
                return ((global::System.Windows.Forms.FormWindowState)(this["StarterFormWindowState"]));
            }
            set {
                this["StarterFormWindowState"] = value;
            }
        }
    }
}
