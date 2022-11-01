using System.Collections.Generic;

namespace ChatBot.Models.DTOs;

public record Order(List<Vehicle> Vehicles, bool Status);