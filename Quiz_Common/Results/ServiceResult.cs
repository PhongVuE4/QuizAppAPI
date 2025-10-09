using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Common.Results
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public int Code { get; set; }
        public static ServiceResult Success(object data = null, string message = "Operation successful.", int code = 200)
        {
            return new ServiceResult { IsSuccess = true, Data = data, Message = message, Code = code };
        }
        public static ServiceResult Failure(string message = "Operation failed.", object data = null, int code = 400)
        {
            return new ServiceResult { IsSuccess = false, Data = data, Message = message, Code = code};
        }
    }
    public class ServiceResult<T> : ServiceResult
    {
        public new T Data { get; set; }
        public static ServiceResult<T> Success(T data, string message = "Operation successful.", int code = 200)
        {
            return new ServiceResult<T> { IsSuccess = true, Data = data, Message = message, Code = code };
        }
        public static new ServiceResult<T> Failure(string message = "Operation failed.", T data = default, int code = 400)
        {
            return new ServiceResult<T> { IsSuccess = false, Data = data, Message = message, Code = code };
        }
    }
}
