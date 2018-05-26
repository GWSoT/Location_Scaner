export interface MessageGroup {
    messageGroupId: string,
    message: Message;
};

export interface Message {
    messageFrom: any;
    isMyMessage: boolean,
    messageTime: string,
    messageBody: string,
};

export interface Conversation {
    conversationId: string,
    groupName: string,
    membersCount: number,
};