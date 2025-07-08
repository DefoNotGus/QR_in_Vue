<template>
  <div>
    <h2>QR Code Generator</h2>
    <input v-model="text" placeholder="Enter text to encode" />
    <canvas ref="canvas" style="margin-top: 10px;"></canvas>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import QRCode from 'qrcode'

const text = ref('Hello QR')
const canvas = ref(null)

watch(text, async (newValue) => {
  if (canvas.value && newValue.trim().length > 0) {
    try {
      await QRCode.toCanvas(canvas.value, newValue)
    } catch (err) {
      console.error('QR generation failed:', err)
    }
  } else {
    const ctx = canvas.value?.getContext('2d')
    if (ctx) {
      ctx.clearRect(0, 0, canvas.value.width, canvas.value.height)
    }
  }
}, { immediate: true })
</script>
