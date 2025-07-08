<template>
  <div id="app">
    <header>
      <h1>QR App</h1>
      <nav>
        <router-link to="/generate">Generate</router-link>
        <router-link to="/scan">Scan</router-link>
      </nav>
    </header>
    <main>
      <router-view />
    </main>
  </div>
</template>

<style scoped>
nav {
  margin: 1rem 0;
}
router-link {
  margin-right: 1rem;
}
</style>
