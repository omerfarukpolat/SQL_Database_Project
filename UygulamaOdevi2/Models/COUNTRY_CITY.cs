namespace UygulamaOdevi2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class COUNTRY_CITY
    {
        [StringLength(3)]
        public string Country_Code { get; set; }

        [Key]
        public int CityID { get; set; }

        [StringLength(100)]
        public string City_Name { get; set; }

        public virtual COUNTRY COUNTRY { get; set; }
    }
}
