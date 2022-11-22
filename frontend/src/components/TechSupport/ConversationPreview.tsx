import { ConversationStatus } from "../DTO/messageWsDTO";

export type ConversationPreview = {
    conversationId: string,
    status: ConversationStatus,
    lastMessageContent: string,
    lastMessageTimestamp: string,
    userName: string
};

export type ConversationDetails = {
    name: string,
    messages: {
        id: string,
        sender: string,
        message: string,
        timestamp: string
    }[]
};
