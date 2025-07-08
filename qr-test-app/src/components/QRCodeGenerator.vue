<template>
  <div>
    <input v-model="valueToEncode" placeholder="Enter value to encode" />
    <qrcode-vue :value="valueToEncode" :size="200" />
    <button @click="verifyWithBackend">Verify with Backend</button>

    <p v-if="verifyResult !== null">{{ verifyResult }}</p>
  </div>
</template>

<script>
import QrcodeVue from 'qrcode.vue';

export default {
  components: { QrcodeVue },
  data() {
    return {
      valueToEncode: '',
      verifyResult: null
    };
  },
  methods: {
    async verifyWithBackend() {
      if (!this.valueToEncode) return;

      try {
        const res = await fetch(`http://localhost:5000/api/hardcoded/verify?value=${encodeURIComponent(this.valueToEncode)}`);
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
