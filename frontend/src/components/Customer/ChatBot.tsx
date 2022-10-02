import './style/style.css';
import ChatIcon from '@mui/icons-material/Chat';
import CancelIcon from '@mui/icons-material/Cancel';
import SendIcon from '@mui/icons-material/Send';
import {useEffect, useState} from "react";
import {MessageWsDTO} from "../DTO/messageWsDTO";
import {botNickName, ChatBot2OpenningMessage, ChatBotOpennigMessage, techSupportNickName} from "../../app.properties";
import {TicketCreateDTO} from "../DTO/ticketCreationDTO";
import {generateTicket} from "./api";
import {ticketDTO} from "../DTO/ticketDTO";


const ChatBot = () => {
const [ticket, setTicket] = useState<ticketDTO>(null)
const [wsMessages, setWsMessages] = useState<MessageWsDTO[]>([])
    
    
const [isChatOpen, setIsChatOpen] = useState(false);  
const [inputMsg, setInputMsg] = useState("");    

const [chatStarted, setChatStarted] = useState<boolean>(false);
enum chatStepEnum {name = "name", email = "email", done = "done"}
const [chatStarterStep, setChatStarterStep] = useState<chatStepEnum>(chatStepEnum.name);
const [TicketCreatorDTO, setTicketCreatorDTO] = useState<TicketCreateDTO>({name: "", email: ""});




const handleCreationOfTicket = () => {
    if (chatStarterStep === chatStepEnum.name) {
        setTicketCreatorDTO({...TicketCreatorDTO, name: inputMsg});
        setWsMessages([...wsMessages,
            {sendClientId: "1", action: "message", msg: inputMsg, nick:''},{sendClientId: "1", action: "message", msg: "What`s your email?", nick: botNickName}])      
        setChatStarterStep(chatStepEnum.email);
       // setWsMessages([...wsMessages, {sendClientId: "1", action: "message", msg: inputMsg, nick:''}])
    }
    if (chatStarterStep === chatStepEnum.email) {
        let ticketDTO: TicketCreateDTO = {name: TicketCreatorDTO.name, email: inputMsg};
        setTicketCreatorDTO(ticketDTO);  //idk wht ...TicketCreatorDTO doesnt work  
        setWsMessages([...wsMessages, {sendClientId: "1", action: "message", msg: inputMsg, nick:''}])

        generateTicket(ticketDTO).then((response) => { setTicket(response.data)});
        setChatStarterStep(chatStepEnum.done);

        StartChat();
    }
}
    

    const handleSend = () => {
   if (!chatStarted) {
       handleCreationOfTicket();
       setInputMsg("");
       return;
   }
    console.log(inputMsg);
   
    setInputMsg("");
    //TODO: send message through ws    
}





const loadMessages = () => { //TODO:WS
    
}

const StartChat = () => {//TODO:connect to web sockets
    
}
    



  



 const addStartChatMessages = () => {
    setWsMessages([...wsMessages, {sendClientId: "1", action: "message", msg: ChatBotOpennigMessage, nick: botNickName},
        {sendClientId: "1", action: "message", msg: ChatBot2OpenningMessage, nick: botNickName}, 
        {sendClientId: "1", action: "message", msg: "What`s your name?", nick: botNickName}])

 }

//useEffect on load of component call addStartChatMessages
var brLoad = 0;
useEffect(() => {
    if (brLoad === 0) {
    addStartChatMessages();}
    brLoad++;
}, [])



    
    return(<>
        <div id="center-text">
       

            <div id="chat-circle"   className="btn btn-raised" onClick={()=> setIsChatOpen(true)}>
                <div id="chat-overlay" ></div>
                <ChatIcon/>
            </div>

            { isChatOpen? <div className="chat-box">
                <div className="chat-box-header">
                    ChatBot
                    <span className="chat-box-toggle" onClick={()=>setIsChatOpen(false)}><CancelIcon/></span>
                </div>
                <div className="chat-box-body">
                    <div className="chat-box-overlay">
                    </div>
                    <div className="chat-logs">
                        

                        
                        {
                            wsMessages.map((WsMessage) => {
                                if (WsMessage.nick == techSupportNickName || WsMessage.nick == botNickName)
                                    return(
                                        <div  className="chat-msg user" >
                                            <span className="msg-avatar">{/*TODO: add bas world logo */}</span>
                                            <div className="cm-msg-text">{WsMessage.msg}</div>
                                        </div>
                                    )
                                else 
                                    return (
                                        <div className="chat-msg self">
                                            <span className="msg-avatar"></span>
                                            <div className="cm-msg-text">{WsMessage.msg}</div>
                                        </div>
                                    )
                                
                                }
                            
                            )}
                        
                    
                    </div>
              
                </div>
                <div className="chat-input">
                    <>
                        <input type="text" id="chat-input" value={inputMsg} onChange={(e)=>setInputMsg(e.target.value)} placeholder="Send a message..."/>
                        <div onClick={handleSend} className="chat-submit" ><SendIcon/></div>
                    </>
                </div>
            </div> : <></> }


        </div>

       
        
    </>)
}
export default ChatBot