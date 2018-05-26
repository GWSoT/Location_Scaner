import axios from 'axios';
import { Observable } from 'rxjs/Rx';

import { BaseService } from '@/services/base.service';
import { Register } from '../models/register.interfaces';

class RegisterService extends BaseService {
    private static instance: RegisterService;

    private constructor() { super(); }

    public static get Instance() {
        return this.instance || (this.instance = new this());
    }

    public register(registerModel: Register): Observable<any> {
        return Observable.fromPromise(axios.post(`${this.api}/account/register`, registerModel))
            .map((result: any) => true)
            .catch((error: any) =>  this.handleError(error));
    }
}

export const registerService = RegisterService.Instance;