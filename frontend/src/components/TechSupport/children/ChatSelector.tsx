import React, {useEffect} from "react";
import {useState} from "react";
import '../styles/style.css';

import LetteredAvatar from 'react-lettered-avatar';
import {status, ticketDTO} from "../../DTO/ticketDTO";
import {getAllTickets} from "../api";


export const ChatSelector = (props) => {
    
    let sampleTicket: ticketDTO[]= [{    ticketNumber: "12345",
        email: "john@example.com",
        name: "John",
        status: status.opened },
        
        {    ticketNumber: "234567",
            email: "toni@example.com",
            name: "toni",
            status: status.closed }]
    
    
    
const [tickets, setTickets] = useState<ticketDTO[]>(sampleTicket); //TODO: remove sample data

    
const selectedTicket = props.pselectedTicket;
const setSelectedTicket = props.psetSelectedTicket;

   const handleSelectChat = (ticket: ticketDTO) => {
        setSelectedTicket(ticket);
   }
    

    
    useEffect(() => {
        getAllTickets().then((response) => {setTickets(response.data)});
    }, []);
    
    
    return(
        
    <>
        <section className="discussions">
            <div className="discussion search">
                <div className="searchbar">
                    <i className="fa fa-search" aria-hidden="true"></i> {/* TODO: backend should expose a query onGet controller*/}
                    <input type="text" placeholder="Search..."></input>
                </div>
            </div>
    
    
    
        { 
            tickets.map((ticket) =>{
                let isOpen: boolean; // the currently rendered out converstion`s ticket in conv selector is open or closed
                let isSelected: boolean; // the currently rendered out conversation in conv selector is selected or not
                
                if (ticket.status == status.opened) isOpen = true;
                else isOpen = false;
                if (selectedTicket == null || selectedTicket.ticketNumber != ticket.ticketNumber) isSelected = false;
                else isSelected = true;
                
                
                
             if (isSelected)
                return (
                     <div className="discussion message-active" onClick={ () => handleSelectChat(ticket)}>
                        <div className="photo">
                            <LetteredAvatar
                                name={ticket.name}
                            />
                            {isOpen?  <div className="online"></div> : <></>}
                            
                            
                        </div>
                        <div className="desc-contact">
                            <p className="name">{ticket.name}</p>
                            <p className="message">{ticket.email}</p>
                        </div>
                        {/*<div className="timer">4 days</div>  //TODO: add  date of the conversion */}
                    </div>
                )
                else
                     return ( 
                         <div className="discussion" onClick={ () => handleSelectChat(ticket)}>
                         <div className="photo">
                             <LetteredAvatar
                                 name={ticket.name}
                             />
                             {isOpen?  <div className="online"></div> : <></>}


                         </div>
                         <div className="desc-contact">
                             <p className="name">{ticket.name}</p>
                             <p className="message">{ticket.email}</p>
                         </div>
                         {/*<div className="timer">4 days</div>  //TODO: add  date of the conversion */}
                     </div>
                     )
                     
            
            })
        }
        


        </section>
    </>
        
    )
}
export default ChatSelector;