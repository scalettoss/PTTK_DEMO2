//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test_Demo_2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CUA_HANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CUA_HANG()
        {
            this.HOA_DON = new HashSet<HOA_DON>();
            this.LUU_TRU = new HashSet<LUU_TRU>();
        }
    
        public string MA_CUA_HANG { get; set; }
        public string DIA_CHI { get; set; }
        public string SO_DIEN_THOAI { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOA_DON> HOA_DON { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LUU_TRU> LUU_TRU { get; set; }
    }
}
