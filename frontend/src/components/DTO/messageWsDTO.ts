export enum QuickSelector {
    Faq = "faq",
    CustomerSupport = "customersupport",
    Order = "order" 
};

export type MessageToServer = {
    action: "sendChatMessage",
    content: string,
    conversation: string,
    quickSelector: QuickSelector
} | {
    action: "newConversation"
} | {
    action: "getConversations"
} | {
    action: "getMessageHistory",
    conversation: string
};

export type MessageFromServer = Conversation | ChatMessage | User;

export type ChatMessage = {
    type: "chatMessage",
    id: string,
    conversation: string,
    author: string,
    content: string,
    timestamp: string
};

export enum ConversationStatus {
    Ongoing = "ongoing",
    Resolved = "resolved",
    Unresolved = "unresolved"
};
export type Conversation = {
    type: "conversation",
    id: string,
    status: ConversationStatus,
    participants: string[]
};

export type User = {
    type: "user",
    id: string,
    name: string
};
