<template>
<div>
    <h1>Title</h1>
    <button @click="geoInfo">Hello</button>
</div>  
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({

})
export default class Geo extends Vue  {
    private geoInfo () {
        navigator.geolocation.getCurrentPosition((position) => {
        alert(position.coords.latitude + " | " + position.coords.longitude + " | " + position.coords.accuracy)
        }, e => {}, {
            enableHighAccuracy: true
        });
    }
}
</script>

<style>

</style>
