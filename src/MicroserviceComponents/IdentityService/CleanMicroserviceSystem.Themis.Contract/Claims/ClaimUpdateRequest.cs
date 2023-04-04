﻿using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Claims;

public class ClaimUpdateRequest : ContractBase
{
    public string Type { get; set; }

    public string Value { get; set; }
}