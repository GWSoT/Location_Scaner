<template>
    <div class="messagebox">
        <div class="modal fade" id="Modal1" tabindex="-1" role="dialog" aria-labelledby="Modal1" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="ModalLabel">Invite your friends</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body" v-for="friend in friends" v-bind:key="friend.id">
                        <div class="friend__item" >
                            <router-link data-dismiss="modal" :to="{name: 'dashboard', params: {'id': friend.id}}">{{ friend.firstName }} {{ friend.lastName }}</router-link>
                            <button class="btn btn-primary" @click="inviteFriend(friend.id)">Invite</button> 
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="Modal2" tabindex="-1" role="dialog" aria-labelledby="Modal2" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="ModalLabe2">Input name of new conversation</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <input v-model="conversationName" type="text" class="form-control">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" @click="createConversation">Add</button>
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="messagebox__conversations">
            <div class="create__converstaion">
                <i class="far fa-edit mx-2 my-1" data-toggle="modal" data-target="#Modal2"></i>
                <i v-show="selectedConversation" class="fas fa-users mr-2" data-toggle="modal" data-target="#Modal1"></i>
                <i v-show="selectedConversation" class="far fa-times-circle" @click="deleteConversation"></i>
            </div>
            
            <div class="conversation__list" 
>
                <div class="conversation__item"
                    v-for="conversation in userConversations" 
                    v-bind:key="conversation.conversationId"
                    :class="[conversation.conversationId === selectedConversation ? 'conversation__item__selected' : '']"
                    @click="selectConversation(conversation.conversationId)">
                    {{conversation.groupName}}
                    <span><i class="fas fa-users" style="color: black !important"></i> {{conversation.membersCount}}</span>
                </div>
            </div>
        </div>

        <div class="messagebox__messagewindow">
            <div class="messagebox__messageslist" >
                <div class="messagebox__item p-2 m-2 text-left" :class="[message.isMyMessage ? 'messagebox__item__right' : 'messagebox__item__left']" v-for="({message}, index) in userMessages" v-bind:key="index">
                    {{ message.messageBody }}
                </div>
            </div>
            <div class="messagebox__control  form-group form-inline">
                <input v-model="message" placeholder="Type your message here..." type="text" class="form-control mr-2">
                <button class="btn btn-primary" @click="sendMessage">Send</button>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { messageService } from '@/services/message.service';
import { dashboardService } from '@/services/dashboard.service';
import { messagingHub } from '@/hubs/messaging.hub';
import { MessageGroup, Message, Conversation } from '@/models/messages.interface';
import store from '@/store/store';

@Component({})
export default class Messages extends Vue {
    private conversations = [];
    private selectedConversation = '';
    private friends = [];
    private conversationName = '';
    private message = '';

    private mounted() {
        this.getFriends();
    }

    private getFriends() {
        dashboardService.getFriends()
        .then((result: any) => {
            this.friends = result.data;
        })
    }

    private deleteConversation() {
        messageService.deleteConversation(this.selectedConversation);
        this.selectedConversation = '';
    }

    private inviteFriend(id: string) {
        if (!this.selectedConversation) return;
        messageService.inviteUserToConversation(id, this.selectedConversation)
        .then((response: any) => {
            console.log("Friend invite success");
        })
    }

    private get userMessages(): MessageGroup[] {
        console.log(this.selectedConversation);
        var messages = this.$store.getters['user/messages'](this.selectedConversation);
        console.log(messages);
        return messages;
    }

    private get userConversations(): Conversation[] {
        var conversations =  this.$store.getters['user/conversations'];
        console.log(conversations);
        return conversations;
    }

    private selectConversation(id: string) {
        this.selectedConversation = id;
        // console.log(this.$store.getters['user/messages'](this.selectedConversation));

    }

    private createConversation() {
        messageService.createConversation(this.conversationName)
        .then((response: any) => {
            console.log("Conversation created");
        })
    }

    private sendMessage() {
        messagingHub.sendMessage(this.message, this.selectedConversation);
    }
}
</script>

<style scoped>
.messagebox {
    display: flex;
    width: 100%;
    height: 450px;
    border: 1px dotted black;
}

.messagebox__messageslist {
    overflow: auto;
    display: inline-block;
    width: 100%;
    height: 368px;
}

.messagebox__messagewindow {
    display: block;
    width: 100%;
}

.conversation__list {
    display: block;
    width: 150px;
}

.conversation__item__selected {
    background: #419FD9 !important;
}

.conversation__item {
    color: black;
    cursor: pointer;
    width: 100%;
    height: 60px;
    background: white;
}

.messagebox__item {
    color: gray;
    width: 50%;
    background: lightskyblue;
    border-radius: 10px;
}

.messagebox__control {
    display: block;
    height: 60px;
}

.messagebox__item__right {
    float: right;
}

.messagebox__item__left {
    float: left;
}

.messagebox__conversations {
    align-content: flex-start;
}

.far, .fas {
    color: #007BFF !important;
    cursor: pointer;
}
</style>
