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
    
    public partial class LabContract
    {
        public LabContract()
        {
            this.LabContractSubsectors = new HashSet<LabContractSubsector>();
        }
    
        public int LabContractID { get; set; }
        public string ConfigFileName { get; set; }
        public string ForGroupName { get; set; }
        public int CreatorContactTVItemID { get; set; }
        public int Year { get; set; }
        public string SecretCode { get; set; }
        public string ErrorText { get; set; }
        public System.DateTime LastUpdateDate_UTC { get; set; }
        public int LastUpdateContactTVItemID { get; set; }
    
        public virtual TVItem TVItem { get; set; }
        public virtual ICollection<LabContractSubsector> LabContractSubsectors { get; set; }
    }
}
