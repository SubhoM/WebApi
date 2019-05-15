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
    using Jcr.Data;

    // UserSecurityAttribute
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.30.0.0")]
    public partial class UserSecurityAttributeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UserSecurityAttribute>
    {
        public UserSecurityAttributeConfiguration()
            : this("dbo")
        {
        }

        public UserSecurityAttributeConfiguration(string schema)
        {
            ToTable("UserSecurityAttribute", schema);
            HasKey(x => new { x.UserId, x.AttributeTypeId });

            Property(x => x.UserId).HasColumnName(@"UserID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.AttributeTypeId).HasColumnName(@"AttributeTypeID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.AttributeValue).HasColumnName(@"AttributeValue").HasColumnType("nvarchar").IsRequired().HasMaxLength(250);
            Property(x => x.AttributeActivationDate).HasColumnName(@"AttributeActivationDate").HasColumnType("datetime").IsOptional();
            Property(x => x.AttributeExpirationDate).HasColumnName(@"AttributeExpirationDate").HasColumnType("datetime").IsOptional();
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime").IsOptional();
            Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsOptional();

            // Foreign keys
            HasRequired(a => a.User).WithMany(b => b.UserSecurityAttributes).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_UserSecurityAttribute_User
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
