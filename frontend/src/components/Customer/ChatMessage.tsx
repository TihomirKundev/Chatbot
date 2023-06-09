﻿import './style/style.css';

const ChatMessage = ({message, fromSelf} : {message: string, fromSelf: boolean}) => 
    <div className={`chat-msg ${fromSelf ? 'self' : 'user'}`}>
        <span className="msg-avatar"></span>
        <div className="cm-msg-text">{message}</div>
    </div>;

export default ChatMessage;
