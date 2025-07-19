# Vue 3 + TypeScript + Vite

This template should help get you started developing with Vue 3 and TypeScript in Vite. The template uses Vue 3 `<script setup>` SFCs, check out the [script setup docs](https://v3.vuejs.org/api/sfc-script-setup.html#sfc-script-setup) to learn more.

Learn more about the recommended Project Setup and IDE Support in the [Vue Docs TypeScript Guide](https://vuejs.org/guide/typescript/overview.html#project-setup).

## Install
To install this template run the following command and select vue and typescript as options
```
npm create vite@latest
```
Then run the following to install packages
```
npm install
```

## Configure
For some systems localhost may be unavailable. Configure vite.config.ts to use the following server

``` ts
server: {
    host: '127.0.0.1'
  }
```

## Jest for Unit Testing
To install Jest there are several steps to get typescript tests working.

Install Jest
```
npm install -D jest
```

Add a configuration for code coverage
```
npm init jest@latest
```

ts-node is needed to be installed
```
npm install -D ts-node
```

babel is needed to transpile tests
```
npm install -D @babel/preset-typescript
npm install -D babel-jest @babel/core @babel/preset-env
```

babel requires a config file, babel.config.js, with the following
``` js
module.exports = {
  presets: [
    ['@babel/preset-env', {targets: {node: 'current'}}],
    '@babel/preset-typescript',
  ],
};
```

Finally, in package.json, remove the type: module if exists

## Cypress for End to End testing
Install Cypress
```
npm install -D cypress
```

Add Cypress to package.json similar to the following
``` json
"scripts": {
    "dev": "vite",
    "build": "vue-tsc -b && vite build",
    "preview": "vite preview",
    "test": "jest",
    "cypress": "cypress open"
  },
```

Run cypress with the following
```
npm run cypress
```

Configure cypress through the UI using this link
https://docs.cypress.io/app/get-started/open-the-app

Install @types/jest
```
npm install -D @types/jest
```

A new typescript config, tsconfig.cypress-ct.json, will be needed.




