﻿using Microvac.Web.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Validation;

namespace App.Models
{
    public class Account : BaseEntity
    {
        public static IDictionary<AccountType, string> RootAccountCodes = new Dictionary<AccountType, string> 
        { 
            {AccountType.INCOME, "A"},
            {AccountType.EXPENSE, "B"}
        };

        [Required(ErrorMessage = "Kode harus diisi")]
        [RegularExpression(@"[a-zA-Z0-9\.]+", ErrorMessage = "Kode harus berupa digit atau titik")]
        //[Index("IX_Code_Type_fkApbdesId", 1, IsUnique = true)]
        public String Code { get; set; }

        [Required(ErrorMessage = "Nama harus diisi")]
        public String Name { get; set; }

        //[Index("IX_Code_Type_fkApbdesId", 2, IsUnique = true)]
        [Index]
        public AccountType Type { get; set; }

        public decimal? Amount { get; set; }

        public string Notes { get; set; }

        //[Index("IX_TargetSource_fkApbdesId", 2, IsUnique = true)]
        //[Index("IX_Code_Type_fkpbdesId", 3, IsUnique = true)]
        [Index]
        [ForeignKey("Apbdes")]
        public long fkApbdesId { get; set; }
        public virtual Apbdes Apbdes { get; set; }


        [ForeignKey("ParentAccount")]
        public long? fkParentAccountId { get; set; }
        public virtual Account ParentAccount { get; set; }



        [ForeignKey("CreatedBy")]
        public string fkCreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public string fkModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }

        public virtual List<Account> ChildAccounts { get; set; }

        public String ParentCode
        {
            get
            {
                if (!string.IsNullOrEmpty(Code))
                    if (!Code.Contains('.'))
                    {
                        if (!Regex.IsMatch(Code, @"[\d\.]+") && RootAccountCodes.ContainsKey(Type))
                            return RootAccountCodes[Type];
                        return null;
                    }
                    else
                        return Code.Substring(0, Code.LastIndexOf('.'));
                return null;
            }
        }

        [NotMapped]
        public decimal TotalRealizationPerAccount
        {
            get
            {
                return new DB().Transactions.Where(e => e.fkAccountId == Id).ToList().Sum(e => e.Amount);
            }
        }

        //[NotMapped]
        //public decimal TotalRealizationPerRootAccount
        //{
        //    get
        //    {

        //        var db = new DB();
        //        var childs = db.Accounts.Where(e => e.fkAPBDesID == this.fkAPBDesID &&
        //            e.fkParentAccountID != null &&
        //            e.ParentAccount.fkParentAccountID == null).SelectMany(e => e.ChildAccounts).ToList();

        //        decimal total = 0;

        //        foreach (var child in childs)
        //        {
        //            IEnumerable<Transaction> transactions = db.Transactions.Where(e => e.fkAccountID == child.ID);

        //            total += transactions.Any()
        //                ? transactions.Select(e => e.Amount).ToList().Sum() : 0;
        //        }
        //        return total;
        //    }
        //}
    }
}