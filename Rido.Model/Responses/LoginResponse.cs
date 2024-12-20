﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Model.Responses
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public dynamic? User { get; set; }
    }

}

