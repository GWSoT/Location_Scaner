import { BaseService } from "@/services/base.service";
import axios from 'axios';
import { Observable } from 'rxjs/Rx';


class ProfileService extends BaseService {
    
    private static instance: ProfileService;

    private constructor() { super(); }

    public static get Instance() {
        return this.instance || (this.instance = new this());
    }

    public get(): Observable<any> {
        return Observable.fromPromise(axios.get(`${this.api}/profile/currentUser`))
            .map((result: any) => result.data)
            .catch((err: any) => this.handleError(err.response));
    }
}

export const profileService = ProfileService.Instance; 