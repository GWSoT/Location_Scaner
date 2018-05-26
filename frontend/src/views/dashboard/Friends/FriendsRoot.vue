<template>
  <div>
      <div class="nav">
        <router-link :to="{name: 'friends'}" class="nav-item nav-link">Friends</router-link>
        <router-link :to="{name: 'receievedFriends'}" class="nav-item nav-link">Incoming friend requests</router-link>
        <router-link :to="{name: 'sentFriendRequests'}" class="nav-item nav-link">Sent friend requests</router-link>
      </div>
      <router-view></router-view>
  </div>
</template>

<script>
import { Vue, Component } from 'vue-property-decorator';

@Component({})
export default class FriendsRoot extends Vue {}
</script>

<style>

</style>
