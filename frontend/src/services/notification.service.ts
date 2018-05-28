import firebase from 'firebase/app';
import 'firebase/messaging';
import { BaseService } from '@/services/base.service';
import store from '@/store/store';
import axios from 'axios';
import { Observable } from 'rxjs/Rx';
import { CONFIGURATION } from '@/constants/app.constants';

class NotificationService extends BaseService {
    private static instance: NotificationService;
    private messaging: firebase.messaging.Messaging;

    public static get Instance() {
        return this.instance || (this.instance = new this());
    }

    private constructor() {
        super();

        firebase.initializeApp({
            messagingSenderId: CONFIGURATION.firebase.messageSenderId
        })

        this.messaging = firebase.messaging();

        if ((Notification as any).permission === 'granted') {
            this.subscribe();
        }

                
        this.messaging.onMessage((payload: any) => {
            console.log("Message receieved: " + payload);
            new Notification(payload.notification.title, payload.notification);
        })

    }

    public subscribe() {

        this.messaging.requestPermission()
            .then(() => {
                this.messaging.getToken()
                    .then((currentToken: any) => {
                        console.log(currentToken);

                        if (currentToken) {
                            this.sendTokenToServer(currentToken);                    
                        } else {
                            store.dispatch('user/userRemoveDeviceId')
                        }
                        
                    })
                    .catch((err: any) => {
                        store.dispatch('user/userRemoveDeviceId')
                    });
            })
            .catch((err: any) => {
                console.warn(err);
            })
    }

    public deleteTokenFromServer(token: any): Observable<any> {
        return Observable.fromPromise(axios.get(`${this.api}/profile/deleteOldDeviceId?=${token}`))
                .catch((err: any) => this.handleError(err));
    }

    private sendTokenToServer(token: any) {
        if (!isTokenAlreadySent(token)) {
            axios.post(`${this.api}/profile/deviceId/`, {
                deviceId: token
            })
            .then((response: any) => {
                store.dispatch('user/userSetDeviceId', token);
            })
        }
    }


}

function isTokenAlreadySent(token: any): boolean {
    let equals = store.getters['user/deviceId'] === token;
    if (!equals) {
        notificationService.deleteTokenFromServer(store.getters['user/deviceId'])
        .subscribe((response: any) => {
            store.dispatch('user/userRemoveDeviceId');
        });
    }
    console.log("isTokenAlreadySent: " + equals);
    return equals;
}

export const notificationService = NotificationService.Instance;