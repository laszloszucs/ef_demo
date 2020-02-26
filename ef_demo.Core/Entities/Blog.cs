using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ef_demo.Core.Entities
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        //public int BlogId { get; set; }

        [Required]
        [MaxLength(200)]
        [ConcurrencyCheck] // Optimista konkurenciakezelés: nem zároljuk az adatot, mert bízunk abban, hogy nem lesz ütközés. Ha mégis előfordul, hogy két felhasználó egyszerre ugyanazt az adatot módosítja, akkor a második módosítás adatbázisba írása előtt figyelmeztetjük a felhasználót, hogy már elavult adatot módosít.
        public string Url { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }
        
        public IEnumerable<Post> Posts { get; set; } // Navigation Property
        /*
            A navigation property-k más entityket tartalmaznak,
            melyek ehhez az entitánshoz kapcsolódnak.
            Külső kulcs hivatkozás alapján kapcsolódik.
         */

        [Timestamp]
        public byte[] Timestamp { get; set; }

        #region Not work
        
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Postgre-vel nem ment
        //public DateTime Inserted { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)] // Postgre-vel nem ment
        //public DateTime LastUpdated { get; set; }

        #endregion
    }
}
