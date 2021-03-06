﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReqHub
{
    public delegate IApiClient ApiClientResolver(string clientName);

    public static class IServiceCollectionExtensions
    {
        // Clients
        public static IServiceCollection AddApiClient(this IServiceCollection services, string baseAddress, string publicKey, string privateKey, string name = "ApiClient")
        {
            // Set up the HTTP client
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netframework-4.8
            // https://visualstudiomagazine.com/blogs/tool-tracker/2019/09/mutliple-httpclients.aspx
            services.AddHttpClient(name, (builder) =>
            {
                builder.BaseAddress = baseAddress != null ? new Uri(baseAddress) : null;
            })
            // https://github.com/aspnet/HttpClientFactory/issues/71
            .AddHttpMessageHandler(() => new ReqHubClientHttpMessageHandler(publicKey, privateKey));

            // Add the API client
            services.TryAddTransient<IApiClient>((serviceProvider) =>
            {
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                return new ApiClient(httpClientFactory, name);
            });

            // Add a resolver delegate in case the user is using multiple services
            // https://stackoverflow.com/questions/39174989/how-to-register-multiple-implementations-of-the-same-interface-in-asp-net-core
            services.TryAddTransient<ApiClientResolver>((serviceProvider) => (clientName) =>
            {
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                return new ApiClient(httpClientFactory, clientName);
            });

            return services;
        }

        // Merchants
        public static IServiceCollection AddReqHub(this IServiceCollection services, string publicKey, string privateKey, string baseAddress = "https://api.reqhub.io", string name = "ReqHub")
        {
            // Set up the HTTP client
            services.AddHttpClient(name, (builder) =>
            {
                builder.BaseAddress = new Uri(baseAddress);
            })
            .AddHttpMessageHandler(() => new ReqHubMerchantHttpMessageHandler(publicKey, privateKey));

            // Add the API client
            services.AddSingleton<IMerchantClient, MerchantClient>((serviceProvider) =>
            {
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                return new MerchantClient(httpClientFactory, name);
            });

            return services;
        }
    }
}
