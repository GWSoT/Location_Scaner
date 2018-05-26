import axios from 'axios';
import { BaseService } from "@/services/base.service";

class MessageService extends BaseService {
    private static instance: MessageService;

    private constructor() { super(); }

    public static get Instance() {
        return this.instance || (this.instance = new this());
    }

    public getConversations() {
        return axios.get(`${this.api}/profile/getConversations`);
    }

    private deleteConversation(conversationId: string) {
        return axios.post(`${this.api}/profile/deleteConversation?conversationId=${conversationId}`);
    }

    public createConversation(conversationName: string) {
        return axios.post(`${this.api}/profile/createConversation?name=${conversationName}`);
    }

    public inviteUserToConversation(userId: string, groupId: string) {
        return axios.post(`${this.api}/profile/inviteUserToConversation?userId=${userId}&groupId=${groupId}`);
    }

    public getMessages(conversationId: string) {
        return axios.get(`${this.api}/profile/getMessages?conversationId=${conversationId}`);
    }
}

export const messageService = MessageService.Instance;