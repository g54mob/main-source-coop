using System.Collections.Generic;
using System.Linq;

namespace EvilAnalytics.Shared.Common
{
	public class ApiResponse
	{
		public bool Success { get; set; }

		public string? Message { get; set; }

		public List<string>? Errors { get; set; }

		public static ApiResponse Ok(string? message = null)
		{
			return new ApiResponse
			{
				Success = true,
				Message = message
			};
		}

		public static ApiResponse Error(string message)
		{
			return new ApiResponse
			{
				Success = false,
				Message = message
			};
		}

		public static ApiResponse Error(IEnumerable<string> errors)
		{
			return new ApiResponse
			{
				Success = false,
				Errors = errors.ToList()
			};
		}
	}
	public class ApiResponse<T> : ApiResponse
	{
		public T? Data { get; set; }

		public static ApiResponse<T> Ok(T data, string? message = null)
		{
			return new ApiResponse<T>
			{
				Success = true,
				Data = data,
				Message = message
			};
		}

		public new static ApiResponse<T> Error(string message)
		{
			return new ApiResponse<T>
			{
				Success = false,
				Message = message
			};
		}
	}
}
