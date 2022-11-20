import './style/style.css';
import ChatIcon from '@mui/icons-material/Chat';
import {useEffect, useState, useRef, useReducer, useMemo} from "react";
import {ChatMessage, MessageFromServer, MessageToServer, QuickSelector} from "../DTO/messageWsDTO";
import {webSocketServerAddress, botNickName, ChatBot2OpenningMessage, ChatBotOpennigMessage, techSupportNickName} from "../../app.properties";
import ChatBox from './ChatBox';
import userApi from '../Login/userApi';

const sortChatMessages = (messages: Record<string, ChatMessage>, conversation: string) => {
    var out: ChatMessage[] = [];

    for (var id in messages)
        if (messages[id].conversation === conversation)
            out.push(messages[id]);

    return out.sort((a, b) => a.timestamp > b.timestamp ? 1 : b.timestamp > a.timestamp ? -1 : 0);
};

const ChatBot = () => {
    const ws = useRef<WebSocket>(null);

    const [conversation, setConversation] = useReducer((_: string, value: string) => {
        sessionStorage.setItem('currentconversation', value);
        return value;
    }, null, () => sessionStorage.getItem('currentconversation'));

    const [messages, setMessages] = useState<Record<string, ChatMessage>>({});
    const messagesSorted = useMemo(() => sortChatMessages(messages, conversation), [messages, conversation]);
    
    const [isChatOpen, setIsChatOpen] = useState(false); 

    const sendMessage = (message: MessageToServer) => {
        if (ws.current?.readyState === WebSocket.OPEN)
            ws.current.send(JSON.stringify(message));
        else
            console.warn('Tried to send a message, but the WebSocket wasn\'t open');
    };

    const handleSend = (clientMessage: string, quickSelect: QuickSelector) => {
        sendMessage({
            action: 'sendChatMessage',
            content: clientMessage,
            conversation: conversation,
            quickSelector: quickSelect
        });
    };

    const handleReceive = (e: MessageEvent<string>) => {
        var data: MessageFromServer = JSON.parse(e.data);
        switch (data.type) {
        case 'conversation':
            setConversation(data.id);
            break;
        case 'chatMessage':
            setMessages(messages => ({...messages, [data.id]: data as ChatMessage}));
            break;
        default:
            console.warn('Unkown message type ' + data.type);
            break;
        }
    };

    const handleOpenChat = () => {
        setIsChatOpen(true);
        if (conversation)
            sendMessage({ action: 'getMessageHistory', conversation: conversation });
        else
            sendMessage({ action: 'newConversation' });
    };

    useEffect(() => {
        ws.current = new WebSocket(webSocketServerAddress);
        ws.current.onopen = e => {
            ws.current.send(userApi.getCurrentUser().token);
        };
        ws.current.onmessage = handleReceive;

        return () => {
            ws.current?.close();
        };
    }, []);

    return(
        <div id="center-text">
            <div id="chat-circle"  className="btn btn-raised" onClick={handleOpenChat}>
                <div id="chat-overlay" ></div>
                <ChatIcon/>
            </div>

            { isChatOpen ? <ChatBox messages={messagesSorted} onSend={handleSend} onClose={() => setIsChatOpen(false)}/> : null }
        </div>
    )
}

export default ChatBot;
