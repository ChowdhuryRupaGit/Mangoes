﻿namespace Mangoes.Services.CouponAPIS.Model.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
