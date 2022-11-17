namespace ChatBot.Models.DTOs;

public class TicketDTO
{
    public string ticketnumber { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public Status status { get; set; }

    public TicketDTO()
    {
    }
}

public enum Status
{
    OPENED,
    CLOSED
}