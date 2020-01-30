namespace Kursach.Models
{
    /// <summary>
    /// Кол-во студентов в группе.
    /// </summary>
    class StudentsCount
    {
        /// <summary>
        /// Общее кол-во студентов.
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Кол-во студентов в академ. отпуске.
        /// </summary>
        public int OnSabbatical { get; }

        /// <summary>
        /// Группа является очной группой.
        /// </summary>
        public bool IsIntramural { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsCount(int total, int onSabbatical, bool isIntramural)
        {
            Total = total;
            OnSabbatical = onSabbatical;
            IsIntramural = isIntramural;
        }
    }
}
