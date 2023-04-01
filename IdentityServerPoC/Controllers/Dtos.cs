﻿using System.ComponentModel.DataAnnotations;

namespace IdentityServerPoC.Controllers;

public record UserDto(Guid Id, string UserName, string Email, DateTime CreatedAt);

public record UpdateUserDto([Required][EmailAddress] string Email);