﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RiseEngine.Core.Storage.NamedBinaryTag.Exceptions
{
    public class NbtQueryException : Exception
    {
        public NbtQueryException(string message) : base(message) { }
    }
}