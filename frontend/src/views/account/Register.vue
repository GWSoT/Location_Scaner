<template>
<div class="container">
    <div class="form-row text-left justify-content-md-center">
        <div class="col col-lg-2">
            <div class="form-group">
                <label for="firstName">First name</label>
                <input type="text" id="firstName" v-model="registerModel.firstName" class="form-control"/>
            </div>
        </div>
        <div class="col col-lg-2">
            <div class="form-group">
                <label for="lastName">Last name</label>
                <input type="text" id="lastName" v-model="registerModel.lastName" class="form-control"/>
            </div>
        </div>
    </div>
    <div class="form-row text-left justify-content-md-center">
        <div class="col col-lg-4">
            <div class="form-group">
                <label for="email">Email</label>
                <input type="text" id="email" class="form-control" v-model="registerModel.email">
            </div>
            <div class="form-group">
                <label for="password">Password</label>
                <input type="text" id="password" class="form-control" v-model="registerModel.password">
            </div>
            <div class="form-group">
                <label for="passwordConfirm">Password confirm</label>
                <input type="text" id="passwordConfirm" class="form-control" v-model="registerModel.passwordConfirm"/>
            </div>
            <div class="form-group">
                <label for="dateOfBirth">Date of brith</label>
                <input type="date" id="dateOfBirth" class="form-control" v-model="registerModel.dateOfBirth"/>
            </div>
            <div class="">
                <button class="btn btn-primary" @click="handleSubmit">Register</button>
                <router-link :to="{name: 'loginForm'}" class="btn btn-default ml-auto">Login</router-link>
            </div>
        </div>
    </div>
</div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Register } from '../../models/register.interfaces';
import { registerService } from '../../services/register.service';

@Component({})
export default class RegistrationForm extends Vue {
    private isBusy: boolean = false;
    private errors: string = '';
    private registerModel = {} as Register; 

    public handleSubmit() {
        this.isBusy = true;
        registerService.register(this.registerModel).finally(() => this.isBusy = false)
        .subscribe((result: any) => {
            this.$router.push({name: 'loginForm', query: { email: this.registerModel.email }});
        },
        (errors: any) => this.errors = errors);
    }
}
</script>

<style scoped>

</style>
