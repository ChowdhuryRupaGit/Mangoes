﻿namespace Mangoes.Services.AuthAPI.Model.DTO
{
    public class LoginRequestDTO
    { 
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
    }
}
