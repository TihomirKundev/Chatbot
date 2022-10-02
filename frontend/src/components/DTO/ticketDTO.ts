export type ticketDTO = {
    ticketNumber: String,
    email: String,
    name: String,
    status: status  
}

export enum status {
    opened = 'opened', 
    closed = 'closed'
}
