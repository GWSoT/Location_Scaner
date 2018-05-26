import axios from 'axios'
import { Observable } from 'rxjs/Rx';
import { BaseService } from '@/services/base.service';
import { Credentials } from '../models/credentials.interface';
import store from '../store/store';
class AuthService extends BaseService {
    private static instance: AuthService;

    private constructor() { super(); }

    public static get Instance() {
        return this.instance || (this.instance = new this());
    }

    public login(credentials: Credentials): Observable<any> {
        return Observable.fromPromise(axios.post(`${this.api}/account/login`, credentials))
            .map((result: any) => result.data.authToken)
            .catch((error: any) => this.handleError(error));
    }
}

export const authService = AuthService.Instance;