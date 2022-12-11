import LetteredAvatar from 'react-lettered-avatar';
import { Conversation, ConversationStatus } from "../../DTO/messageWsDTO";
import { ConversationPreview } from '../ConversationPreview';

export const ChatListEntry = ({conversation, isSelected, onSelect} : {conversation: ConversationPreview, isSelected: boolean, onSelect: (conversation: string) => void}) =>
    <div className={`discussion ${isSelected ? 'message-active' : ''}`} onClick={() => onSelect(conversation.conversationId)}>
        <div className="photo">
            <LetteredAvatar name={conversation.userName}/>
            {conversation.status === ConversationStatus.Ongoing ? <div className="online"></div> : <></>}
        </div>
        <div className="desc-contact">
            <p className="name">{conversation.userName}</p>
            <p className="message">{conversation.lastMessageContent}</p>
        </div>
    </div>;

export default ChatListEntry;
