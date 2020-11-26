namespace UygulamaOdevi2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SUBMISSIONS")]
    public partial class SUBMISSION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUBMISSION()
        {
            SUBMISSIONS1 = new HashSet<SUBMISSION>();
        }

        public int? AuthenticationID { get; set; }

        [StringLength(20)]
        public string ConfID { get; set; }

        public int SubmissionID { get; set; }

        public int? prevSubmissionID { get; set; }

        public virtual CONFERENCE CONFERENCE { get; set; }

        public virtual USER USER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUBMISSION> SUBMISSIONS1 { get; set; }

        public virtual SUBMISSION SUBMISSION1 { get; set; }
    }
}
