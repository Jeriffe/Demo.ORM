﻿using System;

namespace Demo.Application.Common.Exceptions
{

    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base() { }
    }
}