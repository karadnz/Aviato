//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace flightMVC.Resources.Views.Home {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    ///   This class was generated by MSBuild using the GenerateResource task.
    ///   To add or remove a member, edit your .resx file then rerun MSBuild.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Build.Tasks.StronglyTypedResourceBuilder", "15.1.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Flights_en_US {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Flights_en_US() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("flightMVC.Resources.Views.Home.Flights.en-US", typeof(Flights_en_US).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Aircraft:.
        /// </summary>
        internal static string Aircraft {
            get {
                return ResourceManager.GetString("Aircraft", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Arrival:.
        /// </summary>
        internal static string Arrival {
            get {
                return ResourceManager.GetString("Arrival", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Available Seats:.
        /// </summary>
        internal static string Available_Seats {
            get {
                return ResourceManager.GetString("Available Seats", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Book this flight.
        /// </summary>
        internal static string Book {
            get {
                return ResourceManager.GetString("Book", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Departure:.
        /// </summary>
        internal static string Departure {
            get {
                return ResourceManager.GetString("Departure", resourceCulture);
            }
        }
    }
}