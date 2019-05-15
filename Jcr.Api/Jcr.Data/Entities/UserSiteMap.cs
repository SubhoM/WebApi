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

    // UserSiteMap
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.30.0.0")]
    public partial class UserSiteMap
    {
        public int UserId { get; set; } // UserId (Primary key)
        public int SiteId { get; set; } // SiteId (Primary key)
        public int RoleId { get; set; } // RoleID
        public bool DefaultSite { get; set; } // DefaultSite
        public bool AmpAccess { get; set; } // AMPAccess
        public bool? TracersAccess { get; set; } // TracersAccess
        public System.DateTime? CreateDate { get; set; } // CreateDate
        public System.DateTime? UpdateDate { get; set; } // UpdateDate

        // Foreign keys

        /// <summary>
        /// Parent Site pointed by [UserSiteMap].([SiteId]) (FK_UserSiteMap_Site)
        /// </summary>
        public virtual Site Site { get; set; } // FK_UserSiteMap_Site
        /// <summary>
        /// Parent User pointed by [UserSiteMap].([UserId]) (FK_UserSiteMap_User)
        /// </summary>
        public virtual User User { get; set; } // FK_UserSiteMap_User

        public UserSiteMap()
        {
            AmpAccess = false;
            TracersAccess = false;
            CreateDate = System.DateTime.Now;
            UpdateDate = System.DateTime.Now;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
