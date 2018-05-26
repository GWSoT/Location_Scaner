<template>
    <div>
        <div class="row">
            <div class="container">
                <div ref="map" class="map"></div>
            </div>
        </div>
        <div class="row" v-for="(meeting, index) in meetings" v-bind:key="index">
            <div class="col">
                <span>
                    {{ meeting.formattedGeodata }}
                </span>
            </div>
            <div class="col-3">
                <span>
                    {{ formattedDate(meeting.meetingTime) }}
                </span>
            </div>
            <div class="col-2" v-for="(person, idx) in meeting.persons" v-bind:key="idx">
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

@Component({})
export default class FriendMap extends Vue {
    private friendMarkers = [] as google.maps.Marker[];
    private marker!: google.maps.Marker;
    private map!: google.maps.Map;
    private geolocation!: Geolocation;
    private meetings = [] as Meeting[];

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
        this.map.panTo(center ? center : location);

        this.setMarkers(location);

        this.getMeetings();
        
    }

    private formattedGeodata(geodata: Geolocation) {
        return geoService.getFormattedGeodata(geodata);
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

    private getMeetings() {
        dashboardService.getMeetings()
        .then((result: any) => {
            result.data.forEach((meeting: any) => {
                let tempPersons = [] as Person[];
                let meetingLoc = { 
                    latitude: meeting.meetingLocation.latitude,
                    longitude: meeting.meetingLocation.longitude,
                } as Geolocation;

                geoService.getFormattedGeodata(meetingLoc)
                .then((result: any) => {
                    meet.formattedGeodata = result;
                })

                meeting.friends.forEach((person: any) => {
                    tempPersons.push({
                        firstName: person.firstName,
                        lastName: person.lastName,
                        id: person.id,
                    } as Person);
                });

                let meet = {
                    meetingTime: meeting.meetingTime,
                    persons: tempPersons,
                } as Meeting;

                meet.meetingLocation = meetingLoc;
                this.meetings.push(meet);
            });
        })
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
</style>
