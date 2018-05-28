import Vue from 'vue';
import { dashboardService } from '@/services/dashboard.service'; 
import { geoService  } from '@/services/geo.service';
import { Geolocation } from '@/models/geolocation.interface';
import { Post } from '@/models/post.interface';
const state = {
    profileData: {} as any,
    status: '',
    isBusy: false,
    geoError: '',
    formattedGeolocation: '',
    profilePosts: [] as Post[],
    geodata: { latitude: 0, longitude: 0 } as Geolocation,
};

const getters = {
    profileData: (profileState: any) => profileState.profileData, 
    isBusy: (profileState: any) => profileState.isBusy,
    geoError: (profileState: any) => profileState.geoError,
    geolocation: (profileState: any) => profileState.profileData.geolocation,
    formattedGeolocation: (profileState: any) => profileState.formattedGeolocation,
    profilePosts: (profileState: any) => profileState.profilePosts,
    geodata: (profileState: any) => profileState.geodata,

};

const actions = { 
    requestProfile: ({commit, dispatch}: {commit: any, dispatch: any}, id?: string) => {
        commit('profileRequest');
        dashboardService.getProfile(id)
        .subscribe((result: any) => {
            commit('profileSuccess', result);
            dispatch('updateFormattedGeolocation', result.geolocation)
            dispatch('profilePosts', id);
        },
        (errors: any) => {
            commit('profileError');
        })
    },
    geoError: ({commit, dispatch}: {commit: any, dispatch: any}, err: string) => {
        commit('geoError', err);
    },
    geoUpdate: ({commit, dispatch}: {commit: any, dispatch: any}, geodata: string) => {
        commit('geoUpdate', geodata);
    },
    updateFormattedGeolocation: ({commit, dispatch}: {commit: any, dispatch: any}, geodata: Geolocation) => {
        commit('geoUpdate', geodata);
        geoService.getFormattedGeodata(geodata)
        .subscribe((res: any) => {
            commit('formattedGeolocation', res);
        }, (errors: any) => {
            commit('geolocationError'); 
        });
    },
    profilePosts: ({commit, dispatch}: {commit: any, dispatch: any}, userId: string) => {
        dashboardService.getUserPosts(userId)
        .then((res: any) => {
            let tempPostArr = [] as Post[];
            res.data.forEach((post: any) => {
                tempPostArr.push({
                    postAuthor: post.postAuthor.firstName + " " + post.postAuthor.lastName,
                    postDate: post.postDate,
                    postId: post.postId,
                    postBody: post.postBody,
                    likesCount: post.likesCount,
                    isLikedByMe: post.isLikedByMe,
                } as Post);
            });
            commit('postsSuccess', tempPostArr);
        })
        .catch((err: any) => {
            commit('postsError');
        })
    },
    profilePost: ({commit, dispatch}: {commit: any, dispatch: any}, postBody: string) => {
        dashboardService.addPost(postBody)
        .subscribe((result: any) => {
            commit('postSuccess', {
                postAuthor: result.postAuthor.firstName + " " + result.postAuthor.lastName,
                postBody: result.postBody,
                postDate: result.postDate,
                postId: result.postId,
                likesCount: result.likesCount,
                isLikedByMe: result.isLikedByMe,
            } as Post);
        }, (errors: any) => {
            commit('postError');
        });
    },
    profilePostLikeRequest: ({commit, dispatch}: {commit: any, dispatch: any}, postId: string) => {
        dashboardService.likePost(postId)
        .subscribe((result: any) => {
            commit('likeSuccess', postId);
        }, (err: any) => {
            commit('likeError');
        });
    },
};

const mutations = {
    profileRequest: (profileState: any) => {
        profileState.isBusy = true;
        profileState.status = 'requesting user';
    },
    profileSuccess: (profileState: any, profileResp: any) => {
        profileState.isBusy = false;
        Vue.set(profileState, 'profileData', profileResp);
        profileState.status = 'success';
    },
    profileError: (profileState: any) => {
        profileState.isBusy = false;
        profileState.status = 'error';
    },
    geoError: (profileState: any, error: string) => {
        profileState.geoError = error;
    },
    formattedGeolocation: (profileState: any, formattedGeodata: string) => {
        profileState.formattedGeolocation = formattedGeodata;
    },
    geolocationError: (profileState: any) => {
        profileState.status = "Geolocation error";
    },
    geoUpdate: (profileState: any, geodata: Geolocation) => {
        profileState.profileData.geolocation = geodata;
        profileState.geodata = geodata;
    },
    postsError: (profileState: any) => {
        profileState.status = "Error while fetching user post";
    },
    postsSuccess: (profileState: any, posts: Post[]) => {
        profileState.status = "Success fetching user posts";
        profileState.profilePosts = posts;
    },
    postSuccess: (profileState: any, post: Post) => {
        profileState.status = "Successfully add new post.";
        profileState.profilePosts.push(post);
    },
    postError: (profileState: any) => {
        profileState.status = "Error while adding new post.";
    },
    likeSuccess: (profileState: any, postId: string) => {
        profileState.status = "Like request success";
        profileState.profilePosts.forEach((post: Post) => {
            if (post.postId === postId) {
                if (!post.isLikedByMe) {
                    post.isLikedByMe = true;
                    --post.likesCount;
                } else {
                    post.isLikedByMe = false;
                    --post.likesCount;
                };
            };
        });
    },
    likeError: (profileState: any) => {
        profileState.status = "Like error";
    },
};

export default {
    state, 
    getters,
    actions,
    mutations,
};