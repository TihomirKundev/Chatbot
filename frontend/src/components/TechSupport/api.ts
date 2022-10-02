import axios from 'axios';
import {BaseUrl, TicketApi} from "../../app.properties";




export const getAllTickets = async () => {
    return await axios.get(BaseUrl + TicketApi);

}



