//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KTPMUD.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class log
    {
        public Nullable<int> id_loai_hinh_tac_dong { get; set; }
        public Nullable<int> id_user { get; set; }
        public System.DateTime time { get; set; }
        public string du_lieu_tac_dong { get; set; }
    
        public virtual loai_hinh_tac_dong loai_hinh_tac_dong { get; set; }
        public virtual user user { get; set; }
    }
}
