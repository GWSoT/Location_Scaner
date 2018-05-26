<template>
    <div>
        <router-view></router-view>
    </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';

@Component({
    components: {
    },
})
export default class DashboardRoot extends Vue {}
</script>

<style>

</style>
