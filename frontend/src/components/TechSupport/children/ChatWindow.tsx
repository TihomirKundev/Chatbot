import '../styles/style.css';
import '../styles/css_bootstrap.min.css';
import SendIcon from '@mui/icons-material/Send';
import React, {useState} from "react";
import {status, ticketDTO} from "../../DTO/ticketDTO";
import LetteredAvatar from 'react-lettered-avatar';
import {MessageWsDTO} from "../../DTO/messageWsDTO";
import {techSupportNickName} from "../../../app.properties";
import {Margin} from "@mui/icons-material";

const ChatWindow = (props) => {
    const selectedTicket = props.pselectedTicket;
    const setSelectedTicket = props.psetSelectedTicket;
const [inputMsg, setInputMsg] = useState("");




    let sampleWsMessages : MessageWsDTO[] = [{
    sendClientId: "1",
    action: "message",
    msg: "hello",
    nick: "random Guid"
} ,
    {
    sendClientId: "1",
    action: "message",
    msg: "hello john how can i assist you",
    nick: "Bas World Representative"
} ]
const [wsMessages, setWsMessages] = useState<MessageWsDTO[]>( sampleWsMessages)




const handleSend = () => {
    console.log(inputMsg);
    setInputMsg("");
}



    
    const rendername = () => {if (selectedTicket != null) return selectedTicket.name; else return ""}
    const renderEmail = () => {if (selectedTicket != null) return selectedTicket.email; else return ""}

    return(<section className="chat">
        <div className="header-chat" style={{position:"relative"}}>
            <i className="icon fa fa-user-o" aria-hidden="true"></i>
            <p>
                {  rendername()}
            </p> 
            <br/>
            <br/>

            <p style={{position:"absolute", right:15}}>{renderEmail()}</p>
            <i className="icon clickable fa fa-ellipsis-h right" aria-hidden="true"></i>
        </div>
        <div className="messages-chat">



        {
            wsMessages.map((WsMessage) => {
            
                
                
                if (WsMessage.nick != techSupportNickName)
                    return( <div className="message">
                        <div className="photo" >
                            <LetteredAvatar name={rendername()}/>
                         </div>
                         <p className="text"> {WsMessage.msg}</p>
                        </div>)
                else 
                    return (
                        <div className="message text-only">
                            <div className="response">
                                <p className="text"> {WsMessage.msg}</p>
                            </div>
                        </div>
                    )
            
            
            
            
            
            })}
           
        </div>
        
        
        
        
        
        <div className="footer-chat">
            <input type="text" className="write-message" value={inputMsg} onChange={(e)=>setInputMsg(e.target.value)} placeholder="Type your message here"></input>
           
            <i className="icon send fa fa-paper-plane-o clickable" onClick={handleSend} aria-hidden="true"> <SendIcon className="send-message" /></i>
        </div>
    </section>)
}
export default ChatWindow;