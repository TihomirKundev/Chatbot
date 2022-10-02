import './style/style.css';
import ChatIcon from '@mui/icons-material/Chat';
import CancelIcon from '@mui/icons-material/Cancel';
import SendIcon from '@mui/icons-material/Send';
import {useState} from "react";
import {MessageWsDTO} from "../DTO/messageWsDTO";
import {techSupportNickName} from "../../app.properties";


const ChatBot = () => {
const [isChatOpen, setIsChatOpen] = useState(false);  
const [inputMsg, setInputMsg] = useState("");    

const handleSend = () => {
    console.log(inputMsg);
    setInputMsg("");
    //TODO: send message through ws    
}

const loadMessages = () => {
    //
}




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
                                if (WsMessage.nick == techSupportNickName)
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