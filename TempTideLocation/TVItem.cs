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
    
    public partial class TVItem
    {
        public TVItem()
        {
            this.Addresses = new HashSet<Address>();
            this.Addresses1 = new HashSet<Address>();
            this.Addresses2 = new HashSet<Address>();
            this.Addresses3 = new HashSet<Address>();
            this.AppTasks = new HashSet<AppTask>();
            this.AppTasks1 = new HashSet<AppTask>();
            this.BoxModels = new HashSet<BoxModel>();
            this.ClimateSites = new HashSet<ClimateSite>();
            this.Contacts = new HashSet<Contact>();
            this.Emails = new HashSet<Email>();
            this.HydrometricSites = new HashSet<HydrometricSite>();
            this.Infrastructures = new HashSet<Infrastructure>();
            this.LabContracts = new HashSet<LabContract>();
            this.LabContractSubsectors = new HashSet<LabContractSubsector>();
            this.LabContractSubsectorSites = new HashSet<LabContractSubsectorSite>();
            this.LabSheets = new HashSet<LabSheet>();
            this.MapInfos = new HashSet<MapInfo>();
            this.MikeBoundaryConditions = new HashSet<MikeBoundaryCondition>();
            this.MikeScenarios = new HashSet<MikeScenario>();
            this.MikeSources = new HashSet<MikeSource>();
            this.MWQMRuns = new HashSet<MWQMRun>();
            this.MWQMRuns1 = new HashSet<MWQMRun>();
            this.MWQMRuns2 = new HashSet<MWQMRun>();
            this.MWQMRuns3 = new HashSet<MWQMRun>();
            this.MWQMSamples = new HashSet<MWQMSample>();
            this.MWQMSamples1 = new HashSet<MWQMSample>();
            this.MWQMSites = new HashSet<MWQMSite>();
            this.MWQMSubsectors = new HashSet<MWQMSubsector>();
            this.PolSourceObservations = new HashSet<PolSourceObservation>();
            this.PolSourceObservations1 = new HashSet<PolSourceObservation>();
            this.PolSourceSites = new HashSet<PolSourceSite>();
            this.RatingCurves = new HashSet<RatingCurve>();
            this.Spills = new HashSet<Spill>();
            this.Spills1 = new HashSet<Spill>();
            this.Tels = new HashSet<Tel>();
            this.TideDataValues = new HashSet<TideDataValue>();
            this.TideSites = new HashSet<TideSite>();
            this.TVFiles = new HashSet<TVFile>();
            this.TVItemLanguages = new HashSet<TVItemLanguage>();
            this.TVItemLinks = new HashSet<TVItemLink>();
            this.TVItemLinks1 = new HashSet<TVItemLink>();
            this.TVItems1 = new HashSet<TVItem>();
            this.TVItemStats = new HashSet<TVItemStat>();
            this.TVItemUserAuthorizations = new HashSet<TVItemUserAuthorization>();
            this.TVItemUserAuthorizations1 = new HashSet<TVItemUserAuthorization>();
            this.TVTypeUserAuthorizations = new HashSet<TVTypeUserAuthorization>();
            this.UseOfSites = new HashSet<UseOfSite>();
            this.UseOfSites1 = new HashSet<UseOfSite>();
            this.VPScenarios = new HashSet<VPScenario>();
        }
    
        public int TVItemID { get; set; }
        public int TVLevel { get; set; }
        public string TVPath { get; set; }
        public int TVType { get; set; }
        public int ParentID { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime LastUpdateDate_UTC { get; set; }
        public int LastUpdateContactTVItemID { get; set; }
    
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Address> Addresses1 { get; set; }
        public virtual ICollection<Address> Addresses2 { get; set; }
        public virtual ICollection<Address> Addresses3 { get; set; }
        public virtual ICollection<AppTask> AppTasks { get; set; }
        public virtual ICollection<AppTask> AppTasks1 { get; set; }
        public virtual ICollection<BoxModel> BoxModels { get; set; }
        public virtual ICollection<ClimateSite> ClimateSites { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Email> Emails { get; set; }
        public virtual ICollection<HydrometricSite> HydrometricSites { get; set; }
        public virtual ICollection<Infrastructure> Infrastructures { get; set; }
        public virtual ICollection<LabContract> LabContracts { get; set; }
        public virtual ICollection<LabContractSubsector> LabContractSubsectors { get; set; }
        public virtual ICollection<LabContractSubsectorSite> LabContractSubsectorSites { get; set; }
        public virtual ICollection<LabSheet> LabSheets { get; set; }
        public virtual ICollection<MapInfo> MapInfos { get; set; }
        public virtual ICollection<MikeBoundaryCondition> MikeBoundaryConditions { get; set; }
        public virtual ICollection<MikeScenario> MikeScenarios { get; set; }
        public virtual ICollection<MikeSource> MikeSources { get; set; }
        public virtual ICollection<MWQMRun> MWQMRuns { get; set; }
        public virtual ICollection<MWQMRun> MWQMRuns1 { get; set; }
        public virtual ICollection<MWQMRun> MWQMRuns2 { get; set; }
        public virtual ICollection<MWQMRun> MWQMRuns3 { get; set; }
        public virtual ICollection<MWQMSample> MWQMSamples { get; set; }
        public virtual ICollection<MWQMSample> MWQMSamples1 { get; set; }
        public virtual ICollection<MWQMSite> MWQMSites { get; set; }
        public virtual ICollection<MWQMSubsector> MWQMSubsectors { get; set; }
        public virtual ICollection<PolSourceObservation> PolSourceObservations { get; set; }
        public virtual ICollection<PolSourceObservation> PolSourceObservations1 { get; set; }
        public virtual ICollection<PolSourceSite> PolSourceSites { get; set; }
        public virtual ICollection<RatingCurve> RatingCurves { get; set; }
        public virtual ICollection<Spill> Spills { get; set; }
        public virtual ICollection<Spill> Spills1 { get; set; }
        public virtual ICollection<Tel> Tels { get; set; }
        public virtual ICollection<TideDataValue> TideDataValues { get; set; }
        public virtual ICollection<TideSite> TideSites { get; set; }
        public virtual ICollection<TVFile> TVFiles { get; set; }
        public virtual ICollection<TVItemLanguage> TVItemLanguages { get; set; }
        public virtual ICollection<TVItemLink> TVItemLinks { get; set; }
        public virtual ICollection<TVItemLink> TVItemLinks1 { get; set; }
        public virtual ICollection<TVItem> TVItems1 { get; set; }
        public virtual TVItem TVItem1 { get; set; }
        public virtual ICollection<TVItemStat> TVItemStats { get; set; }
        public virtual ICollection<TVItemUserAuthorization> TVItemUserAuthorizations { get; set; }
        public virtual ICollection<TVItemUserAuthorization> TVItemUserAuthorizations1 { get; set; }
        public virtual ICollection<TVTypeUserAuthorization> TVTypeUserAuthorizations { get; set; }
        public virtual ICollection<UseOfSite> UseOfSites { get; set; }
        public virtual ICollection<UseOfSite> UseOfSites1 { get; set; }
        public virtual ICollection<VPScenario> VPScenarios { get; set; }
    }
}
