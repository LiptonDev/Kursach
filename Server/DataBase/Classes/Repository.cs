using ISTraining_Part.Core;
using MySql.Data.MySqlClient;
using Server.DataBase.Interfaces;
using Server.Properties;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Server.DataBase.Classes
{
    /// <summary>
    /// Репозиторий данных.
    /// </summary>
    class Repository : IRepository
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        string connectionString = null;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Repository()
        {
            connectionString = $"server={Settings.Default.mysqlHost};port={Settings.Default.mysqlPort};userid={Settings.Default.mysqlUser};pwd={Settings.Default.mysqlPwd};database={Settings.Default.mysqlDb};Convert Zero Datetime=True";
        }

        /// <summary>
        /// Запрос к базе.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">Запрос.</param>
        /// <param name="defaultValue">Значение при ошибке запроса.</param>
        /// <param name="callerName">Метод, вызывающий запрос.</param>
        /// <returns></returns>
        public async Task<KursachResponse<T>> QueryAsync<T>(Func<IDbConnection, Task<T>> func, T defaultValue = default, [CallerMemberName] string callerName = null)
        {

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var res = await func(connection);

                    return new KursachResponse<T>(KursachResponseCode.Ok, res);
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Ошибка запроса к базе, caller: {callerName}", ex);
                    return new KursachResponse<T>(KursachResponseCode.DbError, defaultValue);
                }
            }
        }
    }
}
