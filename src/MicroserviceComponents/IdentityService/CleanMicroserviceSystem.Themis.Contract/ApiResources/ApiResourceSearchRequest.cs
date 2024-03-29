﻿using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.ApiResources;

public class ApiResourceSearchRequest : ContractBase
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public bool? Enabled { get; set; }

    public int? Start { get; set; }

    public int? Count { get; set; }
}
