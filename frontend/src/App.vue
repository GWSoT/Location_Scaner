<template>
<div id="app" class="container">
    <Navbar><router-view/></Navbar>

</div>
</template>
<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import Navbar from '@/components/Navbar.vue';
import { geoService } from '@/services/geo.service';
import {} from '@types/googlemaps';
import * as googleMaps from '@google/maps';
import { userState } from './hubs/userstate.hub';
import { EventBus } from './event-bus';
import store from './store/store';
@Component({
  components: {
    Navbar,
  },
})
export default class App extends Vue {
  private created() {
    geoService.initialize();

    EventBus.$on('hubInit', (payload: any) => {
      console.log("Initialized");
      this.startHubNotify();
    })

    EventBus.$on('chatInit', (payload: any) => {
      store.dispatch('user/userSubscribeToConversation');
      console.log("created chat");
    });
  }

  private startHubNotify() {
    setInterval(() => {
      console.log("StartHubNotify")
      userState.sendUpdateStatusOnline() 
    }, 60000);
  }
}
</script>

<style lang="scss">
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
// #nav {
//   padding: 30px;
//   a {
//     font-weight: bold;
//     color: #2c3e50;
//     &.router-link-exact-active {
//       color: #bcdbcd;
//     }
//   }
// }
</style>
