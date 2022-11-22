import ChatSelector from "./children/ChatSelector";
import './styles/style.css';
import ChatWindow from "./children/ChatWindow";
import {Box} from "@mui/material";
import {useState, useEffect, useRef, useMemo} from "react";
import PleseSelectChat from "./children/PleseSelectChat";
import { ChatMessage, Conversation, MessageFromServer, MessageToServer, QuickSelector, User } from "../DTO/messageWsDTO";
import { webSocketServerAddress } from "../../app.properties";
import { ConversationDetails, ConversationPreview } from "./ConversationPreview";
import userApi from "../Login/userApi";

const sortConversations = (conversations: Record<string, Conversation>, users: Record<string, User>, messages: Record<string, ChatMessage>) => {
    var previews: ConversationPreview[] = [];

    for (var id in conversations) {
        let latestMessage = findLatestMessage(messages, id);
        previews.push({
            conversationId: id,
            lastMessageContent: latestMessage?.content,
            lastMessageTimestamp: latestMessage?.timestamp,
            userName: users[conversations[id].participants[0]]?.name,
            status: conversations[id].status
        });
    }

    return previews.sort((a, b) => a.lastMessageTimestamp < b.lastMessageTimestamp ? 1 : b.lastMessageTimestamp < a.lastMessageTimestamp ? -1 : 0);
};

const findLatestMessage = (messages: Record<string, ChatMessage>, conversation: string) => {
    var latestMessage: ChatMessage = null;

    for (var id in messages)
        if (messages[id].conversation === conversation && (latestMessage === null || messages[id].timestamp > latestMessage.timestamp))
            latestMessage = messages[id];

    return latestMessage;
};

const sortConversationMessages = (conversation: Conversation, users: Record<string, User>, messages: Record<string, ChatMessage>) => {
    if (!conversation)
        return null;

    var details: ConversationDetails = {
        name: users[conversation.participants[0]]?.name,
        messages: []
    };

    for (var id in messages)
        if (messages[id].conversation === conversation.id)
            details.messages.push({
                id: messages[id].id,
                sender: users[messages[id].author]?.name,
                message: messages[id].content,
                timestamp: messages[id].timestamp
            });

    details.messages.sort((a, b) => a.timestamp > b.timestamp ? 1 : b.timestamp > a.timestamp ? -1 : 0);

    return details;
};

const TechSupportPage = () => {
    const [selectedConversation, setSelectedConversation] = useState<string>(null);

    const [users, setUsers] = useState<Record<string, User>>({});
    const [conversations, setConversations] = useState<Record<string, Conversation>>({});
    const [messages, setMessages] = useState<Record<string, ChatMessage>>({});

    const conversationList = useMemo(() => sortConversations(conversations, users, messages), [users, conversations, messages]);
    const conversation = useMemo(() => sortConversationMessages(conversations[selectedConversation], users, messages), [users, messages, selectedConversation]);

    const ws = useRef<WebSocket>();

    const handleReceive = (e: MessageEvent<string>) => {
        var data: MessageFromServer = JSON.parse(e.data);
        switch (data.type) {
        case 'conversation':
            setConversations(conversations => ({...conversations, [data.id]: data as Conversation}));
            break;
        case 'chatMessage':
            setMessages(messages => ({...messages, [data.id]: data as ChatMessage}));
            break;
        case 'user':
            setUsers(users => ({...users, [data.id]: data as User}));
            break;
        default:
            console.warn('Unkown message type ' + data.type);
            break;
        }
    };

    const handleSend = (message: string) => {
        sendMessage({
            action: 'sendChatMessage',
            content: message,
            conversation: selectedConversation,
            quickSelector: QuickSelector.CustomerSupport
        });
    };

    const sendMessage = (message: MessageToServer) => {
        if (ws.current?.readyState === WebSocket.OPEN)
            ws.current.send(JSON.stringify(message));
        else
            console.warn('Tried to send a message, but the WebSocket wasn\'t open');
    };

    const handleConversationSelect = (conversation: string) => {
        setSelectedConversation(conversation);
        sendMessage({action: 'getMessageHistory', conversation: conversation});
    };

    useEffect(() => {
        ws.current = new WebSocket(webSocketServerAddress);
        ws.current.onopen = e => {
            ws.current.send(userApi.getCurrentUser().token);
            sendMessage({action: 'getConversations'});
        };
        ws.current.onmessage = handleReceive;

        return () => {
            ws.current?.close();
        };
    }, []);

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'row'
            }}
        >
            <ChatSelector selectedConversation={selectedConversation} onSelect={handleConversationSelect} conversations={conversationList} />
            { selectedConversation ? <ChatWindow conversation={conversation} onSendMessage={handleSend}/> : <PleseSelectChat/> }
        </Box>
    );
}
export default TechSupportPage;
