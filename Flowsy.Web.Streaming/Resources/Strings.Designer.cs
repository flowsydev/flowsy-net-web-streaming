﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Flowsy.Web.Streaming.Resources {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Flowsy.Web.Streaming.Resources.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string InvalidFormField {
            get {
                return ResourceManager.GetString("InvalidFormField", resourceCulture);
            }
        }
        
        internal static string InvalidFileType {
            get {
                return ResourceManager.GetString("InvalidFileType", resourceCulture);
            }
        }
        
        internal static string NoBoundarySetForMultipartRequest {
            get {
                return ResourceManager.GetString("NoBoundarySetForMultipartRequest", resourceCulture);
            }
        }
        
        internal static string NoNameWasSpecifiedForFileField {
            get {
                return ResourceManager.GetString("NoNameWasSpecifiedForFileField", resourceCulture);
            }
        }
        
        internal static string FileChunkIsEmpty {
            get {
                return ResourceManager.GetString("FileChunkIsEmpty", resourceCulture);
            }
        }
        
        internal static string ChunkDirectoryNotFoundStartUploadFromTheBeginning {
            get {
                return ResourceManager.GetString("ChunkDirectoryNotFoundStartUploadFromTheBeginning", resourceCulture);
            }
        }
    }
}
