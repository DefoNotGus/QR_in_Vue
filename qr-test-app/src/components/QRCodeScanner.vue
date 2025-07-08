<template>
  <div>
    <qr-scanner @decode="onDecode" />

    <p>Scanned: {{ scannedValue }}</p>
    <button :disabled="!scannedValue" @click="verifyWithBackend">Verify with Backend</button>

    <p v-if="verifyResult !== null">{{ verifyResult }}</p>
  </div>
</template>

<script>
export default {
  data() {
    return {
      scannedValue: '',
      verifyResult: null
    };
  },
  methods: {
    onDecode(decodedText) {
      this.scannedValue = decodedText;
    },
    async verifyWithBackend() {
      try {
        const res = await fetch(`http://localhost:5000/api/hardcoded/verify?value=${encodeURIComponent(this.scannedValue)}`);
        if (!res.ok) throw new Error("Not found");
        const data = await res.json();
        this.verifyResult = `✅ Verified: ${data.name} (ID: ${data.id}, Value: ${data.value})`;
      } catch (err) {
        this.verifyResult = "❌ Item not found in backend.";
      }
    }
  }
};
</script>
