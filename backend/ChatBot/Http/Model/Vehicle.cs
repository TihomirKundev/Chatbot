using FakeAPI.Enums;

namespace ChatBot.Models.DTOs;

public record Vehicle(
    int ReferenceNum,
    string Name,
    bool Availability,
    string Location,
    double Price,
    string City,
    Countries Country);