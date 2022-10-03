import React from "react";
import LetteredAvatar from 'react-lettered-avatar';
import {Status} from "../../DTO/TicketDTO";

export const ChatListEntry = ({ticket, isSelected, onSelect}) =>
    <div className={`discussion ${isSelected ? 'message-active' : ''}`} onClick={() => onSelect(ticket)}>
        <div className="photo">
            <LetteredAvatar name={ticket.name}/>
            {ticket.status === Status.opened ? <div className="online"></div> : <></>}
        </div>
        <div className="desc-contact">
            <p className="name">{ticket.name}</p>
            <p className="message">{ticket.email}</p>
        </div>
        {/*<div className="timer">4 days</div>  //TODO: add  date of the conversion */}
    </div>;

export default ChatListEntry;
