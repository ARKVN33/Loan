//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblBankAccount
    {
        public int Id { get; set; }
        public string BankAccountBankName { get; set; }
        public string BankAccountBranchName { get; set; }
        public string BankAccountNum { get; set; }
        public string BankAccountCardNum { get; set; }
        public Nullable<long> BankAccountInitialBalance { get; set; }
        public string BankAccountDescription { get; set; }
    }
}