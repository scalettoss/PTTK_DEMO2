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
    
    public partial class HOA_DON
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOA_DON()
        {
            this.THONG_TIN_BAN_HANG = new HashSet<THONG_TIN_BAN_HANG>();
        }
    
        public string MA_HOA_DON { get; set; }
        public string MA_CUA_HANG { get; set; }
        public Nullable<System.DateTime> NGAY_GIAO_DICH { get; set; }
    
        public virtual CUA_HANG CUA_HANG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THONG_TIN_BAN_HANG> THONG_TIN_BAN_HANG { get; set; }
    }
}
