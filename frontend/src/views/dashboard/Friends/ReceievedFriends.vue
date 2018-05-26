<template>
    <div class="row">
        <div class="col">
            <input />

            <div v-for="(friend, index) in friends" v-bind:key="index">
                <div class="row justify-content-center">
                    <div>{{friend.firstName}} {{friend.lastName}}            
                        <button class="btn btn-primary btn-sm" @click="acceptFriend(friend.id, friend.requestId)">Accept</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { dashboardService } from '@/services/dashboard.service';
@Component({})
export default class ReceievedFriends extends Vue {
    private friends = [];

    private created() {
        dashboardService.getReceievedFriends()
        .then((result: any) => this.friends = result.data);
    }

    private acceptFriend(id?: string, friendRequestId?: string) {
        dashboardService.acceptFriend(id, friendRequestId)
        .then((result: any) => {});
    }
 };
</script>

<style>

</style>
