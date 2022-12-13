import './style/style.css';
import Linkify from 'react-linkify';
const ChatMessage = ({message, fromSelf} : {message: string, fromSelf: boolean}) => 
    <div style={{wordBreak:"break-word"}} className={`chat-msg ${fromSelf ? 'self' : 'user'}`}>
        <span className="msg-avatar"></span>
        <div className="cm-msg-text"><Linkify>{message}</Linkify></div>
    </div>;

export default ChatMessage;
