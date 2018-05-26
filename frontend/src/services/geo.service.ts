import {} from '@types/googlemaps';
import * as googleMaps from '@google/maps';

import { Observable } from 'rxjs/Rx';
import { BaseService } from './base.service';
import { Geolocation } from '../models/geolocation.interface';
import { userState } from '@/hubs/userstate.hub';
import store  from '@/store/store';
import axios from 'axios';
class GeoService extends BaseService {
    private static instance: GeoService;
    private geocoder!: any;

    private constructor() { 
        super(); 

    }

    public static get Instance() {
        return this.instance || (this.instance = new this());
    }

    public initialize() {
        this.geocoder = new google.maps.Geocoder();
        setInterval(() => {
            console.log('interval');
            this.getGeolocation()
            .then((result: Geolocation) => {
                store.dispatch('user/userGeolocation', result);
                userState.sendGeodata(result)
                .then((response: any) => {
                    store.dispatch('profile/updateFormattedGeolocation', result);
                });
            })
            .catch((err: string) => {
                store.dispatch('profile/geoError', err);
            })
        }, 30000)
    }

    public getGeolocation(): Promise<any> {
        return new Promise((resolve: any, reject: any) => [
            navigator.geolocation.getCurrentPosition((position: any) => {
                resolve({ 
                    longitude: position.coords.longitude,
                    latitude: position.coords.latitude
                } as Geolocation);
            },
            (error: any) => {
                switch(error.code) {
                    case error.PERMISSION_DENIED:
                        reject("User denied the request for Geolocation.");
                        break;
                    case error.POSITION_UNAVAILABLE:
                        reject("Location information is unavailable.");
                        break;
                    case error.TIMEOUT:
                        reject("The request to get user location timed out.");
                        break;
                    case error.UNKNOWN_ERROR:
                        reject("An unknown error occurred.");
                        break;
                }
            })
        ])
    }
    public getFormattedGeodata(geodata: Geolocation): Promise<any> {
        return new Promise((resolve: any, reject: any) => {
            this.geocoder.geocode({
                location: {
                    lat: geodata.latitude,
                    lng: geodata.longitude
                }
            }, function(results: google.maps.GeocoderResult[], status: google.maps.GeocoderStatus) {
                if (status === google.maps.GeocoderStatus.OK) {
                    resolve(results[0].formatted_address)
                } else {
                    reject("Error in obtaining formatted geocoder.");
                }
            });
        })
    }
}

export const geoService = GeoService.Instance;