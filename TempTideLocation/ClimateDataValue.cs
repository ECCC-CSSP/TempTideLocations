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
    
    public partial class ClimateDataValue
    {
        public int ClimateDataValueID { get; set; }
        public int ClimateSiteID { get; set; }
        public System.DateTime DateTime_Local { get; set; }
        public bool Keep { get; set; }
        public int ClimateDataType { get; set; }
        public int StorageDataType { get; set; }
        public Nullable<double> Temperature_C { get; set; }
        public Nullable<double> RelativeHumidity_Perc { get; set; }
        public Nullable<double> WindSpeed_km_h { get; set; }
        public Nullable<double> WindDirection_0North { get; set; }
        public Nullable<double> AtmPressure_kpa { get; set; }
        public Nullable<double> Snow_cm { get; set; }
        public Nullable<double> Rainfall_mm { get; set; }
        public Nullable<double> TotalPrecip_mm_cm { get; set; }
        public Nullable<double> DewPointTemp_C { get; set; }
        public Nullable<double> MaxTemp_C { get; set; }
        public Nullable<double> MinTemp_C { get; set; }
        public Nullable<double> HeatDegDays_C { get; set; }
        public Nullable<double> CoolDegDays_C { get; set; }
        public Nullable<double> SnowOnGround_cm { get; set; }
        public Nullable<double> DirMaxGust_0North { get; set; }
        public Nullable<double> SpdMaxGust_kmh { get; set; }
        public System.DateTime LastUpdateDate_UTC { get; set; }
        public int LastUpdateContactTVItemID { get; set; }
    
        public virtual ClimateSite ClimateSite { get; set; }
    }
}