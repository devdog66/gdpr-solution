<template>
  <header>
    <nav-bar></nav-bar>
  </header>
  <div class="">
    <main>
      <router-view class=""></router-view>
    </main>
    <site-aside></site-aside>
  </div>

  <footer>
    <site-footer></site-footer>
  </footer>
</template>

<style>
</style>

<script setup lang="ts">
import NavBar from './components/NavBar.vue';
import SiteFooter from './components/Footer.vue';
import SiteAside from './components/Aside.vue';
</script>
