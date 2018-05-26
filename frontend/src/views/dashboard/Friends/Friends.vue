<template>
    <div class="row">
        <div class="col">
            <input />

        <div v-for="(friend, index) in friends" v-bind:key="index">
            <div class="row">
                <router-link :to="{name: 'dashboard', params: {'id': friend.id}}">{{friend.firstName}} {{friend.lastName}}</router-link>
            </div>
        </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { mapGetters } from 'vuex';
import { dashboardService } from '../../../services/dashboard.service';

@Component({})
export default class Friends extends Vue { 
    private friends = [];

    private created() {
        dashboardService.getFriends('')
        .then((result: any) => this.friends = result.data);
    }
}
</script>

<style>

</style>
