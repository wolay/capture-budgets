//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BudgetCapture.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Property
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> BudgetYearID { get; set; }
        public Nullable<decimal> ValuedAt { get; set; }
    
        public virtual BudgetYear BudgetYear { get; set; }
    }
}
