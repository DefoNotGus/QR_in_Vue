<template>
  <div id="app">
    <header class="app-header">
      <h1>QR Demo</h1>
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
header.app-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #f0f0f0;
  padding: 1rem;
}
nav {
  margin: 0.5rem 0;
}
router-link {
  margin-right: 1rem;
  font-weight: bold;
}
</style>
