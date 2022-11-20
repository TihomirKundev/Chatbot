import '../styles/style.css';
import '../styles/css_bootstrap.min.css';
import SendIcon from '@mui/icons-material/Send';
import React, {useState, useRef, useEffect, MutableRefObject, useMemo} from "react";
import {techSupportNickName, webSocketServerAddress} from "../../../app.properties";
import ChatMessage from './ChatMessage';
import { ConversationDetails } from '../ConversationPreview';
import userApi from '../../Login/userApi';

function useChatScroll<T>(dep: T): MutableRefObject<HTMLDivElement> {
    const ref = useRef<HTMLDivElement>();
    useEffect(() => {
        if (ref.current) {
            ref.current.scrollTop = ref.current.scrollHeight;
        }
    }, [dep]);
    return ref;
}


const ChatWindow = ({conversation, onSendMessage} : {conversation: ConversationDetails, onSendMessage: (message: string) => void}) => {
    const [inputMsg, setInputMsg] = useState("");

    const ScrollRef = useChatScroll(conversation);

    const selfName = useMemo(() => {
        var me = userApi.getCurrentUser();
        return me.firstName + ' ' + me.lastName;
    }, []);

    const handleSend = () => {
        onSendMessage(inputMsg);
        setInputMsg('');
    };
    
    const detectEnter = (e) => {
        if (e.key === 'Enter' && inputMsg !== "") {
            handleSend();
        }
    }

    return <section className="chat">
        <div className="header-chat" style={{position:"relative"}}>
            <i className="icon fa fa-user-o" aria-hidden="true"></i>
            <p>{conversation.name}</p>
            <i className="icon clickable fa fa-ellipsis-h right" aria-hidden="true"></i>
        </div>
        <div ref={ScrollRef} style={{height:'72vh',overflowY:'scroll'}} className="messages-chat">
            {conversation.messages.map(message => <ChatMessage key={message.id} senderName={message.sender ? message.sender === selfName ? null : message.sender : '?'} message={message.message}/> )}
        </div>
        <div className="footer-chat">
            <input type="text" className="write-message" value={inputMsg} onChange={e => setInputMsg(e.target.value)} onKeyDown={detectEnter} placeholder="Type your message here"></input>
            <i className="icon send fa fa-paper-plane-o clickable" onClick={handleSend} aria-hidden="true"> <SendIcon className="send-message" /></i>
        </div>
    </section>;
}

export default ChatWindow;
