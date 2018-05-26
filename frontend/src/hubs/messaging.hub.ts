import store from '../store/store';
import { HubConnection, IHubProtocol, ITransport, TransportType, HttpConnection } from '@aspnet/signalr'
import { CONFIGURATION } from '../constants/app.constants';
import { EventBus } from '@/event-bus';
import { MessageGroup, Message } from '@/models/messages.interface';


class MessagingHub {
    private _hubConnection: HubConnection;
    private isEstablished = false;
    private static instance: MessagingHub;

    public static get Instance()
    {
        return this.instance || (this.instance = new this())
    }

    private constructor() {
        console.log(store);
        var httpConnection = new HttpConnection(CONFIGURATION.baseUrls.hubUrl 
            + `/hub/messaging?auth_token=${localStorage.getItem('authToken')}`, { 
            transport: TransportType.LongPolling,
        });
        this._hubConnection = new HubConnection(httpConnection);
        this.registerOnServerMessages();
        this.startConnection();
    }

    public startConnection() {
        this._hubConnection.start()
        .then(() => {
            this.isEstablished = true;
            // Emit event on EventBus
            EventBus.$emit('chatInit');
            console.log("Chat hub connection created");
        })
        .catch(() => {
            this.isEstablished = false;
            console.log("Error while establishing connection");
            setTimeout(() => this.startConnection(), 5000);
        })
    }

    public subscribeToGroup(groupId: string) {
        this._hubConnection.invoke('SubscribeToGroup', groupId)
    }

    public unsubscribeFromGroup(groupId: string) {
        this._hubConnection.invoke('UnsubscribeFromGroup', groupId)
    }

    public sendMessage(messageBody: string, groupId: string) {
        console.log("Sending Message");
        this._hubConnection.invoke("SendMessage", messageBody, groupId);
    }

    public registerOnServerMessages() {
        this._hubConnection.on('NewMessage', (payload: any) => {
            store.commit('user/userMessages', {
                messageGroupId: payload.messageGroupId,
                message: {
                    messageBody: payload.messageBody,
                    messageFrom: payload.messageFrom,
                    messageTime: payload.messageTime,
                    isMyMessage: payload.isMyMessage
                } as Message
            } as MessageGroup);
        })
    }
}

export const messagingHub = MessagingHub.Instance;