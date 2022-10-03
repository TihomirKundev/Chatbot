import React, {useEffect, useState} from "react";
import '../styles/style.css';

import {Status, TicketDTO} from "../../DTO/TicketDTO";
import {getAllTickets} from "../api";
import ChatListEntry from './ChatListEntry';

export const ChatSelector = ({tickets, selectedTicket, onSelect}) => {
    return <section className="discussions">
        <div className="discussion search">
            <div className="searchbar">
                <i className="fa fa-search" aria-hidden="true"></i> {/* TODO: backend should expose a query onGet controller*/}
                <input type="text" placeholder="Search..."/>
            </div>
        </div>
        { tickets.map(ticket => <ChatListEntry ticket={ticket} isSelected={selectedTicket?.ticketNumber === ticket.ticketNumber} onSelect={onSelect}/>) }
    </section>;
};

export default ChatSelector;
