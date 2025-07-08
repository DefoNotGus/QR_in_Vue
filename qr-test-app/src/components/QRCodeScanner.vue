<template>
  <div>
    <qrcode-stream @decode="onDecode" @init="onInit" />

    <p>Scanned: {{ scannedValue }}</p>

    <p v-if="verifyResult">{{ verifyResult }}</p>
  </div>
</template>

<script>
import { QrcodeStream } from 'vue-qrcode-reader';

export default {
  components: { QrcodeStream },
  data() {
    return {
      scannedValue: '',
      verifyResult: ''
    };
  },
  methods: {
    onDecode(decodedText) {
      this.scannedValue = decodedText;
      this.verifyWithBackend();
    },
    onInit(promise) {
      promise.catch(error => {
        console.error('Camera initialization failed', error);
        if (error.name === 'NotAllowedError') {
          this.verifyResult = 'Camera access was denied.';
        } else if (error.name === 'NotFoundError') {
          this.verifyResult = 'No camera device found.';
        } else {
          this.verifyResult = 'Unable to start camera.';
        }
      });
    },
    async verifyWithBackend() {
      try {
        const res = await fetch(`http://localhost:5000/api/items/verify?value=${encodeURIComponent(this.scannedValue)}`);
        if (!res.ok) throw new Error();
        this.verifyResult = '✅ Value found in backend.';
      } catch (err) {
        this.verifyResult = '❌ Value not found.';
      }
    }
  }
};
</script>

<style scoped>
div {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
}
</style>
