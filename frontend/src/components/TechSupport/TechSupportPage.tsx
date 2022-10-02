import ChatSelector from "./children/ChatSelector";
import './styles/style.css';
import ChatWindow from "./children/ChatWindow";
import {Box} from "@mui/material";
import {useState} from "react";
import {ticketDTO} from "../DTO/ticketDTO";
const TechSupportPage = () => {

    const [selectedTicket, setSelectedTicket] = useState<ticketDTO >(null);

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'row',
              
            }}
        >
        <ChatSelector pselectedTicket={selectedTicket} psetSelectedTicket={setSelectedTicket} />
        <ChatWindow  pselectedTicket={selectedTicket} psetSelectedTicket={setSelectedTicket} />
        </Box>
    );
}
export default TechSupportPage;