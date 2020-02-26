using Dapper.Contrib.Extensions;

namespace ISTraining_Part.Core.Models
{
    /// <summary>
    /// ID записи в базе.
    /// </summary>
    public interface IId
    {
        /// <summary>
        /// Id.
        /// </summary>
        [Key]
        int Id { get; set; }
    }
}
