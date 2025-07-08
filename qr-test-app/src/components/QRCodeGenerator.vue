<template>
  <div>
    <input v-model="valueToEncode" placeholder="Enter value" />
    <qrcode-vue v-if="valueToEncode" :value="valueToEncode" :size="200" />
    <button @click="addToBackend">Add to Backend</button>

    <p v-if="actionResult">{{ actionResult }}</p>
  </div>
</template>

<script>
import QrcodeVue from 'qrcode.vue';

export default {
  components: { QrcodeVue },
  data() {
    return {
      valueToEncode: '',
      actionResult: null
    };
  },
  methods: {
    async addToBackend() {
      if (!this.valueToEncode) return;

      try {
        const res = await fetch('http://localhost:5000/api/items', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ value: this.valueToEncode })
        });
        if (!res.ok) throw new Error();
        this.actionResult = '✅ Added to backend.';
      } catch (err) {
        this.actionResult = '❌ Failed to add value.';
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
input {
  padding: 0.5rem;
}
button {
  padding: 0.5rem 1rem;
}
</style>
