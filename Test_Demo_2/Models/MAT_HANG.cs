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
    
    public partial class MAT_HANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MAT_HANG()
        {
            this.LUU_TRU = new HashSet<LUU_TRU>();
            this.THONG_TIN_BAN_HANG = new HashSet<THONG_TIN_BAN_HANG>();
        }
    
        public string MA_MAT_HANG { get; set; }
        public string TEN_MAT_HANG { get; set; }
        public Nullable<decimal> DON_GIA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LUU_TRU> LUU_TRU { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THONG_TIN_BAN_HANG> THONG_TIN_BAN_HANG { get; set; }
    }
}