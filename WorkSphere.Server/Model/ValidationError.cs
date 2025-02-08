using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TastyTreats.Types;

namespace TastyTreats.Model.Entities
{
    public class ValidationError
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }

        public ValidationError() { }

        public ValidationError(string desc, ErrorType errorType)
        {
            Description = desc;
            ErrorType = errorType;
        }
    }
}
