import './style/style.css';
import ChatIcon from '@mui/icons-material/Chat';
import {useEffect, useState, useRef} from "react";
import {MessageWsDTO} from "../DTO/messageWsDTO";
import {webSocketServerAddress, botNickName, ChatBot2OpenningMessage, ChatBotOpennigMessage, techSupportNickName} from "../../app.properties";
import {TicketCreateDTO} from "../DTO/ticketCreationDTO";
import {generateTicket} from "./api";
import {ticketDTO} from "../DTO/ticketDTO";
import ChatBox from './ChatBox';


const ChatBot = () => {
    const ws = useRef(null);

    const [ticket, setTicket] = useState<ticketDTO>(null);
    const [wsMessages, setWsMessages] = useState<MessageWsDTO[]>([]);
    
    const [isChatOpen, setIsChatOpen] = useState(false); 

    enum ChatStep {ENTER_NAME, ENTER_EMAIL, CHAT_STARTED}
    const [chatStep, setChatStep] = useState<ChatStep>(ChatStep.ENTER_NAME);
    const [TicketCreatorDTO, setTicketCreatorDTO] = useState<TicketCreateDTO>({name: "", email: ""});
    const handleCreationOfTicket = (clientMessage) => {
        if (chatStep === ChatStep.ENTER_NAME) {
            setTicketCreatorDTO({...TicketCreatorDTO, name: clientMessage});
            setWsMessages([...wsMessages,
                {sendClientId: "1", action: "message", msg: clientMessage, nick:''},{sendClientId: "1", action: "message", msg: "What`s your email?", nick: botNickName}])      
            setChatStep(ChatStep.ENTER_EMAIL);
           // setWsMessages([...wsMessages, {sendClientId: "1", action: "message", msg: clientMessage, nick:''}])
        }

        if (chatStep === ChatStep.ENTER_EMAIL) {
            let ticketDTO: TicketCreateDTO = {name: TicketCreatorDTO.name, email: clientMessage};
            setTicketCreatorDTO(ticketDTO);  //idk wht ...TicketCreatorDTO doesnt work  
            setWsMessages([...wsMessages, {sendClientId: "1", action: "message", msg: clientMessage, nick:''}])

            generateTicket(ticketDTO).then((response) => {
                setTicket(response.data);
                startChat(response.data);
            });
            setChatStep(ChatStep.CHAT_STARTED);
        }
    }

    const handleSend = (clientMessage) => {
        if (chatStep !== ChatStep.CHAT_STARTED) {
            handleCreationOfTicket(clientMessage);
            return;
        }
        if (ws.current !== null) {
            ws.current.send(JSON.stringify({Action: 'send', Content: clientMessage, Nickname: 'User'}));
        }
    };

    const handleReceive = (e, wsMessages/*, setWsMessages*/) => {
        var data = JSON.parse(e.data);
        if (data.Action === 1) // TODO: enum
            setWsMessages([...wsMessages, {action: 'message', msg: data.Content, nick: data.Nickname}]);
    };

    const loadMessages = () => { //TODO:WS
        
    };

    const startChat = (data) => {//TODO:connect to web sockets
        ws.current = new WebSocket(webSocketServerAddress);
        ws.current.onopen = e => {
            ws.current.send(JSON.stringify({
                Action: 'join',
                Content: data.ticketNumber
            }));
            setWsMessages([...wsMessages, {sendClientId: "1", action: "message", msg: "We are connecting you to a representative. Please wait...", nick: botNickName}]);
        };
        ws.current.onmessage = e => handleReceive(e, wsMessages, setWsMessages);
    };

    const addStartChatMessages = () => {
    setWsMessages([...wsMessages, {sendClientId: "1", action: "message", msg: ChatBotOpennigMessage, nick: botNickName},
        {sendClientId: "1", action: "message", msg: ChatBot2OpenningMessage, nick: botNickName}, 
        {sendClientId: "1", action: "message", msg: "What`s your name?", nick: botNickName}]);
    };

    //useEffect on load of component call addStartChatMessages
    var brLoad = 0;
    useEffect(() => {
        if (brLoad === 0)
            addStartChatMessages();
        brLoad++;
    }, [])

    useEffect(() => {
        console.log('new effect');
        if (ws.current)
            ws.current.onmessage = e => handleReceive(e, wsMessages, setWsMessages);
        /*return () => {
            ws.current?.close();
        };*/
    }, [wsMessages]);
    
    return(
        <div id="center-text">
            <div id="chat-circle"  className="btn btn-raised" onClick={() => setIsChatOpen(true)}>
                <div id="chat-overlay" ></div>
                <ChatIcon/>
            </div>

            { isChatOpen ? <ChatBox messages={wsMessages} onSend={handleSend} onClose={() => setIsChatOpen(false)}/> : <></> }
        </div>
    )
}

export default ChatBot;
