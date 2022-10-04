import './style/style.css';
import CancelIcon from '@mui/icons-material/Cancel';
import SendIcon from '@mui/icons-material/Send';
import {MutableRefObject, useEffect, useRef} from "react";
import {botNickName, ChatBotOpennigMessage, techSupportNickName} from "../../app.properties";
import {TicketCreateDTO} from "../DTO/ticketCreationDTO";
import {generateTicket} from "./api";
import {ticketDTO} from "../DTO/ticketDTO";
import ChatMessage from './ChatMessage';

function useChatScroll<T>(dep: T): MutableRefObject<HTMLDivElement> {
    const ref = useRef<HTMLDivElement>();
    useEffect(() => {
        if (ref.current) {
            ref.current.scrollTop = ref.current.scrollHeight;
        }
    }, [dep]);
    return ref;
}



const ChatBox = ({messages, onSend, onClose}) => {
    var chatRef = useRef<HTMLInputElement>();

    const ScrollRef = useChatScroll(messages)


    const detectEnter = (e) => {
        if (e.key === 'Enter' && chatRef.current.value !== "") {
            onSend(chatRef.current.value)
            chatRef.current.value = "";
        }}
    
    return <div className="chat-box">
                <div className="chat-box-header">
                    ChatBot
                    <span className="chat-box-toggle" onClick={onClose}><CancelIcon/></span>
                </div>
                <div className="chat-box-body">
                    <div className="chat-box-overlay">
                    </div>
                    <div ref={ScrollRef} className="chat-logs">
                        { messages.map(wsMessage => <ChatMessage message={wsMessage.msg} fromSelf={wsMessage.nick !== techSupportNickName && wsMessage.nick !== botNickName}/>) }
                    </div>
                </div>
                <div  className="chat-input">
                    <input type="text" id="chat-input" ref={chatRef} onKeyDown={detectEnter}  placeholder="Send a message..."/>
                    <div onClick={() => {onSend(chatRef.current.value); chatRef.current.value = '';}} className="chat-submit" ><SendIcon/></div>
                </div>
            </div>;
}

export default ChatBox;
