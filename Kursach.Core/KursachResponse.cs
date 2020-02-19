namespace ISTraining_Part.Core
{
    /// <summary>
    /// Ответ от сервера.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TArg"></typeparam>
    public class KursachResponse<T, TArg> : KursachResponse<T>
    {
        /// <summary>
        /// Аргумент.
        /// </summary>
        public TArg Arg { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public KursachResponse(KursachResponseCode code, TArg arg, T response) : base(code, response)
        {
            Arg = arg;
        }
    }

    /// <summary>
    /// Ответ от сервера.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KursachResponse<T>
    {
        /// <summary>
        /// Код.
        /// </summary>
        public KursachResponseCode Code { get; }

        /// <summary>
        /// Ответ.
        /// </summary>
        public T Response { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public KursachResponse(KursachResponseCode code, T response)
        {
            Code = code;
            Response = response;
        }

        public static implicit operator bool(KursachResponse<T> response) => response.Code == KursachResponseCode.Ok;
        public static implicit operator string(KursachResponse<T> response) => response.ToString();

        public override string ToString()
        {
            switch (Code)
            {
                case KursachResponseCode.Ok:
                    return "Если вы видите это сообщение, значит что-то пошло не так...";

                case KursachResponseCode.ServerError:
                    return "Ошибка сервера";

                case KursachResponseCode.DbError:
                    return "Ошибка базы данных";

                default:
                    return "Unhandled error";
            }
        }
    }
}
