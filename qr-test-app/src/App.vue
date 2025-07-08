<template>
  <div class="container">
    <QRCodeGenerator />
    <QRCodeScanner />
  </div>
</template>

<script setup>
import QRCodeGenerator from './components/QRCodeGenerator.vue'
import QRCodeScanner from './components/QRCodeScanner.vue'
</script>

<style>
.container {
  display: flex;
  justify-content: center;
  align-items: flex-start;
  gap: 40px;
  padding: 20px;
  flex-wrap: wrap;
}

.container > * {
  width: 400px;
  max-width: 90vw;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  padding: 16px;
  border-radius: 8px;
  background: #f9f9f9;
}
</style>
