namespace UygulamaOdevi2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONFERENCE")]
    public partial class CONFERENCE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CONFERENCE()
        {
            CONFERENCE_TAGS = new HashSet<CONFERENCE_TAGS>();
            SUBMISSIONS = new HashSet<SUBMISSION>();
        }

        [Key]
        [StringLength(20)]
        public string ConfID { get; set; }

        public DateTime? CreationDateTime { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(19)]
        public string ShortName { get; set; }

        public int? Year { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Submission_Deadline { get; set; }

        public int? CreatorUser { get; set; }

        [StringLength(100)]
        public string WebSite { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONFERENCE_TAGS> CONFERENCE_TAGS { get; set; }

        public virtual USER USER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUBMISSION> SUBMISSIONS { get; set; }
    }
}
