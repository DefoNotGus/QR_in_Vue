import { createRouter, createWebHistory } from 'vue-router';
import GenerateQRView from '../views/GenerateQRView.vue';
import ScanQRView from '../views/ScanQRView.vue';

const routes = [
  { path: '/', redirect: '/generate' },
  { path: '/generate', name: 'generate', component: GenerateQRView },
  { path: '/scan', name: 'scan', component: ScanQRView }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
