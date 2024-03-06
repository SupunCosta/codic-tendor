using System;

namespace UPrinceV4.Web.UserException;

public class EmptyListException : Exception
{
    public EmptyListException(string message) : base(message)
    {
    }
}