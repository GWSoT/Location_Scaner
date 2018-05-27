import { BaseService } from "@/services/base.service";
import axios from 'axios';
import store from '@/store/store';
import { Observable } from 'rxjs/Rx';
import profile from "@/store/modules/profile";

class DashboardService extends BaseService {
    
    private static instance: DashboardService;

    private constructor() { super(); }

    public static get Instance() {
        return this.instance || (this.instance = new this());
    }

    public getProfile(id?: string): Observable<any> {
        return Observable.fromPromise(axios.get(`${this.api}/profile/profile/${id || ''}`,
        {             
            headers: {
                'Authorization' : `Bearer ${store.getters['auth/authToken']}`
            }
        }))
        .map((response: any) => response.data)
        .catch((err: any) => this.handleError(err));
    }

    public addFriend(id?: string): Promise<any> {
        return axios.post(`${this.api}/profile/addFriend/${id || ''}`,
        {             
            headers: {
                'Authorization' : `Bearer ${store.getters['auth/authToken']}`
            },
        });
    }

    public getFriends(id?: string): Promise<any> {
        return axios.get(`${this.api}/profile/friends/${id || ''}`, 
        {             
            headers: {
                'Authorization' : `Bearer ${store.getters['auth/authToken']}`
            },
        });
    }
    
    public getReceievedFriends(id?: string) {
        return axios.get(`${this.api}/profile/receievedFriends/${id || ''}`,
        {             
            headers: {
                'Authorization' : `Bearer ${store.getters['auth/authToken']}`
            },
        });
    }

    public acceptFriend(id?: string, friendRequestId?: string) {
        return axios.post(`${this.api}/profile/acceptFriend/${id || ''}?friendRequestId=${friendRequestId}`,
        {             
            headers: {
                'Authorization' : `Bearer ${store.getters['auth/authToken']}`
            },
        });
    }

    public getSentFriendRequest(id?: string) {
        return axios.get(`${this.api}/profile/sentFriendRequest/${id || ''}`,
        {             
            headers: {
                'Authorization' : `Bearer ${store.getters['auth/authToken']}`
            },
        });
    }

    public getFriendGeolocation(): Promise<any> {
        return axios.get(`${this.api}/profile/friendGeolocation`,
        {             
            headers: {
                'Authorization' : `Bearer ${store.getters['auth/authToken']}`
            },
        });
    }

    public getUserPosts(id?: string): Promise<any> {
        return axios.get(`${this.api}/profile/getPosts?userId=${id}`,
        {             
            headers: {
                'Authorization' : `Bearer ${store.getters['auth/authToken']}`
            },
        });
    }

    public addPost(postBody: string) {
        return axios.post(`${this.api}/profile/addPost`,
        {  
            postBody: postBody,           
            headers: {
                'Authorization' : `Bearer ${store.getters['auth/authToken']}`
            },
        });
    }

    public likePost(postId: string) {
        return axios.post(`${this.api}/profile/likePost?postId=${postId}`);
    }
    
    public getMeetings(): Promise<any> {
        return axios.get(`${this.api}/profile/getMeetings`)
    }

    public getHistoricalData(date: string, hour: string): Promise<any> {
        return axios.get(`${this.api}/profile/getHistory?date=${date}&hour=${hour}`);
    }
}


export const dashboardService = DashboardService.Instance; 