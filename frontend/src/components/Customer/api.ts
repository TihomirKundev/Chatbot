import axios from 'axios';
import {BaseUrl, TicketApi} from "../../app.properties";
import {TicketCreateDTO} from "../DTO/ticketCreationDTO";

export const generateTicket = async (incomingCrateTicketDTO: TicketCreateDTO) => {
    return await axios.post(BaseUrl + TicketApi, incomingCrateTicketDTO);
};
