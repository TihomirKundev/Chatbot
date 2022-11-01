using FakeAPI.Enums;

namespace ChatBot.Models.DTOs;

public record Company(
    string Name,
    string WebsiteUrl,
    string Address,
    string ZipCode,
    Countries Country,
    LegalForms LegalForms);
