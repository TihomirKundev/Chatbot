import axios from 'axios';
import {BaseUrl, TicketApi} from "../../app.properties";




export const getAllTickets = async () => {
    var a = await axios.get(BaseUrl + TicketApi);
      return a;
}



