import './style/style.css';
import CancelIcon from '@mui/icons-material/Cancel';
import SendIcon from '@mui/icons-material/Send';
import {MutableRefObject, useEffect, useRef, useState} from "react";
import {botNickName, techSupportNickName} from "../../app.properties";

import ChatMessage from './ChatMessage';
import {Box} from "@mui/material";
import {quickSelector} from "../DTO/messageWsDTO";


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
    const [QuickSelector, setQuickSelector] = useState<quickSelector>(quickSelector.ts);

    const detectEnter = (e) => {
        if (e.key === 'Enter' && chatRef.current.value !== "") {
            onSend(chatRef.current.value, QuickSelector)
            chatRef.current.value = "";
        }}

    return <div className="chat-box">
        <div className="chat-box-header">
            ChatBot
            <span className="chat-box-toggle" onClick={onClose}><CancelIcon/></span>
        </div>
        <div className="chat-box-body" style={{borderBottom:"none"}}>
            <div className="chat-box-overlay">
            </div>
            <div ref={ScrollRef} className="chat-logs">
                { messages.map(wsMessage => <ChatMessage message={wsMessage.msg} fromSelf={wsMessage.nick !== techSupportNickName && wsMessage.nick !== botNickName}/>) }

                <br/>
                <br/>
            </div>
        </div>
        <div  className="chat-input" style={{borderTop:"none"}}>
            <Box sx={{ display: 'inline-flex' }} style={{border:"solid", borderWidth:1 , borderColor:"#d3d3d3", backgroundColor:"#f4f7f9", width:"100%" }}>
                <div className="quickSelector" style={{backgroundColor: QuickSelector? "" : "#5a5eb9", color: QuickSelector? "": "white"}} onClick={()=>setQuickSelector(quickSelector.faq)}>
                    <span >🧪FAQ</span>
                </div>
                <div className="quickSelector" style={{backgroundColor: QuickSelector? "#5a5eb9" : "", color: QuickSelector? "white": ""}} onClick={()=>setQuickSelector(quickSelector.ts)}>
                    <span >🧑🏾‍💻TS</span>
                </div>
            </Box>
            <input type="text" id="chat-input" ref={chatRef} onKeyDown={detectEnter}  placeholder="Send a message..."/>
            <div onClick={() => {onSend(chatRef.current.value,QuickSelector); chatRef.current.value = '';}} className="chat-submit" ><SendIcon/></div>
        </div>
    </div>;
}

export default ChatBox;
