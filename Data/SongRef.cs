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

    // SongRef
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.24.0.0")]
    public class SongRef: BaseEntity
    {
        
        public string Name { get; set; } // Name (length: 256)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<Song> Songs { get; set; } // Song.FK_Song_SongRef

        public SongRef()
        {
            Songs = new System.Collections.Generic.List<Song>();
        }
    }

}
// </auto-generated>
