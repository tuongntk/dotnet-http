﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Booking.Common.HttpClient.Extensions
{
	public static class Extensions
	{
		private const string MediaType = "application/json";

		public static async Task<T> GetAsync<T>(this System.Net.Http.HttpClient client, string uri)
		{
			var response = await client.GetAsync(uri);
			if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				return default(T);
			}
			else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
			{
				throw new HttpRequestException(await response.Content.ReadAsStringAsync());
			}
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsync<T>();
			return result;
		}

		public static Task<T> GetAsync<T>(this System.Net.Http.HttpClient client)
		{
			return client.GetAsync<T>(string.Empty);
		}

		public static Task<T> PostAsync<T>(this System.Net.Http.HttpClient httpClient, object data)
		{
			return httpClient.PostAsync<T>(string.Empty, data);
		}

		public static Task<HttpResponseMessage> PostAsync(this System.Net.Http.HttpClient httpClient, object data)
		{
			return httpClient.PostAsync(string.Empty, data.ToJsonContent());
		}


		public static Task<T> PostAsync<T>(this System.Net.Http.HttpClient httpClient)
		{
			return httpClient.PostAsync<T>(new { });
		}

		public static async Task<T> PostAsync<T>(this System.Net.Http.HttpClient httpClient, string url, object data)
		{
			var responseMessage = await httpClient.PostAsync(url, data);
			responseMessage.EnsureSuccessStatusCode();
			var result = await responseMessage.Content.ReadAsync<T>();
			return result;
		}

		public static Task<HttpResponseMessage> PostAsync(this System.Net.Http.HttpClient httpClient, string url, object data)
		{
			return httpClient.PostAsync(url, data.ToJsonContent());
		}

		public static Task<T> PutAsync<T>(this System.Net.Http.HttpClient httpClient, object data)
		{
			return httpClient.PutAsync<T>(string.Empty, data);
		}

		public static async Task<T> PutAsync<T>(this System.Net.Http.HttpClient httpClient, string url, object data)
		{
			var responseMessage = await httpClient.PutAsync(url, data);
			responseMessage.EnsureSuccessStatusCode();
			var result = await responseMessage.Content.ReadAsync<T>();
			return result;
		}

		public static Task<HttpResponseMessage> PutAsync(this System.Net.Http.HttpClient httpClient, string url, object data)
		{
			return httpClient.PutAsync(url, data.ToJsonContent());
		}

		public static StringContent ToJsonContent(this object model)
		{
			string json = JsonConvert.SerializeObject(model);
			return new StringContent(json, Encoding.UTF8, MediaType);
		}

		public static async Task<T> ReadAsync<T>(this HttpContent httpContent)
		{
			string json = await httpContent.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(json);
		}

		public static string ToQueryString(this Dictionary<string, string> parameters)
		{
			if (parameters == null || parameters.Count == 0) return string.Empty;

			return $"?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}";
		}
	}
}
