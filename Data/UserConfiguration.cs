// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

namespace Data
{

    // User
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.24.0.0")]
    public class UserConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<User>
    {
        public UserConfiguration()
            : this("dbo")
        {
        }

        public UserConfiguration(string schema)
        {
            ToTable("User", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.PasswordHash).HasColumnName(@"PasswordHash").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.PasswordSalt).HasColumnName(@"PasswordSalt").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.UserType).HasColumnName(@"UserType").IsRequired().HasColumnType("tinyint");
            Property(x => x.FirstName).HasColumnName(@"FirstName").IsRequired().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.LastName).HasColumnName(@"LastName").IsRequired().HasColumnType("nvarchar").HasMaxLength(64);
            Property(x => x.EmailAddress).HasColumnName(@"EmailAddress").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Status).HasColumnName(@"Status").IsRequired().HasColumnType("tinyint");
            Property(x => x.PasswordExpire).HasColumnName(@"PasswordExpire").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.AccessFailedCount).HasColumnName(@"AccessFailedCount").IsRequired().HasColumnType("int");
            Property(x => x.LockoutEnd).HasColumnName(@"LockoutEnd").IsOptional().HasColumnType("datetimeoffset");
        }
    }

}
// </auto-generated>
