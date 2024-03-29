﻿using System.Security.Claims;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;

namespace CleanMicroserviceSystem.Themis.Application.Repository;

public interface IOceanusUserRepository : IRepositoryBase<OceanusUser>
{
    Task<PaginatedEnumerable<Claim>> SearchClaims(
        int? userId, string? type, string? value, int? start, int? count);

    Task<PaginatedEnumerable<OceanusUser>> Search(
        int? id, string? userName, string? email, string? phoneNumber, int? start, int? count);

    Task<PaginatedEnumerable<OceanusRole>> SearchRolesAsync(
        int userId, int? id, string? roleName, int? start, int? count);
}
