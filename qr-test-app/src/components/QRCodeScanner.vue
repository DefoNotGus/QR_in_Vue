<template>
  <div>
    <video ref="video" autoplay playsinline width="300" height="200"></video>
    <canvas ref="canvas" style="display: none;"></canvas>
    <p v-if="scanResult">{{ scanResult }}</p>
  </div>
</template>

<script>
import jsQR from "jsqr";

export default {
  name: "QRCodeScanner",
  emits: ["codeScanned"],
  data() {
    return {
      stream: null,
      scanning: true,
      scanResult: "",
    };
  },
  methods: {
    async startCamera() {
      try {
        this.stream = await navigator.mediaDevices.getUserMedia({
          video: { facingMode: "environment" },
        });
        this.$refs.video.srcObject = this.stream;
        this.scanLoop();
      } catch (err) {
        this.scanResult = "Camera access denied or not available.";
        console.error(err);
      }
    },
    async verifyCode(code) {
      try {
       const res = await fetch(`http://localhost:5000/api/items/verify?value=${encodeURIComponent(code)}`, {
        cache: "no-store"
        });


        if (res.status === 200) {
          const data = await res.json();
          if (data.exists === true) {
            this.scanResult = `✅ "${code}" found in database.`;
          } else {
            this.scanResult = `❌ "${code}" not found in database.`;
          }
        } else if (res.status === 404) {
          this.scanResult = `❌ "${code}" not found in database.`;
        } else {
          this.scanResult = `⚠️ Unexpected status: ${res.status}`;
        }

        this.$emit("codeScanned", { code, status: res.status });
      } catch (err) {
        this.scanResult = "⚠️ Error verifying code.";
        console.error("Verification error:", err);
      }
    },
    scanLoop() {
      const video = this.$refs.video;
      const canvas = this.$refs.canvas;
      const context = canvas.getContext("2d");

      const checkFrame = () => {
        if (!this.scanning || video.readyState !== video.HAVE_ENOUGH_DATA) {
          requestAnimationFrame(checkFrame);
          return;
        }

        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;
        context.drawImage(video, 0, 0, canvas.width, canvas.height);

        const imageData = context.getImageData(0, 0, canvas.width, canvas.height);
        const code = jsQR(imageData.data, canvas.width, canvas.height);

        if (code && code.data) {
          this.scanning = false;
          const scannedValue = code.data.trim();
          this.verifyCode(scannedValue);
        } else {
          requestAnimationFrame(checkFrame);
        }
      };

      requestAnimationFrame(checkFrame);
    },
  },
  mounted() {
    this.startCamera();
  },
  beforeUnmount() {
    if (this.stream) {
      this.stream.getTracks().forEach((track) => track.stop());
    }
    this.scanning = false;
  },
};
</script>
