import '../styles/style.css';
import '../styles/css_bootstrap.min.css';
import SendIcon from '@mui/icons-material/Send';
import React, {useState, useRef, useEffect, MutableRefObject} from "react";
import {MessageWsDTO} from "../../DTO/messageWsDTO";
import {techSupportNickName, webSocketServerAddress} from "../../../app.properties";
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


const ChatWindow = ({ticket}) => {
    const [inputMsg, setInputMsg] = useState("");
    const ws = useRef(null);

    let sampleWsMessages : MessageWsDTO[] = [
        {
            sendClientId: "1",
            action: "message",
            msg: "you have been connected to the customer",
            nick: "random Guid"
        }
       
    ];

    const [wsMessages, setWsMessages] = useState<MessageWsDTO[]>(sampleWsMessages);
    const ScrollRef = useChatScroll(wsMessages)

    useEffect(() => {
        ws.current = new WebSocket(webSocketServerAddress);
        ws.current.onopen = () => {
            ws.current.send(JSON.stringify({
                Action: 'join',
                Content: ticket.ticketNumber
            }));
        };
        return () => {
            ws.current.close();
        };
    }, []);

    useEffect(() => {
        ws.current.onmessage = e => {
            var data = JSON.parse(e.data);
            if (data.Action === 1) // TODO: enum
                setWsMessages([...wsMessages, {action: 'message', msg: data.Content, nick: data.Nickname}]);
        };
    }, [wsMessages]);

    const handleSend = () => {
        ws.current.send(JSON.stringify({Action: 'send', Content: inputMsg, Nickname: techSupportNickName, Timestamp: Date.now()}));
        setInputMsg("");
    };
    
    
    const detectEnter = (e) => {
        if (e.key === 'Enter' && inputMsg !== "") {
        handleSend()
    }}
    

    return <section className="chat">
        <div className="header-chat" style={{position:"relative"}}>
            <i className="icon fa fa-user-o" aria-hidden="true"></i>
            <p>{ticket?.name}</p>
            <p style={{position:"absolute", right:15}}>{ticket?.email}</p>
            <i className="icon clickable fa fa-ellipsis-h right" aria-hidden="true"></i>
        </div>
        <div ref={ScrollRef} style={{height:'72vh',overflowY:'scroll'}} className="messages-chat">
            {wsMessages.map(wsMessage => <ChatMessage senderName={wsMessage.nick !== techSupportNickName ? ticket?.name : null} message={wsMessage.msg}/> )}
        </div>
        <div className="footer-chat">
            <input type="text" className="write-message" value={inputMsg} onChange={e => setInputMsg(e.target.value)} onKeyDown={detectEnter} placeholder="Type your message here"></input>
            <i className="icon send fa fa-paper-plane-o clickable" onClick={handleSend}  aria-hidden="true"> <SendIcon className="send-message" /></i>
        </div>
    </section>;
}

export default ChatWindow;
