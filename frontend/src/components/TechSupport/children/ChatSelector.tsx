import { useState } from 'react';
import { ConversationPreview } from '../ConversationPreview';
import '../styles/style.css';

import ChatListEntry from './ChatListEntry';

export const ChatSelector = ({conversations, selectedConversation, onSelect}: {conversations: ConversationPreview[], selectedConversation: string, onSelect: (conversation: string) => void}) => {
    var [query, setQuery] = useState('');

    return <section className="discussions">
        <div className="discussion search">
            <div className="searchbar">
                <i className="fa fa-search" aria-hidden="true"></i>
                <input type="text" placeholder="Search..." value={query} onChange={e => setQuery(e.target.value)}/>
            </div>
        </div>
        { conversations.map(conversation => !query || conversation.userName?.toLowerCase().indexOf(query.toLowerCase()) !== -1 ? <ChatListEntry key={conversation.conversationId} conversation={conversation} isSelected={selectedConversation === conversation.conversationId} onSelect={onSelect}/> : null) }
    </section>;
};

export default ChatSelector;
