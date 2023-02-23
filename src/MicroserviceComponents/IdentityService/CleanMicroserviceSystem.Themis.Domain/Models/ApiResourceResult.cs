﻿using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Domain.Models
{
    public class ApiResourceResult
    {
        public ApiResource? ApiResource { get; set; }

        public bool Succeeded { get => string.IsNullOrEmpty(Error); }

        public string? Error { get; set; }
    }
}