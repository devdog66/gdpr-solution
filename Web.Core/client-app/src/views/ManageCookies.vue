<template>
    <main class="pageContent">
        <div class="centeredContainer">
            <div>
                <h2>Cookie Preferences</h2>
                <hr />
            </div>
            <div>
                <div class="w3-third">
                    <div class="w3-bar-block">
                        <a id="lnkYourPrivacy" class="w3-bar-item w3-button" v-on:click="openPrefs('yourPrivacy')">
                            <h4>Your Privacy</h4>
                        </a>
                        <a id="lnkNecessary" class="w3-bar-item w3-button" v-on:click="openPrefs('necessary')">
                            <h4>Strictly Necessary Cookies</h4>
                        </a>
                        <a id="lnkTracking" class="w3-bar-item w3-button" v-on:click="openPrefs('tracking')">
                            <h4>Tracking Cookies</h4>
                        </a>
                        <a id="lnkAdvertising" class="w3-bar-item w3-button" v-on:click="openPrefs('advertising')">
                            <h4>Advertising Cookies</h4>
                        </a>
                    </div>
                </div>
                <div class="w3-twothird cookieText">
                    <div id="yourPrivacy">
                        <div>
                            <h4>Your Privacy</h4>
                        </div>
                        <p class="w3-left-align">When you visit any web site, it may store or retrieve information on
                            your browser, in the form of cookies.
                            This information might be about you, your preferences or your device and is mostly used to
                            make the site work as you expect it to.
                            The information can give you a more personalized web experience. Because we respect your
                            right to privacy, you can choose not to allow
                            some types of cookies. Click on the different category headings to find out more and change
                            our default settings. However, blocking
                            some types of cookies may impact your experience of the site and the services we are able to
                            offer.
                            <a href="/page/privacy/">Privacy Statement</a>
                        </p>

                    </div>
                    <div id="necessary" class="w3-hide">
                        <div class="prefHeader">
                            <h4>Strictly Necessary Cookies</h4>
                            <!-- Rectangular switch -->
                            <div class="prefOption">
                                <div>
                                    <h5>Always Active</h5>
                                </div>
                            </div>
                        </div>
                        <div>
                            <p class="w3-left-align">These cookies are necessary for the website to function and cannot
                                be switched off.
                                They are usually only set in response to actions made by you which amount to a request
                                for services, such as setting your privacy preferences,
                                logging in or filling in forms. You can set your browser to block or alert you about
                                these cookies, but some parts of the site will not then work.</p>
                        </div>
                    </div>
                    <div id="tracking" class="w3-hide">
                        <div class="prefHeader">
                            <h4>Tracking Cookies</h4>
                            <div class="prefOption">
                                <!-- Rectangular switch -->
                                <label class="switch">
                                    <input name="tracking" type="checkbox" v-model="cookiePrefs.Tracking">
                                    <span class="slider"></span>
                                </label>
                            </div>
                        </div>
                        <div>
                            <p class="w3-left-align">These cookies allow us to count visits and traffic sources so we
                                can measure and improve the performance of our site.
                                They help us to know which pages are the most and least popular and see how visitors
                                move around the site.
                                If you do not allow these cookies we will not know when you have visited our site, and
                                will not be able to monitor its performance.</p>
                        </div>
                    </div>
                    <div id="advertising" class="w3-hide">
                        <div class="prefHeader">
                            <h4>Advertising Cookies</h4>
                            <div class="prefOption">
                                <!-- Rectangular switch -->
                                <label class="switch">
                                    <input name="advertising" type="checkbox" v-model="cookiePrefs.Advertising">
                                    <span class="slider"></span>
                                </label>
                            </div>
                        </div>
                        <div>
                            <p class="w3-left-align">These cookies may be set through our site by our advertising
                                partners. They may be used by those companies to build a
                                profile of your interests and show you relevant adverts on other sites. They are based
                                on uniquely identifying your browser and internet device.
                                If you do not allow these cookies, you will experience less targeted advertising.</p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="w3-threequarter">
                <button class="button w3-right" v-on:click="savePreferences()">Save Preferences</button>
            </div>
        </div>
    </main>
</template>

<style>
.cookieText {
    font-size: medium;
    padding-left: 16px;
}

.prefHeader {
    padding-left: 10px;
    width: calc(100% - 20px);
    padding-top: 10px;
    margin-bottom: 0px;
    padding-bottom: 8px;
    display: inline-block;
}

.prefOption {
    float: right;
}
</style>

<script lang="ts">
import { defineComponent } from 'vue';
import { CookiePreferences } from "../domain/models";
import { CookiePrefsPresenter } from "../presenters/cookies";

let presenter = new CookiePrefsPresenter();

export default defineComponent({
    name: "CookiePreferences",
    data() {
        return {
            cookiePrefs: new CookiePreferences()
        }
    },
    mounted() {
        presenter.getPreferences().then(storedPrefs => {
            this.cookiePrefs = storedPrefs;
        });
    },
    methods: {
        savePreferences() {
            presenter.savePreferences(this.cookiePrefs);
        },
        openPrefs(itemName: string) {
            presenter.openPrefs(itemName);
        }
    }
});
</script>