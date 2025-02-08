using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TastyTreats.Model.Entities;
using TastyTreats.Types;

namespace WorkSphere.Model
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();

        public void AddError(string desc, ErrorType errorType)
        {
            Errors.Add(new ValidationError(desc, errorType));
        }
    }
}
