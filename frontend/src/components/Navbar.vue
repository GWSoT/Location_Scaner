<template>
<div>
<nav class="navbar navbar-expand-lg navbar-light bg-light">
    <a class="navbar-brand" href="#">MSMedia</a>
    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
    </button>

    <div class="collapse navbar-collapse">
        <div class="nav navbar-nav ml-auto">
            <router-link v-show="!isAuthenticated" class="nav-item nav-link" :to="{name: 'loginForm'}">Login</router-link>
            <router-link v-show="!isAuthenticated" class="nav-item nav-link" :to="{name: 'registerForm'}">Registration</router-link>
            <div @click="loadNotifications" v-show="isAuthenticated" class="nav-item nav-link notif"><i class="far fa-bell"></i></div>

            <a v-show="isAuthenticated" class="nav-item nav-link" href="javascript:void(0)" v-on:click="logout">Logout</a>
        </div>
        
    </div>

    </nav>
    <div  class="row">
        <div class="col-md-2 nav-pills">
            <nav class="nav flex-column nav-pills " aria-orientation="vertical">
                <router-link 
                    v-show="isAuthenticated"
                    class="nav-item nav-link active" 
                    :to="{name: 'dashboard', params: { 'id': id } }">My profile</router-link>

                <router-link 
                    v-show="isAuthenticated" 
                    class="nav-item nav-link" 
                    :to="{name: 'friends' }">Friends</router-link>

                <router-link 
                    v-show="isAuthenticated"
                    class="nav-item nav-link"
                    :to="{name: 'friendMap'}">Friend Map</router-link>
                <router-link 
                    v-show="isAuthenticated"
                    class="nav-item nav-link"
                    :to="{name: 'messages'}">Messages</router-link>
            </nav>
        </div>
        <div class="col">
            <slot></slot>
        </div>
    </div>
</div>
</template>

<script lang="ts">
import { userState } from '../hubs/userstate.hub';
import { Vue, Component } from 'vue-property-decorator'
import { mapGetters } from 'vuex';
@Component({
    computed: mapGetters({
        isAuthenticated: 'auth/isAuthenticated',
        id: 'user/profileId'
    }),
})
export default class Navbar extends Vue {
    private logout() {
     this.$store.dispatch('auth/authLogout').then(() => {
        this.$router.push('/');
     });
    }

    private loadNotifications() {

    }
}
</script>

<style lang="scss" scoped>

.notifications {
    position: relative;
    display: inline-block;
    cursor: pointer;
}

.notif:hover {
    transform: rotate(30deg);
    transition: 0.3s;
}

.notif {
    transition: 0.3s;
}
</style>
