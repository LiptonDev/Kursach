namespace ISTraining_Part.Core
{
    /// <summary>
    /// Ответ от сервера.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TArg"></typeparam>
    public class ISTrainingPartResponse<T, TArg> : ISTrainingPartResponse<T>
    {
        /// <summary>
        /// Аргумент.
        /// </summary>
        public TArg Arg { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ISTrainingPartResponse(ISTrainingPartResponseCode code, TArg arg, T response) : base(code, response)
        {
            Arg = arg;
        }
    }

    /// <summary>
    /// Ответ от сервера.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ISTrainingPartResponse<T>
    {
        /// <summary>
        /// Код.
        /// </summary>
        public ISTrainingPartResponseCode Code { get; }

        /// <summary>
        /// Ответ.
        /// </summary>
        public T Response { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ISTrainingPartResponse(ISTrainingPartResponseCode code, T response)
        {
            Code = code;
            Response = response;
        }

        public static implicit operator bool(ISTrainingPartResponse<T> response) => response.Code == ISTrainingPartResponseCode.Ok;
        public static implicit operator string(ISTrainingPartResponse<T> response) => response.ToString();

        public override string ToString()
        {
            switch (Code)
            {
                case ISTrainingPartResponseCode.Ok:
                    return "Если вы видите это сообщение, значит что-то пошло не так...";

                case ISTrainingPartResponseCode.ServerError:
                    return "Ошибка сервера";

                case ISTrainingPartResponseCode.DbError:
                    return "Ошибка базы данных";

                default:
                    return "Unhandled error";
            }
        }
    }
}
