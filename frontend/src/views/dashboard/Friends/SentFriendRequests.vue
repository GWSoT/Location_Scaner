<template>
   <div class="row">
        <div class="col">
            <input />

        <div v-for="(friend, index) in friends" v-bind:key="index">
            <div class="row">
                <span>{{friend.firstName}} {{friend.lastName}} {{ friend.id }} {{ friend.requestId }}</span>
                <div>
                    <button class="btn btn-primary">Cancel</button>
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
export default class SentFriendRequests extends Vue { 
    private friends = [];

    private created() {
        dashboardService.getSentFriendRequest()
        .then((response: any) => this.friends = response.data);
    }
}
</script>

<style>

</style>
