import { HubConnection, IHubProtocol, ITransport, TransportType, HttpConnection } from '@aspnet/signalr'
import { CONFIGURATION } from '../constants/app.constants';
import { EventBus } from '@/event-bus';
import store from '../store/store';
import { Geolocation } from '@/models/geolocation.interface';

class UserState {
    private _hubConnection: HubConnection;
    private isEstablished = false;
    private static instance: UserState;

    public static get Instance()
    {
        return this.instance || (this.instance = new this())
    }

    private constructor() {
        var httpConnection = new HttpConnection(CONFIGURATION.baseUrls.hubUrl 
            + `/hub/userStatus?auth_token=${store.getters["auth/authToken"]}`, { 
            transport: TransportType.LongPolling,
        });
        this._hubConnection = new HubConnection(httpConnection);
        this.startConnection();
    }

    

    public startConnection() {
        this._hubConnection.start()
        .then(() => {
            this.isEstablished = true;
            // Emit event on EventBus
            EventBus.$emit('hubInit');
            console.log("hub connection created");
        })
        .catch(() => {
            this.isEstablished = false;
            console.log("Error while establishing connection");
            setTimeout(() => this.startConnection(), 5000);
        })
    }

    public stopConnection() {
        this._hubConnection.stop();
    }

    public sendGeodata(geolocation: Geolocation) {
        return this._hubConnection.invoke("UpdateGeodata", geolocation);
    }

    public sendUpdateStatusOnline() {
        this._hubConnection.invoke('OnUserUpdateStatus');
    }

    public sendUserStatusOffline() {
        this._hubConnection.invoke('OnUserLogout');
    }

    private onUserStatusCheck() {
        this.sendUpdateStatusOnline();
    }

    private userLogout() {
        this.sendUserStatusOffline();
    }
}

export const userState = UserState.Instance;