namespace UygulamaOdevi2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CONFERENCE_TAGS
    {
        [StringLength(20)]
        public string ConfID { get; set; }

        [Key]
        [StringLength(100)]
        public string Tag { get; set; }

        public virtual CONFERENCE CONFERENCE { get; set; }
    }
}
