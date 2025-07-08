<template>
  <div>
    <qr-scanner @decode="onDecode" />

    <p>Scanned: {{ scannedValue }}</p>

    <p v-if="verifyResult">{{ verifyResult }}</p>
  </div>
</template>

<script>
export default {
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
