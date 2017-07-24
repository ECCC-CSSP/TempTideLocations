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
    
    public partial class AppTask
    {
        public AppTask()
        {
            this.AppTaskLanguages = new HashSet<AppTaskLanguage>();
        }
    
        public int AppTaskID { get; set; }
        public int TVItemID { get; set; }
        public int TVItemID2 { get; set; }
        public int Command { get; set; }
        public int Status { get; set; }
        public int PercentCompleted { get; set; }
        public string Parameters { get; set; }
        public string Language { get; set; }
        public System.DateTime StartDateTime_UTC { get; set; }
        public Nullable<System.DateTime> EndDateTime_UTC { get; set; }
        public Nullable<int> EstimatedLength_second { get; set; }
        public Nullable<int> RemainingTime_second { get; set; }
        public System.DateTime LastUpdateDate_UTC { get; set; }
        public int LastUpdateContactTVItemID { get; set; }
    
        public virtual ICollection<AppTaskLanguage> AppTaskLanguages { get; set; }
        public virtual TVItem TVItem { get; set; }
        public virtual TVItem TVItem1 { get; set; }
    }
}
