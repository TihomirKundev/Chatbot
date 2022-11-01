using ChatBot.Models.Enums;
using System;
using System.Collections.Generic;

namespace ChatBot.Models.DTOs;

public record FakeApiUserDTO(Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Phone,
    Role Role,
    Company Company,
    List<Order> Orders);

