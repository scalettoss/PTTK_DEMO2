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
    
    public partial class THONG_TIN_BAN_HANG
    {
        public string MA_MAT_HANG { get; set; }
        public string MA_HOA_DON { get; set; }
        public Nullable<int> SO_LUONG_BAN_HANG { get; set; }
    
        public virtual HOA_DON HOA_DON { get; set; }
        public virtual MAT_HANG MAT_HANG { get; set; }
    }
}
