﻿namespace Market.Exceptions;

public class DomainException : Exception
{
    public int StatusCode { get; }

    public DomainException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}