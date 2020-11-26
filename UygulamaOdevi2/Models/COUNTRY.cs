namespace UygulamaOdevi2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COUNTRY")]
    public partial class COUNTRY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COUNTRY()
        {
            COUNTRY_CITY = new HashSet<COUNTRY_CITY>();
        }

        [Key]
        [StringLength(3)]
        public string Country_Code { get; set; }

        [StringLength(50)]
        public string Country_Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COUNTRY_CITY> COUNTRY_CITY { get; set; }
    }
}
