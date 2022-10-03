export type TicketDTO = {
    ticketNumber: String,
    email: String,
    name: String,
    status: Status  
}

export enum Status {
    opened = 'opened', 
    closed = 'closed'
}
