<template>
    <div v-show="!isBusy" >
        <div v-if="geoError" class="alert alert-danger">
            {{ geoError }}
        </div>
        <div class="row">
            <div class="col-4">
                <div class="card">
                <img src="http://via.placeholder.com/280x320">
                    <div class="card-body">
                        <h3>{{profileData.firstName}} {{profileData.lastName}}</h3>
                            <button 
                            v-show="!profileData.isMyProfile && !profileData.isFriend" 
                            class="btn btn-primary btn-rounded"
                            @click="addFriend">Add as friend</button>
                    </div>
                </div>
            </div>
            <div class="col">
                <div class="row">
                    <div class="col-md-5 text-left">
                        <span class="h3">{{profileData.firstName}} {{profileData.lastName}}</span>
                    </div>
                    <div class="col-md-2 offset-md-5">
                        <span class="text-right">{{profileData.isOnline ? 'Online' : 'Offline'}}</span>
                    </div>
                </div>
                <div class="row my-4">
                    <div class="col">
                        <router-link :to="{name: 'friendMap', query: {
                                'lat': geolocation.latitude, 
                                'long': geolocation.longitude,
                            }}">
                            <i class="fas fa-map-marker-alt"></i> 
                            Location: {{ geodata }}
                        </router-link>
                    </div>
                </div>
                <button 
                    v-show="profileData.isMyProfile"
                    class="btn btn-primary"
                    @click="subscribe">Subscribe to notifications!</button> 

                <div class="row my-3 d-flex justify-content-center">
                    <div class="col-8" v-show="profileData.isMyProfile">
                    <textarea v-model="messageBody" class="form-control" multiline></textarea>
                    <button @click="addPost" class="btn btn-primary my-2">Add Post</button>
                    </div>
                </div>
                <div class="row" v-for="profilePost in profilePosts" v-bind:key="profilePost.postId">
                    <div class="col">
                        <div class="post">
                            <div class="text-left d-flex align-items-center pt-3">
                                <img class="post__avatar" src="http://via.placeholder.com/60x60" />
                                <div class="post__info justify-content-center ml-2">
                                <div class="row align-items-start">
                                    <div class="col">
                                        <span>{{ profilePost.postAuthor }}</span>
                                    </div>
                                </div>
                                <div class="row align-items-end">
                                    <div class="col">
                                        <span>{{ formattedDate(profilePost.postDate) }}</span>
                                    </div>
                                </div>
                                </div>
                            </div>
                            <hr /> 
                            <div class="post__body text-left">
                                {{ profilePost.postBody }}
                            </div>
                            <hr />
                            <div class="post__control">
                                <button 
                                    class="btn"
                                    :class="[profilePost.isLikedByMe ? 'btn-primary' : 'btn-outline-primary']"
                                    @click="likePost(profilePost.postId)">
                                    <i class="fa-heart"
                                        :class="[profilePost.isLikedByMe ? 'fas' : 'far']"></i>
                                </button>
                                <span class="ml-2">{{ profilePost.likesCount }}</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { dashboardService } from '@/services/dashboard.service';
import { mapGetters } from 'vuex';
import store from '../../store/store';
import { geoService } from '@/services/geo.service';
import { Geolocation } from '@/models/geolocation.interface';
import firebase from 'firebase'
import messaging from '@firebase/messaging'
import { Notification } from 'rxjs';
import { readableDate } from '@/helpers/utility';
import { notificationService } from '@/services/notification.service';

@Component({
    computed: mapGetters({
        isBusy: 'profile/isBusy',
        profileData: 'profile/profileData',
        geoError: 'profile/geoError',
        geodata: 'profile/formattedGeolocation',
        profilePosts: 'profile/profilePosts',
        geolocation: 'profile/geodata',
    }),

    beforeRouteUpdate (to: any, from: any, next: any) {
        store.dispatch('profile/requestProfile', to.params.id);
        console.log(this.profileData);
        next();
    },
    beforeRouteEnter (to: any, from: any, next: any) {
        store.dispatch('profile/requestProfile', to.params.id); 
        next();
    },
})
export default class Dashboard extends Vue {
    private messageBody = '';

    private addFriend() {
        dashboardService.addFriend(this.$route.params.id || '')
        .then((response: any) => {});
    }

    private addPost() {
        if (!this.messageBody.trim()) return;
        this.$store.dispatch('profile/profilePost', this.messageBody);
    }

    private likePost(id: string) {
        this.$store.dispatch('profile/profilePostLikeRequest', id)
    }

    private sendMessage() {
        // TODO: Send message
    }

    private subscribe() {
        notificationService.subscribe();
    }

    private formattedDate(postDate: string) {
        return readableDate(postDate);
    }
}
</script>

<style scoped>
.btn-outline-primary {
    border-radius: 50px;
}

.btn-primary  {
    border-radius: 50px;
}

.post__avatar {
    border-radius: 50%;
}

.post__control {
    align-items: flex-start;
    float: left;
}
</style>
