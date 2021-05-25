﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using ClArc.Sync.Core;

namespace ClArc.Sync.Invoker
{
    internal class UseCaseInvoker : IUseCaseInvoker
    {
        private readonly MethodInfo handleMethod; 
        private readonly MethodInfo handleMethodAsync;
        private readonly IServiceProvider provider;
        private readonly Type usecaseType;

        public UseCaseInvoker(Type usecaseType, Type implementsType, IServiceProvider provider)
        {
            this.usecaseType = usecaseType;
            this.provider = provider;

            handleMethod = implementsType.GetMethod("Handle");
            handleMethodAsync = implementsType.GetMethod("HandleAsync");
        }

        public TResponse Invoke<TResponse>(IInputData<TResponse> inputData)
            where TResponse : IOutputData
        {
            var instance = provider.GetService(usecaseType);

            object responseObject;
            try
            {
                responseObject = handleMethod.Invoke(instance, new object[] { inputData });
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }

            var response = (TResponse)responseObject;

            return response;
        }
        public async Task<TResponse> InvokeAsync<TResponse>(IInputData<TResponse> inputData)
    where TResponse : IOutputData
        {
            var instance = provider.GetService(usecaseType);

            Task<TResponse> responseObject;
            try
            {
                responseObject = (Task<TResponse>)handleMethodAsync.Invoke(instance, new object[] { inputData });
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }

            return await responseObject;
        }
    }
}