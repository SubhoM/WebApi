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

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.30.0.0")]
    public partial class ApiGetUserByUserLogonIdReturnModel
    {
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.30.0.0")]
        public class ResultSetModel1
        {
            public System.Int32 UserID { get; set; }
            public System.String FirstName { get; set; }
            public System.String LastName { get; set; }
            public System.String MiddleName { get; set; }
            public System.String UserLogonID { get; set; }
            public System.Int16 Status { get; set; }
            public System.DateTime? ExpirationDate { get; set; }
            public System.Int32? CreatedByUserID { get; set; }
            public System.Int32? UpdatedByUserID { get; set; }
            public System.DateTime? UpdateDateByUser { get; set; }
            public System.Int32? UpdatedByGlobalAdminUserID { get; set; }
            public System.DateTime? UpdateDateByGlobalAdmin { get; set; }
            public System.Guid UserGUID { get; set; }
            public System.DateTime? CreateDate { get; set; }
            public System.DateTime? UpdateDate { get; set; }
        }
        public System.Collections.Generic.List<ResultSetModel1> ResultSet1;

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.30.0.0")]
        public class ResultSetModel2
        {
            public System.Int32 UserID { get; set; }
            public System.Int32 AttributeTypeID { get; set; }
            public System.String AttributeValue { get; set; }
            public System.DateTime? AttributeActivationDate { get; set; }
            public System.DateTime? AttributeExpirationDate { get; set; }
            public System.DateTime? CreateDate { get; set; }
            public System.DateTime? UpdateDate { get; set; }
        }
        public System.Collections.Generic.List<ResultSetModel2> ResultSet2;

    }

}
// </auto-generated>
