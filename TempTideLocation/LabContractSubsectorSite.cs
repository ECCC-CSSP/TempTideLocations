//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsFormsApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class LabContractSubsectorSite
    {
        public int LabContractSubsectorSiteID { get; set; }
        public int LabContractSubsectorID { get; set; }
        public int MWQMSiteTVItemID { get; set; }
        public System.DateTime LastUpdateDate_UTC { get; set; }
        public int LastUpdateContactTVItemID { get; set; }
    
        public virtual LabContractSubsector LabContractSubsector { get; set; }
        public virtual TVItem TVItem { get; set; }
    }
}
