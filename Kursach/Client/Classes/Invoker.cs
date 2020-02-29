using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{
    /// <summary>
    /// Отправка запросов на сервер.
    /// </summary>
    abstract class Invoker
    {
        /// <summary>
        /// Конфигуратор.
        /// </summary>
        public IHubConfigurator HubConfigurator { get; }

        /// <summary>
        /// Прокси.
        /// </summary>
        public IHubProxy Proxy { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Invoker(IHubConfigurator configurator, string hubName)
        {
            HubConfigurator = configurator;
            Proxy = HubConfigurator.Hub.CreateHubProxy(hubName);
        }

        /// <summary>
        /// Отправить запрос.
        /// </summary>
        /// <returns></returns>
        public Task TryInvokeAsync([CallerMemberName]string method = null, object[] args = null)
        {
            args = args ?? new object[0];

            if (HubConfigurator.Hub.State != ConnectionState.Connected)
                return Task.CompletedTask;

            try
            {
                return Proxy.Invoke(method, args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Отправить запрос.
        /// </summary>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<T>> TryInvokeAsync<T>([CallerMemberName]string method = null, T defaultValue = default, params object[] args)
        {
            if (HubConfigurator.Hub.State != ConnectionState.Connected)
                return Task.FromResult(new ISTrainingPartResponse<T>(ISTrainingPartResponseCode.ServerError, defaultValue));

            try
            {
                return Proxy.Invoke<ISTrainingPartResponse<T>>(method, args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Task.FromResult(new ISTrainingPartResponse<T>(ISTrainingPartResponseCode.ServerError, defaultValue));
            }
        }

        /// <summary>
        /// Отправить запрос.
        /// </summary>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<T, TArg>> TryInvokeAsync<T, TArg>([CallerMemberName]string method = null, T defaultValue = default, TArg argDefault = default, params object[] args)
        {
            if (HubConfigurator.Hub.State != ConnectionState.Connected)
                return Task.FromResult(new ISTrainingPartResponse<T, TArg>(ISTrainingPartResponseCode.ServerError, argDefault, defaultValue));

            try
            {
                return Proxy.Invoke<ISTrainingPartResponse<T, TArg>>(method, args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Task.FromResult(new ISTrainingPartResponse<T, TArg>(ISTrainingPartResponseCode.ServerError, argDefault, defaultValue));
            }
        }
    }
}
