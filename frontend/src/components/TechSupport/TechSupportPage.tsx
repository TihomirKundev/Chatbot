﻿import ChatSelector from "./children/ChatSelector";
import './styles/style.css';
import ChatWindow from "./children/ChatWindow";
import {Box} from "@mui/material";
import {useState, useEffect} from "react";
import {TicketDTO, Status} from "../DTO/TicketDTO";
import {getAllTickets} from "./api";


const TechSupportPage = () => {
    let sampleTickets: TicketDTO[] = [
        {
            ticketNumber: "12345",
            email: "john@example.com",
            name: "John",
            status: Status.opened
        },
        {
            ticketNumber: "234567",
            email: "toni@example.com",
            name: "toni",
            status: Status.closed
        }
    ];

    const [tickets, setTickets] = useState<TicketDTO[]>(sampleTickets);
    const [selectedTicket, setSelectedTicket] = useState<TicketDTO>(null);

    useEffect(() => {
        getAllTickets().then((response) => {setTickets(response.data)});
    }, []);

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'row'
            }}
        >
            <ChatSelector selectedTicket={selectedTicket} onSelect={setSelectedTicket} tickets={tickets} />
            { selectedTicket ? <ChatWindow ticket={selectedTicket} /> : <p>select a ticket :-)</p> }
        </Box>
    );
}
export default TechSupportPage;
