<template>
  <div>
    <video ref="video" autoplay playsinline width="300" height="200"></video>
    <canvas ref="canvas" style="display: none;"></canvas>
    <p v-if="error">{{ error }}</p>
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
      error: null,
      scanning: true,
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
        this.error = "Camera access denied or not available.";
        console.error(err);
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

        if (code) {
          this.$emit("codeScanned", code.data);
          this.scanning = false; // stop scanning after successful read
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
