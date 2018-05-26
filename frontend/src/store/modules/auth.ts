import { Credentials } from '../../models/credentials.interface';
import { authService } from '../../services/auth.service';
const state = { 
    token: localStorage.getItem('authToken') || '', 
    status: ''
}

const getters = {
    isAuthenticated: (authState: any) => !!authState.token,
    authStatus: (authState: any) => authState.status,
    authToken: (authState: any) => authState.token
}

const actions = {
    authRequest: ({commit, dispatch}: {commit: any, dispatch: any} , credentials: Credentials) => {
        return new Promise((resolve, reject) => {
            commit('authRequest');
            authService.login(credentials)
            .subscribe((result: any) => {
                localStorage.setItem('authToken', result);
                commit('authSuccess', result);
                dispatch('user/userRequest', null, { root: true });
                resolve(result);
            }, (errors: any) => {
                commit('authError');
                localStorage.removeItem('authToken');
                reject(errors);
            });
        });
    },
    authLogout: ({commit, dispatch}: {commit: any, dispatch: any}) => {
        return new Promise((resolve, reject) => {
            commit('authLogout');
            localStorage.removeItem('authToken');
            localStorage.removeItem('_id');
            dispatch('user/userRemoveDeviceId', null, { root: true });
            resolve();
        })
    }
}

const mutations = {
    authRequest: (authState: any) => {
        authState.status = 'Authenticating';
    }, 
    authSuccess: (authState: any, authToken: string) => {
        authState.token = authToken;
        authState.status = 'Authentication success';
    },
    authError: (authState: any) => {
        authState.status = 'error';
    },
    authLogout: (authState: any) => {
        authState.token = '';
    }
}

export default {
    state, 
    getters, 
    actions,
    mutations
}