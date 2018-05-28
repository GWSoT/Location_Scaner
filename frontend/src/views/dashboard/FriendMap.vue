<template>
    <div>
        <div class="row">
            <div class="container">
                <div ref="map" class="map"></div>
            </div>
        </div>
        <div class="history my-2 py-2">
            <div class="history__heading">
                <span class="h2">
                    Get History Geolocation
                </span>
            </div>
            <div class="col">
                <div class="form-inline">
                    <label>Date Time</label>
                    <input type="date" class="form-control mx-2" v-model="dateTime"/>
                    <label>Time </label>
                    <input type="time" class="form-control mx-2" v-model="hour"/>
                    <button class="btn btn-outline-primary ml-auto" @click="loadHistoricalData">Get data</button>
                </div>
                <hr />
                <div class="row my-2" v-for="(historicalGeolocation, idx) in historicalData" v-bind:key="idx">
                    <div class="col">{{ historicalGeolocation.geolocation.latitude }}</div>
                    <div class="col">{{ historicalGeolocation.geolocation.longitude }}</div>
                    <div class="col">{{ formattedDate(historicalGeolocation.dateTime) }}</div>
                </div>
            </div>

            <div v-show="isMarkersExists" ref="historyMap" class="map"></div>
        </div>
        <div class="row" v-for="(meeting, index) in meetings" v-bind:key="index">
            <div class="col">
                <span>
                    {{ meeting.meetingLocation.longitude }} {{ meeting.meetingLocation.latitude }}
                </span>
            </div>
            <div class="col-3">
                <span>
                    {{ formattedDate(meeting.meetingTime) }}
                </span>
            </div>
            <div class="col-2" v-for="(person, idx) in meeting.friends" v-bind:key="idx">
                <router-link :to="{name: 'dashboard', params: {'id': person.id}}">
                    {{ person.firstName }} {{ person.lastName }}
                </router-link>
            </div>
        </div>
        
    </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { Geolocation } from "@/models/geolocation.interface";
import { Meeting, Person } from '@/models/meeting.interface';
import { dashboardService } from '@/services/dashboard.service';
import { geoService } from '@/services/geo.service';
import { readableDate } from '@/helpers/utility';
import { HistoryGeolocation } from '@/models/history.geolocation';

@Component({})
export default class FriendMap extends Vue {
    private friendMarkers = [] as google.maps.Marker[];
    private marker!: google.maps.Marker;
    private map!: google.maps.Map;
    private historyMap!: google.maps.Map;
    private geolocation!: Geolocation;

    private historicalData = [] as HistoryGeolocation[];
    private poly!: google.maps.Polyline;
    private coordinates = [] as any;     
    private polymarkers = [] as google.maps.Marker[];
    private dateTime = '';
    private hour = '';
    private isMarkersExists = false;

    private mounted() {
        this.geolocation = this.$store.getters['user/userGeo'];
        console.log(this.geolocation);

        let location = new google.maps.LatLng(this.geolocation.latitude, this.geolocation.longitude);;
        let center = null;

        if (this.$route.query.lat && this.$route.query.long) {
            center = new google.maps.LatLng(
                Number.parseFloat(this.$route.query.lat), 
                Number.parseFloat(this.$route.query.long)
            );
        }
        var mapProps = {
            center: center ? center : location,
            zoom: 15,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
        };
        this.map = new google.maps.Map(this.$refs.map as Element, mapProps);
        this.historyMap = new google.maps.Map(this.$refs.historyMap as Element, mapProps);
        this.map.panTo(center ? center : location);

        this.setMarkers(location);

        this.$store.dispatch('user/userRequestMeetings');
    }

    private setMarkers(location: google.maps.LatLng) {
        if (!this.marker) {
            this.marker = new google.maps.Marker({
                position: location,
                map: this.map,
                title: "You",
                label: "Me"
            })
        } else {
            //this.marker.setPosition(location);
        }
        this.setFriendMarkers();
    }

    private setFriendMarkers() {
        dashboardService.getFriendGeolocation()
        .then((result: any) => {
            console.log(result.data);
            for(var i = 0; i < result.data.length; i++) {
                this.friendMarkers.push(new google.maps.Marker({
                    position: new google.maps.LatLng(result.data[i].geolocation.latitude, result.data[i].geolocation.longitude),
                    map: this.map,
                    title: result.data[i].fullName,
                    label: "F",
                }));
                var from = new google.maps.LatLng(this.geolocation.latitude, this.geolocation.longitude);
                var to = new google.maps.LatLng(result.data[i].geolocation.latitude, result.data[i].geolocation.longitude);
                var dist = google.maps.geometry.spherical.computeDistanceBetween(from, to);
                console.log(dist);
            }
        })
    }

    private loadHistoricalData() {
        dashboardService.getHistoricalData(this.dateTime, this.hour)
        .then((result: any) => {
            this.isMarkersExists = true;
            this.historicalData = [] as HistoryGeolocation[];
            this.coordinates = [];

            result.data.forEach((elem: any) => {
                var latLng = new google.maps.LatLng(elem.latitude, elem.longitude);
                this.addNewHistoricalRecord(elem);
                this.addNewRecordToCoordinates(elem);
                this.addNewMapMarkerToPolymarkersArray(latLng, elem);
            });
            
            this.setUpPolyline();
            this.poly.setMap(this.historyMap);
        })
    }

    private get meetings() {
        return this.$store.getters['user/meetings'];
    } 

    private addNewRecordToCoordinates(elem: any) {
        this.coordinates.push({
            lat: elem.latitude,
            lng: elem.longitude,
        })
    }

    private addNewHistoricalRecord(elem: any) {
        this.historicalData.push({
            dateTime: elem.dateTime,
            geolocation: {
                latitude: elem.latitude,
                longitude: elem.longitude,
            } as Geolocation,
        } as HistoryGeolocation);
    }

    private setUpPolyline() {
        this.poly = new google.maps.Polyline({
            path: this.coordinates,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 1.0,
            strokeWeight: 2
        });
    }

    private addNewMapMarkerToPolymarkersArray(latLng: google.maps.LatLng, elem: any) {
        this.polymarkers.push(new google.maps.Marker({
            position: latLng,
            title: "#" + elem.dateTime,
            map: this.historyMap
        }));
    }

    private formattedDate(date: string) {
        return readableDate(date);
    }
} 
</script>

<style scoped>
.map {
    height: 400px;
    width: 100%;
}

.history {
    width: 100%;
    height: 20%;
    box-shadow: 0 0.7px 1px 1px rgba(0, 0, 0, 0.2);
}

</style>
