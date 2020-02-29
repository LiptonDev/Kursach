using ISTraining_Part.Core;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Server.DataBase.Interfaces
{
    /// <summary>
    /// Репозиторий данных.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Запрос к базе.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">Запрос.</param>
        /// <param name="defaultValue">Значение при ошибке запроса.</param>
        /// <param name="callerName">Метод, вызывающий запрос.</param>
        /// <returns></returns>
        Task<ISTrainingPartResponse<T>> QueryAsync<T>(Func<IDbConnection, Task<T>> func, T defaultValue = default, [CallerMemberName]string callerName = null);
    }
}
