using System.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace Market.Exceptions;

public class AdminException : Exception
{
    public int StatusCode { get; }

    public AdminException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}