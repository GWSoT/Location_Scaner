import { Geolocation } from './geolocation.interface';

export interface Meeting {
    meetingTime: string,
    meetingLocation: Geolocation,
    persons: Person[],
    formattedGeodata: string,
};

export interface Person {
    firstName: string,
    lastName: string, 
    id: string,
};