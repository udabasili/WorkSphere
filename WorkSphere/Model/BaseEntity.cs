using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkSphere.Model
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedAt
        {
            get
            {
                return new DateTime();
            }
        }

        public DateTime UpdatedAt
        {
            get
            {
                return new DateTime();
            }
        }
    }
}
