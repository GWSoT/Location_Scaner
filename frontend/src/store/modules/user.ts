import { profileService } from "@/services/profile.service";
import Vue from 'vue';
import { Geolocation } from "@/models/geolocation.interface";
import { userState } from "@/hubs/userstate.hub";
import { MessageGroup, Conversation, Message } from "@/models/messages.interface";
import { messageService } from '@/services/message.service';
import { messagingHub } from '@/hubs/messaging.hub';

const state = {
    isSubscribed: false,
    profile: {},
    id: localStorage.getItem('_id') || '',
    status: '',
    deviceId: localStorage.getItem('_deviceId') || '',
    geolocation: localStorage.getItem('_geo') || {},
    messages: [] as MessageGroup[],
    conversations: [] as Conversation[],
};

const getters = {
    profile: (userState: any) => userState.profile,
    profileId: (userState: any) => userState.id,
    deviceId: (userState: any) => userState.deviceId,
    userGeo: (userState: any) => JSON.parse(userState.geolocation),
    messages: (userState: any) => {
        return (keyword: string) => userState.messages.filter((item: MessageGroup) => {
            return item.messageGroupId === keyword;
        }) as MessageGroup[];
    },
    conversations: (userState: any) => userState.conversations,
    isSubscribed: (userState: any) => userState.isSubscribed,
};

const actions = {
    userRequest: ({commit, dispatch} : {commit: any, dispatch: any}) => {
        commit('userRequest');
        profileService.get()
        .subscribe((result: any) => {
            commit('userSuccess', result);
            localStorage.setItem('_id', result.id);
        }, 
        (errors: any) => {
            localStorage.removeItem('_id');
            commit('userError');
            dispatch('auth/authLogout', null, { root: true });
        })
    },
    userSetDeviceId: ({commit, dispatch}: {commit: any, dispatch: any}, deviceId: any) => {
        localStorage.setItem("_deviceId", deviceId);
        commit('userDeviceIdAdd', deviceId);
    },
    userRemoveDeviceId: ({commit, dispatch}: {commit: any, dispatch: any}) => {
        localStorage.removeItem('_deviceId');
        commit('userDeviceIdRemove');
    },
    userGeolocation: ({commit, dispatch}: {commit: any, dispatch: any}, geolocation: Geolocation) => {
        console.log(geolocation);
        localStorage.setItem('_geo', JSON.stringify(geolocation));
        commit('userAddGeolocation', geolocation);
    },
    userSubscribeToConversation: ({commit, dispatch}: {commit: any, dispatch: any}) => {
        messageService.getConversations()
        .then((result: any) => {
          result.data.forEach((conversation: any) => {
            commit('userConversations', {
                conversationId: conversation.id,
                groupName: conversation.groupName,
                membersCount: conversation.membersCount,
            } as Conversation);

            messagingHub.subscribeToGroup(conversation.id);

            messageService.getMessages(conversation.id)
            .then((messages: any) => {
              messages.data.forEach((message: any) => {
                commit('userMessages', {
                    messageGroupId: conversation.id,
                    message: {
                        messageFrom: message.messageFrom,
                        messageTime: message.messageTime,
                        messageBody: message.messageBody,
                        isMyMessage: message.isMyMessage,
                    } as Message
                  } as MessageGroup);
              });
            })
          });
        })
    },
};

const mutations = {
    userRequest: (userState: any) => {
        userState.status = 'loading user profile'
    },
    userSuccess: (userState: any, userResp: any) => {
        userState.status = 'success';
        userState.id = userResp.id;
        Vue.set(userState, 'profile', userResp);
    },
    userError: (userState: any) => {
        userState.status = 'error';
    },
    userDeviceIdAdd: (userState: any, deviceId: any) => {
        userState.deviceId = deviceId;
    },
    userDeviceIdRemove: (userState: any) => {
        userState.deviceId = '';
    },
    userAddGeolocation: (userState: any, geolocation: Geolocation) => {
        userState.geolocation = JSON.stringify(geolocation);
    },
    userMessages: (userState: any, newMessage: MessageGroup) => {
        userState.messages.push(newMessage);
    },
    userConversations: (userState: any, newConversation: Conversation) => {
        userState.conversations.push(newConversation);
    },
    userSubscriptionSuccess: (userState: any) => {
        userState.isSubscribed = true;
    }
};

export default {
    state,
    getters,
    actions,
    mutations,
};
