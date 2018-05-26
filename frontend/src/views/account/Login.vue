<template>
    <div class="container text-left">
        <div class="form-row justify-content-md-center">
            <div class="col-lg-4">
                <div class="form-group">
                    <label>Email</label>
                    <input class="form-control" v-model="loginForm.email" placeholder="Email"/>
                </div>
            </div>
        </div>
        <div class="form-row justify-content-md-center">
            <div class="col-lg-4">
                <div class="form-group">
                    <label>Password</label>
                    <input class="form-control" v-model="loginForm.password" placeholder="Password"/>
                </div>
                <div>
                    <button @click="handleSubmit" class="btn btn-primary">Sign in</button>
                </div>
            </div>

        </div>
    </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { Credentials } from '../../models/credentials.interface';
import { authService } from '@/services/auth.service';

@Component({})
export default class LoginForm extends Vue {
    private loginForm = {} as Credentials;
    private isBusy: boolean = false;
    private errors: string = '';

    private created() {
        if (this.$route.query.email) {
            this.loginForm.email = this.$route.query.email;
        }
    }



    private handleSubmit() {
        this.isBusy = true;
        this.$store.dispatch('auth/authRequest', this.loginForm)
        .then((result) => {
            this.$router.push('/home');
        })
        .catch((err) => this.errors = err)
        .then(() => {
            this.isBusy = false;
        });
    }
}
</script>

<style scoped>

</style>
