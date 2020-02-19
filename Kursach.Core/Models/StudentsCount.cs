using Newtonsoft.Json;

namespace ISTraining_Part.Core.Models
{
    /// <summary>
    /// Кол-во студентов в группе.
    /// </summary>
    public class StudentsCount
    {
        /// <summary>
        /// Общее кол-во студентов.
        /// </summary>
        [JsonProperty("t")]
        public int Total { get; }

        /// <summary>
        /// Кол-во студентов в академ. отпуске.
        /// </summary>
        [JsonProperty("sab")]
        public int OnSabbatical { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsCount(int total, int onSabbatical)
        {
            Total = total;
            OnSabbatical = onSabbatical;
        }
    }
}
