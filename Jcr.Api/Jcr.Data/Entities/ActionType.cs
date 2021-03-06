// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.5
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace Jcr.Data
{

    // ActionType
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.30.0.0")]
    public partial class ActionType
    {
        public int ActionTypeId { get; set; } // ActionTypeID (Primary key)
        public string ActionTypeName { get; set; } // ActionTypeName (length: 50)
        public string ActionTypeDescription { get; set; } // ActionTypeDescription (length: 255)
        public string UpdateBy { get; set; } // UpdateBy (length: 20)
        public System.DateTime UpdateDate { get; set; } // UpdateDate
        public System.DateTime CreateDate { get; set; } // CreateDate
        public int ActionTypeGroupId { get; set; } // ActionTypeGroupID

        public ActionType()
        {
            UpdateDate = System.DateTime.Now;
            CreateDate = System.DateTime.Now;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
