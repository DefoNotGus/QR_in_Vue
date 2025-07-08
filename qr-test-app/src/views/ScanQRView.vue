<template>
  <div>
    <QRCodeScanner @codeScanned="handleScannedCode" />
    <p v-if="result">{{ result }}</p>
  </div>
</template>

<script>
import QRCodeScanner from "@/components/QRCodeScanner.vue";

export default {
  components: { QRCodeScanner },
  data() {
    return {
      result: "",
    };
  },
  methods: {
    async handleScannedCode(code) {
      try {
        const res = await fetch(`/api/items/verify?value=${encodeURIComponent(code)}`);
        if (res.ok) {
          this.result = "✅ Code found in database.";
        } else {
          this.result = "❌ Code not found.";
        }
      } catch {
        this.result = "⚠️ Network error.";
      }
    },
  },
};
</script>
