using Kursach.Core;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Kursach.Client
{
    static class InvokerHelper
    {
        public static Task TryInvokeAsync(this IHubProxy hubProxy, [CallerMemberName]string method = null)
        {
            try
            {
                return hubProxy.Invoke(method);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Task.CompletedTask;
            }
        }

        public static Task<KursachResponse<T>> TryInvokeAsync<T>(this IHubProxy hubProxy, [CallerMemberName]string method = null, T defaultValue = default, params object[] args)
        {
            try
            {
                return hubProxy.Invoke<KursachResponse<T>>(method, args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Task.FromResult(new KursachResponse<T>(KursachResponseCode.ServerError, defaultValue));
            }
        }

        public static Task<KursachResponse<T, TArg>> TryInvokeAsync<T, TArg>(this IHubProxy hubProxy, [CallerMemberName]string method = null, T defaultValue = default, TArg argDefault = default, params object[] args)
        {
            try
            {
                return hubProxy.Invoke<KursachResponse<T, TArg>>(method, args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Task.FromResult(new KursachResponse<T, TArg>(KursachResponseCode.ServerError, argDefault, defaultValue));
            }
        }
    }
}
