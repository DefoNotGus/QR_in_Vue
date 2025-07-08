<template>
  <div>
    <h2>QR Code Scanner</h2>
    <qrcode-stream
      @decode="onDecode"
      @init="onInit"
      style="max-width: 100%; height: auto;"
    />
    <p v-if="result">Scanned result: {{ result }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { QrcodeStream } from 'vue-qrcode-reader'

const result = ref(null)

function onDecode(decoded) {
  result.value = decoded
}

function onInit(promise) {
  promise.catch(error => {
    console.error("Camera init failed:", error)
  })
}
</script>
