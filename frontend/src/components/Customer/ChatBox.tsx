import './style/style.css';
import CancelIcon from '@mui/icons-material/Cancel';
import SendIcon from '@mui/icons-material/Send';
import {MutableRefObject, ReactNode, useEffect, useMemo, useRef, useState} from "react";
import {botNickName, techSupportNickName} from "../../app.properties";

import ChatMessageElement from './ChatMessage';
import {Box} from "@mui/material";
import {ChatMessage, QuickSelector} from "../DTO/messageWsDTO";
import userApi from '../Login/userApi';


function useChatScroll<T>(dep: T): MutableRefObject<HTMLDivElement> {
    const ref = useRef<HTMLDivElement>();
    useEffect(() => {
        if (ref.current) {
            ref.current.scrollTop = ref.current.scrollHeight;
        }
    }, [dep]);
    return ref;
}

const QuickSelectorButton = ({ onClick, targetValue, currentValue, children }: { onClick: (value: QuickSelector) => void, targetValue: QuickSelector, currentValue: QuickSelector, children: ReactNode }) => {
    var selected = targetValue === currentValue;

    return <div className="quickSelector" style={{ borderColor: "#05ad61",backgroundColor: selected && "#05ad61", color: selected && "white" }} onClick={() => onClick(targetValue)}>
        <span>{children}</span>
    </div>;
};

const ChatBox = ({messages, onSend, onClose}: {messages: ChatMessage[], onSend: (message: string, quickSelector: QuickSelector) => void, onClose: () => void }) => {
    var chatRef = useRef<HTMLInputElement>();

    const ScrollRef = useChatScroll(messages)
    const [quickSelector, setQuickSelector] = useState<QuickSelector>(QuickSelector.Auto);

    const myId = useMemo(() => userApi.getCurrentUser().id, []);

    const detectEnter = (e) => {
        if (e.key === 'Enter' && chatRef.current.value !== "") {
            onSend(chatRef.current.value, quickSelector)
            chatRef.current.value = "";
        }}
    
    return <div className="chat-box" style={{minWidth:370}}>
        <div className="chat-box-header">
            ChatBot
            <span className="chat-box-toggle" onClick={onClose}><CancelIcon/></span>
        </div>
        <div className="chat-box-body" style={{borderBottom:"none"}}>
            <div className="chat-box-overlay">
            </div>
            <div ref={ScrollRef} className="chat-logs">
                { messages.map(wsMessage => <ChatMessageElement key={wsMessage.id}   message={wsMessage.content} fromSelf={wsMessage.author === myId}/>) }

                <br/>
                <br/>
            </div>
        </div>
        <div  className="chat-input" style={{borderTop:"none"}}>
            <Box sx={{ display: 'inline-flex' }} style={{border:"solid", borderWidth:1 , borderColor:"#d3d3d3", backgroundColor:"#f4f7f9", width:"100%" }}>
                <QuickSelectorButton onClick={setQuickSelector} targetValue={QuickSelector.Auto} currentValue={quickSelector}>🔄️AUTO</QuickSelectorButton>
                <QuickSelectorButton onClick={setQuickSelector} targetValue={QuickSelector.Faq} currentValue={quickSelector}>🧪FAQ</QuickSelectorButton>
                <QuickSelectorButton onClick={setQuickSelector} targetValue={QuickSelector.Order} currentValue={quickSelector}>🚛Ord</QuickSelectorButton>
                <QuickSelectorButton onClick={setQuickSelector} targetValue={QuickSelector.CustomerSupport} currentValue={quickSelector}>🧑🏾‍💻TS</QuickSelectorButton>
            </Box>
            <input type="text" id="chat-input" ref={chatRef} onKeyDown={detectEnter}  placeholder="Send a message..."/>
            <div onClick={() => {onSend(chatRef.current.value, quickSelector); chatRef.current.value = '';}} className="chat-submit" ><SendIcon/></div>
        </div>
    </div>;
}

export default ChatBox;
