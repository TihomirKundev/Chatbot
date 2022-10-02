namespace ChatBot.Models.DTOs;

public class TicketDTO  {
public string ticketNumber { get; set; }
public string email { get; set; }
public string name { get; set; }
public Status status { get; set; }
}

public enum Status {
    OPENED, 
    CLOSED 
}