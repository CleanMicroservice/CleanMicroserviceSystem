﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

namespace CleanMicroserviceSystem.Themis.Domain.Configuration
{
    public class ApiResource : AuditableEntity<int>
    {
        [Required]
        public string Name { get; set; }

        [DefaultValue(true)]
        public bool Enabled { get; set; }

        public string? Description { get; set; }

        public virtual IEnumerable<ApiScope>? ApiScopes { get; set; }
    }
}
