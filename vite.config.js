import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  define: {
    // Expose environment variables to the client
    'process.env.EXPO_PUBLIC_CLERK_PUBLISHABLE_KEY': 
      JSON.stringify(process.env.EXPO_PUBLIC_CLERK_PUBLISHABLE_KEY || 
      'pk_test_Y2FyaW5nLWJpc29uLTE5LmNsZXJrLmFjY291bnRzLmRldiQ')
  },
  optimizeDeps: {
    include: ['react', 'react-dom', 'react-native-web'],
  },
  resolve: {
    alias: {
      'react-native': 'react-native-web',
    },
  },
  build: {
    outDir: 'dist',
    rollupOptions: {
      input: {
        main: 'index.html',
      },
    },
  },
  server: {
    port: 8080
  },
});
